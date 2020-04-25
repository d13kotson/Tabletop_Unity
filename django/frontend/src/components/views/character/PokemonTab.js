import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import PokemonInfoTab from "./pokemon/PokemonInfoTab";
import PokemonSkillsTab from "./pokemon/PokemonSkillsTab";
import PokemonCombatTab from "./pokemon/PokemonCombatTab";
import PokemonMovesTab from "./pokemon/PokemonMovesTab";

class PokemonTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"infoTab": React.createRef(),
			"skillsTab": React.createRef(),
			"combatTab": React.createRef(),
			"movesTab": React.createRef(),
		};
	}

	render() {
		const pokemon = this.props.pokemon;
		const classProp = this.props.class + " tabContent";
		const subClass = pokemon.trainer + "Pokemon" + pokemon.id + "TabContent";
		return (
			<div ref={this.tabRefs["self"]} className={classProp}>
				<div className="lightTab">
					<a className="tablink" onClick={() => this.openTab(event, subClass, 'infoTab')}>Info</a>
					<a className="tablink" onClick={() => this.openTab(event, subClass, 'skillsTab')}>Skills</a>
					<a className="tablink" onClick={() => this.openTab(event, subClass, 'combatTab')}>Combat</a>
					<a className="tablink" onClick={() => this.openTab(event, subClass, 'movesTab')}>Moves</a>
				</div>

				<PokemonInfoTab class={subClass} ref={this.tabRefs["infoTab"]} pokemon={pokemon} update={this.props.update}/>
				<PokemonSkillsTab class={subClass} ref={this.tabRefs["skillsTab"]} pokemon={pokemon} update={this.props.update} rollDie={this.props.rollDie}/>
				<PokemonCombatTab class={subClass} ref={this.tabRefs["combatTab"]} pokemon={pokemon} update={this.props.update}/>
				<PokemonMovesTab class={subClass} ref={this.tabRefs["movesTab"]} pokemon={pokemon} update={this.props.update}/>
			</div>
		);
	}
}

export default PokemonTab;
