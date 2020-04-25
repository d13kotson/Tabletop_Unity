using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    private GameController controller;
    private InputField input;
    private Button send;
    private int position;
    public Transform content = default;
    private RectTransform contentRect;
    public GameObject message = default;

    void Start()
    {
        this.position = -20;
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
#if UNITY_WEBGL
        this.controller.socket.chat = this;
#endif
        this.content = this.gameObject.transform.Find("Chat").Find("Viewport").Find("Content").gameObject.transform;
        this.contentRect = this.content.gameObject.GetComponent<RectTransform>();
        this.input = this.gameObject.transform.Find("Input").gameObject.GetComponent<InputField>();
        this.send = this.gameObject.transform.Find("Send").gameObject.GetComponent<Button>();
        this.send.onClick.AddListener(delegate { this.sendMessage(); });
        this.getMessages();
    }

    private void sendMessage()
    {
        string message = this.input.text;
        this.input.text = string.Empty;
        string user = string.Empty;
        if (this.controller.isGM)
        {
            user = "GM";
        }
        else
        {
            user = this.controller.trainer.name;
        }
#if UNITY_WEBGL
        print(string.Format("{{\"type\": \"chat\", \"content\": {{\"display_name\": \"{0}\", \"message\": \"{1}\"}}}}", user, message));
        this.controller.socket.SendChatMessage(user, message);
#endif
    }

    private void getMessages()
    {
        string url = string.Empty;
        if (this.controller.isGM)
        {
            url = "http://localhost/api/messages/" + this.controller.game.id;
        }
        else
        {
            url = "http://localhost/api/messages/" + this.controller.trainer.game;
        }
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", string.Format("Token {0}", this.controller.authToken));
        AsyncOperation response = request.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (request.responseCode == 200)
            {
                MessageList messages = JsonUtility.FromJson<MessageList>(string.Format("{{\"messages\": {0}}}", request.downloadHandler.text));
                foreach (Message message in messages.messages)
                {
                    this.AddMessage(message.message, message.display_name);
                }
            }
        };
    }

    public void ReceiveMessage(string content)
    {
        Message message = JsonUtility.FromJson<Message>(content);
        this.AddMessage(message.message, message.display_name);
    }

    public void AddMessage(string message, string displayName)
    {
        GameObject messageObject = Instantiate(this.message);
        messageObject.transform.Find("UserName").gameObject.GetComponent<Text>().text = displayName;
        messageObject.transform.Find("Message").gameObject.GetComponent<Text>().text = message;

        messageObject.transform.SetParent(this.content);
        RectTransform rect = messageObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, position);
        rect.sizeDelta = new Vector2(184, 40);
        position -= 40;
        if (this.contentRect.sizeDelta.y < Math.Abs(position))
        {
            this.contentRect.sizeDelta = new Vector2(this.contentRect.sizeDelta.x, Math.Abs(position));
        }
    }
}
