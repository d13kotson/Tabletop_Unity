import React, { Component } from "react";
import Cookies from 'js-cookie';

class TrainerAddFeature extends Component {
	constructor(props) {
		super(props);
		this.state = {
			"features": [],
			"dataLoaded": false,
		}
		this.getFeatures();
	}

	async getFeatures() {
	    const trainer = this.props.trainer
		let features = [];
		await fetch("/api/features/?game=" + trainer.game)
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				features = data;
			});
		this.setState({
			"features": features,
			"dataLoaded": true,
		});
	}

	render() {
		const trainer = this.props.trainer;
		const features = this.state.features;
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
				<th>Tags</th>
				<th>Frequency</th>
				<th>Trigger</th>
				<th>Notes</th>
			</tr>
			{features.map((feature, index) => {
				return (
					<tr key={index}>
						<td>{feature.name}</td>
						<td>{feature.prerequisites}</td>
						<td>{feature.effect}</td>
						<td>{feature.tags}</td>
						<td>{feature.frequency}</td>
						<td>{feature.trigger}</td>
						<td>{feature.notes}</td>
						<td><a className="button" onClick={() => this.addFeature(feature)}>Add Feature</a></td>
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

	async addFeature(feature) {
		let trainer = this.props.trainer;
		let trainerFeature = {
			"trainer": trainer.id,
			"feature": feature.id
		}

		await fetch("api/addTrainerFeature/", {
		  method: 'POST',
		  body: JSON.stringify(trainerFeature),
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

export default TrainerAddFeature;
