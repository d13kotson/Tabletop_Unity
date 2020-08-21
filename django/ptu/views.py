import json
import os
from array import array
from pathlib import Path

from django.db.models import Q
from django.http import HttpResponse
from django.shortcuts import get_object_or_404
from django.views.decorators.csrf import csrf_exempt

from rest_framework import generics
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from .models import *

from . import serializers

""" List Views """


class UserTrainerList(generics.ListCreateAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.TrainerSimpleSerializer

    def get_queryset(self):
        return Trainer.objects.filter(user=self.request.user)


class GameTrainerList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.TrainerSerializer

    def get_queryset(self):
        return Trainer.objects.filter(game=self.kwargs['game'])


class GamePokemonList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.PokemonSerializer

    def get_queryset(self):
        return Pokemon.objects.filter(game=self.kwargs['game'], trainer__isnull=True)


class GameList(generics.ListCreateAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.GameSimpleSerializer

    def get_queryset(self):
        return Game.objects.filter(gm=self.request.user)


class EdgeList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Edge.objects.all()
    serializer_class = serializers.EdgeSerializer


class FeatureList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.FeatureSerializer

    def get_queryset(self):
        if 'game' in self.request.query_params:
            return Feature.objects.filter(Q(game=self.request.query_params['game']) | Q(game__isnull=True))
        else:
            return Feature.objects.filter(game__isnull=True)


class AttackList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.AttackSerializer

    def get_queryset(self):
        if 'game' in self.request.query_params:
            return Attack.objects.filter(Q(game=self.request.query_params['game']) | Q(game__isnull=True))
        else:
            return Attack.objects.filter(game__isnull=True)


class SpeciesList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Species.objects.all().order_by('dex_num')
    serializer_class = serializers.SpeciesSimpleSerializer


class ItemList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Item.objects.all()
    serializer_class = serializers.ItemSerializer


class GameMessageList(generics.ListAPIView):
    serializer_class = serializers.MessageSerializer

    def get_queryset(self):
        return Message.objects.filter(game=self.kwargs['game'])


class ImageList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Image.objects.all()
    serializer_class = serializers.ImageSerializer


class BackgroundList(generics.ListCreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Background.objects.all()
    serializer_class = serializers.BackgroundSerializer


class TokenList(generics.ListCreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Token.objects.all()
    serializer_class = serializers.TokenSerializer


""" Detail Views """


class UserInformation(generics.GenericAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.UserInfoSerializer

    def get(self, request, username):
        try:
            serializer = self.serializer_class(User.objects.get(username=username))
            return Response(serializer.data)
        except User.DoesNotExist:
            return Response(data='User not found.', status=404)


class AttackDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Attack.objects.all()
    serializer_class = serializers.AttackSerializer


class EdgeDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Edge.objects.all()
    serializer_class = serializers.EdgeSerializer


class EvolutionDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Evolution.objects.all()
    serializer_class = serializers.EvolutionSerializer


class FeatureDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Feature.objects.all()
    serializer_class = serializers.FeatureSerializer


class GameDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Game.objects.all()
    serializer_class = serializers.GameSerializer


class TrainerItemDetail(generics.RetrieveUpdateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerItem.objects.all()
    serializer_class = serializers.TrainerItemSimpleSerializer


class TrainerItemAdd(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerItem.objects.all()
    serializer_class = serializers.TrainerItemSimpleSerializer


class TrainerItemIncrease(generics.GenericAPIView):
    permission_classes = [IsAuthenticated]

    def post(self, request, pk):
        item = TrainerItem.objects.get(pk=pk)
        item.number += 1
        item.save()
        return Response(data=None, status=200)


class TrainerItemDecrease(generics.GenericAPIView):
    permission_classes = [IsAuthenticated]

    def post(self, request, pk):
        item = TrainerItem.objects.get(pk=pk)
        item.number -= 1
        if item.number <= 0:
            item.delete()
        else:
            item.save()
        return Response(data=None, status=200)



class PokemonDetail(generics.RetrieveUpdateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Pokemon.objects.all()
    serializer_class = serializers.PokemonSimpleSerializer


class PokemonGenerate(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    serializer_class = serializers.PokemonSimpleSerializer


class PokemonAttackDetail(generics.RetrieveDestroyAPIView):
    permission_classes = [IsAuthenticated]
    queryset = PokemonAttack.objects.all()
    serializer_class = serializers.PokemonAttackSerializer


class PokemonAttackAdd(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = PokemonAttack.objects.all()
    serializer_class = serializers.PokemonAttackSimpleSerializer


class SpeciesDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Species.objects.all()
    serializer_class = serializers.SpeciesSerializer


class SpeciesAttackDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = SpeciesAttack.objects.all()
    serializer_class = serializers.SpeciesAttackSerializer


class TrainerDetail(generics.RetrieveUpdateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Trainer.objects.all()
    serializer_class = serializers.TrainerSerializer


class TrainerAttackDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerAttack.objects.all()
    serializer_class = serializers.TrainerAttackSerializer


class TrainerAttackAdd(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerAttack.objects.all()
    serializer_class = serializers.TrainerAttackSimpleSerializer


class TrainerEdgeDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerEdge.objects.all()
    serializer_class = serializers.TrainerEdgeSerializer


class TrainerEdgeAdd(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerEdge.objects.all()
    serializer_class = serializers.TrainerEdgeSimpleSerializer


class TrainerFeatureDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerFeature.objects.all()
    serializer_class = serializers.TrainerFeatureSerializer


class TrainerFeatureAdd(generics.CreateAPIView):
    permission_classes = [IsAuthenticated]
    queryset = TrainerFeature.objects.all()
    serializer_class = serializers.TrainerFeatureSimpleSerializer


class ImageDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Image.objects.all()
    serializer_class = serializers.ImageSerializer


class BackgroundDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Background.objects.all()
    serializer_class = serializers.BackgroundSerializer


class TokenDetail(generics.RetrieveAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Token.objects.all()
    serializer_class = serializers.TokenSerializer


def image(request, pk):
    image = get_object_or_404(Image, id=pk)
    try:
        with open(get_image_url(image.path), 'rb') as image_content:
            return HttpResponse(image_content.read(), content_type='image/png')
    except:
        return HttpResponse(status=404)


@csrf_exempt
def upload_image(request):
    if request.method == 'POST':
        body = json.loads(request.body)
        image_data = body['image']
        name = body['name']
        height = body['height']
        width = body['width']
        image = Image(
            name=name,
            height=height,
            width=width
        )
        image.save()
        image_path = get_image_url(image.path)
        Path(os.path.dirname(image_path)).mkdir(parents=True, exist_ok=True)
        image_file = open(image_path, 'wb+')
        image_file.write(array.array('B', image_data))
        image_file.close()
    return HttpResponse();


def get_image_url(image_path):
    return f'media/images/{image_path}'
