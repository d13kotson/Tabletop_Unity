from django.db import models

from ptu.models import Game

class Message(models.Model):
	game = models.ForeignKey(Game, on_delete=models.CASCADE)
	message = models.CharField(max_length=200)
	display_name = models.CharField(max_length=50)

	def __str__(self):
		return self.message
