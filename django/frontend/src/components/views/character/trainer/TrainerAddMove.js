import React, { Component } from "react";
import Cookies from 'js-cookie';

class TrainerAddMove extends Component {
	constructor(props) {
		super(props);
		this.state = {
			"edges": [],
			"dataLoaded": false,
		}
		this.getMoves();
	}

	async getMoves() {
		let moves = [];
		await fetch("/api/attacks/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				moves = data;
			});
		this.setState({
			"moves": moves,
			"dataLoaded": true,
		});
	}

	render() {
		const trainer = this.props.trainer;
		const moves = this.state.moves;
		const dataLoaded = this.state.dataLoaded;
		if(dataLoaded) {
			return (
<div style={{width: "100%", position: "absolute", backgroundColor: "white", borderRight: "2px solid gray", borderBottom: "2px solid gray", top: "5%", left: "2%", width: "90%", height: "80%", zIndex: "1"}}>
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
			</tr>
			{moves.map(function(move, index, arr) {
				return (
					<tr key={index}>
						<td>{move.name}</td>
						<td>{move.type}</td>
						<td>{move.attackClass}</td>
						<td>{move.frequency}</td>
						<td>{move.damageBase}</td>
						<td>{move.range}</td>
						<td>{move.effect}</td>
						<td><a className="button" onClick={() => this.addMove(move)}>Add Move</a></td>
					</tr>
				);
			}, this)}
		</tbody>
	</table>
</div>
			);
		}
		else {
			return (
<div style={{width: "100%", position: "absolute", backgroundColor: "white", borderRight: "2px solid gray", borderBottom: "2px solid gray", top: "5%", left: "2%", width: "90%", height: "80%", zIndex: "1"}}>
	<p>Loading...</p>
</div>
			)
		}
	}

	async addMove(move) {
		let trainer = this.props.trainer;
		let trainerAttack = {
			"trainer": trainer.id,
			"attack": move.id
		}

		await fetch("api/addTrainerAttack/", {
		  method: 'POST',
		  body: JSON.stringify(trainerAttack),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return response.json();
		});
		this.props.onComplete();
	}
}

export default TrainerAddMove;
