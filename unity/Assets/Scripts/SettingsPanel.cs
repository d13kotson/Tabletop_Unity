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
            UnityWebRequest request = UnityWebRequest.Get(string.Format("http://localhost/api/backgrounds/", this.controller.URL));
            request.SetRequestHeader("Authorization", string.Format("Token {0}", this.controller.authToken));
            AsyncOperation response = request.SendWebRequest();
            response.completed += async (AsyncOperation obj) =>
            {
                if (request.responseCode == 200)
                {
                    this.backgroundList = JsonUtility.FromJson<BackgroundList>(string.Format("{{\"list\": {0}}}", request.downloadHandler.text));
                    foreach(BackgroundStruct background in this.backgroundList.list)
                    {
                        this.backgroundSelect.options.Add(new Dropdown.OptionData() { text = background.title });
                    }
                }
            };
            /*
             * This might be needed to refesh the menu?
            this.backgroundSelect.value = 1;
            this.backgroundSelect.value = 0;
            */
        }
    }

    public void SetBackground()
    {
        int backgroundID = this.backgroundSelect.value - 1;
#if UNITY_WEBGL
        this.controller.socket.UpdateBackground(this.backgroundList.list[backgroundID]);
#endif
    }
}
