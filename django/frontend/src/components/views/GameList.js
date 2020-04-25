import React, { Component } from "react";

class GameList extends Component {
	constructor(props) {
		super(props);
		this.state = {
			"games": [],
			"dataLoaded": false
		};
		this.getGames();
	}

	async getGames() {
		await fetch("/api/games/")
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
					"games": data,
					"dataLoaded": true
				});
			});
	}

	render() {
		const games = this.state.games;
		const dataLoaded = this.state.dataLoaded;
		if(dataLoaded) {
			return (
<div>
	<ul>
				{ games.map(function(game, index, arr) {
					return <li onClick={() => this.props.selectionMade({game: game.id})} key={index}>{game.title}</li>;
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

export default GameList;
