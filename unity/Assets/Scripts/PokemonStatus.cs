﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokemonStatus : Window
{
    private Pokemon pokemon;
    private GameObject statsTab;
    private GameObject skillsTab;
    private GameObject movesTab;
    private GameObject levelUpPanelObj;
	private RectTransform panelsPanel;

    public InputField ExpInput = default;
    public GameObject LevelUpPanel = default;

    void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.transform.SetParent(canvas.transform);
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(490, -100);
	}

    internal void Set(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        this.UpdatePokemon();

        this.setTokenSprite();

        this.ShowStats();
        this.Disable();
    }

    private void UpdatePokemon()
    {
        Transform content = this.gameObject.transform.Find("Viewport").Find("Content");
        content.Find("Main").Find("Name").gameObject.GetComponent<Text>().text = this.pokemon.name;
        content.Find("Main").Find("DexNum").gameObject.GetComponent<Text>().text = this.pokemon.species.dex_num.ToString();
        content.Find("Level").Find("Level").gameObject.GetComponent<Text>().text = this.pokemon.level.ToString();
        content.Find("Level").Find("Exp").gameObject.GetComponent<Text>().text = this.pokemon.experience.ToString();
		this.panelsPanel = (RectTransform)content.Find("Panels");
		this.statsTab = content.Find("Panels").Find("StatsPanel").gameObject;
        this.statsTab.GetComponent<PokemonStatsTab>().Set(this.pokemon);
        this.skillsTab = content.Find("Panels").Find("SkillsPanel").gameObject;
        this.skillsTab.GetComponent<PokemonSkillsTab>().Set(this.pokemon);
        this.movesTab = content.Find("Panels").Find("MovesPanel").gameObject;
        this.movesTab.GetComponent<PokemonMovesTab>().Set(this.pokemon);
    }

    private void setTokenSprite()
    {
        this.controller.SendTextureRequest(string.Format("api/image/{0}", pokemon.token.image.id), (request) =>
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            this.gameObject.transform.Find("Viewport").Find("Content").Find("Buttons").Find("AddToken").GetComponent<Image>().sprite = sprite;
        }, (request) =>
        {

        });
    }

    public void ShowStats()
    {
        this.HideTabs();
        this.statsTab.SetActive(true);
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, 200);

	}

    public void ShowSkills()
    {
        this.HideTabs();
        this.skillsTab.SetActive(true);
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, 220);
	}

    public void ShowMoves()
    {
        this.HideTabs();
        this.movesTab.SetActive(true);

		float size = this.pokemon.pokemon_attack.Length * 180;
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, size);
		RectTransform movesPanel = (RectTransform)this.panelsPanel.Find("MovesPanel");
		movesPanel.localPosition = new Vector2(movesPanel.localPosition.x, 0);
	}

    private void HideTabs()
    {
        this.statsTab.SetActive(false);
        this.skillsTab.SetActive(false);
        this.movesTab.SetActive(false);
    }

	public void UpdateHP(InputField input)
	{
		int newHP = int.Parse(input.text);
		this.pokemon.current_hp = newHP;
		this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) => {
			PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
			pokemon.current_hp = newHP;
			this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => { }, (request) => { });
		}, (request) => {

		});
	}

    public void UpdateAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.pokemon.attack_cs = newCS;
        this.UpdatePokemon();
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.attack_cs = newCS;
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => {}, (request) =>{});
        }, (request) =>
        {

        });
    }

    public void UpdateDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.pokemon.defense_cs = newCS;
        this.UpdatePokemon();
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.defense_cs = newCS;
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => { }, (request) => { });
        }, (request) =>
        {

        });
    }

    public void UpdateSpecialAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.pokemon.special_attack_cs = newCS;
        this.UpdatePokemon();
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.special_attack_cs = newCS;
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => { }, (request) => { });
        }, (request) =>
        {

        });
    }

    public void UpdateSpecialDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.pokemon.special_defense_cs = newCS;
        this.UpdatePokemon();
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.special_defense_cs = newCS;
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => { }, (request) => { });
        }, (request) =>
        {

        });
    }

    public void UpdateSpeed(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.pokemon.speed_cs = newCS;
        this.UpdatePokemon();
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.speed_cs = newCS;
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (request) => { }, (request) => { });
        }, (request) =>
        {

        });
    }

    public void AddToken()
    {
        this.controller.socket.AddToken(this.pokemon.token.id, Camera.main.transform.position.x, Camera.main.transform.position.y, TokenType.pokemon, this.pokemon.id);
    }

    public void AddExp()
    {
        try
        {
            int exp = int.Parse(this.ExpInput.text);
            this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
            {
                PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
                pokemon.experience += exp;
                string data = JsonUtility.ToJson(pokemon);
                this.controller.SendPutRequest(string.Format("api/pokemon/{0}", this.pokemon.id), data, (updateRequest) =>
                {
                    Pokemon updated = JsonUtility.FromJson<Pokemon>(updateRequest.downloadHandler.text);
                    if (updated.level > pokemon.level)
                    {
                        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
                        this.levelUpPanelObj = Instantiate(this.LevelUpPanel);
                        this.levelUpPanelObj.transform.SetParent(canvas.transform);
                        this.levelUpPanelObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        this.levelUpPanelObj.GetComponent<PokemonLevelUp>().Set(this.pokemon);
                    }
					else {
						this.controller.Reload();
					}
                }, (updateRequest) =>
                {

                });
            }, (pokemonRequest) =>
            {

            });
        }
        catch(FormatException)
        {

        }
    }
}
