from channels.auth import AuthMiddlewareStack
from channels.routing import ProtocolTypeRouter, URLRouter
from django.urls import re_path
from ptu.consumers import PTUConsumer

websocket_urlpatterns = [
	# (http->django views is added by default)
	re_path(r'ws/(?P<room_name>\w+)/(?P<is_gm>\w+)$', PTUConsumer),
]

application = ProtocolTypeRouter({
	# (http -> django views is added by default)
	'websocket': AuthMiddlewareStack(
		URLRouter(
			websocket_urlpatterns
		)
	),
})
