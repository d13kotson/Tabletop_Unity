import json
import time

import PyPDF2
import re

import requests
from lxml import html

import django
import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'tabletop.local_pycharm_settings'
django.setup()
from ptu.models import Attack

def parse_dex(pdf_file):
    pages = list()
    with open(pdf_file, 'rb') as file:
        file_reader = PyPDF2.PdfFileReader(file)
        for i in range(11, file_reader.getNumPages() - 2):
            text = file_reader.getPage(i).extractText()
            text = re.sub('˚', 'fi', text)
            pages.append(text)
    stripped_pages = [re.sub('\s+', ' ', page) for page in pages]
    split_pages = [page.split(' ') for page in stripped_pages]
    pokemon = list()
    for page in split_pages:
        type1 = None
        type2 = None
        basic_abilities = list()
        advanced_abilities = list()
        high_abilities = list()
        for i in range(15, len(page)):
            if page[i] == 'Type' and page[i + 1] == ':':
                type1 = page[i + 2]
                if page[i + 3] == '/':
                    type2 = page[i + 4]
            elif page[i] == 'Basic' and page[i + 1] == 'Ability':
                ability = page[i + 3]
                curr = i + 4
                while page[curr + 1] != 'Ability':
                    ability = f'{ability} {page[curr]}'
                    curr += 1
                basic_abilities.append(ability)
            elif page[i] == 'Adv' and page[i + 1] == 'Ability':
                ability = page[i + 3]
                curr = i + 4
                while not (page[curr + 1] == 'Ability:' or page[curr + 1] == 'Ability'):
                    ability = f'{ability} {page[curr]}'
                    curr += 1
                advanced_abilities.append(ability)
            elif page[i] == 'High' and page[i + 1] == 'Ability:':
                ability = page[i + 2]
                curr = i + 3
                while page[curr] != 'Evolution:':
                    ability = f'{ability} {page[curr]}'
                    curr += 1
                high_abilities.append(ability)
        pokemon.append({
            'name': re.sub('\d+', '', page[0]),
            'constitution': page[4],
            'attack': page[6],
            'defense': page[8],
            'special_attack': page[11],
            'special_defense': page[14],
            'speed': page[16],
            'type1': type1,
            'type2': type2,
            'basic_abilities': basic_abilities,
            'advanced_abilities': advanced_abilities,
            'high_abilities': high_abilities
        })
    return pokemon


def parse_basic_info(basic_info):
    type_1 = None
    type_2 = None
    basic_abilities = list()
    advanced_abilities = list()
    high_ability = None

    lines = basic_info.strip('\n').split('\n')
    for line in lines:
        text = re.sub(r'\s+', ' ', line).strip(' ')
        if text.startswith('Type'):
            start = text.index(':') + 1
            text = text[start:]
            if '/' in text:
                split = text.split('/')
                type_1 = re.sub(r'\s+', '', split[0].strip(' '))
                type_2 = re.sub(r'\s+', '', split[1].strip(' '))
            else:
                type_1 = re.sub(r'\s+', '', text)
        elif text.startswith('Basic'):
            start = text.index(':') + 1
            text = text[start:].strip(' ')
            basic_abilities.append(text)
        elif text.startswith('Adv'):
            start = text.index(':') + 1
            text = text[start:].strip(' ')
            advanced_abilities.append(text)
        elif text.startswith('High'):
            start = text.index(':') + 1
            text = text[start:].strip(' ')
            high_ability = text
    return type_1, type_2, basic_abilities, advanced_abilities, high_ability


def parse_evolution(evolution, poke_name, dex_num):
    evolutions = list()
    current_evos = list()
    current_stage = 1

    lines = evolution.strip(' ').strip('\n').split('\n')
    for line in lines:
        text = re.sub(r'\s+', ' ', line).strip(' ')
        split = text.split('-')
        stage = int(re.sub(r'\s+', '', split[0]))
        evolution = split[1].strip(' ')
        split = evolution.split(' ')
        name = split[0]
        requirments = ' '.join(split[1:])
        base = dex_num[poke_name]
        evolved = dex_num[name]
        evolutions.append((name, stage, requirments, base, evolved))
        if name.lower() == poke_name.lower():
            current_stage = stage
    for evolution in evolutions:
        if evolution[1] == current_stage + 1:
            current_evos.append(evolution)
    return current_evos


def parse_breeding(breeding_info):
    lines = breeding_info.split('\n')
    egg_groups = ''
    for line in lines:
        if 'Egg Group' in line:
            egg_groups = line.split(':')[1].strip()
    return egg_groups


