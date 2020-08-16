using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float scrollRate = 0.1f;
    public float minZoom = 0.1f;
    public float maxZoom = 10.0f;

    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    private GameController controller = default;
    private float zoom = 2;

    private void Start()
    {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void LateUpdate()
    {
		if(!this.controller.IsMeasuring) {
			if(!this.controller.IsDragging && !this.controller.UIActive) {
				if(Input.GetMouseButton(0)) {
					Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
					if(Drag == false) {
						Drag = true;
						Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					}
				} else {
					Drag = false;
				}
				if(Drag == true) {
					Camera.main.transform.position = Origin - Diference;
				}
				float scroll = Input.mouseScrollDelta.y;
				if(scroll != 0) {
					if(Input.GetAxis("Fast Zoom") > 0) {
						scroll *= 10;
					}
					this.zoom -= scroll * this.scrollRate;
					if(this.zoom < this.minZoom) {
						this.zoom = this.minZoom;
					} else if(this.zoom > this.maxZoom) {
						this.zoom = this.maxZoom;
					}
					Camera.main.orthographicSize = this.zoom;
				}
			}
		}
    }
}
