from django.urls import path

from . import views

urlpatterns = [
    path('messages/<int:game>/', views.GameMessageList.as_view()),
]
