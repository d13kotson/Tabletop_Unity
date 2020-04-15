from channels.auth import AuthMiddlewareStack
from channels.routing import ProtocolTypeRouter, URLRouter
from django.urls import re_path
from chat.consumers import ChatConsumer
from maps.consumers import MapsConsumer

websocket_urlpatterns = [
	# (http->django views is added by default)
	re_path(r'ws/chat/(?P<room_name>\w+)/(?P<is_gm>\w+)$', ChatConsumer),
	re_path(r'ws/map/(?P<room_name>\w+)$', MapsConsumer),
]

application = ProtocolTypeRouter({
	# (http -> django views is added by default)
	'websocket': AuthMiddlewareStack(
		URLRouter(
			websocket_urlpatterns
		)
	),
})
