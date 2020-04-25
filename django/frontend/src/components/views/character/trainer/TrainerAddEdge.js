import React, { Component } from "react";
import Cookies from 'js-cookie';

class TrainerAddEdge extends Component {
	constructor(props) {
		super(props);
		this.state = {
			"edges": [],
			"dataLoaded": false,
		}
		this.getEdges();
	}

	async getEdges() {
		let edges = [];
		await fetch("/api/edges/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				edges = data;
			});
		this.setState({
			"edges": edges,
			"dataLoaded": true,
		});
	}

	render() {
		const trainer = this.props.trainer;
		const edges = this.state.edges;
		const dataLoaded = this.state.dataLoaded;
		if(dataLoaded) {
			return (
<div style={{width: "100%", position: "absolute", backgroundColor: "white", borderRight: "2px solid gray", borderBottom: "2px solid gray", top: "5%", left: "2%", width: "90%", height: "80%", zIndex: "1"}} className='scrollable'>
	<table className="stat-block">
		<tbody>
			<tr>
				<th>Name</th>
				<th>Prerequisite</th>
				<th>Effect</th>
				<th>Notes</th>
			</tr>
			{edges.map((edge, index) => {
				return (
					<tr key={index}>
						<td>{edge.name}</td>
						<td>{edge.prerequisite}</td>
						<td>{edge.effect}</td>
						<td>{edge.notes}</td>
						<td><a className="button" onClick={() => this.addEdge(edge)}>Add Edge</a></td>
					</tr>
				);
			})}
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

	async addEdge(edge) {
		let trainer = this.props.trainer;
		let trainerEdge = {
			"trainer": trainer.id,
			"edge": edge.id
		}

		await fetch("api/addTrainerEdge/", {
		  method: 'POST',
		  body: JSON.stringify(trainerEdge),
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

export default TrainerAddEdge;
