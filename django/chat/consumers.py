from channels.generic.websocket import WebsocketConsumer
from asgiref.sync import async_to_sync
from .models import Message
from ptu.models import Game
import json
import re
import random

class ChatConsumer(WebsocketConsumer):
	def connect(self):
		self.room_name = self.scope['url_route']['kwargs']['room_name']
		self.room_group_name = 'chat_%s' % self.room_name
		self.is_gm = self.scope['url_route']['kwargs']['is_gm'] == 'true'
		self.game = Game.objects.get(id=self.room_name)

		# join room group
		async_to_sync(self.channel_layer.group_add)(
			self.room_group_name,
			self.channel_name
		)
		self.accept()

	def disconnect(self, close_code):
		async_to_sync(self.channel_layer.group_discard)(
			self.room_group_name,
			self.channel_name
		)

	def receive(self, text_data):
		text_data_json = json.loads(text_data)
		message = text_data_json['message']
		display_name = text_data_json['display_name']
		roll = ''
		gm_roll = False
		roll_pattern = re.compile('^/roll *[0-9]* *d *[0-9]+[\+-]?[0-9]*$')
		gm_roll_pattern = re.compile('^/gmroll *[0-9]*d[0-9]+[\+-]?[0-9]*$')
		if roll_pattern.match(message):
			message = self.roll_die(message[5:])
		elif gm_roll_pattern.match(message):
			message = self.roll_die(message[7:])
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

	def roll_die(self, message):
		num_die, die_num, add = self.parse_die_roll(message)
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

	def chat_message(self, event):
		message = event['message']
		display_name = event['display_name']
		gm_roll = event['gm_roll']

		if gm_roll and not self.is_gm:
			return

		self.send(text_data=json.dumps({
			'display_name': display_name,
			'message': message
		}))