def parse_capabilites(capabilities_info):
    movement = {
        'overland': '',
        'swimming': '',
        'burrow': '',
        'sky': '',
        'levitate': '',
        'teleport': ''
    }
    capabilities = capabilities_info.split(',')
    for capability in capabilities:
        info = capability.strip()
        if info.startswith('Overland'):
            movement['overland'] = info.split(' ')[1]
        elif info.startswith('Swim'):
            movement['swimming'] = info.split(' ')[1]
        elif info.startswith('Burrow'):
            movement['burrow'] = info.split(' ')[1]
        elif info.startswith('Sky'):
            movement['sky'] = info.split(' ')[1]
        elif info.startswith('Levitate'):
            movement['levitate'] = info.split(' ')[1]
        elif info.startswith('Teleport'):
            movement['teleport'] = info.split(' ')[1]
    return movement


def parse_skills(skills_info):
    skills = {
        'acrobatics': '',
        'athletics': '',
        'combat': '',
        'intimidate': '',
        'stealth': '',
        'survival': '',
        'gen_education': '',
        'med_education': '',
        'occ_education': '',
        'pok_education': '',
        'tec_education': '',
        'guile': '',
        'perception': '',
        'charm': '',
        'command': '',
        'focus': '',
        'intuition': ''
    }
    for skill in re.sub(r'[\n\-]', '', skills_info).split(','):
        info = skill.strip()
        if info.startswith('Acro'):
            skills['acrobatics'] = info.split(' ')[1]
        elif info.startswith('Athl'):
            skills['athletics'] = info.split(' ')[1]
        elif info.startswith('Combat'):
            skills['combat'] = info.split(' ')[1]
        elif info.startswith('Intimidate'):
            skills['intimidate'] = info.split(' ')[1]
        elif info.startswith('Stealth'):
            skills['stealth'] = info.split(' ')[1]
        elif info.startswith('Survival'):
            skills['survival'] = info.split(' ')[1]
        elif info.startswith('Edu: Gen'):
            skills['gen_education'] = info.split(' ')[1]
        elif info.startswith('Edu: Med'):
            skills['med_education'] = info.split(' ')[1]
        elif info.startswith('Edu: Occ'):
            skills['occ_education'] = info.split(' ')[1]
        elif info.startswith('Edu: Poke'):
            skills['pok_education'] = info.split(' ')[1]
        elif info.startswith('Edu: Tech'):
            skills['tec_education'] = info.split(' ')[1]
        elif info.startswith('Guile'):
            skills['guile'] = info.split(' ')[1]
        elif info.startswith('Percep'):
            skills['perception'] = info.split(' ')[1]
        elif info.startswith('Charm'):
            skills['charm'] = info.split(' ')[1]
        elif info.startswith('Command'):
            skills['command'] = info.split(' ')[1]
        elif info.startswith('Focus'):
            skills['focus'] = info.split(' ')[1]
        elif info.startswith('Intuition'):
            skills['intuition'] = info.split(' ')[1]
    return skills


def parse_move_list(move_info):
    moves = move_info.split('\n')
    move_list = list()
    for move in moves:
        move = move.strip()
        if move != '':
            info = move.split(' - ')[0]
            match = re.search(r'\d+', info)
            level = match.group(0)
            name = info[match.end() + 1:]
            move_list.append((level, name))
    return move_list


