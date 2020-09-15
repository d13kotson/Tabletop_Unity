using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
	private UILineRenderer lineRend;
	private Vector2 mousePosCam;
	private Vector2 mousePos;
	private Vector2 startMousePos;
	private Vector2 startMousePosCam;

	[SerializeField]
	private Text distanceText = default;
	public float scale;
	private float distance;

	private bool lineDrawn;

    private GameController controller = default;
    void Start()
    {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		this.lineRend = GetComponent<UILineRenderer>();
    }
	
    void Update()
    {
        if(this.controller.IsMeasuring) {
			if (Input.GetMouseButtonDown(0)) {
				this.startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.startMousePosCam = Input.mousePosition;
				this.lineDrawn = true;
			}

			if (Input.GetMouseButton(0)) {
				this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.mousePosCam = Input.mousePosition;
				this.lineRend.Points = new Vector2[] { this.startMousePosCam, this.mousePosCam };
				this.lineRend.SetVerticesDirty();
				this.distance = (this.mousePos - this.startMousePos).magnitude;
				this.distanceText.text = (this.distance * this.scale).ToString("F2") + "meters";
			}
		}
		else if(this.lineDrawn) {
			this.lineRend.Points = new Vector2[] { Vector2.zero, Vector2.zero };
			this.lineRend.SetVerticesDirty();
			this.distanceText.text = "";
		}
    }
}
