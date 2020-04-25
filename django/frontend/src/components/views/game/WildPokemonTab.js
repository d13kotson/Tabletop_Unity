import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import PokemonTab from "./../character/PokemonTab";

class WildPokemonTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		const game = this.props.game
		let pokemon = game.pokemon;
		if(pokemon == null) {
			pokemon = [];
		}
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<div className="darkTab">
{ pokemon.map(function(poke, index, arr) {
		if(poke.trainer == null) {
			this.tabRefs[poke.id] = React.createRef();
			return <a className="tablink" key={index} onClick={() => this.openTab(event, 'wildPokemonTabContent', poke.id)}>{poke.name}</a>
		}
}, this)}
	</div>
	{ pokemon.map(function(poke, index, arr) {
		if(poke.trainer == null) {
			return <PokemonTab ref={this.tabRefs[poke.id]} key={index} pokemon={poke} update={() => { return; }}/>;
		}
	}, this)}
</div>
		);
	}
}

export default WildPokemonTab;
