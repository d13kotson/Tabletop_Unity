import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class PokemonAddMove extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"moves": [],
		}
	}

	render() {
		const pokemon = this.props.pokemon;
		const learnableMoves = pokemon.species.speciesAttack;
		const knownMoves = pokemon.pokemonAttack;
		for(let move of knownMoves) {
		    this.tabRefs["moves"][move.attack.id] = React.createRef();
		}
		for(let move of learnableMoves) {
			if(move.level <= pokemon.level) {
				this.tabRefs["moves"][move.attack.id] = React.createRef();
			}
		}
		const classProp = this.props.class + " tabContent scrollable";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
    <table className="stat-block">
        <tbody>
            <tr>
                <th>Name</th>
                <th>Type</th>
                <th>Class</th>
                <th>Frequency</th>
                <th>Damage Base</th>
                <th>Range</th>
                <th>Effect</th>
                <th>Known</th>
            </tr>
            {knownMoves.map(function(move, index, arr) {
                return (
                    <tr key={index}>
                        <td>{move.attack.name}</td>
                        <td>{move.attack.type}</td>
                        <td>{move.attack.attackClass}</td>
                        <td>{move.attack.frequency}</td>
                        <td>{move.attack.damageBase}</td>
                        <td>{move.attack.range}</td>
                        <td>{move.attack.effect}</td>
                        <td><input ref={this.tabRefs["moves"][move.attack.id]} type="checkbox" name={move.attack.id} defaultChecked={true}/></td>
                    </tr>
                );
            }, this)}
            {learnableMoves.map(function(move, index, arr) {
                let alreadyKnown = false;
                for(let knownMove of knownMoves) {
                    if(move.attack.id === knownMove.attack.id) {
                        alreadyKnown = true;
                        break;
                    }
                }
                if(move.level <= pokemon.level && !alreadyKnown) {
                    return (
                        <tr key={index}>
                            <td>{move.attack.name}</td>
                            <td>{move.attack.type}</td>
                            <td>{move.attack.attackClass}</td>
                            <td>{move.attack.frequency}</td>
                            <td>{move.attack.damageBase}</td>
                            <td>{move.attack.range}</td>
                            <td>{move.attack.effect}</td>
                            <td><input ref={this.tabRefs["moves"][move.attack.id]} type="checkbox" name={move.attack.id}/></td>
                        </tr>
                    );
                }
            }, this)}
        </tbody>
    </table>
</div>
		);
	}
}

export default PokemonAddMove;
