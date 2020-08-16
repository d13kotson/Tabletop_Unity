using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    private GameController controller = default;

    void Awake() {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public void SetMeasuring() {
		this.controller.SetControl(true);
	}

	public void SetDragging() {
		this.controller.SetControl(false);
	}
}
