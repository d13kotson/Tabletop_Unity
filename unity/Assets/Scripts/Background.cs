using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadBackground(string backgroundID)
    {
        StartCoroutine(this.GetBackground(string.Format("http://localhost/map/background/{0}", backgroundID)));
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
