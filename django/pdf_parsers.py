import PyPDF2
import re

def parse_dex(pdf_file):
    pages = list()
    with open(pdf_file, 'rb') as file:
        file_reader = PyPDF2.PdfFileReader(file)
        for i in range(11, file_reader.getNumPages() - 2):
            pages.append(file_reader.getPage(i).extractText())
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
