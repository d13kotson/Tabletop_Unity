import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Cookies from 'js-cookie';

class BoxTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		const pokemon = this.props.trainer.pokemon;
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			{pokemon.map((poke, index, arr) => {
				if(!poke.inParty) {
					return (
						<tr key={index}>
							<td>{poke.name}</td>
							<td><a className="button" onClick={() => this.addToParty(poke)}>Add to Party</a></td>
						</tr>
					);
				}
			}, this)}
		</tbody>
	</table>
</div>
		);
	}

	async addToParty(pokemon) {
		let pokemonUpdate = {};
		await fetch("/api/pokemon/" + pokemon.id + "/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				pokemonUpdate = data;
			});
		pokemonUpdate.inParty = true;
		fetch("/api/pokemon/" + pokemon.id + "/", {
		  method: 'PUT',
		  body: JSON.stringify(pokemonUpdate),
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

export default BoxTab;
