from django.conf.urls import url, include
from django.urls import path
from rest_framework import routers
from . import views

router = routers.DefaultRouter()
router.register(r'users', views.UserViewSet)

urlpatterns = [
	url(r'^', include(router.urls)),
	url(r'^auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('signup/', views.SignUp.as_view(), name='signup'),
]
