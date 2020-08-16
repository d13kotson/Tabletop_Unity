using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
	private LineRenderer lineRend;
	private Vector2 mousePos;
	private Vector2 startMousePos;

	[SerializeField]
	private Text distanceText;
	public float scale;
	private float distance;

	private bool lineDrawn;

    private GameController controller = default;
    void Start()
    {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		this.lineRend = GetComponent<LineRenderer>();
		lineRend.positionCount = 2;
    }
	
    void Update()
    {
        if(this.controller.IsMeasuring) {
			if (Input.GetMouseButtonDown(0)) {
				startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.lineDrawn = true;
			}

			if (Input.GetMouseButton(0)) {
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				lineRend.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
				lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
				distance = (mousePos - startMousePos).magnitude;
				distanceText.text = (distance * scale).ToString("F2") + "meters";
			}
		}
		else if(this.lineDrawn) {
			lineRend.SetPosition(0, Vector3.zero);
			lineRend.SetPosition(1, Vector3.zero);
			this.distanceText.text = "";
		}
    }
}
