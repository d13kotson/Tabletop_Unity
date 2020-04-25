import json
import os
from array import array
from pathlib import Path

from django.db.models import Q
from django.http import HttpResponse
from django.shortcuts import get_object_or_404

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
    queryset = Species.objects.all()
    serializer_class = serializers.SpeciesSerializer


class ItemList(generics.ListAPIView):
    permission_classes = [IsAuthenticated]
    queryset = Item.objects.all()
    serializer_class = serializers.ItemSerializer


class GameMessageList(generics.ListAPIView):
    serializer_class = serializers.MessageSerializer

    def get_queryset(self):
        return Message.objects.filter(game=self.kwargs['game'])


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


def create_background(request):
    if request.method == 'POST':
        body = json.loads(request.body)
        background_image = body['image']
        title = body['title']
        gm = request.user
        background = Background(
            image='',
            title=title,
            gm=gm
        )
        background.save()
        background.image = get_background_image_url(str(background.id) + '.png')
        Path(os.path.dirname(background.image)).mkdir(parents=True, exist_ok=True)
        image = open(background.image, 'wb+')
        image.write(array.array('B', background_image))
        image.close()
        background.save()
    return HttpResponse();


def background(request, pk):
    background = get_object_or_404(Background, id=pk)
    try:
        with open(background.image, 'rb') as image:
            return HttpResponse(image.read(), content_type='image/png')
    except:
        return HttpResponse()


def backgrounds(request):
    backgroundsQS = Background.objects.filter(gm=request.user)
    backgrounds = []
    for background in backgroundsQS:
        backgrounds.append({
            "title": background.title,
            "id": background.id
        })
    return HttpResponse(json.dumps(backgrounds))


def token(request, pk):
    token = get_object_or_404(Token, id=pk)
    try:
        with open(token.image, 'rb') as image:
            return HttpResponse(image.read(), content_type='image/png')
    except:
        return HttpResponse(status=404)


def create_token(request):
    if request.method == 'POST':
        body = json.loads(request.body)
        tokenImage = body['image']
        title = body['title']
        height = body['height']
        width = body['width']
        gm = request.user
        token = Token(
            image='',
            title=title,
            gm=gm,
            height=height,
            width=width
        )
        token.save()
        token.image = get_token_image_url(str(token.id) + '.png')
        Path(os.path.dirname(token.image)).mkdir(parents=True, exist_ok=True)
        image = open(token.image, 'wb+')
        image.write(array.array('B', tokenImage))
        image.close()
        token.save()
    return HttpResponse();


def tokens(request):
    tokensQS = Token.objects.filter(gm=request.user)
    tokens = []
    for token in tokensQS:
        tokens.append({
            "title": token.title,
            "id": token.id
        })
    return HttpResponse(json.dumps(tokens))


def get_background_image_url(url):
    return 'media/backgrounds/' + url


def get_token_image_url(url):
    return 'media/tokens/' + url
