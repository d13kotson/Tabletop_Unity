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
        int position = -50;
        foreach(PokemonAttack attack in this.pokemon.pokemon_attack)
        {
            GameObject movePanel = Instantiate(this.MovePanel);
            movePanel.transform.SetParent(content);
            RectTransform rect = movePanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, position);
            rect.sizeDelta = new Vector2(0, 120);
            movePanel.GetComponentInChildren<Text>().text = name;
            movePanel.GetComponent<MovePanel>().Set(attack.attack, pokemon.id, TokenType.pokemon);
            position -= 120;
        }
    }
}