def parse_dex_2(txt_file='dex.json', output_file_suffix='_base'):
    species = list()
    species_attacks = list()
    with open('dex_num.json') as dex_file:
        dex = json.loads(dex_file.read())
    with open('attacks_dict.json') as attacks_file:
        attacks = json.loads(attacks_file.read())
    types = {
        'Bug': 1,
        'Dark': 2,
        'Dragon': 3,
        'Electric': 4,
        'Fairy': 5,
        'Fighting': 6,
        'Fire': 7,
        'Flying': 8,
        'Ghost': 9,
        'Grass': 10,
        'Ground': 11,
        'Ice': 12,
        'Normal': 13,
        'Poison': 14,
        'Psychic': 15,
        'Rock': 16,
        'Steel': 17,
        'Water': 18
    }
    pokemon_indices = dict()
    all_evolutions = dict()
    evolutions_list = list()
    images = list()
    tokens = list()
    with open(txt_file, 'r') as file:
        json_text = file.read()
        pages = json.loads(json_text)
    species_index = 1
    attack_index = 1
    for page in pages:
        name_split = re.split('Base Stats:', page)
        name = re.sub(r'[\s\d]+', '', name_split[0]).capitalize()
        if name == 'Pumpkaboo' or name == 'Gourgeist' or name == 'Rotomnormalform' or name == 'Rotomapplianceforms' or\
            name == 'Tornadusincarnateforme' or name == 'Tornadustherianforme' or name == 'Thundurusincarnateforme' or\
            name == 'Thundurustherianforme' or name == 'Landorusincarnateforme' or name == 'Landorustherianforme' or\
            name == 'Shayminlandforme' or name == 'Shayminskyforme' or name == 'Meloettaariaform' or\
            name == 'Meloettastepform' or name.startswith('Deoxys') or name.startswith('Kyurem') or\
            name.startswith('Giratina') or name.startswith('Hoopa') or name.startswith('Porygon') or 'mime' in name or 'Mime' in name:
            continue
        base_stats_split = re.split('Basic Information', name_split[1])
        base_stats = base_stats_split[0]
        base_stats = re.sub(r'\s+', ' ', base_stats).strip(' ')
        stats = re.split(r'[A-Za-z ]+: ', base_stats)[1:]
        basic_information_split = re.split('Evolution:', base_stats_split[1])
        basic_information = basic_information_split[0]
        type_1, type_2, basic_abilities, adv_abilities, high_ability = parse_basic_info(basic_information)
        evolution_split = re.split('Size Information', basic_information_split[1])
        evolution = evolution_split[0]
        evolutions = parse_evolution(evolution, name, dex)
        size_split = re.split('Breeding Information', evolution_split[1])
        size_info = re.sub(r'\s+', ' ', size_split[0])
        size_match = re.search(r'\([A-Za-z]+\)', size_info)
        size = size_match.group(0)[1:-1]
        weight_match = re.search(r'\([0-9]+\)', size_info)
        weight = weight_match.group(0)[1:-1]
        breeding_split = re.split('Capability List', size_split[1])
        egg_groups = parse_breeding(breeding_split[0])
        capabilities_split = re.split('Skill List', breeding_split[1])
        capabilities_info = re.sub('\n', '', capabilities_split[0])
        movement = parse_capabilites(capabilities_info)
        skills_split = re.split('Move List', capabilities_split[1])
        skills_info = re.sub(r'[\n\-]', '', skills_split[0])
        skills = parse_skills(skills_info)
        move_groups = list()
        group_indices = list()
        results = re.finditer(r'\w.*Move List', page)
        index = 0
        for result in results:
            move_groups.append(result.group(0))
            group_indices.append([result.start(0)])
            if index > 0:
                group_indices[index - 1].append(result.start(0))
            index += 1
        move_info = dict()
        for i in range(len(move_groups)):
            if len(group_indices[i]) == 2:
                move_info[move_groups[i]] = page[group_indices[i][0] + len(move_groups[i]):group_indices[i][1]]
            else:
                move_info[move_groups[i]] = page[group_indices[i][0] + len(move_groups[i]):]
        move_list = list()
        if 'Level Up Move List' in move_info:
            move_list = parse_move_list(move_info['Level Up Move List'])
        tm_list = ''
        if 'TM/HM Move List' in move_info:
            tm_list = re.sub(r'\s+', ' ', move_info['TM/HM Move List']).strip()
        egg_list = ''
        if 'Egg Move List' in move_info:
            egg_list = re.sub(r'\s+', ' ', move_info['Egg Move List']).strip()
        tutor_list = ''
        if 'Tutor Move List' in move_info:
            tutor_list = re.sub(r'\s+', ' ', move_info['Tutor Move List']).strip()

        species.append(f"""
  {{
    "model": "ptu.species",
    "pk": {dex[name]},
    "fields": {{
        "name": "{name}",
        "dex_num": {dex[name]},
        "type_1": {types[type_1] if type_1 is not None else "null"},
        "type_2": {types[type_2] if type_2 is not None else "null"},
        "base_constitution": {stats[0]},
        "base_attack": {stats[1]},
        "base_defense": {stats[2]},
        "base_special_attack": {stats[3]},
        "base_special_defense": {stats[4]},
        "base_speed": {stats[5]},
        "acrobatics": "{skills['acrobatics']}",
        "athletics": "{skills['athletics']}",
        "combat": "{skills['combat']}",
        "intimidate": "{skills['intimidate']}",
        "stealth": "{skills['stealth']}",
        "survival": "{skills['survival']}",
        "gen_education": "{skills['gen_education']}",
        "med_education": "{skills['med_education']}",
        "occ_education": "{skills['occ_education']}",
        "pok_education": "{skills['pok_education']}",
        "tec_education": "{skills['tec_education']}",
        "guile": "{skills['guile']}",
        "perception": "{skills['perception']}",
        "charm": "{skills['charm']}",
        "command": "{skills['command']}",
        "focus": "{skills['focus']}",
        "intuition": "{skills['intuition']}",
        "overland": "{movement['overland']}",
        "swimming": "{movement['swimming']}",
        "burrow": "{movement['burrow']}",
        "sky": "{movement['sky']}",
        "levitate": "{movement['levitate']}",
        "teleport": "{movement['teleport']}",
        "tm_moves": "{tm_list}",
        "egg_moves": "{egg_list}",
        "tutor_moves": "{tutor_list}",
        "basic_abilities": "{', '.join(basic_abilities)}",
        "adv_abilities": "{', '.join(adv_abilities)}",
        "high_ability": "{high_ability}",
        "size": "{size}",
        "weight": {weight},
        "egg_groups": "{egg_groups}"
    }}
  }}""")
        image_width = 0
        if size == 'Small':
            image_width = 32
        elif size == 'Medium':
            image_width = 32
        elif size == 'Large':
            image_width = 64
        elif size == 'Huge':
            image_width = 96
        elif size == 'Gigantic':
            image_width = 128
        images.append(f"""
    {{
        "model": "ptu.image",
        "pk": {species_index},
        "fields": {{
            "name": "{name}",
            "path": "{str(dex[name]).zfill(3)}.png",
            "height": {image_width},
            "width": {image_width}
        }}
    }}""")
        pokemon_indices[name] = species_index
        all_evolutions[species_index] = evolutions
        for attack in move_list:
            species_attacks.append(f"""
        {{
        "model": "ptu.speciesattack",
        "pk": {attack_index},
        "fields": {{
            "species": {dex[name]},
            "attack": {attacks[attack[1].strip()]},
            "level": {attack[0]}
            }}
        }}""")
            attack_index += 1
        species_index += 1
    evolution_index = 1
    for key, value in all_evolutions.items():
        for evolution in value:
            evolutions_list.append(f"""
    {{
    "model": "ptu.evolution",
    "pk": {evolution_index},
    "fields": {{
        "base": {evolution[3]},
        "evolved": {evolution[4]},
        "requirements": "{evolution[2]}"
        }}
    }}""")
            evolution_index += 1

    with open(f'species{output_file_suffix}.json', 'w') as species_json:
        species_json.write('[\n')
        species_json.write(','.join(species))
        species_json.write(',\n')
        species_json.write(','.join(evolutions_list))
        species_json.write(',\n')
        species_json.write(','.join(species_attacks))
        species_json.write(']')

    with open(f'tokens{output_file_suffix}.json', 'w') as tokens_json:
        tokens_json.write('[\n')
        tokens_json.write(','.join(images))
        tokens_json.write(']')


