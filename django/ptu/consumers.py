from .models import Attack, DamageBase, Game, Message, Pokemon, Token, Trainer
from channels.generic.websocket import WebsocketConsumer
from asgiref.sync import async_to_sync
import json
import re
import random


class PTUConsumer(WebsocketConsumer):
    def connect(self):
        self.room_name = self.scope['url_route']['kwargs']['room_name']
        self.room_group_name = 'map_%s' % self.room_name
        self.game = Game.objects.get(id=self.room_name)

        # join room group
        async_to_sync(self.channel_layer.group_add)(
            self.room_group_name,
            self.channel_name
        )
        state = getattr(self.channel_layer, 'state', None)
        if state is None:
            self.channel_layer.state = {
                'background': '',
                'tokens': [],
            }
        self.accept()

    def disconnect(self, close_code):
        async_to_sync(self.channel_layer.group_discard)(
            self.room_group_name,
            self.channel_name
        )

    def receive(self, text_data):
        text_data_json = json.loads(text_data)
        type = text_data_json['type']
        content = text_data_json['content']

        if type == 'set_background':
            self.set_background_state(content)
        elif type == 'add_token':
            self.add_token_state(content)
        elif type == 'update_token':
            self.update_token_state(content)
        elif type == 'chat':
            return self.chat(content)
        elif type == 'roll':
            return self.roll(content)
        elif type == 'attack':
            return self.attack(content)

        async_to_sync(self.channel_layer.group_send)(
            self.room_group_name,
            {
                'type': type,
                'content': content
            }
        )

    def update(self, event):
        self.send(text_data=json.dumps({
            'type': 'update',
            'content': event['content']
        }))

    def set_background(self, event):
        self.send(text_data=json.dumps({
            'type': 'set_background',
            'content': event['content']
        }))

    def set_background_state(self, content):
        self.channel_layer.state['background'] = content

    def add_token(self, event):
        token = Token.objects.get(id=event['content'])

        self.send(text_data=json.dumps({
            'type': 'addToken',
            'content': {
                'src': token.id,
                'width': token.width,
                'height': token.height
            }
        }))

    def add_token_state(self, content):
        token = Token.objects.get(id=content)
        self.channel_layer.state['tokens'].append({
            'id': token.id,
            'x': 0,
            'y': 0,
            'width': token.width,
            'height': token.height
        })

    def update_token(self, event):
        content = event['content']

        self.send(text_data=json.dumps({
            'type': 'updateToken',
            'content': json.dumps(content)
        }))

    def update_token_state(self, content):
        token = self.channel_layer.state['tokens'][int(content['tokenID'])]
        token['x'] = content['tokenX']
        token['y'] = content['tokenY']

    def update_state(self, event):
        self.channel_layer.state = event['content']

        self.send(text_data=json.dumps({
            'type': 'updateState',
            'content': json.dumps(self.channel_layer.state)
        }))

    def request_state(self, event):
        content = self.channel_layer.state

        self.send(text_data=json.dumps({
            'type': 'updateState',
            'content': json.dumps(content)
        }))

    def clear_state(self, event):
        self.channel_layer.state = {
            'background': '',
            'tokens': list(),
        }

        self.send(text_data=json.dumps({
            'type': 'updateState',
            'content': self.channel_layer.state
        }))

    def chat(self, content):
        message = content['message']
        display_name = content['display_name']
        gm_roll = False
        roll_pattern = re.compile('^/roll *[0-9]* *d *[0-9]+[\+-]?[0-9]*$')
        gm_roll_pattern = re.compile('^/gmroll *[0-9]*d[0-9]+[\+-]?[0-9]*$')
        if roll_pattern.match(message):
            num_die, die_num, add = self.parse_die_roll(message[5:])
            message = self.roll_die(num_die, die_num, add)
        elif gm_roll_pattern.match(message):
            num_die, die_num, add = self.parse_die_roll(message[7:])
            message = self.roll_die(num_die, die_num, add)
            gm_roll = True

        if not gm_roll:
            entry = Message()
            entry.game = self.game
            entry.message = message
            entry.display_name = display_name
            entry.save()

        async_to_sync(self.channel_layer.group_send)(
            self.room_group_name,
            {
                'type': 'chat_message',
                'display_name': display_name,
                'gm_roll': gm_roll,
                'message': message
            }
        )

    def roll(self, content):
        display_name = content['display_name']
        num_die = content['num_die']
        die_num = content['die_num']
        add = content['add']
        message = self.roll_die(num_die, die_num, add)
        async_to_sync(self.channel_layer.group_send)(
            self.room_group_name,
            {
                'type': 'chat_message',
                'display_name': display_name,
                'gm_roll': False,
                'message': message
            }
        )

    def attack(self, content):
        display_name = content['display_name']
        attacker_id = content['attacker_id']
        attacker_type = content['attacker_type']
        defender_id = content['defender_id']
        defender_type = content['defender_type']
        attacker = None
        defender = None
        if attacker_type == 'trainer':
            attacker = Trainer.objects.get(pk=attacker_id)
        else:
            attacker = Pokemon.objects.get(pk=attacker_id)
        if defender_type == 'trainer':
            defender = Trainer.objects.get(pk=defender_id)
        else:
            defender = Pokemon.objects.get(pk=defender_id)

        message = self.roll_damage(1, 2, 3, 4, 5, 6)
        async_to_sync(self.channel_layer.group_send)(
            self.room_group_name,
            {
                'type': 'chat_message',
                'display_name': display_name,
                'gm_roll': False,
                'message': message
            }
        )

    def parse_die_roll(self, message):
        num_die = message.split('d')[0].strip()
        if num_die == '':
            num_die = 1
        else:
            num_die = int(num_die)
        add = 0
        if '+' in message:
            add = int(message.split('+')[1].strip())
            die_num = int(message.split('d')[1].split('+')[0].strip())
        elif '-' in message:
            add = int(message.split('-')[1].strip())
            die_num = int(message.split('d')[1].split('-')[0].strip())
        else:
            die_num = int(message.split('d')[1].strip())
        return num_die, die_num, add

    def roll_die(self, num_die, die_num, add):
        sum = 0
        rolls = []
        message = 'rolling %s d%s: ' % (num_die, die_num)
        for i in range(num_die):
            random_int = random.randint(1, die_num)
            rolls.append(str(random_int))
            sum += random_int
        if add == 0:
            message += '+'.join(rolls) + '=' + str(sum)
        else:
            sum += add
            message += '+'.join(rolls) + '+' + str(add) + '=' + str(sum)
        return message

    def roll_damage(self, attack_id, attacker_type, attack, defender_type_1, defender_type_2, defense):
        attack = Attack.objects.get(pk=attack_id)
        stab = False
        db_id = attack.damage_base.id
        if attacker_type == attack.type.id:
            stab = True
            db_id += 2

        crit = False
        db = DamageBase.objects.get(pk=db_id)
        num_die = db.num_die
        die_num = db.die_num
        add = db.add
        hit_roll = random.randint(1, 20)
        if hit_roll == 20:
            crit = True
            num_die *= 2

        sum = 0
        rolls = list()
        for i in range(num_die):
            random_int = random.randint(1, die_num)
            rolls.append(str(random_int))
            sum += random_int
        sum += add
        sum += attack
        sum -= defense

        message = f'''Rolled a {hit_roll} to hit.{" Critical Hit!" if crit else ""}
{"+".join(rolls)}+{add}+{attack}-{defense}={sum} damage.'''
        return message


    def chat_message(self, event):
        message = event['message']
        display_name = event['display_name']
        gm_roll = event['gm_roll']

        if gm_roll and not self.is_gm:
            return

        self.send(text_data=json.dumps({
            'type': 'chat_message',
            'content': json.dumps({
                'display_name': display_name,
                'message': message
            })
        }))
