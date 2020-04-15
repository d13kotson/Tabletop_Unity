from django.db import models

from django.contrib.auth.models import User

class Background(models.Model):
	gm = models.ForeignKey(User, on_delete=models.CASCADE)
	title = models.CharField(max_length=20)
	image = models.CharField(max_length=100)

	def __str__(self):
		return self.title

class Token(models.Model):
	gm = models.ForeignKey(User, related_name='gm', on_delete=models.CASCADE)
	title = models.CharField(max_length=20)
	image = models.CharField(max_length=100)
	height = models.IntegerField()
	width = models.IntegerField()

	def __str__(self):
		return self.title
