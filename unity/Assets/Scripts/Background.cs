using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.GetBackground("http://localhost/map/background/86"));
    }

    // Update is called once per frame
    void Update()
    {
        
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
