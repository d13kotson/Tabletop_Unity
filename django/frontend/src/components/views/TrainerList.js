import React, { Component } from "react";

class TrainerList extends Component {
	constructor(props) {
		super(props);
		this.state = {
			"trainers": [],
			"dataLoaded": false
		};
		this.getTrainers();
	}

	async getTrainers() {
		await fetch("/api/trainers/")
			.then(response => {
				if (response.status === 200) {
					return response.json();
				}
				else {
					document.location.href = "/accounts/login";
				}
			})
			.then(data => {
				this.setState({
					"trainers": data,
					"dataLoaded": true
				});
			});
	}

	render() {
		const trainers = this.state.trainers;
		const dataLoaded = this.state.dataLoaded;
		if(dataLoaded) {
		return (
<div>
	<ul>
			{ trainers.map(function(trainer, index, arr) {
				return <li onClick={() => this.props.selectionMade({trainer: trainer.id})} key={index}>{trainer.name}</li>;
			}, this)}
	</ul>
</div>
		);
		}
		else {
			return (
<div>
	<p>Loading...</p>
</div>
			);
		}
	}
}

export default TrainerList;
