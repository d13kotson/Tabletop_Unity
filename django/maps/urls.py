from django.urls import path

from . import views

urlpatterns= [
	path('background/', views.createBackground),
	path('background/<int:pk>/', views.background),
	path('backgrounds/', views.backgrounds),
	path('token/', views.createToken),
	path('token/<int:pk>/', views.token),
	path('tokens/', views.tokens),
]
