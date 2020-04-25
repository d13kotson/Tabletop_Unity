import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class PokemonSkillsTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		const pokemon = this.props.pokemon;
		const species = pokemon.species;
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Body</th>
			</tr>
			<tr>
				<td>Acrobatics</td>
				<td>{species.acrobatics}</td>
			</tr>
			<tr>
				<td>Athletics</td>
				<td>{species.athletics}</td>
			</tr>
			<tr>
				<td>Combat</td>
				<td>{species.combat}</td>
			</tr>
			<tr>
				<td>Intimidate</td>
				<td>{species.intimidate}</td>
			</tr>
			<tr>
				<td>Stealth</td>
				<td>{species.stealth}</td>
			</tr>
			<tr>
				<td>Survival</td>
				<td>{species.survival}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Mind</th>
			</tr>
			<tr>
				<td>General Education</td>
				<td>{species.gen_education}</td>
			</tr>
			<tr>
				<td>Medical Education</td>
				<td>{species.med_education}</td>
			</tr>
			<tr>
				<td>Occult Education</td>
				<td>{species.occ_education}</td>
			</tr>
			<tr>
				<td>Pokemon Education</td>
				<td>{species.pok_education}</td>
			</tr>
			<tr>
				<td>Technology Education</td>
				<td>{species.tec_education}</td>
			</tr>
			<tr>
				<td>Guile</td>
				<td>{species.guile}</td>
			</tr>
			<tr>
				<td>Perception</td>
				<td>{species.perception}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Spirit</th>
			</tr>
			<tr>
				<td>Charm</td>
				<td>{species.charm}</td>
			</tr>
			<tr>
				<td>Command</td>
				<td>{species.command}</td>
			</tr>
			<tr>
				<td>Focus</td>
				<td>{species.focus}</td>
			</tr>
			<tr>
				<td>Intuition</td>
				<td>{species.intuition}</td>
			</tr>
		</tbody>
	</table>
</div>
		);
	}
}

export default PokemonSkillsTab;
