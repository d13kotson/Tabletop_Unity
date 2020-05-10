using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private string authToken = default;
    private UserInfo userInfo = default;
    internal Game game = default;
    internal Trainer trainer = default;
    private GameObject background = default;
    private List<GameObject> tokens = new List<GameObject>();
    internal Socket socket = default;
    public bool IsDragging;
    internal bool isGM;
    internal List<GameObject> OpenScreens = new List<GameObject>();
    private Queue<string> messages = new Queue<string>();

    public List<Action> RefreshActions = new List<Action>();

    public Text LoginError;
    private string URL = default;

    public static bool created;

    void Awake()
    {
        this.OpenScreens = new List<GameObject>();
        this.URL = Application.absoluteURL;
        if(this.URL == "")
        {
            this.URL = "http://localhost";
        }
        else if(this.URL.Contains(':'))
        {
            this.URL = this.URL.Substring(0, Application.absoluteURL.IndexOf(':'));
        }

        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        while(this.messages.Count > 0)
        {
            string message = this.messages.Dequeue();
            this.ParseMessage(message);
        }
    }

    public void Login()
    {
        GameObject usernameObj = GameObject.Find("Username");
        this.username = usernameObj.GetComponent<InputField>().text;
        GameObject passwordObj = GameObject.Find("Password");
        string password = passwordObj.GetComponent<InputField>().text;
        this.SendPostRequest("rest-auth/login", string.Format("{{\"username\": \"{0}\", \"password\": \"{1}\"}}", this.username, password), (request) =>
        {
            Key key = JsonUtility.FromJson<Key>(request.downloadHandler.text);
            this.authToken = key.key;
            request.Dispose();
            this.sendUserInfo();
        }, (request) =>
        {
            LoginError.gameObject.SetActive(true);
        }, false);
    }

    private void sendUserInfo()
    {
        this.SendGetRequest(string.Format("api/user/{0}", this.username), (request) =>
        {
            this.userInfo = JsonUtility.FromJson<UserInfo>(request.downloadHandler.text);
            request.Dispose();
            StartCoroutine("waitForSceneLoad", new Tuple<string, Action>("CharacterSelect", this.InstantiateCharacterSelect));
        }, (request) =>
        {

        });
    }

    private void InstantiateCharacterSelect()
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
            if (screen != null)
            {
                screen.SetActive(false);
            }
        }
        this.OpenScreens = new List<GameObject>();
    }

    private void loadGame(int id)
    {
        this.isGM = true;
        this.SendGetRequest(string.Format("api/game/{0}", id), (request) =>
        {
            this.game = JsonUtility.FromJson<Game>(request.downloadHandler.text);
            this.socket = new Socket(string.Format("ws://localhost/ws/{0}/true", this.game.id), this);
            SceneManager.LoadScene("Game");
        }, (request) =>
        {

        });
    }

    private void loadTrainer(int id)
    {
        this.isGM = false;
        this.SendGetRequest(string.Format("api/trainer/{0}", id), (request) =>
        {
            this.trainer = JsonUtility.FromJson<Trainer>(request.downloadHandler.text);
            this.socket = new Socket(string.Format("ws://localhost/ws/{0}/false", this.trainer.game), this);
            SceneManager.LoadScene("Game");
        }, (request) =>
        {

        });
    }

    public void Reload()
    {
        if (this.isGM)
        {
            this.SendGetRequest(string.Format("api/game/{0}", this.game.id), (request) =>
            {
                this.game = JsonUtility.FromJson<Game>(request.downloadHandler.text);
                this.Refresh();
            }, (request) =>
            {

            });
        }
        else
        {
            this.SendGetRequest(string.Format("api/trainer/{0}", this.trainer.id), (request) =>
            {
                this.trainer = JsonUtility.FromJson<Trainer>(request.downloadHandler.text);
                this.Refresh();
            }, (request) =>
            {

            });
        }
    }

    private void Refresh()
    {
        foreach(Action action in this.RefreshActions)
        {
            action();
        }
    }

    public void InstantiateMap()
    {
        if (this.background == null)
        {
            this.background = Instantiate(this.BackgroundPrefab, Vector3.zero, Quaternion.identity);
        }
        this.SetBackground();
        if (this.socket.Tokens != null)
        {
            foreach(int key in this.socket.Tokens.Keys)
            {
                MapToken token = this.socket.Tokens[key];
                this.AddToken(token);
            }
        }
    }

    public void SetBackground()
    {
        if (!string.IsNullOrEmpty(this.socket.Background))
        {
            this.background.GetComponent<Background>().LoadBackground(this.socket.Background);
        }
    }

    public void SendGetRequest(string url, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onFailure)
    {
        UnityWebRequest request = UnityWebRequest.Get(string.Format("{0}/{1}/", this.URL, url));
        request.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = request.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (request.responseCode >= 200 && request.responseCode < 300)
            {
                onSuccess(request);
            }
            else
            {
                onFailure(request);
            }
        };
    }

    public void SendPostRequest(string url, string data, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onFailure, bool authorized = true)
    {
        UnityWebRequest request = UnityWebRequest.Post(string.Format("{0}/{1}/", this.URL, url), data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        if (authorized)
        {
            request.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        }
        AsyncOperation response = request.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (request.responseCode >= 200 && request.responseCode < 300)
            {
                onSuccess(request);
            }
            else
            {
                onFailure(request);
            }
        };
    }

    public void SendPutRequest(string url, string data, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onFailure)
    {
        UnityWebRequest request = UnityWebRequest.Put(string.Format("{0}/{1}/", this.URL, url), data);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = request.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (request.responseCode >= 200 && request.responseCode < 300)
            {
                onSuccess(request);
            }
            else
            {
                onFailure(request);
            }
        };
    }

    public void SendTextureRequest(string url, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onFailure)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(string.Format("{0}/{1}/", this.URL, url));
        request.SetRequestHeader("Authorization", string.Format("Token {0}", this.authToken));
        AsyncOperation response = request.SendWebRequest();
        response.completed += (AsyncOperation obj) =>
        {
            if (request.responseCode >= 200 && request.responseCode < 300)
            {
                onSuccess(request);
            }
            else
            {
                onFailure(request);
            }
        };
    }

    internal void LogOut()
    {
        this.OpenScreens = new List<GameObject>();
        this.authToken = "";
        SceneManager.LoadScene("Login");

    }

    internal void CharacterSelect()
    {
        this.OpenScreens = new List<GameObject>();
        StartCoroutine("waitForSceneLoad", new Tuple<string, Action>("CharacterSelect", this.InstantiateCharacterSelect));
    }

    internal void AddToken(MapToken token)
    {
        GameObject goToken = Instantiate(this.TokenPrefab, new Vector2(token.x, token.y), Quaternion.identity);
        goToken.GetComponent<Token>().LoadToken(token.tokenID, token.imageID);
        
        this.tokens.Add(goToken);
    }

    internal void RemoveToken(int id)
    {
        GameObject removeObj = null;
        for (int i = 0; i < this.tokens.Count; i++)
        {
            GameObject tokenObj = this.tokens[i];
            Token token = tokenObj.GetComponent<Token>();
            if(token.tokenID == id)
            {
                removeObj = tokenObj;
                break;
            }
        }
        if (removeObj != null)
        {
            this.tokens.Remove(removeObj);
            Destroy(removeObj);
        }
    }

    internal void UpdateMap(MapToken token)
    {
        GameObject goToken = this.tokens.First<GameObject>((g) => g.GetComponent<Token>().tokenID == token.tokenID);
        goToken.transform.position = new Vector2(token.x, token.y);
    }

    internal void AddMessage(string message)
    {
        this.messages.Enqueue(message);
    }

    public void ParseMessage(string content)
    {
        this.socket.ParseMessage(content);
    }

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

    internal void Print(string message)
    {
        print(message);
    }
}