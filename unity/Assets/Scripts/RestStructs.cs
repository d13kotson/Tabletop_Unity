using System;
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

[Serializable]
internal struct MapToken
{
    public int id;
    public int x;
    public int y;
    public int width;
    public int height;
}

[Serializable]
internal struct MapState
{
    public string background;
    public MapToken[] tokens;
}

[Serializable]
internal struct UpdateToken
{
    public int tokenID;
    public int tokenX;
    public int tokenY;
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
internal struct Attack
{
    public int id;
    public string name;
    public string type;
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
internal struct Pokemon
{
    public int id;
    public PokemonAttack[] pokemon_attack;
    public Species species;
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
    public int current_hp;
    public string ability;
    public int trainer;
    public int game;
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
internal struct Item
{
    public int id;
    public string name;
    public int price;
    public string roll_text;
}

[Serializable]
internal struct TrainerItem
{
    public int id;
    public Item item;
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
#pragma warning restore 0649