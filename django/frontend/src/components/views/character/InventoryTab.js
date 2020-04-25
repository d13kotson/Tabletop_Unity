import React, { Component } from "react";
import TabbedComponent from "./../TabbedComponent";
import Cookies from 'js-cookie';

class InventoryTab extends TabbedComponent {
    constructor(props) {
        super(props);
        this.tabRefs = {
            "self": React.createRef(),
            "newItem": React.createRef(),
            "newItemName": React.createRef(),
            "newItemAmount": React.createRef(),
            "moneyAmountChange": React.createRef(),
        };
        this.state = {
            "dataLoaded": false,
        };
        fetch("/api/items/")
            .then(response => {
                if (response.status !== 200) {
                    return this.setState({ placeholder: "Something went wrong."});
                }
                return response.json();
            })
            .then(data => {
                this.setState({
                    "items": data,
                    "dataLoaded": true
                });
            });
    }

    render() {
        const dataLoaded = this.state.dataLoaded;
        const trainer = this.props.trainer;
        const items = trainer.item;
        const classProp = this.props.class + " tabContent";
        if(dataLoaded) {
            const itemOptions = this.state.items
            return (
<div ref={this.tabRefs["self"]} className={classProp}>
    <table className="stat-block">
        <tbody>
            <tr>
                <th>Money</th>
                <th colSpan="2">Change Amount</th>
            </tr>
            <tr>
                <td>{trainer.money}</td>
                <td><input name="amount" ref={this.tabRefs["moneyAmountChange"]} type="number"/></td>
                <td>
                        <a className="button" onClick={() => this.increaseMoney()}>Add</a>
                        <br />
                        <a className="button" onClick={() => this.decreaseMoney()}>Sub</a></td>
            </tr>
        </tbody>
    </table>
    <table className="stat-block">
        <tbody>
            <tr>
                <th>Item</th>
                <th>Price</th>
                <th>Number</th>
                <th colSpan="2">Change Amount</th>
            </tr>
            {items.map((item, index, arr) => {
                this.tabRefs[item.id + "AmountChange"] = React.createRef();
                if(item.item != null) {
                    return (
                        <tr key={index}>
                            <td>{item.item.name}</td>
                            <td>{item.item.price}</td>
                            <td>{item.number}</td>
                            <td><input name="amount" ref={this.tabRefs[item.id + "AmountChange"]} type="number"/></td>
                            <td>
                                <a className="button" onClick={() => this.increaseItem(item)}>Add</a>
                                <br />
                                <a className="button" onClick={() => this.decreaseItem(item)}>Sub</a>
                            </td>
                        </tr>
                    );
                }
                else {
                    return (
                        <tr key={index}>
                            <td>{item.item_name}</td>
                            <td></td>
                            <td>{item.number}</td>
                            <td><input name="amount" ref={this.tabRefs[item.id + "AmountChange"]} type="number"/></td>
                            <td>
                                <a className="button" onClick={() => this.increaseItem(item)}>Add</a>
                                <br />
                                <a className="button" onClick={() => this.decreaseItem(item)}>Sub</a>
                            </td>
                        </tr>
                    );
                }
            }, this)}
            <tr>
                <td>
                    <select ref={this.tabRefs["newItem"]}>
                        <option value="0">Custom Name:</option>
                        {itemOptions.map(function(item, index) {
                            return (
                                <option key={index} value={item.id}>{item.name}</option>
                            );
                        })}
                    </select>
                    <input ref={this.tabRefs["newItemName"]} type="text" />
                </td>
                <td></td>
                <td><input ref={this.tabRefs["newItemAmount"]} type="number"/></td>
                <td colSpan="2"><a className="button" onClick={() => this.addItem()}>Add</a></td>
            </tr>
        </tbody>
    </table>
</div>
            );
        }
        else {
            return (<div ref={this.tabRefs["self"]} className={classProp} />);
        }
    }

    addItem() {
        let itemInput = this.tabRefs["newItem"].current.value
        let item = null
        if(itemInput != 0) {
            item = {
                "trainer": this.props.trainer.id,
                "item": itemInput,
                "number": this.tabRefs["newItemAmount"].current.value,
            }
        }
        else {
            item = {
                "trainer": this.props.trainer.id,
                "item_name": this.tabRefs["newItemName"].current.value,
                "number": this.tabRefs["newItemAmount"].current.value,
            }
        }
        fetch("api/addItem/", {
          method: 'POST',
          body: JSON.stringify(item),
            headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
        }).then(response => {0
            if (response.status !== 200) {
                return this.setState({ placeholder: "Something went wrong."});
            }
            return response.json();
        })
        .then(data => {
            this.props.update();
        });
    }

    increaseItem(item) {
        if(item.item != null) {
            item.item = item.item.id
        }
        item.number += parseInt(this.tabRefs[item.id + "AmountChange"].current.value);
        fetch("/api/item/" + item.id + "/", {
          method: 'PUT',
          body: JSON.stringify(item),
            headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
        }).then(response => {
            if (response.status !== 200) {
                return this.setState({ placeholder: "Something went wrong."});
            }
            return response.json();
        })
        .then(data => {
            this.props.update();
        });
    }

    decreaseItem(item) {
        if(item.item != null) {
            item.item = item.item.id
        }
        item.number -= parseInt(this.tabRefs[item.id + "AmountChange"].current.value);
        fetch("/api/item/" + item.id + "/", {
          method: 'PUT',
          body: JSON.stringify(item),
            headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
        }).then(response => {
            if (response.status !== 200) {
                return this.setState({ placeholder: "Something went wrong."});
            }
            return response.json();
        })
        .then(data => {
            this.props.update();
        });
    }

    increaseMoney() {
        let trainer = this.props.trainer;
        trainer.money += parseInt(this.tabRefs["moneyAmountChange"].current.value);
        fetch("/api/trainer/" + trainer.id + "/", {
          method: 'PUT',
          body: JSON.stringify(trainer),
            headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
        }).then(response => {
            if (response.status !== 200) {
                return this.setState({ placeholder: "Something went wrong."});
            }
            return response.json();
        })
        .then(data => {
            this.props.update();
        });
    }

    decreaseMoney() {
        let trainer = this.props.trainer;
        trainer.money -= parseInt(this.tabRefs["moneyAmountChange"].current.value);
        fetch("/api/trainer/" + trainer.id + "/", {
          method: 'PUT',
          body: JSON.stringify(trainer),
            headers: new Headers({ "Content-Type": "application/json", "X-CSRFTOKEN": Cookies.get('csrftoken') })
        }).then(response => {
            if (response.status !== 200) {
                return this.setState({ placeholder: "Something went wrong."});
            }
            return response.json();
        })
        .then(data => {
            this.props.update();
        });
    }
}

export default InventoryTab;
