import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Cookies from 'js-cookie';

class MapEditorTab extends TabbedComponent {
	constructor(props) {
		super(props);
		this.tabRefs = {
			"self": React.createRef(),
		};
		this.inputRefs = {
			"backgroundImage": React.createRef(),
			"backgroundTitle": React.createRef(),
			"tokenImage": React.createRef(),
			"tokenTitle": React.createRef(),
			"tokenHeight": React.createRef(),
			"tokenWidth": React.createRef(),
		};
		this.state = {
			"backgroundImage": "",
			"tokenImage": "",
		};
	}

	render() {
		const classProp = this.props.class + " tabContent";
		return (
<div ref={this.tabRefs["self"]} className={classProp}>
	<form>
		<table className="stat-block">
			<tbody>
				<tr>
					<td>Background Image</td>
					<td>
						<input ref={this.inputRefs["backgroundImage"]} type="file" accept="image/png" onChange={(e) => this.selectBackground(e)}/>
					</td>
				</tr>
				<tr>
					<td>Title</td>
					<td>
						<input ref={this.inputRefs["backgroundTitle"]} type="text"/>
					</td>
				</tr>
			</tbody>
		</table>
		<a className="button" onClick={() => this.addBackground()}>Add Background</a>
	</form>
	<form>
		<table className="stat-block">
			<tbody>
				<tr>
					<td>Token Image</td>
					<td>
						<input ref={this.inputRefs["tokenImage"]} type="file" accept="image/png" onChange={(e) => this.selectToken(e)}/>
					</td>
				</tr>
				<tr>
					<td>Title</td>
					<td>
						<input ref={this.inputRefs["tokenTitle"]} type="text"/>
					</td>
				</tr>
				<tr>
					<td>Height</td>
					<td>
						<input ref={this.inputRefs["tokenHeight"]} type="number"/>
					</td>
				</tr>
				<tr>
					<td>Width</td>
					<td>
						<input ref={this.inputRefs["tokenWidth"]} type="number"/>
					</td>
				</tr>
			</tbody>
		</table>
		<a className="button" onClick={() => this.addToken()}>Add Token</a>
	</form>
</div>
		);
	}

	async selectBackground(e) {
		const file = e.target.files[0];

		const reader = new FileReader();
		reader.onload = (event) => {
			let encoder = new TextEncoder();
			this.setState({
				"backgroundImage": new Uint8Array(reader.result)
			});
		};
		reader.readAsArrayBuffer(file);
	}

	async addBackground() {
		const game = this.props.game;
		const backgroundImage = Array.from(this.state.backgroundImage);
		let background = {
			"image": backgroundImage,
			"title": this.inputRefs["backgroundTitle"].current.value,
			"gm": game.gm
		}
		fetch("/map/background/", {
		  method: 'POST',
		  body: JSON.stringify(background),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return true;
		})
        .then(data => {
            this.props.update();
        });
	}

	async selectToken(e) {
		const file = e.target.files[0];

		const reader = new FileReader();
		reader.onload = (event) => {
			let encoder = new TextEncoder();
			this.setState({
				"tokenImage": new Uint8Array(reader.result)
			});
		};
		reader.readAsArrayBuffer(file);
	}

	async addToken() {
		const game = this.props.game;
		const tokenImage = Array.from(this.state.tokenImage);
		let token = {
			"image": tokenImage,
			"title": this.inputRefs["tokenTitle"].current.value,
			"gm": game.gm,
			"height": this.inputRefs["tokenHeight"].current.value,
			"width": this.inputRefs["tokenWidth"].current.value
		}
		fetch("/map/token/", {
		  method: 'POST',
		  body: JSON.stringify(token),
			headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
		}).then(response => {
			if (response.status !== 200) {
				return this.setState({ placeholder: "Something went wrong."});
			}
			return true;
		})
        .then(data => {
            this.props.update();
        });
	}
}

export default MapEditorTab;
