import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Map from "./../modules/Map";

class MapTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
	}

	render() {
		let roomName = this.props.roomName
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<Map roomName={roomName} gmView={false}/>
</div>
		);
	}
}

export default MapTab;
