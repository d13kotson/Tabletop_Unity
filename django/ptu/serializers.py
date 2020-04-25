from .models import *

from rest_framework import serializers

import random
import re


class TypeSerializer(serializers.ModelSerializer):
    class Meta:
        model = Type
        fields = '__all__'


class TypeEffectivenessSerializer(serializers.ModelSerializer):
    class Meta:
        model = TypeEffectiveness
        fields = '__all__'


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
    species_attack = SpeciesAttackSerializer(many=True, read_only=True)

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


class NatureSerializer(serializers.ModelSerializer):
    class Meta:
        model = Nature
        fields = '__all__'


class PokemonSerializer(serializers.ModelSerializer):
    pokemon_attack = PokemonAttackSerializer(many=True, read_only=True)
    species = SpeciesSerializer()
    nature = NatureSerializer()
    total_constitution = serializers.SerializerMethodField()
    total_attack = serializers.SerializerMethodField()
    total_defense = serializers.SerializerMethodField()
    total_special_attack = serializers.SerializerMethodField()
    total_special_defense = serializers.SerializerMethodField()
    total_speed = serializers.SerializerMethodField()
    
    def get_total_constitution(self, obj):
        return obj.total_constitution

    def get_total_attack(self, obj):
        return obj.total_attack

    def get_total_defense(self, obj):
        return obj.total_defense

    def get_total_special_attack(self, obj):
        return obj.total_special_attack

    def get_total_special_defense(self, obj):
        return obj.total_special_defense

    def get_total_speed(self, obj):
        return obj.total_speed

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
        instance.constitution_cs = validated_data.get('constitution_cs', instance.constitution_cs)
        instance.attack_cs = validated_data.get('attack_cs', instance.attack_cs)
        instance.defense_cs = validated_data.get('defense_cs', instance.defense_cs)
        instance.special_attack_cs = validated_data.get('special_attack_cs', instance.special_attack_cs)
        instance.special_defense_cs = validated_data.get('special_defense_cs', instance.special_defense_cs)
        instance.speed_cs = validated_data.get('speed_cs', instance.speed_cs)
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
    trainer_edge = TrainerEdgeSerializer(many=True, read_only=True)
    trainer_feature = TrainerFeatureSerializer(many=True, read_only=True)
    item = TrainerItemSerializer(many=True, read_only=True)
    trainer_attack = TrainerAttackSerializer(many=True, read_only=True)

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


class UserInfoSerializer(serializers.ModelSerializer):
    trainer = TrainerSimpleSerializer(many=True, read_only=True)
    game = GameSimpleSerializer(many=True, read_only=True)

    class Meta:
        model = User
        fields = ['id', 'username', 'trainer', 'game']


class MessageSerializer(serializers.ModelSerializer):
    class Meta:
        model = Message
        fields = '__all__'
