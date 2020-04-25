import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Map from "./../modules/Map";

class MapTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.inputRefs = {
			"backgroundSelect": React.createRef(),
			"tokenSelect": React.createRef()
		};
		this.state = {
			"dataLoaded": false
		};
		this.getMapOptions();
		this.chatSocket = new WebSocket(
			"ws://" + window.location.host + "/ws/map/" + this.props.roomName
		);
		this.chatSocket.onclose = function(e) {
			console.error("Chat socket closed unexpectedly");
		};
	}

	async getMapOptions() {
		await fetch("/map/backgrounds/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				this.setState({
					"backgrounds": data
				});
			});
		await fetch("/map/tokens/")
			.then(response => {
				if (response.status !== 200) {
					return this.setState({ placeholder: "Something went wrong."});
				}
				return response.json();
			})
			.then(data => {
				this.setState({
					"tokens": data,
					"dataLoaded": true
				});
			});
	}

	render() {
		const dataLoaded = this.state.dataLoaded;
		const roomName = this.props.roomName;
		const classProp = this.props.class + " tabContent";
		if(dataLoaded) {
			const backgrounds = this.state.backgrounds;
			const tokens = this.state.tokens;
			return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<Map roomName={roomName} gmView={true}/>
	<div>
		<select ref={this.inputRefs["backgroundSelect"]}>
			<option value="0">Please select an option</option>
			{backgrounds.map(function(background, index) {
				return (
					<option key={index} value={background.id}>{background.title}</option>
				);
			})}
		</select>
		<a className="button" onClick={() => this.setBackground()}>Set Background</a>
	</div>
	<div>
		<select ref={this.inputRefs["tokenSelect"]}>
			<option value="0">Please select an option</option>
			{tokens.map(function(token, index) {
				return (
					<option key={index} value={token.id}>{token.title}</option>
				);
			})}
		</select>
		<a onClick={() => this.addToken()}>Add Token</a>
	</div>
	<div>
	    <a onClick={() => this.clearState()}>Clear Map</a>
	</div>
</div>
			);
		}
		else {
			return (
<div ref={this.tabRefs["self"]} className={classProp} />
			);
		}
	}

	setBackground() {
		let backgroundSource = this.inputRefs["backgroundSelect"].current.value;
		this.chatSocket.send(JSON.stringify({
			"type": "setBackground",
			"content": backgroundSource
		}));
	}

	addToken() {
		let tokenSource = this.inputRefs["tokenSelect"].current.value;
		this.chatSocket.send(JSON.stringify({
			"type": "addToken",
			"content": tokenSource
		}));
	}
	
	clearState() {
	    this.chatSocket.send(JSON.stringify({
	        "type": "clearState",
	        "content": ""
	    }))
	}
}

export default MapTab;
