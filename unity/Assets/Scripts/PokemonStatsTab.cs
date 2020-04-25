using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonStatsTab : MonoBehaviour
{
    private Pokemon pokemon;
    void Start()
    {
        
    }

    internal void Set(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        Transform content = this.gameObject.transform;
        {
            Transform statsPanel = content.Find("StatsPanel");
            statsPanel.Find("Constitution").gameObject.GetComponent<Text>().text = (pokemon.constitution + pokemon.species.base_constitution).ToString();
            statsPanel.Find("Attack").gameObject.GetComponent<Text>().text = (pokemon.attack + pokemon.species.base_attack).ToString();
            statsPanel.Find("Defense").gameObject.GetComponent<Text>().text = (pokemon.defense + pokemon.species.base_defense).ToString();
            statsPanel.Find("SpAttack").gameObject.GetComponent<Text>().text = (pokemon.special_attack + pokemon.species.base_special_attack).ToString();
            statsPanel.Find("SpDefense").gameObject.GetComponent<Text>().text = (pokemon.special_defense + pokemon.species.base_special_defense).ToString();
            statsPanel.Find("Speed").gameObject.GetComponent<Text>().text = (pokemon.speed + pokemon.species.base_speed).ToString();
        }
        {
            Transform combatPanel = content.Find("CombatPanel");
            combatPanel.Find("MaxHP").gameObject.GetComponent<Text>().text = (pokemon.level + (pokemon.constitution + pokemon.species.base_constitution) * 3 + 10).ToString();
            combatPanel.Find("Burrow").gameObject.GetComponent<Text>().text = pokemon.species.burrow;
            combatPanel.Find("Run").gameObject.GetComponent<Text>().text = pokemon.species.overland;
            combatPanel.Find("Sky").gameObject.GetComponent<Text>().text = pokemon.species.sky;
            combatPanel.Find("Swim").gameObject.GetComponent<Text>().text = pokemon.species.swimming;
            combatPanel.Find("Levitate").gameObject.GetComponent<Text>().text = pokemon.species.levitate;
            combatPanel.Find("Teleport").gameObject.GetComponent<Text>().text = pokemon.species.teleport;
        }
    }
}
