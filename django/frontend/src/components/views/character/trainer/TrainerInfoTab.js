import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";
import Cookies from 'js-cookie';

class TrainerInfoTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		const trainer = this.props.trainer;
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp} style={{display: "block"}}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th>Name</th>
				<td colSpan="2">{trainer.name}</td>
			</tr>
			<tr>
				<th>Level</th>
				<td>{trainer.level}</td>
				<td><a className="button" onClick={() => this.levelUp()}>Level Up</a></td>
			</tr>
		</tbody>
	</table>
</div>
		);
	}

	async levelUp() {
		let trainer = this.props.trainer;
		trainer.level += 1;

		fetch("/api/trainer/" + trainer.id + "/", {
		  method: 'PUT',
		  body: JSON.stringify(trainer),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return response.json();
		})
		.then(data => {
			this.props.update();
		});
	}
}

export default TrainerInfoTab;
