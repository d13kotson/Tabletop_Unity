import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Cookies from 'js-cookie';

class CalculatorTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.inputRefs = {
			"capLevel": React.createRef(),
			"capHP": React.createRef(),
			"capMaxHP": React.createRef(),
			"capStages": React.createRef(),
			"capPersist": React.createRef(),
			"capVolatile": React.createRef(),
			"capShiny": React.createRef(),
			"capLegendary": React.createRef(),
			"capStuck": React.createRef(),
			"capSlow": React.createRef(),
			"captureRate": React.createRef(),
			"genName": React.createRef(),
			"genSpecies": React.createRef(),
			"genLevel": React.createRef(),
		};
		this.state = {
			"dataLoaded": false,
		};
		fetch("/api/species/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				this.setState({
					"species": data,
					"dataLoaded": true
				});
			});
	}

	render() {
		const dataLoaded = this.state.dataLoaded;
		const classProp = this.props.class + " tabContent";
		if(dataLoaded) {
			const species = this.state.species;
			return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<div>
		<div>
			<label htmlFor="capLevel">Level: </label>
			<input ref={this.inputRefs["capLevel"]} type="number" />
		</div>
		<div>
			<label htmlFor="capHP">HP: </label>
			<input ref={this.inputRefs["capHP"]} type="number" />
		</div>
		<div>
		<label htmlFor="capMaxHP">Max HP: </label>
		<input ref={this.inputRefs["capMaxHP"]} type="number" />
		</div>
		<div>
			<label htmlFor="capStages">Evo Stages Remaining: </label>
			<input ref={this.inputRefs["capStages"]} type="number" />
		</div>
		<div>
			<label htmlFor="capPersist">Persistant Conditions: </label>
			<input ref={this.inputRefs["capPersist"]} type="number" />
		</div>
		<div>
			<label htmlFor="capVolatile">Volatile Conditions: </label>
			<input ref={this.inputRefs["capVolatile"]} type="number" />
		</div>
		<div>
			<label htmlFor="capShiny">Shiny: </label>
			<input ref={this.inputRefs["capShiny"]} type="checkbox" />
		</div>
		<div>
			<label htmlFor="capLegendary">Legendary: </label>
			<input ref={this.inputRefs["capLegendary"]} type="checkbox" />
		</div>
		<div>
			<label htmlFor="capStuck">Stuck: </label>
			<input ref={this.inputRefs["capStuck"]} type="checkbox" />
		</div>
		<div>
			<label htmlFor="capSlow">Slow: </label>
			<input ref={this.inputRefs["capSlow"]} type="checkbox" />
		</div>
		<button onClick={() => this.calculateCapture()}>Generate Capture Rate</button>
		<input ref={this.inputRefs["captureRate"]} type="number"/>
	</div>

	<div>
		<label htmlFor="genName">Name: </label>
		<input ref={this.inputRefs["genName"]} type="text" />
	</div>
	<div>
		<label htmlFor="genSpecies">Species: </label>
		<select ref={this.inputRefs["genSpecies"]}>
			<option value="0">Please select an option</option>
			{species.map(function(pokemon, index) {
				return (
					<option key={index} value={pokemon.id}>{pokemon.name}</option>
				);
			})}
		</select>
	</div>
	<div>
		<label htmlFor="genLevel">Level: </label>
		<input ref={this.inputRefs["genLevel"]} type="number" />
	</div>
	<div>
		<button onClick={() => this.generatePokemon()}>Create Wild Pokemon</button>
	</div>
</div>
			);
		}
		else {
			return (<div ref={this.tabRefs["self"]} className={classProp} />);
		}
	}

	async generatePokemon() {
		let name = this.inputRefs["genName"].current.value;
		let species = this.inputRefs["genSpecies"].current.value;
		let level = this.inputRefs["genLevel"].current.value;
		const game = this.props.game
		let pokemon = {
			"name": name,
			"species": species,
			"level": level,
			"game": game,
			"nature": 0,
			"constitution": 0,
			"attack": 0,
			"defense": 0,
			"special_attack": 0,
			"special_defense": 0,
			"speed": 0,
		};
		await fetch("/api/generate/", {
				method: 'POST',
		    mode: 'cors',
		    cache: 'no-cache',
		    credentials: 'same-origin',
		    headers: {
		      'Content-Type': 'application/json',
					"X-CSRFTOKEN": Cookies.get('csrftoken')
		    },
		    redirect: 'follow',
		    referrer: 'no-referrer',
		    body: JSON.stringify(pokemon)
			}
		)
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			});
		this.props.update();
	}

	calculateCapture() {
		let level = this.inputRefs["capLevel"].current.value;
		let hp = this.inputRefs["capHP"].current.value;
		let maxhp = this.inputRefs["capMaxHP"].current.value;
		let stagesRemaining = this.inputRefs["capStages"].current.value;
		let isShiny  = this.inputRefs["capShiny"].current.checked;
		let isLegendary  = this.inputRefs["capLegendary"].current.checked;
		let isStuck  = this.inputRefs["capStuck"].current.checked;
		let isSlow  = this.inputRefs["capSlow"].current.checked;
		let numPersistConditions  = this.inputRefs["capPersist"].current.value;
		let numVolatileConditions  = this.inputRefs["capVolatile"].current.value;

		let hpPercentage = hp / maxhp;
		let hpReduction = -30;
		if (hpPercentage <= 0.25){
			hpReduction = 15;
		} else if (hpPercentage <= 0.5) {
			hpReduction = 0;
		} else if (hpPercentage <= 0.75) {
			hpReduction = -15;
		}
		if (hp == 1) {
			hpReduction = 30;
		}

		let evoReduction = (stagesRemaining - 1) * 10;
		let shinyReduction = 0;
		if (isShiny) {
			shinyReduction = -10;
		}
		let legendaryReduction = 0;
		if (isLegendary) {
			legendaryReduction = -30;
		}
		let stuckReduction = 0;
		if (isStuck) {
			stuckReduction = 10;
		}
		let slowReduction = 0;
		if (isSlow) {
			slowReduction = 5;
		}

		let rate = 100 - (level * 2) + hpReduction + evoReduction + shinyReduction + legendaryReduction + (numPersistConditions * 10) + (numVolatileConditions * 5) + stuckReduction + slowReduction;
		this.inputRefs["captureRate"].current.value = rate;
	}
}

export default CalculatorTab;
