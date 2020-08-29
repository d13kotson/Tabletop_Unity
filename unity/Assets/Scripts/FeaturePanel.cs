using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeaturePanel : MonoBehaviour
{
	private GameController controller = default;

    void Start()
    {
        
    }

    internal void Set(Feature feature, int attackerID) {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Transform content = this.gameObject.transform;
        content.Find("Name").gameObject.GetComponent<Text>().text = feature.name;
        content.Find("Frequency").gameObject.GetComponent<Text>().text = feature.frequency;
        content.Find("Trigger").gameObject.GetComponent<Text>().text = feature.trigger;
		content.Find("Effect").gameObject.GetComponent<Text>().text = feature.effect;
    }
}