def create_dex_json():
    dex = dict()
    for i in range(1, 810):
        print(f'Fetching Pokemon {i}')
        url = f'https://www.serebii.net/pokedex-sm/{str(i).zfill(3)}.shtml'
        page = requests.get(url)
        tree = html.fromstring(page.content)
        name = tree.xpath('//main/div[1]/div[1]/table[5]/tr[2]/td[1]')[0].text
        if name.isspace():
            name = tree.xpath('//main/div[1]/div[1]/table[4]/tr[2]/td[1]')[0].text
        print(name)
        dex[name] = i
    with open('dex_num.json', 'w') as file:
        file.write(json.dumps(dex))


def create_attacks_json():
    attacks = Attack.objects.all()
    attack_dict = {attack.name: attack.id for attack in attacks}
    with open('attacks_dict.json', 'w') as file:
        file.write(json.dumps(attack_dict))


def pdf_to_txt(pdf_file='C:/Users/Danny/Documents/Pokemon/PTU 1.05 + errata/PTU 1.05 + errata/dex.pdf', start_page=11, end_page=746, to_file='dex.json'):
    pages = list()
    with open(pdf_file, 'rb') as file:
        file_reader = PyPDF2.PdfFileReader(file)
        for i in range(start_page, end_page):
            if i != 681 and i != 607:
                text = file_reader.getPage(i).extractText()
                text = re.sub('˚', 'fi', text)
                pages.append(text)
    with open(to_file, 'w') as file:
        file.write(json.dumps(pages))


def alola_to_txt(pdf_file='C:/Users/Danny/Documents/Pokemon\PTU 1.05 + errata\PTU 1.05 + errata\Alola Dex\dex.pdf', start_page=3, end_page=117):
    pdf_to_txt(pdf_file, start_page, end_page, 'alola.json')
