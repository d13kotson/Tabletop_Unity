using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject BackgroundPrefab = default;
    public GameObject TokenPrefab = default;

    public GameObject CharacterPrefab = default;

    private string username = string.Empty;
    public string authToken = default;
    private UserInfo userInfo = default;
    internal Game game = default;
    internal Trainer trainer = default;
    private UnityWebRequest currentRequest = default;
    private List<GameObject> tokens = new List<GameObject>();
#if UNITY_WEBGL
    internal Socket socket = default;
#endif
    public bool IsDragging;
    internal bool isGM;
    internal List<GameObject> OpenScreens = new List<GameObject>();

    public List<Action> RefreshActions = new List<Action>();

    public InputField Username, Password;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
        response.completed += async (AsyncOperation obj) =>
        {
            if (this.currentRequest.responseCode == 200)
            {
                this.userInfo = JsonUtility.FromJson<UserInfo>(this.currentRequest.downloadHandler.text);
                this.currentRequest.Dispose();
                StartCoroutine("waitForSceneLoad", new Tuple<string, Action>("CharacterSelect", this.InstantiateCharacterSelect));
            }
        };
    }

    public void InstantiateCharacterSelect()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        try
        {
            float total = this.userInfo.game.Length + this.userInfo.trainer.Length;
            float position = (total / 2) * 35;
            foreach (GameTitle game in this.userInfo.game)
            {
                GameObject button = Instantiate(this.CharacterPrefab, Vector3.zero, Quaternion.identity);
                button.transform.SetParent(canvas.transform, false);
                button.transform.position = new Vector2(button.transform.position.x, button.transform.position.y + position);
                button.GetComponentInChildren<Text>().text = game.title;
                button.GetComponent<Button>().onClick.AddListener(delegate { this.loadGame(game.id); });
                position -= 35;
            }
            foreach (TrainerName trainer in this.userInfo.trainer)
            {
                GameObject button = Instantiate(this.CharacterPrefab, Vector3.zero, Quaternion.identity);
                button.transform.SetParent(canvas.transform, false);
                button.transform.position = new Vector2(button.transform.position.x, button.transform.position.y + position);
                button.GetComponentInChildren<Text>().text = trainer.name;
                button.GetComponent<Button>().onClick.AddListener(delegate { this.loadTrainer(trainer.id); });
                position -= 35;
            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    public void CloseScreens()
    {
        foreach(GameObject screen in this.OpenScreens)
        {
            screen.SetActive(false);
        }
        this.OpenScreens = new List<GameObject>();
    }

    private void loadGame(int id)
    {
        this.isGM = true;
        this.currentRequest = UnityWebRequest.Get(string.Format("http://localhost/api/game/{0}/", id));
        this.currentRequest.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = this.currentRequest.SendWebRequest();
        response.completed += async (AsyncOperation obj) =>
        {
            if (this.currentRequest.responseCode == 200)
            {
                this.game = JsonUtility.FromJson<Game>(this.currentRequest.downloadHandler.text);
#if UNITY_WEBGL
                this.socket = new Socket(string.Format("ws://localhost/ws/{0}/true", this.game.id), this);
#endif
                StartCoroutine("waitForSceneLoad", new Tuple<string, Action>("Game", this.InstantiateMap));
            }
        };
    }

    private void loadTrainer(int id)
    {
        this.isGM = false;
        this.currentRequest = UnityWebRequest.Get(string.Format("http://localhost/api/trainer/{0}/", id));
        this.currentRequest.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = this.currentRequest.SendWebRequest();
        response.completed += async (AsyncOperation obj) =>
        {
            if (this.currentRequest.responseCode == 200)
            {
                this.trainer = JsonUtility.FromJson<Trainer>(this.currentRequest.downloadHandler.text);
#if UNITY_WEBGL
                this.socket = new Socket(string.Format("ws://localhost/ws/{0}/false", this.trainer.game), this);
#endif
                StartCoroutine("waitForSceneLoad", new Tuple<string, Action>("Game", this.InstantiateMap));
            }
        };
    }

    public void Refresh()
    {
        foreach(Action action in this.RefreshActions)
        {
            action();
        }
    }

    public void InstantiateMap()
    {
#if UNITY_WEBGL
        try
        {
            GameObject background = Instantiate(this.BackgroundPrefab, Vector3.zero, Quaternion.identity);
            if (!string.IsNullOrEmpty(this.socket.Background))
            {
                background.GetComponent<Background>().LoadBackground(this.socket.Background);
            }
            if (this.socket.Tokens != null)
            {
                for (int i = 0; i < this.socket.Tokens.Length; i++)
                {
                    MapToken token = this.socket.Tokens[i];
                    GameObject goToken = Instantiate(this.TokenPrefab, Vector3.zero, Quaternion.identity);
                    goToken.GetComponent<Token>().LoadToken(token.id, i);
                    this.tokens.Add(goToken);
                }
            }
        }
        catch (Exception e)
        {
            print(e);
        }
#endif
    }

    internal void UpdateMap(MapToken token)
    {
        GameObject goToken = this.tokens.First<GameObject>((g) => g.GetComponent<Token>().mapID == token.id);
        goToken.transform.position = new Vector2(token.x, token.y);
    }

#if UNITY_WEBGL
    public void ParseMessage(string content)
    {
        this.socket.ParseMessage(content);
    }
#endif
    private IEnumerator waitForSceneLoad(Tuple<string, Action> input)
    {
        string sceneName = input.Item1;
        Action operation = input.Item2;
        SceneManager.LoadScene(sceneName);
        while(SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }
        operation();
    }
}