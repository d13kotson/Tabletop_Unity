import React, { Component } from "react";

class Chat extends Component {

	constructor(props) {
		super(props);
		this.tabRefs = {
		    "self": React.createRef(),
			"chatLog": React.createRef(),
			"chatInput": React.createRef(),
			"dbInput": React.createRef(),
			"attackInput": React.createRef()
		};
		this.chatSocket = new WebSocket(
			"ws://" + window.location.host + "/ws/chat/" + this.props.roomName + "/" + this.props.gmView
		);
		this.chatSocket.tabRefs = this.tabRefs;
		this.chatSocket.onmessage = (e) => this.addMessage(JSON.parse(e.data));
		this.chatSocket.onclose = function(e) {
			console.error('Chat socket closed unexpectedly');
		};
	}

	componentDidMount() {
		this.getHistory(this.props.roomName);
	}

	async getHistory(gameID) {
	    await fetch("/chat/messages/" + gameID)
			.then(response => {
				if (response.status === 200) {
					return response.json();
				}
			})
			.then(data => {
			    for(let i = 0; i < data.length; i++) {
			        this.addMessage(data[i]);
			    }
			});
	}

	addMessage(data) {
		this.tabRefs["chatLog"].current.value += (data["display_name"] + ": " + data["message"] + "\n");
	}

	sendMessage() {
		let message = this.tabRefs["chatInput"].current.value;
		this.sendMessage(message);
	}

	sendMessage(message) {
	    this.chatSocket.send(JSON.stringify({
			'message': message,
			'display_name': this.props.displayName
		}));
		this.tabRefs["chatInput"].current.value = "";
	}

	rollAttack() {
	    let db = parseInt(this.tabRefs["dbInput"].current.value);
	    let attack = parseInt(this.tabRefs["attackInput"].current.value);
	    let numDie = 0;
	    let dieNum = 0;
	    let addition = 0;
	    switch(db) {
            case 1:
                numDie = 1;
                dieNum = 6;
                addition = 1;
                break;
            case 2:
                numDie = 1;
                dieNum = 6;
                addition = 3;
                break;
            case 3:
                numDie = 1;
                dieNum = 6;
                addition = 5;
                break;
            case 4:
                numDie = 1;
                dieNum = 8;
                addition = 6;
                break;
            case 5:
                numDie = 1;
                dieNum = 8;
                addition = 8;
                break;
            case 6:
                numDie = 2;
                dieNum = 6;
                addition = 8;
                break;
            case 7:
                numDie = 2;
                dieNum = 6;
                addition = 10;
                break;
            case 8:
                numDie = 2;
                dieNum = 8;
                addition = 10;
                break;
            case 9:
                numDie = 2;
                dieNum = 10;
                addition = 10;
                break;
            case 10:
                numDie = 3;
                dieNum = 8;
                addition = 10;
                break;
            case 11:
                numDie = 3;
                dieNum = 10;
                addition = 10;
                break;
            case 12:
                numDie = 3;
                dieNum = 12;
                addition = 10;
                break;
            case 13:
                numDie = 4;
                dieNum = 10;
                addition = 10;
                break;
            case 14:
                numDie = 4;
                dieNum = 10;
                addition = 15;
                break;
            case 15:
                numDie = 4;
                dieNum = 10;
                addition = 20;
                break;
            case 16:
                numDie = 5;
                dieNum = 10;
                addition = 20;
                break;
            case 17:
                numDie = 5;
                dieNum = 12;
                addition = 25;
                break;
            case 18:
                numDie = 6;
                dieNum = 12;
                addition = 25;
                break;
            case 19:
                numDie = 6;
                dieNum = 12;
                addition = 30;
                break;
            case 20:
                numDie = 6;
                dieNum = 12;
                addition = 35;
                break;
            case 21:
                numDie = 6;
                dieNum = 12;
                addition = 40;
                break;
            case 22:
                numDie = 6;
                dieNum = 12;
                addition = 45;
                break;
            case 23:
                numDie = 6;
                dieNum = 12;
                addition = 50;
                break;
            case 24:
                numDie = 6;
                dieNum = 12;
                addition = 55;
                break;
            case 25:
                numDie = 6;
                dieNum = 12;
                addition = 60;
                break;
            case 26:
                numDie = 7;
                dieNum = 12;
                addition = 65;
                break;
            case 27:
                numDie = 8;
                dieNum = 12;
                addition = 70;
                break;
            case 28:
                numDie = 8;
                dieNum = 12;
                addition = 80;
                break;
            default:
                return;
	    }
	    this.sendMessage("/roll " + numDie + "d" + dieNum + "+" + (addition + parseInt(attack)));
	}

	render() {
		return (
<div>
    <textarea ref={this.tabRefs["chatLog"]} id="chat-log" readOnly="readonly" style={{width: "90%", height: window.innerHeight * .8 + "px", resize: "none"}}></textarea>
    <input ref={this.tabRefs["chatInput"]} id="chat-message-input" type="text" onKeyUp={(event) => { if(event.keyCode == 13) this.sendMessage() }}/>
    <input id="chat-message-submit" type="button" value="send" onClick={() => this.sendMessage()} />
    <br/>
    <a className="button" onClick={() => this.sendMessage("/roll d20")}>Roll d20</a>
    <br/>
    <label>DB: </label>
    <input ref={this.tabRefs["dbInput"]} type="number" />
    <br/>
    <label>Attack: </label>
    <input ref={this.tabRefs["attackInput"]} type="number" />
    <a className="button" onClick={() => this.rollAttack()}>Roll Attack</a>
</div>
		);
	}
}

export default Chat
