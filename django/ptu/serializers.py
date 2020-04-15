from .models import *

from rest_framework import serializers

import random
import re


class AttackSerializer(serializers.ModelSerializer):
    class Meta:
        model = Attack
        fields = '__all__'


class EdgeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Edge
        fields = '__all__'


class EvolutionSerializer(serializers.ModelSerializer):
    class Meta:
        model = Evolution
        fields = '__all__'


class FeatureSerializer(serializers.ModelSerializer):
    class Meta:
        model = Feature
        fields = '__all__'


class ItemSerializer(serializers.ModelSerializer):
    class Meta:
        model = Item
        fields = '__all__'


class TrainerItemSerializer(serializers.ModelSerializer):
    item = ItemSerializer()
    class Meta:
        model = TrainerItem
        fields = '__all__'


class TrainerItemSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = TrainerItem
        fields = '__all__'

    def update(self, instance, validated_data):
        instance.number = validated_data.get('number', instance.number)
        if instance.number <= 0:
            instance.delete()
        else:
            instance.save()
        return instance


class SpeciesAttackSerializer(serializers.ModelSerializer):
    attack = AttackSerializer()

    class Meta:
        model = SpeciesAttack
        fields = '__all__'


class SpeciesSerializer(serializers.ModelSerializer):
    speciesAttack = SpeciesAttackSerializer(many=True, read_only=True)

    class Meta:
        model = Species
        fields = '__all__'


class PokemonAttackSerializer(serializers.ModelSerializer):
    attack = AttackSerializer()

    class Meta:
        model = PokemonAttack
        fields = '__all__'


class PokemonAttackSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = PokemonAttack
        fields = '__all__'


class PokemonSerializer(serializers.ModelSerializer):
    pokemonAttack = PokemonAttackSerializer(many=True, read_only=True)
    species = SpeciesSerializer()

    class Meta:
        model = Pokemon
        fields = '__all__'


class PokemonSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = Pokemon
        fields = '__all__'

    def update(self, instance, validated_data):
        instance.experience = validated_data.get('experience', instance.experience)
        while instance.experience >= instance.exp_to_level(instance.level + 1):
            instance.level += 1
        instance.name = validated_data.get('name', instance.name)
        instance.trainer = validated_data.get('trainer', instance.trainer)
        instance.species = validated_data.get('species', instance.species)
        instance.nature = validated_data.get('nature', instance.nature)
        instance.inParty = validated_data.get('inParty', instance.inParty)
        instance.constitution = validated_data.get('constitution', instance.constitution)
        instance.attack = validated_data.get('attack', instance.attack)
        instance.defense = validated_data.get('defense', instance.defense)
        instance.special_attack = validated_data.get('special_attack', instance.special_attack)
        instance.special_defense = validated_data.get('special_defense', instance.special_defense)
        instance.speed = validated_data.get('speed', instance.speed)
        instance.current_hp = validated_data.get('current_hp', instance.current_hp)
        instance.save()
        return instance

    def create(self, validated_data):
        pokemon = Pokemon()

        pokemon.name = validated_data['name']
        pokemon.species = validated_data['species']
        pokemon.nature = random.randint(0, 35)
        pokemon.level = validated_data['level']
        pokemon.game = validated_data['game']
        pokemon.experience = pokemon.exp_to_level(pokemon.level)

        pokemon.constitution = 0
        pokemon.attack = 0
        pokemon.defense = 0
        pokemon.special_attack = 0
        pokemon.special_defense = 0
        pokemon.speed = 0
        pokemon.current_hp = 0

        species = pokemon.species
        base_stat_total = species.base_constitution + species.base_attack + species.base_defense + \
            species.base_special_attack + species.base_special_defense + species.base_speed

        for i in range(10 + pokemon.level):
            stat = random.randint(0, base_stat_total)
            if stat < species.base_constitution:
                pokemon.constitution = pokemon.constitution + 1
                continue
            stat = stat - species.base_constitution
            if stat < species.base_attack:
                pokemon.attack = pokemon.attack + 1
                continue
            stat = stat - species.base_attack
            if stat < species.base_defense:
                pokemon.defense = pokemon.defense + 1
                continue
            stat = stat - species.base_defense
            if stat < species.base_special_attack:
                pokemon.special_attack = pokemon.special_attack + 1
                continue
            stat = stat - species.base_special_attack
            if stat < species.base_special_defense:
                pokemon.special_defense = pokemon.special_defense + 1
                continue
            pokemon.speed = pokemon.speed + 1

        pokemon.save()

        species_attacks = SpeciesAttack.objects.filter(species=pokemon.species,
                                                       level__lte=pokemon.level).select_related('attack').order_by(
            '-level')[:6]
        for speciesAttack in species_attacks:
            pokemon_attack = PokemonAttack()
            pokemon_attack.attack = speciesAttack.attack
            pokemon_attack.pokemon = pokemon
            pokemon_attack.save()

        return pokemon


