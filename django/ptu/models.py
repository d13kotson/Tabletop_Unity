from django.db import models

from django.contrib.auth.models import User

from math import floor


class Game(models.Model):
    title = models.CharField(max_length=20)
    gm = models.ForeignKey(User, on_delete=models.CASCADE)

    def __str__(self):
        return self.title


class Trainer(models.Model):
    name = models.CharField(max_length=20)
    game = models.ForeignKey(Game, related_name='trainer', on_delete=models.CASCADE)
    user = models.ForeignKey(User, on_delete=models.CASCADE)
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
    current_hp = models.IntegerField(default=0)
    current_ap = models.IntegerField(default=5)

    def __str__(self):
        return self.name


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


class Species(models.Model):
    name = models.CharField(max_length=20)
    dexNum = models.IntegerField(unique=True)
    type = models.CharField(max_length=20)
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

    def __str__(self):
        return self.name


class Evolution(models.Model):
    base = models.ForeignKey(Species, related_name="base", on_delete=models.CASCADE)
    evolved = models.ForeignKey(Species, related_name="evolved", on_delete=models.CASCADE)
    level = models.IntegerField()

    def __str__(self):
        return self.evolved.name


class Attack(models.Model):
    name = models.CharField(max_length=20)
    type = models.CharField(max_length=20)
    frequency = models.CharField(max_length=20)
    ac = models.IntegerField()
    damageBase = models.IntegerField()
    attackClass = models.CharField(max_length=20)
    range = models.CharField(max_length=60)
    effect = models.TextField(max_length=400)
    game = models.ForeignKey(Game, on_delete=models.CASCADE, null=True)

    def __str__(self):
        return self.name


class SpeciesAttack(models.Model):
    species = models.ForeignKey(Species, related_name='speciesAttack', on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)
    level = models.IntegerField()

    def __str__(self):
        return f'{str(self.species)} {str(self.attack)}'


class Pokemon(models.Model):
    name = models.CharField(max_length=20)
    trainer = models.ForeignKey(Trainer, null=True, related_name='pokemon', on_delete=models.CASCADE)
    game = models.ForeignKey(Game, null=True, related_name='pokemon', on_delete=models.CASCADE)
    species = models.ForeignKey(Species, on_delete=models.CASCADE)
    nature = models.IntegerField()
    level = models.IntegerField(default=1)
    experience = models.IntegerField(default=0)
    inParty = models.BooleanField(default=False)
    constitution = models.IntegerField()
    attack = models.IntegerField()
    defense = models.IntegerField()
    special_attack = models.IntegerField()
    special_defense = models.IntegerField()
    speed = models.IntegerField()
    current_hp = models.IntegerField(default=0)
    ability = models.CharField(max_length=40, null=True, blank=True)

    def __str__(self):
        return self.name

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
    pokemon = models.ForeignKey(Pokemon, related_name='pokemonAttack', on_delete=models.CASCADE)
    attack = models.ForeignKey(Attack, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.pokemon)} {str(self.attack)}'


class TrainerAttack(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='trainerAttack', on_delete=models.CASCADE)
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
    trainer = models.ForeignKey(Trainer, related_name='trainerEdge', on_delete=models.CASCADE)
    edge = models.ForeignKey(Edge, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} {str(self.edge)}'


class TrainerFeature(models.Model):
    trainer = models.ForeignKey(Trainer, related_name='trainerFeature', on_delete=models.CASCADE)
    feature = models.ForeignKey(Feature, on_delete=models.CASCADE)

    def __str__(self):
        return f'{str(self.trainer)} {str(self.feature)}'
