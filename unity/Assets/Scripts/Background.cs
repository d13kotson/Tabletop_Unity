using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Background : MonoBehaviour
{
    private GameController controller;

    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        
    }

    public void LoadBackground(string backgroundID)
    {
        StartCoroutine(this.GetBackground(string.Format("http://localhost/api/image/{1}", this.controller.URL, backgroundID)));
    }

    private IEnumerator GetBackground(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
