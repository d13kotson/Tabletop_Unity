from django.db import models

from django.contrib.auth.models import User

from math import floor


class Image(models.Model):
    name = models.CharField(max_length=200)
    path = models.CharField(max_length=200)
    height = models.IntegerField()
    width = models.IntegerField()

    def __str__(self):
        return self.name


class Background(models.Model):
    gm = models.ForeignKey(User, on_delete=models.CASCADE)
    title = models.CharField(max_length=20)
    image = models.ForeignKey(Image, on_delete=models.CASCADE)

    def __str__(self):
        return self.title


class Token(models.Model):
    user = models.ForeignKey(User, related_name='gm', on_delete=models.CASCADE)
    title = models.CharField(max_length=20)
    image = models.ForeignKey(Image, on_delete=models.CASCADE)

    def __str__(self):
        return self.title


class Game(models.Model):
    title = models.CharField(max_length=20)
    gm = models.ForeignKey(User, related_name='game', on_delete=models.CASCADE)

    def __str__(self):
        return self.title


class Trainer(models.Model):
    name = models.CharField(max_length=20)
    game = models.ForeignKey(Game, related_name='trainer', on_delete=models.CASCADE)
    user = models.ForeignKey(User, related_name='trainer', on_delete=models.CASCADE)
    token = models.ForeignKey(Token, related_name='trainer', on_delete=models.CASCADE, null=True, blank=True)
    level = models.IntegerField(default=1)
    money = models.IntegerField(default=5000)
    acrobatics = models.IntegerField(default=2)
    athletics = models.IntegerField(default=2)
    combat = models.IntegerField(default=2)
    intimidate = models.IntegerField(default=2)
    stealth = models.IntegerField(default=2)
    survival = models.IntegerField(default=2)
    gen_education = models.IntegerField(default=2)
    med_education = models.IntegerField(default=2)
    occ_education = models.IntegerField(default=2)
    pok_education = models.IntegerField(default=2)
    tec_education = models.IntegerField(default=2)
    guile = models.IntegerField(default=2)
    perception = models.IntegerField(default=2)
    charm = models.IntegerField(default=2)
    command = models.IntegerField(default=2)
    focus = models.IntegerField(default=2)
    intuition = models.IntegerField(default=2)
    constitution = models.IntegerField(default=10)
    attack = models.IntegerField(default=5)
    defense = models.IntegerField(default=5)
    special_attack = models.IntegerField(default=5)
    special_defense = models.IntegerField(default=5)
    speed = models.IntegerField(default=5)
    attack_cs = models.IntegerField(default=0)
    defense_cs = models.IntegerField(default=0)
    special_attack_cs = models.IntegerField(default=0)
    special_defense_cs = models.IntegerField(default=0)
    speed_cs = models.IntegerField(default=0)
    current_hp = models.IntegerField(default=0)
    current_ap = models.IntegerField(default=5)

    def __str__(self):
        return self.name

    def total_stat(self, stat, cs):
        if stat > 0:
            modifier = (cs + 2) / 2
        elif stat < 0:
            modifier = 2 / (2 - cs)
        else:
            modifier = 1
        return stat * modifier

    @property
    def total_attack(self):
        return self.total_stat(self.attack, self.attack_cs)

    @property
    def total_defense(self):
        return self.total_stat(self.defense, self.defense_cs)

    @property
    def total_special_attack(self):
        return self.total_stat(self.special_attack, self.special_attack_cs)

    @property
    def total_special_defense(self):
        return self.total_stat(self.special_defense, self.special_defense_cs)

    @property
    def total_speed(self):
        return self.total_stat(self.speed, self.speed_cs)


class Note(models.Model):
    note = models.TextField(max_length=400)
    trainer = models.ForeignKey(Trainer, related_name='note', on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} Note: {self.id}'


class Item(models.Model):
    name = models.CharField(max_length=50)
    price = models.IntegerField(default=0)
    roll_text = models.CharField(max_length=50, null=True, blank=True)

    def __str__(self):
        return str(self.name)


class TrainerItem(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='item', on_delete=models.CASCADE)
    item = models.ForeignKey(Item, on_delete=models.CASCADE, null=True, blank=True)
    item_name = models.CharField(max_length=50, null=True, blank=True)
    number = models.IntegerField(default=0)

    def __str__(self):
        if self.item is None:
            return f'{str(self.trainer)} {str(self.item_name)}'
        else:
            return f'{str(self.trainer)} {str(self.item.name)}'


class Type(models.Model):
    name = models.CharField(max_length=20)

    def __str__(self):
        return self.name


class TypeEffectiveness(models.Model):
    attack = models.ForeignKey(Type, on_delete=models.CASCADE, null=True, blank=True, related_name='attack_type')
    defend = models.ForeignKey(Type, on_delete=models.CASCADE, null=True, blank=True, related_name='defend_type')
    value = models.FloatField(default=1)

    def __str__(self):
        return f'{self.attack} > {self.defend}'


