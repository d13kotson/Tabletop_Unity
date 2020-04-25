import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import TrainerTab from "./TrainerTab";
import PokemonTab from "./PokemonTab";

class PartyTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"trainerTab": React.createRef(),
		};
	}

	render() {
		const trainer = this.props.trainer;
		const pokemon = trainer.pokemon;
		const classProp = this.props.class + " tabContent";
		const subClass = trainer.name + "PartyTabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp} style={{display: "block"}}>
	<div className="darkTab">
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'trainerTab')}>{trainer.name}</a>
		{ pokemon.map(function(poke, index, arr) {
			if(poke.inParty) {
				this.tabRefs[poke.id] = React.createRef();
				return <a className="tablink" key={index} onClick={() => this.openTab(event, subClass, poke.id)}>{poke.name}</a>
			}
		}, this)}
	</div>
	<TrainerTab class={subClass} ref={this.tabRefs["trainerTab"]} trainer={trainer} update={this.props.update} rollDie={this.props.rollDie}/>
	{ pokemon.map(function(poke, index, arr) {
		if(poke.inParty) {
			return <PokemonTab class={subClass} ref={this.tabRefs[poke.id]} key={index} pokemon={poke} update={this.props.update} rollDie={this.props.rollDie}/>;
		}
	}, this)}
</div>
		);
	}
}

export default PartyTab;
