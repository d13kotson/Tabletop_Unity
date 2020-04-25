import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class TrainerCombatTab extends TabbedComponent {
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
		const trainer = this.props.trainer;
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
				<td>{Math.floor(3 + (trainer.athletics + trainer.acrobatics) / 2)}</td>
			</tr>
			<tr>
				<td>Swimming</td>
				<td>{Math.floor((3 + (trainer.athletics + trainer.acrobatics) / 2) / 2)}</td>
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
				<td>{trainer.level * 2 + (trainer.constitution * 3) + 10}</td>
			</tr>
			<tr>
				<td>Current HP</td>
				<td><input type="number" defaultValue={trainer.current_hp} /></td>
			</tr>
			<tr>
				<td>Max AP</td>
				<td>{Math.floor(5 + trainer.level / 5)}</td>
			</tr>
			<tr>
				<td>Current AP</td>
				<td><input type="number" defaultValue={Math.floor(5 + trainer.level / 5)} /></td>
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
				<td>{trainer.constitution}</td>
			</tr>
			<tr>
				<td>Attack</td>
				<td ref={this.statRefs["attackStat"]}>{trainer.attack}</td>
				<td><input ref={this.statRefs["attackCS"]} type="number" defaultValue={0} onChange={() => this.updateAttack()}/></td>
				<td ref={this.statRefs["attackTotal"]}>{trainer.attack}</td>
			</tr>
			<tr>
				<td>Defense</td>
				<td ref={this.statRefs["defenseStat"]}>{trainer.defense}</td>
				<td><input ref={this.statRefs["defenseCS"]} type="number" defaultValue={0} onChange={() => this.updateDefense()}/></td>
				<td ref={this.statRefs["defenseTotal"]}>{trainer.defense}</td>
			</tr>
			<tr>
				<td>Special Attack</td>
				<td ref={this.statRefs["specialAttackStat"]}>{trainer.special_attack}</td>
				<td><input ref={this.statRefs["specialAttackCS"]} type="number" defaultValue={0} onChange={() => this.updateSpecialAttack()}/></td>
				<td ref={this.statRefs["specialAttackTotal"]}>{trainer.special_attack}</td>
			</tr>
			<tr>
				<td>Special Defense</td>
				<td ref={this.statRefs["specialDefenseStat"]}>{trainer.special_defense}</td>
				<td><input ref={this.statRefs["specialDefenseCS"]} type="number" defaultValue={0} onChange={() => this.updateSpecialDefense()}/></td>
				<td ref={this.statRefs["specialDefenseTotal"]}>{trainer.special_defense}</td>
			</tr>
			<tr>
				<td>Speed</td>
				<td ref={this.statRefs["speedStat"]}>{trainer.speed}</td>
				<td><input ref={this.statRefs["speedCS"]} type="number" defaultValue={0} onChange={() => this.updateSpeed()}/></td>
				<td ref={this.statRefs["speedTotal"]}>{trainer.speed}</td>
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
				<td ref={this.statRefs["physicalEvasion"]}>{Math.floor(trainer.defense / 5)}</td>
			</tr>
			<tr>
				<td>Special</td>
				<td ref={this.statRefs["specialEvasion"]}>{Math.floor(trainer.special_defense / 5)}</td>
			</tr>
			<tr>
				<td>Speed</td>
				<td ref={this.statRefs["speedEvasion"]}>{Math.floor(trainer.speed / 5)}</td>
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

export default TrainerCombatTab;
