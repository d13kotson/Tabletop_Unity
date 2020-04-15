using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Token : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateToken(35);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateToken(int tokenID)
    {
        StartCoroutine(this.GetToken(string.Format("http://localhost/map/token/{0}", tokenID)));
    }

    private IEnumerator GetToken(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