class Species(models.Model):
    name = models.CharField(max_length=20)
    dex_num = models.IntegerField(unique=True)
    type_1 = models.ForeignKey(Type, on_delete=models.CASCADE, null=True, blank=True, related_name='type_1')
    type_2 = models.ForeignKey(Type, on_delete=models.CASCADE, null=True, blank=True, related_name='type_2')
    base_constitution = models.IntegerField()
    base_attack = models.IntegerField()
    base_defense = models.IntegerField()
    base_special_attack = models.IntegerField()
    base_special_defense = models.IntegerField()
    base_speed = models.IntegerField()
    acrobatics = models.CharField(max_length=10, blank=True)
    athletics = models.CharField(max_length=10, blank=True)
    combat = models.CharField(max_length=10, blank=True)
    intimidate = models.CharField(max_length=10, blank=True)
    stealth = models.CharField(max_length=10, blank=True)
    survival = models.CharField(max_length=10, blank=True)
    gen_education = models.CharField(max_length=10, blank=True)
    med_education = models.CharField(max_length=10, blank=True)
    occ_education = models.CharField(max_length=10, blank=True)
    pok_education = models.CharField(max_length=10, blank=True)
    tec_education = models.CharField(max_length=10, blank=True)
    guile = models.CharField(max_length=10, blank=True)
    perception = models.CharField(max_length=10, blank=True)
    charm = models.CharField(max_length=10, blank=True)
    command = models.CharField(max_length=10, blank=True)
    focus = models.CharField(max_length=10, blank=True)
    intuition = models.CharField(max_length=10, blank=True)
    overland = models.CharField(max_length=10, blank=True)
    swimming = models.CharField(max_length=10, blank=True)
    burrow = models.CharField(max_length=10, blank=True)
    sky = models.CharField(max_length=10, blank=True)
    levitate = models.CharField(max_length=10, blank=True)
    teleport = models.CharField(max_length=10, blank=True)
    tm_moves = models.TextField(blank=True, null=True)
    egg_moves = models.TextField(blank=True, null=True)
    tutor_moves = models.TextField(blank=True, null=True)
    basic_abilities = models.CharField(max_length=200, blank=True, null=True)
    adv_abilities = models.CharField(max_length=200, blank=True, null=True)
    high_ability = models.CharField(max_length=100, blank=True, null=True)
    size = models.CharField(max_length=50)
    weight = models.IntegerField()
    egg_groups = models.CharField(max_length=200, blank=True, null=True)


    def __str__(self):
        return self.name


class Evolution(models.Model):
    base = models.ForeignKey(Species, related_name="base", on_delete=models.CASCADE)
    evolved = models.ForeignKey(Species, related_name="evolved", on_delete=models.CASCADE)
    requirements = models.CharField(max_length=200)

    def __str__(self):
        return self.evolved.name


class DamageBase(models.Model):
    num_die = models.IntegerField(default=1)
    die_num = models.IntegerField(default=1)
    add = models.IntegerField(default=0)

    def __str__(self):
        return f'Damage Base {self.id}'


class Attack(models.Model):
    name = models.CharField(max_length=20)
    type = models.ForeignKey(Type, on_delete=models.CASCADE, null=True, blank=True, related_name='attack')
    frequency = models.CharField(max_length=20)
    ac = models.IntegerField()
    damage_base = models.ForeignKey(DamageBase, on_delete=models.CASCADE, null=True, blank=True)
    attack_class = models.CharField(max_length=20)
    range = models.CharField(max_length=60)
    effect = models.TextField(max_length=400)
    game = models.ForeignKey(Game, on_delete=models.CASCADE, null=True, blank=True)

    def __str__(self):
        return self.name


class SpeciesAttack(models.Model):
    species = models.ForeignKey(Species, related_name='species_attack', on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)
    level = models.IntegerField()

    def __str__(self):
        return f'{str(self.species)} {str(self.attack)}'


class Nature(models.Model):
    name = models.CharField(max_length=20)
    constitution = models.IntegerField()
    attack = models.IntegerField()
    defense = models.IntegerField()
    special_attack = models.IntegerField()
    special_defense = models.IntegerField()
    speed = models.IntegerField()

    def __str__(self):
        return self.name


