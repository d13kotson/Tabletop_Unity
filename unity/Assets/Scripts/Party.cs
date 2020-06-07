﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Party : MonoBehaviour
{
    public GameObject button = default;
    public RectTransform viewPort = default;
    public GameObject pokemonStatus = default;
    public GameObject trainerStatus = default;

    private GameController controller = default;

    private Dictionary<int, TrainerStatus> trainers;
    private Dictionary<int, PokemonStatus> pokemon;

    private void Awake()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>();
        this.trainers = new Dictionary<int, TrainerStatus>();
        this.pokemon = new Dictionary<int, PokemonStatus>();
        if (this.controller.isGM)
        {
            int position = -20;
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                GameObject trainerStatus = Instantiate(this.trainerStatus);
                trainerStatus.GetComponent<Transform>().SetParent(canvas);
                trainerStatus.GetComponent<TrainerStatus>().Set(trainer);
                position = this.addButton(position, trainer.name, delegate { this.enableTrainerStats(trainerStatus); });
                this.trainers.Add(trainer.id, trainerStatus.GetComponent<TrainerStatus>());
            }
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                foreach (Pokemon pokemon in trainer.pokemon)
                {
                    GameObject pokemonStatus = Instantiate(this.pokemonStatus);
                    pokemonStatus.GetComponent<Transform>().SetParent(canvas);
                    pokemonStatus.GetComponent<PokemonStatus>().Set(pokemon);
                    position = this.addButton(position, pokemon.name, delegate { this.enablePokemonStats(pokemonStatus); });
                    this.pokemon.Add(pokemon.id, pokemonStatus.GetComponent<PokemonStatus>());
                }
            }
        }
        else
        {
            int position = -20;
            GameObject trainerStatus = Instantiate(this.trainerStatus);
            trainerStatus.GetComponent<Transform>().SetParent(canvas);
            trainerStatus.GetComponent<TrainerStatus>().Set(this.controller.trainer);
            position = this.addButton(position, this.controller.trainer.name, delegate { this.enableTrainerStats(trainerStatus); });
            this.trainers.Add(this.controller.trainer.id, trainerStatus.GetComponent<TrainerStatus>());
            foreach (Pokemon pokemon in this.controller.trainer.pokemon)
            {
                GameObject pokemonStatus = Instantiate(this.pokemonStatus);
                pokemonStatus.GetComponent<Transform>().SetParent(canvas);
                pokemonStatus.GetComponent<PokemonStatus>().Set(pokemon);
                position = this.addButton(position, pokemon.name, delegate { this.enablePokemonStats(pokemonStatus); });
                this.pokemon.Add(pokemon.id, pokemonStatus.GetComponent<PokemonStatus>());
            }
        }
        this.controller.RefreshActions.Add(() => this.Refresh());
    }

    private void Refresh()
    {
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>();
        if (this.controller.isGM)
        {
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                this.trainers[trainer.id].Set(trainer);
            }
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                foreach (Pokemon pokemon in trainer.pokemon)
                {
                    this.pokemon[pokemon.id].Set(pokemon);
                }
            }
        }
        else
        {
            this.trainers[this.controller.trainer.id].Set(this.controller.trainer);
            foreach (Pokemon pokemon in this.controller.trainer.pokemon)
            {
                this.pokemon[pokemon.id].Set(pokemon);
            }
        }
    }

    private void enableTrainerStats(GameObject status)
    {
        status.GetComponent<TrainerStatus>().Enable();
    }

    private void enablePokemonStats(GameObject status)
    {
        status.GetComponent<PokemonStatus>().Enable();
    }

    private int addButton(int position, string name, UnityAction function)
    {
        GameObject button = Instantiate(this.button);
        button.transform.SetParent(this.viewPort);
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, position);
        buttonRect.sizeDelta = new Vector2(0, 40);
        button.GetComponentInChildren<Text>().text = name;
        button.GetComponent<Button>().onClick.AddListener(function);
		float viewPortWidth = this.viewPort.sizeDelta.x;
		this.viewPort.sizeDelta = new Vector2(viewPortWidth, -position + 20);
        return position - 40;
    }

    public void Enable()
    {
        this.controller.CloseScreens();
        bool value = !this.gameObject.activeSelf;
        if (value)
        {
            this.controller.OpenScreens.Add(this.gameObject);
        }
        else
        {
            this.controller.OpenScreens.Remove(this.gameObject);
        }
        this.gameObject.SetActive(value);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
