using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private string username = string.Empty;
    public string authToken = default;
    private UserInfo userInfo = default;
    private UnityWebRequest currentRequest = default;

    public InputField Username, Password;
    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void Login()
    {
        this.sendLogin();
    }

    private void sendLogin()
    {
        this.username = Username.text;
        string password = Password.text;
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        this.currentRequest = UnityWebRequest.Post("http://localhost/rest-auth/login/", form);
        AsyncOperation response = this.currentRequest.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (this.currentRequest.responseCode == 200)
            {
                Key key = JsonUtility.FromJson<Key>(this.currentRequest.downloadHandler.text);
                this.authToken = key.key;
                this.currentRequest.Dispose();
                this.sendUserInfo();
            }
        };
    }

    private void sendUserInfo()
    {
        this.currentRequest = UnityWebRequest.Get(string.Format("http://localhost/api/user/{0}/", this.username));
        this.currentRequest.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = this.currentRequest.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (this.currentRequest.responseCode == 200)
            {
                UserInfo userInfo = JsonUtility.FromJson<UserInfo>(this.currentRequest.downloadHandler.text);
                this.userInfo = userInfo;
                this.currentRequest.Dispose();
                print("ws://localhost/ws/map/test");
                WebSocketInit("ws://localhost/ws/map/test");
                SceneManager.LoadScene("Game");
            }
        };
    }

    [DllImport("__Internal")]
    private static extern void WebSocketInit(string url);

    [DllImport("__Internal")]
    private static extern void WebSocketSend(string message);

    private class Key
    {
        public string key;
    }

    private class UserInfo
    {
        public int id;
        public string username;
        public bool gm;
        public int item_id;
        public string game_name;
    }
}