class TrainerAttackSerializer(serializers.ModelSerializer):
    attack = AttackSerializer()

    class Meta:
        model = TrainerAttack
        fields = '__all__'


class TrainerAttackSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = TrainerAttack
        fields = '__all__'


class TrainerEdgeSerializer(serializers.ModelSerializer):
    edge = EdgeSerializer()

    class Meta:
        model = TrainerEdge
        fields = '__all__'


class TrainerEdgeSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = TrainerEdge
        fields = '__all__'


class TrainerFeatureSerializer(serializers.ModelSerializer):
    feature = FeatureSerializer()

    class Meta:
        model = TrainerFeature
        fields = '__all__'


class TrainerFeatureSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = TrainerFeature
        fields = '__all__'

    def create(self, validated_data):
        trainer_feature = TrainerFeature.objects.create(
            trainer=validated_data['trainer'],
            feature=validated_data['feature']
        )
        trainer_feature.save()
        if '+' in trainer_feature.feature.tags:
            tags_split = trainer_feature.feature.tags.split(',')
            for tag in tags_split:
                m = re.search('(?<=\+)\w+', tag)
                if m is not None:
                    attribute = m.group(0)
                    attribute_value = getattr(trainer_feature.trainer, attribute, 0)
                    attribute_value += 1
                    setattr(trainer_feature.trainer, attribute, attribute_value)
                    trainer_feature.trainer.save()

        return trainer_feature


class TrainerSerializer(serializers.ModelSerializer):
    pokemon = PokemonSerializer(many=True, read_only=True)
    trainerEdge = TrainerEdgeSerializer(many=True, read_only=True)
    trainerFeature = TrainerFeatureSerializer(many=True, read_only=True)
    item = TrainerItemSerializer(many=True, read_only=True)
    trainerAttack = TrainerAttackSerializer(many=True, read_only=True)

    class Meta:
        model = Trainer
        fields = '__all__'


class TrainerSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = Trainer
        fields = ['id', 'name']


class GameSerializer(serializers.ModelSerializer):
    trainer = TrainerSerializer(many=True, read_only=True)
    pokemon = PokemonSerializer(many=True, read_only=True)

    class Meta:
        model = Game
        fields = '__all__'


class GameSimpleSerializer(serializers.ModelSerializer):
    class Meta:
        model = Game
        fields = ['id', 'title']


class UserInfoSerializer(serializers.Serializer):
    id = serializers.IntegerField()
    username = serializers.CharField()
    gm = serializers.SerializerMethodField()
    item_id = serializers.SerializerMethodField()
    game_name = serializers.SerializerMethodField()

    _gm = None
    _item_name = None
    _game_name = None

    def retrieve_data(self, obj):
        self._user_id = obj.id
        if Game.objects.filter(gm=obj).exists():
            self._gm = True
            game = Game.objects.get(gm=obj)
            self._item_id = game.id
            self._game_name = game.title
        else:
            self._gm = False
            if Trainer.objects.filter(user=obj).exists():
                trainer = Trainer.objects.get(user=obj)
                self._item_id = trainer.id
                self._game_name = trainer.game.title
            else:
                self._item_id = 0
                self._game_name = ''

    def get_gm(self, obj):
        if self._gm is None:
            self.retrieve_data(obj)
        return self._gm

    def get_item_id(self, obj):
        if self._item_id is None:
            self.retrieve_data(obj)
        return self._item_id

    def get_game_name(self, obj):
        if self._game_name is None:
            self.retrieve_data(obj)
        return self._game_name
