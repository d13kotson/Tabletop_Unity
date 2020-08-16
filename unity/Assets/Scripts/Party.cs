using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Party : Window
{
    public GameObject button = default;
    public RectTransform viewPort = default;
    public GameObject pokemonStatus = default;
    public GameObject trainerStatus = default;

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
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                GameObject trainerStatus = Instantiate(this.trainerStatus);
                trainerStatus.GetComponent<Transform>().SetParent(canvas);
                trainerStatus.GetComponent<TrainerStatus>().Set(trainer);
                this.addButton(trainer.name, delegate { this.enableTrainerStats(trainerStatus); });
                this.trainers.Add(trainer.id, trainerStatus.GetComponent<TrainerStatus>());
				this.controller.trainerStatuses.Add(trainer.id, trainerStatus);
			}
            foreach (Trainer trainer in this.controller.game.trainer)
            {
                foreach (Pokemon pokemon in trainer.pokemon)
                {
                    GameObject pokemonStatus = Instantiate(this.pokemonStatus);
                    pokemonStatus.GetComponent<Transform>().SetParent(canvas);
                    pokemonStatus.GetComponent<PokemonStatus>().Set(pokemon);
                    this.addButton(pokemon.name, delegate { this.enablePokemonStats(pokemonStatus); });
                    this.pokemon.Add(pokemon.id, pokemonStatus.GetComponent<PokemonStatus>());
					this.controller.pokemonStatuses.Add(pokemon.id, pokemonStatus);
                }
            }
        }
        else
        {
            GameObject trainerStatus = Instantiate(this.trainerStatus);
            trainerStatus.GetComponent<Transform>().SetParent(canvas);
            trainerStatus.GetComponent<TrainerStatus>().Set(this.controller.trainer);
            this.addButton(this.controller.trainer.name, delegate { this.enableTrainerStats(trainerStatus); });
            this.trainers.Add(this.controller.trainer.id, trainerStatus.GetComponent<TrainerStatus>());
			this.controller.trainerStatuses.Add(this.controller.trainer.id, trainerStatus);
			foreach (Pokemon pokemon in this.controller.trainer.pokemon)
            {
                GameObject pokemonStatus = Instantiate(this.pokemonStatus);
                pokemonStatus.GetComponent<Transform>().SetParent(canvas);
                pokemonStatus.GetComponent<PokemonStatus>().Set(pokemon);
                this.addButton(pokemon.name, delegate { this.enablePokemonStats(pokemonStatus); });
                this.pokemon.Add(pokemon.id, pokemonStatus.GetComponent<PokemonStatus>());
				this.controller.pokemonStatuses.Add(pokemon.id, pokemonStatus);
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

    private void addButton(string name, UnityAction function)
    {
        GameObject button = Instantiate(this.button);
        button.transform.SetParent(this.viewPort);
        button.GetComponentInChildren<Text>().text = name;
        button.GetComponent<Button>().onClick.AddListener(function);
    }
}
