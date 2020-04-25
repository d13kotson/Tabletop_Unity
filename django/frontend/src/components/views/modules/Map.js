import React, { Component } from "react";

class Map extends Component {
	constructor(props) {
		super(props);
		this.chatSocket = new WebSocket(
			"ws://" + window.location.host + "/ws/map/" + this.props.roomName
		);
		this.mapRefs = {
			"mapCanvas": React.createRef()
		};
		this.state = {
			"zoomScale": 1,
			"offset": {
				"x": 0,
				"y": 0,
			},
			"background": new Image(),
			"tokens": [],
			"movingToken": -1,
			"movingMap": false
		};
		this.chatSocket.parseMessage = (type, content) => this.parseMessage(type, content);
		this.chatSocket.onmessage = function(e) {
			let data = JSON.parse(e.data);
			let type = data["type"];
			let content = data["content"];
			this.parseMessage(type, content);
		};
		this.chatSocket.onclose = function(e) {
			console.error('Chat socket closed unexpectedly');
		};
		this.chatSocket.onopen = (e) => this.sendMessage('requestState', '');
	}

	sendMessage(type, content) {
		this.chatSocket.send(JSON.stringify({
			'type': type,
			'content': content
		}));
	}

	parseMessage(type, content) {
		switch(type) {
			case "setBackground":
				this.setBackground(content);
				break;
			case "addToken":
				this.addToken(content);
				break;
			case "updateToken":
				this.updateToken(content);
				break;
			case "updateState":
				this.updateState(content);
				break;
		}
	}

	setBackground(content) {
	    let background = new Image();
	    background.src = content;
		this.setState({
			"background": background
		});
		background.onload = () => this.drawMap();
	}

	addToken(content) {
		let token = {
			image: new Image(),
			x: 0,
			y: 0,
			width: content.width,
			height: content.height
		};
		token.image.src = content.src;
		let tokens = [...this.state.tokens];
		tokens.push(token);
		this.setState({
			"tokens": tokens
		});
		token.image.onload = () => this.drawMap();
	}

	updateToken(content) {
		let tokens = this.state.tokens;
		tokens[content.tokenID].x = content.tokenX;
		tokens[content.tokenID].y = content.tokenY;
		this.setState({
			"tokens": tokens
		});
		this.drawMap();
	}

	updateState(content) {
	    let state = this.state;
	    state.background = new Image();
	    state.background.src = content.background;
	    state.tokens = [];
	    for(let tokenID in content.tokens) {
	        let token = content.tokens[tokenID];
	        let newToken = {
	            image: new Image(),
                x: token.x,
                y: token.y,
                width: token.width,
                height: token.height
	        }
	        newToken.image.src = token.source;
	        state.tokens.push(newToken);
	    }
	    this.setState(state);
	    state.background.onload = () => this.drawMap();
	}

	drawMap() {
		const zoomScale = this.state.zoomScale;
		const offset = this.state.offset;
		let canvas = this.mapRefs["mapCanvas"].current;
		let context = canvas.getContext("2d");
		const background = this.state.background;
		context.clearRect(0, 0, canvas.clientWidth, canvas.clientHeight);
		context.drawImage(background,
			0, 0,
			background.width, background.height,
			offset.x, offset.y,
			background.width * zoomScale, background.height * zoomScale);
		const tokens = this.state.tokens;
		for(let i = 0; i < tokens.length; i++) {
			let token = tokens[i];
			let coord = this.mapCoordToCanvasCoord({"x": token.x, "y": token.y});
			context.drawImage(token.image,
				coord.x, coord.y,
				token.width * zoomScale, token.height * zoomScale);
		}
	}

	render() {
		return (
<div>
	<canvas ref={this.mapRefs["mapCanvas"]}
		onWheel={(e) => this.wheel(e)}
		onMouseDown={(e) => this.mouseDown(e)}
		onMouseMove={(e) => this.mouseMove(e)}
		onMouseUp={(e) => this.mouseUp(e)}
		style={{"border": "1px solid #000000"}}/>
</div>
		);
	}

	componentDidMount() {
		let canvas = this.mapRefs["mapCanvas"].current;
		canvas.width = window.innerWidth * .75;
		canvas.height = window.innerHeight * .8;
	}

	wheel(e) {
		const zoomScale = this.state.zoomScale;
		if(e.deltaY < 0) {
			this.setState({"zoomScale": zoomScale + 0.01});
		}
		else if(e.deltaY > 0) {
			this.setState({"zoomScale": zoomScale - 0.01});
		}
		this.drawMap();
		e.preventDefault();
	}

	mouseDown(e) {
		const tokens = this.state.tokens;
		const canvas = this.mapRefs["mapCanvas"].current;
		let rect = canvas.getBoundingClientRect();
		let canvasCoord = {
			"x": e.clientX - rect.left,
			"y": e.clientY - rect.top
		};
		let mapCoord = this.canvasCoordToMapCoord(canvasCoord);
		let isToken = false;
		for(let i = 0; i < tokens.length; i++) {
			let token = tokens[i];
			if(mapCoord.x > token.x && mapCoord.x < token.x + token.width && mapCoord.y > token.y && mapCoord.y < token.y + token.height) {
				this.setState({
					"movingToken": i
				});
				isToken = true;
				break;
			}
		}
		if(!isToken) {
			this.setState({
				"movingMap": true
			});
		}
	}

	mouseMove(e) {
		const movingMap = this.state.movingMap;
		const movingToken = this.state.movingToken;
		if(movingMap) {
			let offset = this.state.offset;
			const zoomScale = this.state.zoomScale;
			const canvas = this.mapRefs["mapCanvas"].current;
			const background = this.state.background;
			offset.x += e.movementX;
			offset.y += e.movementY;
			this.setState({
				"offset": offset
			});
			this.drawMap();
		}
		else if(movingToken != -1)
		{
			const canvas = this.mapRefs["mapCanvas"].current;
			let tokens = this.state.tokens;
			let canvasCoord = {
				"x": e.clientX - canvas.offsetLeft,
				"y": e.clientY - canvas.offsetTop
			};
			let mapCoord = this.canvasCoordToMapCoord(canvasCoord);
			tokens[movingToken].x = mapCoord.x - tokens[movingToken].width / 2;
			tokens[movingToken].y = mapCoord.y - tokens[movingToken].height / 2;
			this.setState({
				"tokens": tokens
			});
			this.drawMap();
		}
	}

	mouseUp(e) {
		const movingToken = this.state.movingToken;
		if(movingToken != -1) {
			const tokens = this.state.tokens;
			let content = {
				"tokenID": movingToken,
				"tokenX": tokens[movingToken].x,
				"tokenY": tokens[movingToken].y
			}
			this.sendMessage("updateToken", content);
		}
		this.setState({
			"movingMap": false,
			"movingToken": -1
		});
	}

	canvasCoordToMapCoord(canvasCoord) {
		const offset = this.state.offset;
		const zoomScale = this.state.zoomScale;
		return {
			"x": (canvasCoord.x - offset.x) / zoomScale,
			"y": (canvasCoord.y - offset.y) / zoomScale
		};
	}

	mapCoordToCanvasCoord(mapCoord) {
		const offset = this.state.offset;
		const zoomScale = this.state.zoomScale;
		return {
			"x": mapCoord.x * zoomScale + offset.x,
			"y": mapCoord.y * zoomScale + offset.y,
		};
	}
}

export default Map;
