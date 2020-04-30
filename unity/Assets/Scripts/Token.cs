using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Token : MonoBehaviour
{
    private bool isDragging = false;
    private GameController controller = default;
    public int mapID;

    private void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void OnMouseDown()
    {
        this.isDragging = true;
        this.controller.IsDragging = true;
    }

    public void OnMouseUp()
    {
        this.isDragging = false;
        this.controller.IsDragging = false;
#if UNITY_WEBGL
        this.controller.socket.UpdateToken(this);
#endif
    }

    void Update()
    {
        if(isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    public void LoadToken(int id, int mapID)
    {
        this.mapID = mapID;
        StartCoroutine(this.GetToken(string.Format("http://localhost/map/token/{0}", id)));
    }

    private IEnumerator GetToken(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        GetComponent<SpriteRenderer>().sprite = sprite;
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        Vector2 spriteSize = sprite.bounds.size;
        collider.size = spriteSize;
        collider.offset = new Vector2(spriteSize.x / 2, spriteSize.y / 2);
    }
}
