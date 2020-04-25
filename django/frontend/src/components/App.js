import React, { Component } from "react";
import ReactDOM from "react-dom";
import Trainer from "./views/Trainer";
import Game from "./views/Game";
import TrainerList from "./views/TrainerList";
import GameList from "./views/GameList";

class App extends Component {
	constructor(props) {
		super(props);
		this.state = {
			trainer: null,
			game: null
		};
	}

	render() {
		const trainer = this.state.trainer;
		const game = this.state.game;
		if (trainer == null && game == null) {
			return (
				<div style={{width: "100%"}}>
					<p>Trainers:</p>
					<TrainerList selectionMade={(state) => this.selectionMade(state)}/>
					<p>Games:</p>
					<GameList selectionMade={(state) => this.selectionMade(state)}/>
				</div>
			);
		}
		else if(game != null) {
			return (
				<Game game={game}/>
			);
		}
		else if(trainer != null) {
			return (
				<Trainer
					trainer={trainer}
					gmView={false}
					display={true}/>
			);
		}
	}

	selectionMade(state) {
		this.setState(state);
	}
}
const wrapper = document.getElementById("app");
wrapper ? ReactDOM.render(<App />, wrapper) : null;
