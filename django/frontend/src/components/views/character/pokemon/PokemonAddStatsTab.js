import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class PokemonAddStatsTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"constitution": React.createRef(),
			"attack": React.createRef(),
			"defense": React.createRef(),
			"specialAttack": React.createRef(),
			"specialDefense": React.createRef(),
			"speed": React.createRef(),
		};
	}

	render() {
		const pokemon = this.props.pokemon;
		const species = pokemon.species;
		let nature = [0, 0, 0, 0, 0, 0];
		if(pokemon.nature < 30) {
			let skip = 0;
			if(pokemon.nature < 5) {
				skip = 0;
				nature[0] = 1;
			}
			else if(pokemon.nature < 10) {
				skip = 1;
				nature[1] = 2;
			}
			else if(pokemon.nature < 15) {
				skip = 2;
				nature[2] = 2;
			}
			else if(pokemon.nature < 20) {
				skip = 3;
				nature[3] = 2;
			}
			else if(pokemon.nature < 25) {
				skip = 4;
				nature[4] = 2;
			}
			else {
				skip = 5;
				nature[5] = 2;
			}
			let index = pokemon.nature % 5;
			if(index >= skip) {
				index++;
			}
			if(index = 0) {
				nature[index] = -1;
			}
			else {
				nature[index] = -2;
			}
		}

		let constitution = pokemon.constitution + species.base_constitution + nature[0];
		let attack = pokemon.attack + species.base_attack + nature[1];
		let defense = pokemon.defense + species.base_defense + nature[2];
		let specialAttack = pokemon.special_attack + species.base_special_attack + nature[3];
		let specialDefense = pokemon.special_defense + species.base_special_defense + nature[4];
		let speed = pokemon.speed + species.base_speed + nature[5];

		const classProp = this.props.class + " tabContent";

		return (
<div ref={this.tabRefs["self"]} style={{display: "block", width: "100%"}} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Stats</th>
			</tr>
			<tr>
				<td>Constitution</td>
				<td>{constitution}</td>
				<td><input ref={this.tabRefs["constitution"]} type="number" defaultValue={0}/></td>
			</tr>
			<tr>
				<td>Attack</td>
				<td>{attack}</td>
				<td><input ref={this.tabRefs["attack"]} type="number" defaultValue={0}/></td>
			</tr>
			<tr>
				<td>Defense</td>
				<td>{defense}</td>
				<td><input ref={this.tabRefs["defense"]} type="number" defaultValue={0}/></td>
			</tr>
			<tr>
				<td>Special Attack</td>
				<td>{specialAttack}</td>
				<td><input ref={this.tabRefs["specialAttack"]} type="number" defaultValue={0}/></td>
			</tr>
			<tr>
				<td>Special Defense</td>
				<td>{specialDefense}</td>
				<td><input ref={this.tabRefs["specialDefense"]} type="number" defaultValue={0}/></td>
			</tr>
			<tr>
				<td>Speed</td>
				<td>{speed}</td>
				<td><input ref={this.tabRefs["speed"]} type="number" defaultValue={0}/></td>
			</tr>
		</tbody>
	</table>
</div>
		);
	}
}

export default PokemonAddStatsTab;
