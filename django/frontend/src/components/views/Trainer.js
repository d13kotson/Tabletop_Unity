import React, { Component } from "react";
import TabbedComponent from "./TabbedComponent";
import PartyTab from "./character/PartyTab";
import BoxTab from "./character/BoxTab";
import InventoryTab from "./character/InventoryTab";
import MapTab from "./character/MapTab";
import Chat from "./modules/Chat";

class Trainer extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"partyTab": React.createRef(),
			"boxTab": React.createRef(),
			"inventoryTab": React.createRef(),
			"mapTab": React.createRef(),
			"chat": React.createRef()
		};
		this.state = {
			"trainer": this.props.trainer,
			"dataLoaded": this.props.gmView,
		};
		if(!this.props.gmView) {
			this.getTrainerInfo(this.props.trainer);
		}
	}

	async getTrainerInfo(trainerID) {
		await fetch("/api/trainer/" + trainerID)
			.then(response => {
				if (response.status === 200) {
					return response.json();
				}
			})
			.then(data => {
				this.setState({
					"trainer": data,
					"dataLoaded": true
				});
			});
	}

	rollDie(numDie, dieNum, addition) {
	    let chat = this.tabRefs["chat"].current;
	    if(chat != null) {
	        chat.sendMessage("/roll " + numDie + "d" + dieNum + "+" + addition);
	    }
	}

	render() {
		const dataLoaded = this.state.dataLoaded;
		const trainer = this.state.trainer;
		const style = this.props.display ? {display: "flex", width: "100%"} : {width: "100%"};
		const gmView = this.props.gmView;
		let classProp = this.props.class;
		if(classProp === "") {
			classProp = trainer.name + "TabContent";
		}
		classProp += " tabContent"
		if(dataLoaded) {
		const subClass = trainer.name + 'TabContent';
			if(gmView)
			{
// Need to check for whether this is loaded as a part of the game view. If not game view set style={{"display": block}} otherwise tabContent sets display to none.
				return (
<div ref={this.tabRefs["self"]} style={style} className={classProp}>
	<div>
		<div className="lightTab">
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'partyTab')}>Party</a>
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'boxTab')}>Box</a>
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'inventoryTab')}>Inventory</a>
		</div>
		<PartyTab class={subClass} ref={this.tabRefs["partyTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)}/>
		<BoxTab class={subClass} ref={this.tabRefs["boxTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)}/>
		<InventoryTab class={subClass} ref={this.tabRefs["inventoryTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)}/>
	</div>
</div>
				);
			}
			else
			{
// Need to check for whether this is loaded as a part of the game view. If not game view set style={{"display": block}} otherwise tabContent sets display to none.
				return (
<div ref={this.tabRefs["self"]} style={style} className={classProp}>
	<div style={{width: "80%", display: "table-cell"}}>
		<div className="lightTab">
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'partyTab')}>Party</a>
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'boxTab')}>Box</a>
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'inventoryTab')}>Inventory</a>
			<a className="tablink" onClick={() => this.openTab(event, subClass, 'mapTab')}>Map</a>
		</div>
		<PartyTab class={subClass} ref={this.tabRefs["partyTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)} rollDie={(n, m, a) => this.rollDie(n, m, a)}/>
		<BoxTab class={subClass} ref={this.tabRefs["boxTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)}/>
		<InventoryTab class={subClass} ref={this.tabRefs["inventoryTab"]} trainer={trainer} update={() => this.getTrainerInfo(trainer.id)}/>
		<MapTab class={subClass} ref={this.tabRefs["mapTab"]} roomName={trainer.game}/>
	</div>
	<div style={{flexGrow: "1", width: "20%", display: "table-cell"}}>
		<Chat class={subClass} ref={this.tabRefs["chat"]} roomName={trainer.game} displayName={trainer.name} gmView={gmView}/>
	</div>
</div>
				);
			}
		}
		else {
			return(
<div ref={this.tabRefs["self"]} style={style} className={classProp}>
	<p>Loading...</p>
</div>
			);
		}
	}
}

export default Trainer;
