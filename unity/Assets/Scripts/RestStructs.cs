using System;
using System.Collections.Generic;
#pragma warning disable 0649

[Serializable]
internal struct TrainerName
{
    public int id;
    public string name;
}

[Serializable]
internal struct GameTitle
{
    public int id;
    public string title;
}

[Serializable]
internal struct Key
{
    public string key;
}

[Serializable]
internal struct UserInfo
{
    public int id;
    public string username;
    public TrainerName[] trainer;
    public GameTitle[] game;
}

public enum TokenType {
	trainer = 1,
	pokemon = 2
}

[Serializable]
internal struct MapToken
{
    public int tokenID;
    public int imageID;
    public float x;
    public float y;
    public int width;
    public int height;
	public TokenType tokenType;
	public int repID;
	public int owner;
}

[Serializable]
internal struct MapState
{
    public string background;
    public Dictionary<int, MapToken> tokens;
}

[Serializable]
internal struct UpdateToken
{
    public int tokenID;
    public float tokenX;
    public float tokenY;
}

[Serializable]
internal struct SocketMessage
{
    public string type;
    public string content;
}

[Serializable]
internal struct SpeciesAttack
{
    public int id;
    public Attack attack;
    public int level;
    public int species;
}

[Serializable]
internal struct SpeciesAttackList
{
    public SpeciesAttack[] list;
}

[Serializable]
internal struct Species
{
    public int id;
    public SpeciesAttack species_attack;
    public string name;
    public int dex_num;
    public string type;
    public int base_constitution;
    public int base_attack;
    public int base_defense;
    public int base_special_attack;
    public int base_special_defense;
    public int base_speed;
    public string acrobatics;
    public string athletics;
    public string combat;
    public string intimidate;
    public string stealth;
    public string survival;
    public string gen_education;
    public string med_education;
    public string occ_education;
    public string pok_education;
    public string tec_education;
    public string guile;
    public string perception;
    public string charm;
    public string command;
    public string focus;
    public string intuition;
    public string overland;
    public string swimming;
    public string burrow;
    public string sky;
    public string levitate;
    public string teleport;
}

[Serializable]
internal struct Type {
	public string name;
}

[Serializable]
internal struct Attack
{
    public int id;
    public string name;
    public Type type;
    public string frequency;
    public int ac;
    public int damage_base;
    public string attack_class;
    public string range;
    public string effect;
    public int game;
}

[Serializable]
internal struct PokemonAttack
{
    public int id;
    public Attack attack;
    public int pokemon;
}

[Serializable]
internal struct Nature
{
    public string name;
    public int constitution;
    public int attack;
    public int defense;
    public int special_attack;
    public int special_defense;
    public int speed;
}

[Serializable]
internal struct Pokemon
{
    public int id;
    public PokemonAttack[] pokemon_attack;
    public Species species;
    public string name;
    public Nature nature;
    public int level;
    public int experience;
    public bool in_party;
    public int constitution;
    public int attack;
    public int defense;
    public int special_attack;
    public int special_defense;
    public int attack_cs;
    public int defense_cs;
    public int special_attack_cs;
    public int special_defense_cs;
    public int speed_cs;
    public int speed;
    public int current_hp;
    public string ability;
    public int trainer;
    public int game;
    public TokenStruct token;
}

[Serializable]
internal struct PokemonSimple
{
    public int id;
    public int species;
    public string name;
    public int nature;
    public int level;
    public int experience;
    public bool in_party;
    public int constitution;
    public int attack;
    public int defense;
    public int special_attack;
    public int special_defense;
    public int speed;
    public int attack_cs;
    public int defense_cs;
    public int special_attack_cs;
    public int special_defense_cs;
    public int speed_cs;
    public int current_hp;
    public string ability;
    public int trainer;
    public int game;
    public int token;
}

[Serializable]
internal struct Edge
{
    public int id;
    public string name;
    public string notes;
    public string prerequisites;
    public string effect;
}

[Serializable]
internal struct Feature
{
    public int id;
    public string name;
    public string notes;
    public string tags;
    public string prerequisites;
    public string frequency;
    public string trigger;
    public string effect;
    public int game;
}

[Serializable]
internal struct TrainerEdge
{
    public int id;
    public Edge edge;
    public int trainer;
}

[Serializable]
internal struct TrainerFeature
{
    public int id;
    public Feature feature;
    public int trainer;
}

[Serializable]
internal struct ItemStruct
{
    public int id;
    public string name;
    public int price;
    public string roll_text;
}

[Serializable]
internal struct ItemList
{
    public ItemStruct[] list;
}

[Serializable]
internal struct TrainerItem
{
    public int id;
    public ItemStruct item;
    public string item_name;
    public int number;
    public int trainer;
}

[Serializable]
internal struct TrainerItemSimple
{
    public int id;
    public int item;
    public string item_name;
    public int number;
    public int trainer;
}

[Serializable]
internal struct TrainerAttack
{
    public int id;
    public Attack attack;
    public int trainer;
}

[Serializable]
internal struct Trainer
{
    public int id;
    public Pokemon[] pokemon;
    public TrainerEdge[] trainer_edge;
    public TrainerFeature[] trainer_feature;
    public TrainerItem[] item;
    public TrainerAttack[] trainer_attack;
    public TokenStruct token;
    public string name;
    public int level;
    public int money;
    public int acrobatics;
    public int athletics;
    public int combat;
    public int intimidate;
    public int stealth;
    public int survival;
    public int gen_education;
    public int med_education;
    public int occ_education;
    public int pok_education;
    public int tec_education;
    public int guile;
    public int perception;
    public int charm;
    public int command;
    public int focus;
    public int intuition;
    public int constitution;
    public int attack;
    public int defense;
    public int special_attack;
    public int special_defense;
    public int speed;
    public int attack_cs;
    public int defense_cs;
    public int special_attack_cs;
    public int special_defense_cs;
    public int speed_cs;
    public int current_hp;
    public int game;
    public int user;
}

[Serializable]
internal struct Game
{
    public int id;
    public Trainer[] trainer;
    public Pokemon[] pokemon;
    public string title;
    public int gm;
}

[Serializable]
internal struct Message
{
    public int id;
    public string message;
    public string display_name;
    public int game;
}

[Serializable]
internal struct MessageList
{
    public Message[] messages;
}

[Serializable]
internal struct BackgroundStruct
{
    public int id;
    public int gm;
    public string title;
    public ImageStruct image;
}

[Serializable]
internal struct BackgroundList
{
    public BackgroundStruct[] list;
}

[Serializable]
internal struct TokenStruct
{
    public int id;
    public int user;
    public string title;
    public ImageStruct image;
}

[Serializable]
internal struct ImageStruct
{
    public int id;
    public string name;
    public string path;
    public int height;
    public int width;
}

#pragma warning restore 0649