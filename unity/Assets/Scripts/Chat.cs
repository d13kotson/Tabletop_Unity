using System;
using UnityEngine;
using UnityEngine.UI;

public class Chat : Window
{
    private InputField input;
    private Button send;
    public Transform content = default;
    private RectTransform contentRect;
    public GameObject message = default;

    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.socket.chat = this;
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
        this.controller.socket.SendChatMessage(user, message);
    }

    private void getMessages()
    {
        this.controller.SendGetRequest(string.Format("api/messages/{0}", this.controller.isGM ? this.controller.game.id : this.controller.trainer.game), (request) =>
        {
            MessageList messages = JsonUtility.FromJson<MessageList>(string.Format("{{\"messages\": {0}}}", request.downloadHandler.text));
            foreach (Message message in messages.messages)
            {
                this.AddMessage(message.message, message.display_name);
            }
        }, (request) =>
        {

        });
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
		GameObject textField = messageObject.transform.Find("Message").gameObject;
		textField.GetComponent<Text>().text = message;
        messageObject.transform.SetParent(this.content);
		Canvas.ForceUpdateCanvases();
		this.gameObject.GetComponentInChildren<ScrollRect>().normalizedPosition = Vector2.zero;
	}
}
