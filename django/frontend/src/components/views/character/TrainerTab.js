import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import TrainerInfoTab from "./trainer/TrainerInfoTab";
import TrainerSkillsTab from "./trainer/TrainerSkillsTab";
import TrainerCombatTab from "./trainer/TrainerCombatTab";
import TrainerEdgeFeatureTab from "./trainer/TrainerEdgeFeatureTab";
import TrainerMovesTab from "./trainer/TrainerMovesTab";

class TrainerTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
			"infoTab": React.createRef(),
			"skillsTab": React.createRef(),
			"combatTab": React.createRef(),
			"edgeFeatureTab": React.createRef(),
			"movesTab": React.createRef(),
		};
	}

	render() {
		const trainer = this.props.trainer;
		const classProp = this.props.class + " tabContent";
		const subClass = trainer.id + "TrainerTabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp} style={{display: "block"}}>
	<div className="lightTab">
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'infoTab')}>Info</a>
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'skillsTab')}>Skills</a>
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'combatTab')}>Combat</a>
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'edgeFeatureTab')}>Edges & Features</a>
		<a className="tablink" onClick={() => this.openTab(event, subClass, 'movesTab')}>Moves</a>
	</div>

	<TrainerInfoTab class={subClass} ref={this.tabRefs["infoTab"]} trainer={trainer} update={this.props.update}/>
	<TrainerSkillsTab class={subClass} ref={this.tabRefs["skillsTab"]} trainer={trainer} update={this.props.update} rollDie={this.props.rollDie}/>
	<TrainerCombatTab class={subClass} ref={this.tabRefs["combatTab"]} trainer={trainer} update={this.props.update}/>
	<TrainerEdgeFeatureTab class={subClass} ref={this.tabRefs["edgeFeatureTab"]} trainer={trainer} update={this.props.update}/>
	<TrainerMovesTab class={subClass} ref={this.tabRefs["movesTab"]} trainer={trainer} update={this.props.update}/>
</div>
		);
	}
}

export default TrainerTab;
