using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private GameController controller = default;
    public Dropdown backgroundSelect = default;
    private BackgroundList backgroundList;

    void Start()
    {
        this.gameObject.SetActive(false);
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (this.controller.isGM)
        {
            RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
            this.gameObject.transform.Find("Background").gameObject.SetActive(true);
            rectTransform.sizeDelta = new Vector2(200, 90);
            rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - 15);
            this.controller.SendGetRequest("api/backgrounds", (request) =>
            {
                this.backgroundList = JsonUtility.FromJson<BackgroundList>(string.Format("{{\"list\": {0}}}", request.downloadHandler.text));
                foreach (BackgroundStruct background in this.backgroundList.list)
                {
                    this.backgroundSelect.options.Add(new Dropdown.OptionData() { text = background.title });
                }
            }, (request) =>
            {

            });
        }
    }

    public void SetBackground()
    {
        int backgroundID = this.backgroundSelect.value - 1;
        this.controller.socket.UpdateBackground(this.backgroundList.list[backgroundID]);
    }
}
