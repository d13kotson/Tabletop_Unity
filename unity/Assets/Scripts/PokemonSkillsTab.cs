using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSkillsTab : MonoBehaviour
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
            Transform bodyPanel = content.Find("BodyPanel");
            bodyPanel.Find("Acr").gameObject.GetComponent<Text>().text = pokemon.species.acrobatics;
            bodyPanel.Find("Ath").gameObject.GetComponent<Text>().text = pokemon.species.athletics;
            bodyPanel.Find("Com").gameObject.GetComponent<Text>().text = pokemon.species.combat;
            bodyPanel.Find("Int").gameObject.GetComponent<Text>().text = pokemon.species.intimidate;
            bodyPanel.Find("Ste").gameObject.GetComponent<Text>().text = pokemon.species.stealth;
            bodyPanel.Find("Sur").gameObject.GetComponent<Text>().text = pokemon.species.survival;
        }
        {
            Transform mindPanel = content.Find("MindPanel");
            mindPanel.Find("Gen").gameObject.GetComponent<Text>().text = pokemon.species.gen_education;
            mindPanel.Find("Med").gameObject.GetComponent<Text>().text = pokemon.species.med_education;
            mindPanel.Find("Occ").gameObject.GetComponent<Text>().text = pokemon.species.occ_education;
            mindPanel.Find("Pok").gameObject.GetComponent<Text>().text = pokemon.species.pok_education;
            mindPanel.Find("Tec").gameObject.GetComponent<Text>().text = pokemon.species.tec_education;
            mindPanel.Find("Gui").gameObject.GetComponent<Text>().text = pokemon.species.guile;
            mindPanel.Find("Per").gameObject.GetComponent<Text>().text = pokemon.species.perception;
        }
        {
            Transform spiritPanel = content.Find("SpiritPanel");
            spiritPanel.Find("Cha").gameObject.GetComponent<Text>().text = pokemon.species.charm;
            spiritPanel.Find("Com").gameObject.GetComponent<Text>().text = pokemon.species.command;
            spiritPanel.Find("Foc").gameObject.GetComponent<Text>().text = pokemon.species.focus;
            spiritPanel.Find("Ins").gameObject.GetComponent<Text>().text = pokemon.species.intuition;
        }
    }
}
