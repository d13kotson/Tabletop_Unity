using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerStatsTab : MonoBehaviour
{
    private Trainer trainer;
    void Start()
    {
        
    }

    internal void Set(Trainer trainer)
    {
        this.trainer = trainer;
        Transform content = this.gameObject.transform;
        {
            Transform statsPanel = content.Find("StatsPanel");
            statsPanel.Find("Constitution").gameObject.GetComponent<Text>().text = trainer.constitution.ToString();
            statsPanel.Find("Attack").gameObject.GetComponent<Text>().text = trainer.attack.ToString();
            statsPanel.Find("Defense").gameObject.GetComponent<Text>().text = trainer.defense.ToString();
            statsPanel.Find("SpAttack").gameObject.GetComponent<Text>().text = trainer.special_attack.ToString();
            statsPanel.Find("SpDefense").gameObject.GetComponent<Text>().text = trainer.special_defense.ToString();
            statsPanel.Find("Speed").gameObject.GetComponent<Text>().text = trainer.speed.ToString();
        }
        {
            Transform combatPanel = content.Find("CombatPanel");
            combatPanel.Find("MaxHP").gameObject.GetComponent<Text>().text = (trainer.level * 2 + trainer.constitution * 3 + 10).ToString();
            combatPanel.Find("Run").gameObject.GetComponent<Text>().text = (3 + (trainer.athletics + trainer.acrobatics) / 2).ToString();
            combatPanel.Find("Swim").gameObject.GetComponent<Text>().text = ((3 + (trainer.athletics + trainer.acrobatics) / 2) /2).ToString();
        }
    }
}
