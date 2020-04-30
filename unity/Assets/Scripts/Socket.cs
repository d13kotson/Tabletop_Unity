#if UNITY_WEBGL
using System.Runtime.InteropServices;
using UnityEngine;

internal class Socket
{
    private GameController controller = default;
    private readonly string url = default;
    private MapState mapState = default;

    public Chat chat;

    public string Background
    {
        get
        {
            return this.mapState.background;
        }
    }

    public MapToken[] Tokens
    {
        get
        {
            return this.mapState.tokens;
        }
    }

    public Socket(string url, GameController controller)
    {
        this.controller = controller;
        this.url = url;
        this.initializeSocket();
    }

    private void initializeSocket()
    {
        WebSocketInit(this.url);
    }

    public void SendChatMessage(string user, string message)
    {
        WebSocketSend(string.Format("{{\"type\": \"chat\", \"content\": {{\"display_name\": \"{0}\", \"message\": \"{1}\"}}}}", user, message));
    }

    public void UpdateToken(Token token)
    {
        MapToken mapToken = this.mapState.tokens[token.mapID];
        mapToken.x = (int)token.gameObject.transform.position.x;
        mapToken.y = (int)token.gameObject.transform.position.y;
        WebSocketSend(string.Format("{{\"type\": \"updateToken\", \"content\": {{\"tokenID\": {0}, \"tokenX\": {1}, \"tokenY\": {2}}}}}", token.mapID, mapToken.x, mapToken.y));
    }

    public void UpdateBackground(BackgroundStruct background)
    {
        WebSocketSend(string.Format("{{\"type\": \"set_background\", \"content\": \"{0}\"}}", background.image));
    }

    public void ParseMessage(string content)
    {
        SocketMessage message = JsonUtility.FromJson<SocketMessage>(content);
        switch (message.type)
        {
            case "updateState":
                this.UpdateState(message.content);
                this.controller.InstantiateMap();
                break;
            case "updateToken":
                this.UpdateToken(message.content);
                break;
            case "set_background":
                this.SetBackground(message.content);
                this.controller.SetBackground();
                break;
            case "chat_message":
                this.chat.ReceiveMessage(message.content);
                break;
        }
    }

    private void UpdateState(string content)
    {
        this.mapState = JsonUtility.FromJson<MapState>(content);
    }

    private void SetBackground(string content)
    {
        this.mapState.background = content;
    }

    private void UpdateToken(string content)
    {
        UpdateToken update = JsonUtility.FromJson<UpdateToken>(content);
        this.mapState.tokens[update.tokenID].x = update.tokenX;
        this.mapState.tokens[update.tokenID].y = update.tokenY;
        this.controller.UpdateMap(this.mapState.tokens[update.tokenID]);
    }

    [DllImport("__Internal")]
    private static extern void WebSocketInit(string url);

    [DllImport("__Internal")]
    private static extern void WebSocketSend(string content);
}
#endif