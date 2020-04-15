from rest_framework import generics

from .models import Message

from . import serializers


class GameMessageList(generics.ListAPIView):
    serializer_class = serializers.MessageSerializer

    def get_queryset(self):
        return Message.objects.filter(game=self.kwargs['game'])