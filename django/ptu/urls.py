from django.urls import path

from . import views

urlpatterns = [
    path('trainers/', views.UserTrainerList.as_view()),
    path('trainers/<int:game>/', views.GameTrainerList.as_view()),
    path('wildpokemon/<int:game>/', views.GamePokemonList.as_view()),
    path('games/', views.GameList.as_view()),
    path('edges/', views.EdgeList.as_view()),
    path('features/', views.FeatureList.as_view()),
    path('attacks/', views.AttackList.as_view()),
    path('species/', views.SpeciesList.as_view()),
    path('items/', views.ItemList.as_view()),

    path('user/<str:username>/', views.UserInformation.as_view(), name='user-info'),
    path('attack/<int:pk>/', views.AttackDetail.as_view(), name='attack-detail'),
    path('edge/<int:pk>/', views.EdgeDetail.as_view(), name='edge-detail'),
    path('evolution/<int:pk>/', views.EvolutionDetail.as_view(), name='evolution-detail'),
    path('feature/<int:pk>/', views.FeatureDetail.as_view(), name='feature-detail'),
    path('game/<int:pk>/', views.GameDetail.as_view(), name='game-detail'),
    path('item/<int:pk>/', views.TrainerItemDetail.as_view(), name='item-detail'),
    path('addItem/', views.TrainerItemAdd.as_view(), name='item-add'),
    path('pokemon/<int:pk>/', views.PokemonDetail.as_view(), name='pokemon-detail'),
    path('generate/', views.PokemonGenerate.as_view(), name='generate-pokemon'),
    path('pokemonAttack/<int:pk>/', views.PokemonAttackDetail.as_view(), name='pokemonattack-detail'),
    path('addPokemonAttack/', views.PokemonAttackAdd.as_view(), name='pokemonattack-add'),
    path('species/<int:pk>/', views.SpeciesDetail.as_view(), name='species-detail'),
    path('speciesAttack/<int:pk>/', views.SpeciesAttackDetail.as_view(), name='speciesattack-detail'),
    path('trainer/<int:pk>/', views.TrainerDetail.as_view(), name='trainer-detail'),
    path('trainerAttack/<int:pk>/', views.TrainerAttackDetail.as_view(), name='trainerattack-detail'),
    path('addTrainerAttack/', views.TrainerAttackAdd.as_view(), name='trainerattack-add'),
    path('trainerEdge/<int:pk>/', views.TrainerEdgeDetail.as_view(), name='traineredge-detail'),
    path('addTrainerEdge/', views.TrainerEdgeAdd.as_view(), name='traineredge-add'),
    path('trainerFeature/<int:pk>/', views.TrainerFeatureDetail.as_view(), name='trainerfeature-detail'),
    path('addTrainerFeature/', views.TrainerFeatureAdd.as_view(), name='trainerfeature-add'),
]