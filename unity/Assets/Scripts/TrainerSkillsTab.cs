using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerSkillsTab : MonoBehaviour
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
            Transform bodyPanel = content.Find("BodyPanel");
            bodyPanel.Find("Acr").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.acrobatics);
            bodyPanel.Find("Ath").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.athletics);
            bodyPanel.Find("Com").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.combat);
            bodyPanel.Find("Int").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.intimidate);
            bodyPanel.Find("Ste").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.stealth);
            bodyPanel.Find("Sur").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.survival);
        }
        {
            Transform mindPanel = content.Find("MindPanel");
            mindPanel.Find("Gen").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.gen_education);
            mindPanel.Find("Med").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.med_education);
            mindPanel.Find("Occ").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.occ_education);
            mindPanel.Find("Pok").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.pok_education);
            mindPanel.Find("Tec").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.tec_education);
            mindPanel.Find("Gui").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.guile);
            mindPanel.Find("Per").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.perception);
        }
        {
            Transform spiritPanel = content.Find("SpiritPanel");
            spiritPanel.Find("Cha").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.charm);
            spiritPanel.Find("Com").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.command);
            spiritPanel.Find("Foc").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.focus);
            spiritPanel.Find("Ins").gameObject.GetComponent<Text>().text = string.Format("{0}d6", trainer.intuition);
        }
    }
}
