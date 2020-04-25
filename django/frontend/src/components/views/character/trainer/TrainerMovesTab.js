import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";
import TrainerAddMove from "./TrainerAddMove";

class TrainerMovesTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.state = {
			"addMove": false,
		}
	}

	render() {
		const trainer = this.props.trainer;
		const moves = trainer.trainerAttack;
		const addMove = this.state.addMove;
		const classProp = this.props.class + " tabContent";
		if(addMove) {
			return (
<TrainerAddMove trainer={trainer} onComplete={() => this.addMove()}/>
			);
		}
		else {
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
	<a className="button" onClick={() => this.setState({"addMove": true})}>Add Move</a>
</div>
			);
		}
	}

	addMove() {
		this.setState({"addMove": false});
		this.props.update();
	}
}

export default TrainerMovesTab;
