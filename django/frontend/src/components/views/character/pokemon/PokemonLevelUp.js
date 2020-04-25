import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";
import PokemonAddMove from "./PokemonAddMove";
import PokemonAddStatsTab from "./PokemonAddStatsTab";
import Cookies from 'js-cookie';

class PokemonLevelUp extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"addMovesTab": React.createRef(),
			"addStatsTab": React.createRef(),
		}
	}

	render() {
		const pokemon = this.props.pokemon;
		const levelsGained = this.props.levelsGained;
		const subClass = pokemon.trainer + "Pokemon" + pokemon.id + "LevelUpTabContent";
		return (
<div style={{width: "100%", position: "absolute", backgroundColor: "white", borderRight: "2px solid gray", borderBottom: "2px solid gray", top: "5%", left: "2%", width: "90%", height: "80%", zIndex: "1", overflow: "hidden", overflow: "auto"}}>
	<div className="lightTab">
		<a className="tablink" onClick={() => this.openTab(event, subClass, "addStatsTab")}>Add Stats</a>
		<a className="tablink" onClick={() => this.openTab(event, subClass, "addMovesTab")}>Add Moves</a>
	</div>
	<div>
		{pokemon.name} Gained {levelsGained} level(s)!
	</div>
	<PokemonAddStatsTab class={subClass} ref={this.tabRefs["addStatsTab"]} pokemon={pokemon}/>
	<PokemonAddMove class={subClass} ref={this.tabRefs["addMovesTab"]} pokemon={pokemon}/>
	<a className="button" onClick={() => this.levelup()}>Level Up</a>
</div>
		);
	}

	async levelup() {
		let pokemon = Object.assign({}, this.props.pokemon);
		pokemon.species = pokemon.species.id;

		let constitution = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["constitution"].current.value);
		let attack = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["attack"].current.value);
		let defense = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["defense"].current.value);
		let specialAttack = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["specialAttack"].current.value);
		let specialDefense = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["specialDefense"].current.value);
		let speed = parseInt(this.tabRefs["addStatsTab"].current.tabRefs["speed"].current.value);

		pokemon.constitution += constitution;
		pokemon.attack += attack;
		pokemon.defense += defense;
		pokemon.special_attack += specialAttack;
		pokemon.special_defense += specialDefense;
		pokemon.speed += speed;

		fetch("/api/pokemon/" + pokemon.id + "/", {
			method: 'PUT',
			body: JSON.stringify(pokemon),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return response.json();
		});

		let moves = Object.entries(this.tabRefs["addMovesTab"].current.tabRefs["moves"]);
		let learnedMoves = [];
		for(let [key, value] of moves) {
			if(value.current.checked) {
				learnedMoves.push(key);
			}
		}
		let removedMoves = [];
		for(let i = 0; i < pokemon.pokemonAttack.length; i++) {
			if(!learnedMoves.includes(pokemon.pokemonAttack[i].url)) {
				removedMoves.push(pokemon.pokemonAttack[i]);
			}
		}
		let newMoves = [];
		for(let move of learnedMoves) {
			let alreadyKnown = false;
			for(let knownMove of pokemon.pokemonAttack) {
				if(move === knownMove.url) {
					alreadyKnown = true;
				}
			}
			if(!alreadyKnown) {
				newMoves.push(move);
			}
		}

		for(let removedMove of removedMoves) {
			await fetch("/api/pokemonAttack/" + removedMove.id + "/", {
			  method: 'DELETE',
				headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
			})
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			});
		}

		for(let newMove of newMoves) {
			let move = {
				"pokemon": pokemon.id,
				"attack": newMove
			}
			await fetch("api/addPokemonAttack/", {
			  method: 'POST',
			  body: JSON.stringify(move),
				headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
			}).then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
		}

		this.props.onComplete();
	}
}

export default PokemonLevelUp;
