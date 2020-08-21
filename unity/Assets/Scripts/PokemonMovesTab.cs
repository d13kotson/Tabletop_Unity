using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonMovesTab : MonoBehaviour
{
    private Pokemon pokemon;
    public GameObject MovePanel = default;
    void Start()
    {

    }

    internal void Set(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        Transform content = this.gameObject.transform;
        foreach(PokemonAttack attack in this.pokemon.pokemon_attack)
        {
            GameObject movePanel = Instantiate(this.MovePanel);
            movePanel.transform.SetParent(content);
            movePanel.GetComponentInChildren<Text>().text = name;
            movePanel.GetComponent<MovePanel>().Set(attack.attack, pokemon.id, TokenType.pokemon);
        }
    }
}
