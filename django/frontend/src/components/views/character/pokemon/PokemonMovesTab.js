import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class PokemonMovesTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		const pokemon = this.props.pokemon;
		const moves = pokemon.pokemonAttack;
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th>Name</th>
				<th>Type</th>
				<th>Class</th>
				<th>Frequency</th>
				<th>AC</th>
				<th>Damage Base</th>
				<th>Range</th>
				<th>Effect</th>
			</tr>
			{moves.map(function(move, index) {
				return (
					<tr key={index}>
						<td>{move.attack.name}</td>
						<td>{move.attack.type}</td>
						<td>{move.attack.attackClass}</td>
						<td>{move.attack.frequency}</td>
						<td>{move.attack.ac}</td>
						<td>{move.attack.damageBase}</td>
						<td>{move.attack.range}</td>
						<td>{move.attack.effect}</td>
					</tr>
				);
			})}
		</tbody>
	</table>
</div>
		);
	}
}

export default PokemonMovesTab;
