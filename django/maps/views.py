from django.shortcuts import render, get_object_or_404
from django.http import HttpResponse
from .models import Background, Token

from pathlib import Path

from django.contrib.auth.models import User

import json
import array
import os

# Create your views here.

def createBackground(request):
	if request.method == 'POST':
		body = json.loads(request.body)
		backgroundImage = body['image']
		title = body['title']
		gm = request.user
		background = Background(
			image='',
			title=title,
			gm=gm
		)
		background.save()
		background.image = getBackgroundImageURL(str(background.id) + '.png')
		Path(os.path.dirname(background.image)).mkdir(parents=True, exist_ok=True)
		image = open(background.image, 'wb+')
		image.write(array.array('B', backgroundImage))
		image.close()
		background.save()
	return HttpResponse();

def background(request, pk):
	background = get_object_or_404(Background, id=pk)
	try:
		with open(background.image, 'rb') as image:
			return HttpResponse(image.read(), content_type='image/png')
	except:
		print('uh oh')
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
		return HttpResponse()

def createToken(request):
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
		token.image = getTokenImageURL(str(token.id) + '.png')
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

def getBackgroundImageURL(url):
	return 'media/backgrounds/' + url

def getTokenImageURL(url):
	return 'media/tokens/' + url
