using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	internal GameController controller;

	public void OnPointerEnter(PointerEventData eventData) {
		this.controller.UIActive = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		this.controller.UIActive = false;
	}

	public void Enable() {
		this.controller.CloseScreens();
		bool value = !this.gameObject.activeSelf;
		if(value) {
			this.controller.OpenScreens.Add(this.gameObject);
		} else {
			this.controller.OpenScreens.Remove(this.gameObject);
		}
		this.gameObject.SetActive(value);
	}

	public void Disable() {
		this.gameObject.SetActive(false);
	}
}
