import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";

class TrainerSkillsTab extends TabbedComponent {
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
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Body</th>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.acrobatics, 6, 0)}>
				<td>Acrobatics</td>
				<td>{trainer.acrobatics}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.athletics, 6, 0)}>
				<td>Athletics</td>
				<td>{trainer.athletics}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.combat, 6, 0)}>
				<td>Combat</td>
				<td>{trainer.combat}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.intimidate, 6, 0)}>
				<td>Intimidate</td>
				<td>{trainer.intimidate}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.stealth, 6, 0)}>
				<td>Stealth</td>
				<td>{trainer.stealth}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.survival, 6, 0)}>
				<td>Survival</td>
				<td>{trainer.survival}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Mind</th>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.gen_education, 6, 0)}>
				<td>General Education</td>
				<td>{trainer.gen_education}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.med_education, 6, 0)}>
				<td>Medical Education</td>
				<td>{trainer.med_education}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.occ_education, 6, 0)}>
				<td>Occult Education</td>
				<td>{trainer.occ_education}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.pok_education, 6, 0)}>
				<td>Pokemon Education</td>
				<td>{trainer.pok_education}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.tec_education, 6, 0)}>
				<td>Technology Education</td>
				<td>{trainer.tec_education}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.guile, 6, 0)}>
				<td>Guile</td>
				<td>{trainer.guile}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.perception, 6, 0)}>
				<td>Perception</td>
				<td>{trainer.perception}</td>
			</tr>
		</tbody>
	</table>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="2">Spirit</th>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.charm, 6, 0)}>
				<td>Charm</td>
				<td>{trainer.charm}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.command, 6, 0)}>
				<td>Command</td>
				<td>{trainer.command}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.focus, 6, 0)}>
				<td>Focus</td>
				<td>{trainer.focus}</td>
			</tr>
			<tr onClick={() => this.props.rollDie(trainer.intuition, 6, 0)}>
				<td>Intuition</td>
				<td>{trainer.intuition}</td>
			</tr>
		</tbody>
	</table>
</div>
		);
	}
}

export default TrainerSkillsTab;
