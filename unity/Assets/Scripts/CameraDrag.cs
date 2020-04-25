using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    private GameController controller = default;

    private void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void LateUpdate()
    {
        if (!this.controller.IsDragging)
        {
            if (Input.GetMouseButton(0))
            {
                Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                if (Drag == false)
                {
                    Drag = true;
                    Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                Drag = false;
            }
            if (Drag == true)
            {
                Camera.main.transform.position = Origin - Diference;
            }
        }
    }
}
