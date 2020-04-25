import React, { Component } from "react";

class TabbedComponent extends Component {

	openTab(event, tabClass, tabName) {
		let i, tabcontent, tablinks;

		tabcontent = document.getElementsByClassName(tabClass);
		for(i = 0; i < tabcontent.length; i++) {
			tabcontent[i].style.display = "none";
		}
		if(this.tabRefs[tabName].current.tabRefs != null) {
			this.tabRefs[tabName].current.openTab(event, tabClass, "self");
		}
		else {
			this.tabRefs[tabName].current.style.display = "block";
		}
	}
}

export default TabbedComponent