class Pokemon(models.Model):
    name = models.CharField(max_length=20)
    trainer = models.ForeignKey(Trainer, null=True, related_name='pokemon', on_delete=models.CASCADE)
    game = models.ForeignKey(Game, null=True, related_name='pokemon', on_delete=models.CASCADE)
    species = models.ForeignKey(Species, on_delete=models.CASCADE)
    nature = models.ForeignKey(Nature, on_delete=models.CASCADE)
    level = models.IntegerField(default=1)
    experience = models.IntegerField(default=0)
    in_party = models.BooleanField(default=False)
    constitution = models.IntegerField()
    attack = models.IntegerField()
    defense = models.IntegerField()
    special_attack = models.IntegerField()
    special_defense = models.IntegerField()
    speed = models.IntegerField()
    attack_cs = models.IntegerField(default=0)
    defense_cs = models.IntegerField(default=0)
    special_attack_cs = models.IntegerField(default=0)
    special_defense_cs = models.IntegerField(default=0)
    speed_cs = models.IntegerField(default=0)
    current_hp = models.IntegerField(default=0)
    ability = models.CharField(max_length=40, null=True, blank=True)
    token = models.ForeignKey(Token, on_delete=models.CASCADE, null=True, blank=True)

    def __str__(self):
        return self.name

    def get_cs_mult(self, cs):
        if cs <= -6:
            return 0.25
        elif cs == -5:
            return 2.0/7.0
        elif cs == -4:
            return 2.0/6.0
        elif cs == -3:
            return 0.4
        elif cs == -2:
            return .5
        elif cs == -1:
            return 2.0/3.0
        elif cs == 0:
            return 1
        elif cs == 1:
            return 1.5
        elif cs == 2:
            return 2
        elif cs == 3:
            return 2.5
        elif cs == 4:
            return 3
        elif cs == 5:
            return 3.5
        elif cs >= 6:
            return 4

    @property
    def total_constitution(self):
        return self.constitution + self.species.base_constitution + self.nature.constitution

    @property
    def total_attack(self):
        cs_mult = self.get_cs_mult(self.attack_cs)
        return (self.attack + self.species.base_attack + self.nature.attack) * cs_mult

    @property
    def total_defense(self):
        cs_mult = self.get_cs_mult(self.defense_cs)
        return (self.defense + self.species.base_defense + self.nature.defense) * cs_mult

    @property
    def total_special_attack(self):
        cs_mult = self.get_cs_mult(self.special_attack_cs)
        return (self.special_attack + self.species.base_special_attack + self.nature.special_attack) * cs_mult

    @property
    def total_special_defense(self):
        cs_mult = self.get_cs_mult(self.special_defense_cs)
        return (self.special_defense + self.species.base_special_defense + self.nature.special_defense) * cs_mult

    @property
    def total_speed(self):
        cs_mult = self.get_cs_mult(self.speed_cs)
        return (self.speed + self.species.base_speed + self.nature.speed) * cs_mult

    def exp_to_level(self, level):
        factor = floor(level / 10)
        remainder = level % 10
        experience = 0
        for i in range(factor):
            experience += 100 * (2 ** i)
        for i in range(remainder):
            experience += 10 * (2 ** factor)
        return experience


class PokemonAttack(models.Model):
    pokemon = models.ForeignKey(Pokemon, related_name='pokemon_attack', on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.pokemon)} {str(self.attack)}'


class TrainerAttack(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='trainer_attack', on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} {str(self.attack)}'


class Edge(models.Model):
    name = models.TextField(max_length=200)
    notes = models.CharField(max_length=100, null=True)
    prerequisites = models.CharField(max_length=100, null=True)
    effect = models.CharField(max_length=400)

    def __str__(self):
        if self.notes is not None:
            return f'{self.name} {self.notes}'
        else:
            return self.name


class EdgeAttack(models.Model):
    edge = models.ForeignKey(Edge, on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.edge)} {str(self.attack)}'


class Feature(models.Model):
    name = models.TextField(max_length=100)
    notes = models.CharField(max_length=100, null=True, blank=True)
    tags = models.CharField(max_length=100, null=True, blank=True)
    prerequisites = models.CharField(max_length=100, null=True, blank=True)
    frequency = models.CharField(max_length=100, null=True, blank=True)
    trigger = models.TextField(max_length=200, null=True, blank=True)
    effect = models.TextField(max_length=1000)
    game = models.ForeignKey(Game, on_delete=models.CASCADE, null=True, blank=True)

    def __str__(self):
        if self.notes is not None:
            return f'{self.name} {self.notes}'
        else:
            return self.name


class FeatureAttack(models.Model):
    feature = models.ForeignKey(Feature, on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.edge)} {str(self.attack)}'


class TrainerEdge(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='trainer_edge', on_delete=models.CASCADE)
    edge = models.ForeignKey(Edge, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} {str(self.edge)}'


class TrainerFeature(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='trainer_feature', on_delete=models.CASCADE)
    feature = models.ForeignKey(Feature, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} {str(self.feature)}'


class Message(models.Model):
    game = models.ForeignKey(Game, on_delete=models.CASCADE)
    message = models.CharField(max_length=200)
    display_name = models.CharField(max_length=50)

    def __str__(self):
        return self.message
