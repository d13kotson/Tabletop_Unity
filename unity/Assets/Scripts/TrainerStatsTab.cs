using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerStatsTab : MonoBehaviour
{
    private Trainer trainer;

    internal void Set(Trainer trainer)
    {
        this.trainer = trainer;
        Transform content = this.gameObject.transform;
        {
            Transform statsPanel = content.Find("StatsPanel");
            statsPanel.Find("Constitution").gameObject.GetComponent<Text>().text = trainer.constitution.ToString();
            statsPanel.Find("Attack").gameObject.GetComponent<Text>().text = this.GetStat(trainer.attack, trainer.attack_cs).ToString();
            statsPanel.Find("Defense").gameObject.GetComponent<Text>().text = this.GetStat(trainer.defense, trainer.defense_cs).ToString();
            statsPanel.Find("SpAttack").gameObject.GetComponent<Text>().text = this.GetStat(trainer.special_attack, trainer.special_attack_cs).ToString();
            statsPanel.Find("SpDefense").gameObject.GetComponent<Text>().text = this.GetStat(trainer.special_defense, trainer.special_defense_cs).ToString();
            statsPanel.Find("Speed").gameObject.GetComponent<Text>().text = this.GetStat(trainer.speed, trainer.speed_cs).ToString();
        }
        {
            Transform combatPanel = content.Find("CombatPanel");
            combatPanel.Find("MaxHP").gameObject.GetComponent<Text>().text = (trainer.level * 2 + trainer.constitution * 3 + 10).ToString();
            combatPanel.Find("Run").gameObject.GetComponent<Text>().text = (3 + (trainer.athletics + trainer.acrobatics) / 2).ToString();
            combatPanel.Find("Swim").gameObject.GetComponent<Text>().text = ((3 + (trainer.athletics + trainer.acrobatics) / 2) /2).ToString();
        }
    }

    private int GetStat(int stat, int cs)
    {
        if(cs < 0)
        {
            float modifier = 2.0f / (-cs + 2);
            return Mathf.FloorToInt(stat * modifier);
        }
        else if(cs > 0)
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
