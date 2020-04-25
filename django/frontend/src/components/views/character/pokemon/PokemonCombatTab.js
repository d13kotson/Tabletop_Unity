import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class PokemonCombatTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.statRefs = {
			"attackStat": React.createRef(),
			"attackCS": React.createRef(),
			"attackTotal": React.createRef(),
			"defenseStat": React.createRef(),
			"defenseCS": React.createRef(),
			"defenseTotal": React.createRef(),
			"specialAttackStat": React.createRef(),
			"specialAttackCS": React.createRef(),
			"specialAttackTotal": React.createRef(),
			"specialDefenseStat": React.createRef(),
			"specialDefenseCS": React.createRef(),
			"specialDefenseTotal": React.createRef(),
			"speedStat": React.createRef(),
			"speedCS": React.createRef(),
			"speedTotal": React.createRef(),
			"physicalEvasion": React.createRef(),
			"specialEvasion": React.createRef(),
			"speedEvasion": React.createRef()
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
			if(index == 0) {
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
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Movement</th>
			</tr>
			<tr>
				<td>Overland</td>
				<td>{species.overland}</td>
			</tr>
			<tr>
				<td>Swimming</td>
				<td>{species.swimming}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Health</th>
			</tr>
			<tr>
				<td>Max HP</td>
				<td>{pokemon.level + (constitution * 3) + 10}</td>
			</tr>
			<tr>
				<td>Current HP</td>
				<td><input type="number" defaultValue={pokemon.current_hp} /></td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Stats</th>
				<th>Combat Stage</th>
				<th>Total</th>
			</tr>
			<tr>
				<td>Constitution</td>
				<td>{constitution}</td>
			</tr>
			<tr>
				<td>Attack</td>
				<td ref={this.statRefs["attackStat"]}>{attack}</td>
				<td><input ref={this.statRefs["attackCS"]} type="number" defaultValue={0} onChange={() => this.updateAttack()}/></td>
				<td ref={this.statRefs["attackTotal"]}>{attack}</td>
			</tr>
			<tr>
				<td>Defense</td>
				<td ref={this.statRefs["defenseStat"]}>{defense}</td>
				<td><input ref={this.statRefs["defenseCS"]} type="number" defaultValue={0} onChange={() => this.updateDefense()}/></td>
				<td ref={this.statRefs["defenseTotal"]}>{defense}</td>
			</tr>
			<tr>
				<td>Special Attack</td>
				<td ref={this.statRefs["specialAttackStat"]}>{specialAttack}</td>
				<td><input ref={this.statRefs["specialAttackCS"]} type="number" defaultValue={0} onChange={() => this.updateSpecialAttack()}/></td>
				<td ref={this.statRefs["specialAttackTotal"]}>{specialAttack}</td>
			</tr>
			<tr>
				<td>Special Defense</td>
				<td ref={this.statRefs["specialDefenseStat"]}>{specialDefense}</td>
				<td><input ref={this.statRefs["specialDefenseCS"]} type="number" defaultValue={0} onChange={() => this.updateSpecialDefense()}/></td>
				<td ref={this.statRefs["specialDefenseTotal"]}>{specialDefense}</td>
			</tr>
			<tr>
				<td>Speed</td>
				<td ref={this.statRefs["speedStat"]}>{speed}</td>
				<td><input ref={this.statRefs["speedCS"]} type="number" defaultValue={0} onChange={() => this.updateSpeed()}/></td>
				<td ref={this.statRefs["speedTotal"]}>{speed}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Evasion</th>
			</tr>
			<tr>
				<td>Physical</td>
				<td ref={this.statRefs["physicalEvasion"]}>{Math.floor(defense / 5)}</td>
			</tr>
			<tr>
				<td>Special</td>
				<td ref={this.statRefs["specialEvasion"]}>{Math.floor(specialDefense / 5)}</td>
			</tr>
			<tr>
				<td>Speed</td>
				<td ref={this.statRefs["speedEvasion"]}>{Math.floor(speed / 5)}</td>
			</tr>
		</tbody>
	</table>
</div>
		);
	}

	getMultiplier(cs)
	{
		let multiplier = 1;
		switch(parseInt(cs))
		{
			case -6:
				multiplier = 0.25;
				break
			case -5:
				multiplier = 2/7;
				break
			case -4:
				multiplier = 2/6;
				break
			case -3:
				multiplier = 0.4;
				break
			case -2:
				multiplier = 0.5;
				break
			case -1:
				multiplier = 2/3;
				break
			case 0:
				multiplier = 1;
				break
			case 1:
				multiplier = 1.5;
				break
			case 2:
				multiplier = 2;
				break
			case 3:
				multiplier = 2.5;
				break
			case 4:
				multiplier = 3;
				break
			case 5:
				multiplier = 3.5;
				break
			case 6:
				multiplier = 4;
				break
			default:
				multiplier = 1;
			}
			return multiplier;
	}

	updateAttack() {
		let baseAttack = this.statRefs["attackStat"].current.innerHTML;
		let cs = this.statRefs["attackCS"].current.value;
		let multiplier = this.getMultiplier(cs);
		let total = Math.floor(baseAttack * multiplier);
		this.statRefs["attackTotal"].current.innerHTML = total;
	}

	updateDefense() {
		let baseDefense = this.statRefs["defenseStat"].current.innerHTML;
		let cs = this.statRefs["defenseCS"].current.value;
		let multiplier = this.getMultiplier(cs);
		let total = Math.floor(baseDefense * multiplier);
		this.statRefs["defenseTotal"].current.innerHTML = total;
		let evasion = Math.floor(total / 5);
		this.statRefs["physicalEvasion"].current.innerHTML = evasion;
	}

	updateSpecialAttack() {
		let baseSpecialAttack = this.statRefs["specialAttackStat"].current.innerHTML;
		let cs = this.statRefs["specialAttackCS"].current.value;
		let multiplier = this.getMultiplier(cs);
		let total = Math.floor(baseSpecialAttack * multiplier);
		this.statRefs["specialAttackTotal"].current.innerHTML = total;
	}

	updateSpecialDefense() {
		let baseSpecialDefense = this.statRefs["specialDefenseStat"].current.innerHTML;
		let cs = this.statRefs["specialDefenseCS"].current.value;
		let multiplier = this.getMultiplier(cs);
		let total = Math.floor(baseSpecialDefense * multiplier);
		this.statRefs["specialDefenseTotal"].current.innerHTML = total;
		let evasion = Math.floor(total / 5);
		this.statRefs["specialEvasion"].current.innerHTML = evasion;
	}

	updateSpeed() {
		let baseSpeed = this.statRefs["speedStat"].current.innerHTML;
		let cs = this.statRefs["speedCS"].current.value;
		let multiplier = this.getMultiplier(cs);
		let total = Math.floor(baseSpeed * multiplier);
		this.statRefs["speedTotal"].current.innerHTML = total;
		let evasion = Math.floor(total / 5);
		this.statRefs["speedEvasion"].current.innerHTML = evasion;
	}
}

export default PokemonCombatTab;
