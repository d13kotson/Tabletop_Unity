import React, { Component } from "react";
import TabbedComponent from "./TabbedComponent";
import Trainer from "./Trainer";
import CalculatorTab from "./game/CalculatorTab";
import WildPokemonTab from "./game/WildPokemonTab";
import MapTab from "./game/MapTab";
import MapEditorTab from "./game/MapEditorTab";
import Chat from "./modules/Chat";

class Game extends TabbedComponent {
	constructor(props) {
		super(props);
		this.state = {
			"game": this.props.game,
			"dataLoaded": false,
		};
		this.tabRefs = {
			"calculatorTab": React.createRef(),
			"wildPokemonTab": React.createRef(),
			"mapTab": React.createRef(),
			"mapEditorTab": React.createRef(),
			"chatTab": React.createRef()
		}
		this.getGameInfo(this.props.game);
	}

	async getGameInfo(gameID) {
		await fetch("/api/game/" + gameID)
			.then(response => {
				if (response.status === 200) {
					return response.json();
				}
			})
			.then(data => {
				this.setState({
					"game": data,
					"dataLoaded": true
				});
			});
	}

	render() {
		const game = this.state.game;
		const dataLoaded = this.state.dataLoaded;
		if(dataLoaded) {
			for(let trainer of game.trainer) {
				this.tabRefs["Trainer" + trainer.id] = React.createRef();
			}
			return (
<div style={{width: "100%", display: "flex"}}>
    <div style={{width: "80%"}}>
        <div className="darkTab">
            {game.trainer.map(function(trainer, index, arr) {
                return (
                    <a key={index} className="tablink" onClick={() => this.openTab(event, game.title + "TabContent", "Trainer" + trainer.id)}>{trainer.name}</a>
                );
            }, this)}
            <a className="tablink" onClick={() => this.openTab(event, game.title + "TabContent", "calculatorTab")}>Calculator</a>
            <a className="tablink" onClick={() => this.openTab(event, game.title + "TabContent", "wildPokemonTab")}>Wild Pokemon</a>
            <a className="tablink" onClick={() => this.openTab(event, game.title + "TabContent", "mapTab")}>Map</a>
            <a className="tablink" onClick={() => this.openTab(event, game.title + "TabContent", "mapEditorTab")}>Map Editor</a>
        </div>
        {game.trainer.map(function(trainer, index, arr) {
            return (
                <Trainer class={game.title + "TabContent"} key={index} class={game.title + "TabContent"} ref={this.tabRefs["Trainer" + trainer.id]} trainer={trainer} gmView={true} display={index === 0}/>
            );
        }, this)}
        <CalculatorTab class={game.title + "TabContent"} ref={this.tabRefs["calculatorTab"]} game={game.id} update={() => this.getGameInfo(game.id)}/>
        <WildPokemonTab class={game.title + "TabContent"} ref={this.tabRefs["wildPokemonTab"]} game={game}/>
        <MapTab class={game.title + "TabContent"} ref={this.tabRefs["mapTab"]} roomName={game.id} game={game}/>
        <MapEditorTab class={game.title + "TabContent"} ref={this.tabRefs["mapEditorTab"]} game={game} update={() => this.getGameInfo(game.id)}/>
    </div>
	<div style={{flexGrow: "1", width: "20%", display: "table-cell"}}>
        <Chat class={game.title + "TabContent"} ref={this.tabRefs["chatTab"]} roomName={game.id} displayName="GM" gmView={true}/>
	</div>
</div>
			);
		}
		else {
			return (
<div style={{width: "100%"}}>
	<p>Loading...</p>
</div>
			);
		}
	}
}

export default Game;
