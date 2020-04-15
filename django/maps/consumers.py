from channels.generic.websocket import WebsocketConsumer
from asgiref.sync import async_to_sync
from .models import Token
import json

class MapsConsumer(WebsocketConsumer):
	def connect(self):
		self.room_name = self.scope['url_route']['kwargs']['room_name']
		self.room_group_name = 'map_%s' % self.room_name

		# join room group
		async_to_sync(self.channel_layer.group_add)(
			self.room_group_name,
			self.channel_name
		)
		state = getattr(self.channel_layer, 'state', {
			'background': '',
			'tokens': list(),
		})
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

		if type == 'setBackground':
			self.setBackgroundState(content)
		elif type == 'addToken':
			self.addTokenState(content)
		elif type == 'updateToken':
			self.updateTokenState(content)

		async_to_sync(self.channel_layer.group_send)(
			self.room_group_name,
			{
				'type': type,
				'content': content
			}
		)

	def setBackground(self, event):
		content = '/map/background/' + event['content']

		self.send(text_data=json.dumps({
			'type': 'setBackground',
			'content': content
		}))

	def setBackgroundState(self, content):
		content = '/map/background/' + content
		self.channel_layer.state['background'] = content

	def addToken(self, event):
		token = Token.objects.get(id=event['content'])
		content = '/map/token/' + event['content']

		self.send(text_data=json.dumps({
			'type': 'addToken',
			'content': {
				'src': content,
				'width': token.width,
				'height': token.height
			}
		}))

	def addTokenState(self, content):
		token = Token.objects.get(id=content)
		content = '/map/token/' + content
		self.channel_layer.state['tokens'].append({
			'source': content,
			'x': 0,
			'y': 0,
			'width': token.width,
			'height': token.height
		})

	def updateToken(self, event):
		content = event['content']

		self.send(text_data=json.dumps({
			'type': 'updateToken',
			'content': content
		}))

	def updateTokenState(self, content):
		token = self.channel_layer.state['tokens'][int(content['tokenID'])]
		token['x'] = content['tokenX']
		token['y'] = content['tokenY']

	def updateState(self, event):
		self.channel_layer.state = event['content']

		self.send(text_data=json.dumps({
			'type': 'updateState',
			'content': self.channel_layer.state
		}))

	def requestState(self, event):
		content = self.channel_layer.state

		self.send(text_data=json.dumps({
			'type': 'updateState',
			'content': content
		}))

	def clearState(self, event):
		self.channel_layer.state = {
			'background': '',
			'tokens': list(),
		}

		self.send(text_data=json.dumps({
			'type': 'updateState',
			'content': self.channel_layer.state
		}))
