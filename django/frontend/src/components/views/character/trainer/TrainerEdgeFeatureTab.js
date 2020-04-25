import React, { Component } from "react";
import TabbedComponent from "./../../TabbedComponent";
import TrainerAddEdge from "./TrainerAddEdge.js";
import TrainerAddFeature from "./TrainerAddFeature.js";

class TrainerEdgeFeatureTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.state = {
			"addEdge": false,
			"addFeature": false,
		};
	}

	render() {
		const trainer = this.props.trainer;
		const edges = trainer.trainerEdge;
		const features = trainer.trainerFeature;
		const addEdge = this.state.addEdge;
		const addFeature = this.state.addFeature;
		const classProp = this.props.class + " tabContent";
		if(addEdge) {
			return (
<TrainerAddEdge trainer={trainer} onComplete={() => this.addEdge()}/>
			);
		}
		else if(addFeature) {
			return (
<TrainerAddFeature trainer={trainer} onComplete={() => this.addFeature()}/>
			);
		}
		else {
			return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="3">Edges</th>
			</tr>
			<tr>
			    <th>Name</th>
			    <th>Effect</th>
			    <th>Notes</th>
			</tr>
			{ edges.map(function(edge, index) {
				return (
					<tr key={index}>
						<td>{edge.edge.name}</td>
						<td>{edge.edge.effect}</td>
						<td>{edge.notes}</td>
					</tr>
				);
			})}
		</tbody>
	</table>
	<a className="button" onClick={() => this.setState({"addEdge": true})}>Add Edge</a>
	<table className="stat-block">
		<tbody>
			<tr>
				<th colSpan="6">Features</th>
			</tr>
			<tr>
			    <th>Name</th>
			    <th>Effect</th>
			    <th>Tags</th>
			    <th>Frequency/Cost</th>
			    <th>Trigger</th>
			    <th>Notes</th>
			</tr>
			{ features.map(function(feature, index) {
				return (
					<tr key={index}>
						<td>{feature.feature.name}</td>
						<td>{feature.feature.effect}</td>
						<td>{feature.feature.tags}</td>
						<td>{feature.feature.frequency}</td>
						<td>{feature.feature.trigger}</td>
						<td>{feature.feature.notes}</td>
					</tr>
				);
			})}
		</tbody>
	</table>
	<a className="button" onClick={() => this.setState({"addFeature": true})}>Add Feature</a>
</div>
			);
		}
	}

	addEdge() {
		this.setState({"addEdge": false});
		this.props.update();
	}

	addFeature() {
		this.setState({"addFeature": false});
		this.props.update();
	}
}

export default TrainerEdgeFeatureTab;
