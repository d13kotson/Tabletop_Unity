import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";
import PokemonLevelUp from "./PokemonLevelUp.js"
import Cookies from 'js-cookie';

class PokemonInfoTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"expInput": React.createRef(),
		};
		this.state = {
			"displayLevelUp": false
		}
	}

	render() {
		const pokemon = this.props.pokemon;
		const species = pokemon.species;
		const displayLevelUp = this.state.displayLevelUp;
		const classProp = this.props.class + " tabContent";
		if(!displayLevelUp) {
			return (
<div ref={this.tabRefs["self"]} className={classProp} style={{display: "block"}}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th>Name</th>
				<td colSpan="3">{pokemon.name}</td>
			</tr>
			<tr>
				<th>Level</th>
				<td colSpan="3">{pokemon.level}</td>
			</tr>
			<tr>
				<th>EXP</th>
				<td>{pokemon.experience}</td>
				<td><input ref={this.tabRefs["expInput"]} name="addExp" defaultValue="0"/></td>
				<td><a className="button" onClick={() => this.addExp()}>Add Experience</a></td>
			</tr>
			<tr>
				<th>Species</th>
				<td colSpan="3">{species.name}</td>
			</tr>
			<tr>
				<th>Ability</th>
				<td colSpan="3">{pokemon.ability}</td>
			</tr>
		</tbody>
	</table>
	<a className="button" onClick={() => this.moveToBox()}>Move to Box</a>
</div>
			);
		}
		else {
			const levelsGained = this.state.levelsGained;
			return (
<div>
	<PokemonLevelUp pokemon={pokemon} levelsGained={levelsGained} onComplete={() => this.levelup()}/>
</div>
			);
		}
	}

	levelup() {
		this.setState({"displayLevelUp": false});
		this.props.update();
	}

	async addExp() {
		let pokemon = {};
		await fetch("/api/pokemon/" + this.props.pokemon.id + "/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				pokemon = data;
			});
		let addExp = parseInt(this.tabRefs["expInput"].current.value);
		pokemon.experience += addExp;
		fetch("/api/pokemon/" + pokemon.id + "/", {
		  method: 'PUT',
		  body: JSON.stringify(pokemon),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return response.json();
		})
		.then(data => {
			if(data.level > this.props.pokemon.level) {
				this.setState({
					"displayLevelUp": true,
					"levelsGained": data.level - this.props.pokemon.level
				});
			}
			this.props.update();
		});
	}

	async moveToBox() {
		let pokemon = {};
		await fetch("api/pokemon/" + this.props.pokemon.id + "/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				pokemon = data;
			});
		pokemon.inParty = false;
		fetch("api/pokemon/" + this.props.pokemon.id + "/", {
		  method: 'PUT',
		  body: JSON.stringify(pokemon),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return response.json();
		})
		.then(data => {
			this.props.update();
		});
	}
}

export default PokemonInfoTab;
