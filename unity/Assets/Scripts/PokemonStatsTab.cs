using UnityEngine;
using UnityEngine.UI;

public class PokemonStatsTab : MonoBehaviour
{
    private Pokemon pokemon;

    internal void Set(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        Transform content = this.gameObject.transform;
        {
            Transform statsPanel = content.Find("StatsPanel");
            statsPanel.Find("Constitution").gameObject.GetComponent<Text>().text = (pokemon.constitution + pokemon.species.base_constitution + pokemon.nature.constitution).ToString();
            statsPanel.Find("AttackCS").gameObject.GetComponent<InputField>().text = pokemon.attack_cs.ToString();
            statsPanel.Find("Attack").gameObject.GetComponent<Text>().text = this.GetStat(pokemon.attack + pokemon.species.base_attack + pokemon.nature.attack, pokemon.attack_cs).ToString ();
            statsPanel.Find("DefenseCS").gameObject.GetComponent<InputField>().text = pokemon.defense_cs.ToString();
            statsPanel.Find("Defense").gameObject.GetComponent<Text>().text = this.GetStat(pokemon.defense + pokemon.species.base_defense + pokemon.nature.defense, pokemon.defense_cs).ToString();
            statsPanel.Find("SpAttackCS").gameObject.GetComponent<InputField>().text = pokemon.special_attack_cs.ToString();
            statsPanel.Find("SpAttack").gameObject.GetComponent<Text>().text = this.GetStat(pokemon.special_attack + pokemon.species.base_special_attack + pokemon.nature.special_attack, pokemon.special_attack_cs).ToString();
            statsPanel.Find("SpDefenseCS").gameObject.GetComponent<InputField>().text = pokemon.special_defense_cs.ToString();
            statsPanel.Find("SpDefense").gameObject.GetComponent<Text>().text = this.GetStat(pokemon.special_defense + pokemon.species.base_special_defense + pokemon.nature.special_defense, pokemon.special_defense_cs).ToString();
            statsPanel.Find("SpeedCS").gameObject.GetComponent<InputField>().text = pokemon.speed_cs.ToString();
            statsPanel.Find("Speed").gameObject.GetComponent<Text>().text = this.GetStat(pokemon.speed + pokemon.species.base_speed + pokemon.nature.speed, pokemon.speed_cs).ToString();
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

    private int GetStat(int stat, int cs)
    {
        if (cs < 0)
        {
            float modifier = 2.0f / (-cs + 2);
            return Mathf.FloorToInt(stat * modifier);
        }
        else if (cs > 0)
        {
            float modifier = (cs + 2) / 2.0f;
            return Mathf.FloorToInt(stat * modifier);
        }
        else
        {
            return stat;
        }
    }
}
