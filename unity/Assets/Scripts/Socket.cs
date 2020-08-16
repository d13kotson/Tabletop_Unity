#if UNITY_WEBGL
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

internal class SocketClient
{
    private GameController controller = default;
    private readonly string url = default;
    public SocketClient(string url, GameController controller)
    {
        this.controller = controller;
        this.url = url;
        this.initializeSocket();
    }

    private void initializeSocket()
    {
        WebSocketInit(this.url);
    }

    internal async void SendMessage(string type, string content)
    {
        string message = string.Format("{{\"type\": \"{0}\", \"content\": {1}}}", type, content);
        WebSocketSend(message);
    }

    [DllImport("__Internal")]
    private static extern void WebSocketInit(string url);

    [DllImport("__Internal")]
    private static extern void WebSocketSend(string content);
}
#else
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

internal class SocketClient
{
    private ClientWebSocket client;
    private GameController controller;
    private readonly string url;
    
    public SocketClient(string url, GameController controller)
    {
        this.controller = controller;
        this.url = url;
        this.client = new ClientWebSocket();
        this.initializeSocket();
    }

    private async void initializeSocket()
    {
        await this.client.ConnectAsync(new Uri(this.url), CancellationToken.None);
        await Task.Factory.StartNew(async () =>
        {
            byte[] rcvBytes = new byte[1024];
            ArraySegment<byte> rcvBuffer = new ArraySegment<byte>(rcvBytes);
            while(true)
            {
                WebSocketReceiveResult rcvResult = await this.client.ReceiveAsync(rcvBuffer, CancellationToken.None);
                byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
                string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                this.controller.AddMessage(rcvMsg);
            }
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        this.SendMessage("request_state", "\"\"");
    }

    internal async void SendMessage(string type, string content)
    {
        string message = string.Format("{{\"type\": \"{0}\", \"content\": {1}}}", type, content);
        byte[] sendBytes = Encoding.UTF8.GetBytes(message);
        ArraySegment<byte> sendBuffer = new ArraySegment<byte>(sendBytes);
        await this.client.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
    }
}

#endif

internal class Socket
{
    private SocketClient client;
    private GameController controller = default;
    private MapState mapState = default;

    public Chat chat;

    public string Background
    {
        get
        {
            return this.mapState.background;
        }
    }

    public Dictionary<int, MapToken> Tokens
    {
        get
        {
            return this.mapState.tokens;
        }
    }

    public Socket(string url, GameController controller)
    {
        this.controller = controller;
        this.client = new SocketClient(url, controller);
    }

    public void SendChatMessage(string user, string message)
    {
		message = message.Replace("\"", "\\\"");
        this.client.SendMessage("chat", string.Format("{{\"display_name\": \"{0}\", \"message\": \"{1}\"}}", user, message));
    }

    public void UpdateToken(Token token)
    {
        MapToken mapToken = this.mapState.tokens.First((t) => t.Value.tokenID == token.tokenID).Value;
        mapToken.x = (float)token.gameObject.transform.position.x;
        mapToken.y = (float)token.gameObject.transform.position.y;
        this.client.SendMessage("update_token", string.Format("{{\"tokenID\": {0}, \"tokenX\": {1}, \"tokenY\": {2}}}", mapToken.tokenID, mapToken.x, mapToken.y));
    }

    public void UpdateToken(int tokenID, float x, float y)
    {
        int mapID = 0;
        for (int i = 0; i < this.mapState.tokens.Count; i++)
        {
            if (this.mapState.tokens[i].tokenID == tokenID)
            {
                mapID = i;
                break;
            }
        }
        this.client.SendMessage("update_token", string.Format("{{\"tokenID\": {0}, \"tokenX\": {1}, \"tokenY\": {2}}}", mapID, x, y));
    }

    public void AddToken(int tokenID, float x, float y, TokenType type, int id)
    {
        this.client.SendMessage("add_token", string.Format("{{\"tokenID\": {0}, \"x\": {1}, \"y\": {2}, \"tokenType\": {3}, \"repID\": {4}}}", tokenID, x, y, (int)type, id));
    }

    public void DeleteToken(int tokenID)
    {
        this.client.SendMessage("delete_token", tokenID.ToString());
    }

    public void UpdateBackground(BackgroundStruct background)
    {
        this.client.SendMessage("set_background", background.image.id.ToString());
    }

    public void Roll(string user, int numDie, int dieNum, int add)
    {
        this.client.SendMessage("roll", string.Format("{{\"display_name\": \"{0}\", \"num_die\": {1}, \"die_num\": {2}, \"add\": {3}}}", user, numDie, dieNum, add));
    }

	public void Attack(int attackerID, TokenType attackerType, int defenderID, TokenType defenderType, int attackID) {
		string displayName = this.controller.isGM ? "GM" : this.controller.trainer.name;
		this.client.SendMessage("attack", string.Format("{{" +
			"\"display_name\": \"{0}\", " +
			"\"attacker_id\": {1}, " +
			"\"attacker_type\": \"{2}\", " +
			"\"defender_id\": {3}, " +
			"\"defender_type\": \"{4}\", " +
			"\"attack_id\": {5}" +
   "}}", displayName, attackerID, attackerType, defenderID, defenderType, attackID));
	}

    public void ParseMessage(string content)
    {
        SocketMessage message = JsonUtility.FromJson<SocketMessage>(content);
        switch (message.type)
        {
            case "update_state":
                this.UpdateState(message.content);
                this.controller.InstantiateMap();
                break;
            case "update_token":
                this.UpdateToken(message.content);
                break;
            case "delete_token":
                this.DeleteToken(message.content);
                break;
            case "set_background":
                this.SetBackground(message.content);
                this.controller.SetBackground();
                break;
            case "add_token":
                this.AddToken(message.content);
                break;
            case "chat_message":
                this.chat.ReceiveMessage(message.content);
                break;
        }
    }

    private void UpdateState(string content)
    {
        Dictionary<int, MapToken> tokens = new Dictionary<int, MapToken>();
        MapState mapState = new MapState();
        string message = content.Substring(1, content.Length - 2);
        int backgroundBegin = message.IndexOf("\"background\": ") + 14;
        int backgroundEnd = message.IndexOf(",", backgroundBegin);
        if(backgroundBegin == backgroundEnd)
        {
            mapState.background = "";
        }
        else
        {
            mapState.background = message.Substring(backgroundBegin, backgroundEnd - backgroundBegin);
        }
        int tokensBegin = message.IndexOf("\"tokens\": {") + 11;
        int tokensEnd = message.Length - 1;
        if (tokensBegin == tokensEnd)
        {
            mapState.tokens = tokens;
        }
        else
        {
            string tokensString = message.Substring(tokensBegin, tokensEnd - tokensBegin);
            string[] tokenSplits = tokensString.Split('}');
            foreach(string tokenSplit in tokenSplits)
            {
                if (!string.IsNullOrEmpty(tokenSplit))
                {
                    string tokenString = tokenSplit.Trim(',').Trim();
                    int id = int.Parse(tokenString.Substring(0, tokenString.IndexOf(':')).Trim('\"'));
                    string mapTokenString = tokenString.Substring(tokenString.IndexOf(": {") + 2) + "}";
                    MapToken token = JsonUtility.FromJson<MapToken>(mapTokenString);
                    tokens.Add(id, token);
                }
            }
            mapState.tokens = tokens;
        }
        this.mapState = mapState;
    }

    private void SetBackground(string content)
    {
        this.mapState.background = content;
    }

    private void AddToken(string content)
    {
        MapToken mapToken = JsonUtility.FromJson<MapToken>(content);
		if(this.mapState.tokens == null) {
			this.mapState.tokens = new Dictionary<int, MapToken>();
		}
		if(this.mapState.tokens.ContainsKey(mapToken.tokenID)) {
			this.mapState.tokens[mapToken.tokenID] = mapToken;
		} else {
			this.mapState.tokens.Add(mapToken.tokenID, mapToken);
			this.controller.AddToken(mapToken);
		}
    }

    private void UpdateToken(string content)
    {
        UpdateToken update = JsonUtility.FromJson<UpdateToken>(content);
        MapToken token = this.mapState.tokens[update.tokenID];
        token.x = update.tokenX;
        token.y = update.tokenY;
        this.mapState.tokens[update.tokenID] = token;
        this.controller.UpdateMap(this.mapState.tokens[update.tokenID]);
    }

    private void DeleteToken(string content)
    {
        int id = int.Parse(content);
        this.mapState.tokens.Remove(id);
        this.controller.RemoveToken(id);
    }
}