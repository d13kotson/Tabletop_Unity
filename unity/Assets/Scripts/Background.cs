using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Background : MonoBehaviour
{
    private GameController controller;

    void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void LoadBackground(string backgroundID)
    {
        this.controller.SendTextureRequest(string.Format("api/image/{0}", backgroundID), (request) =>
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            GetComponent<SpriteRenderer>().sprite = sprite;
        }, (request) =>
        {

        });
    }
}
