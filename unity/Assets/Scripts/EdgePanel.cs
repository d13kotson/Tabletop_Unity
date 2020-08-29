using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EdgePanel : MonoBehaviour
{
	private GameController controller = default;

    void Start()
    {
        
    }

    internal void Set(Edge edge, int attackerID) {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Transform content = this.gameObject.transform;
        content.Find("Name").gameObject.GetComponent<Text>().text = edge.name;
		content.Find("Effect").gameObject.GetComponent<Text>().text = edge.effect;
    }
}
