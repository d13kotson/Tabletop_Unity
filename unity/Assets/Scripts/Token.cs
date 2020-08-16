using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour
{
    private bool isDragging = false;
    private GameController controller = default;
    private Vector2 size = Vector2.zero;
    public int imageID;
    public int tokenID;
    public GameObject Canvas;
    public GameObject RightClickMenu;
    private GameObject StatusPanel;
	private int owner;

	private int repID;
	private TokenType tokenType;

    private void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public void OnMouseDown()
    {
		if(this.controller.isGM || this.controller.trainer.id == this.owner) {
			this.isDragging = true;
			this.controller.IsDragging = true;
		}
    }

    public void OnMouseUp() {
		if(this.controller.isGM || this.controller.trainer.id == this.owner) {
			this.isDragging = false;
			this.controller.IsDragging = false;
			this.controller.socket.UpdateToken(this);
		}
		if(this.controller.IsAttacking) {
			this.controller.socket.Attack(this.controller.attacker, this.controller.attackerType, this.repID, this.tokenType, this.controller.attack.id);
			this.controller.IsAttacking = false;
		}
	}

    public void OnMouseOver() {
		if(this.controller.isGM || this.controller.trainer.id == this.owner) {
			if(Input.GetMouseButtonDown(1)) {
				this.controller.OpenScreens.Add(this.RightClickMenu);
				this.RightClickMenu.SetActive(!this.RightClickMenu.activeSelf);
				this.RightClickMenu.GetComponent<RectTransform>().position = Input.mousePosition;
			}
		}
    }


    void Update()
    {
		if(!this.controller.IsMeasuring && (this.controller.isGM || this.controller.trainer.id == this.owner)) {
			if(isDragging) {
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				transform.Translate(mousePosition - size / 2);
			}
		}
    }

    public void LoadToken(int tokenID, int imageID, int repID, TokenType tokenType, int owner)
    {
        this.tokenID = tokenID;
        this.imageID = imageID;
        this.GetToken(imageID);
		this.repID = repID;
		this.tokenType = tokenType;
		this.owner = owner;
    }

    public void SetStatusPanel(GameObject statusPanel)
    {
        this.StatusPanel = statusPanel;
    }

    private void GetToken(int id)
    {
        this.controller.SendGetRequest(string.Format("api/image-info/{0}", id), (infoRequest) =>
        {
            ImageStruct imageInfo = JsonUtility.FromJson<ImageStruct>(infoRequest.downloadHandler.text);
            this.controller.SendTextureRequest(string.Format("api/image/{0}", id), (textureRequest) =>
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(textureRequest);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                float scale = (float)imageInfo.width / (float)sprite.texture.width;
                this.size = sprite.bounds.size * scale;
                GetComponent<SpriteRenderer>().sprite = sprite;
                BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
                Vector2 spriteSize = sprite.bounds.size;
                collider.size = spriteSize;
                collider.offset = new Vector2(spriteSize.x / 2, spriteSize.y / 2);
                this.transform.localScale = new Vector2(scale, scale);
            }, (textureRequest) =>
            {

            });
        }, (infoRequest) =>
        {

        });
    }

    public void Delete()
    {
        this.controller.socket.DeleteToken(this.tokenID);
    }

    public void ShowStats()
    {
        if (this.StatusPanel != null)
        {
            PokemonStatus pokeStatus = this.StatusPanel.GetComponent<PokemonStatus>();
            if (pokeStatus == null)
            {
                TrainerStatus trainerStatus = this.StatusPanel.GetComponent<TrainerStatus>();
                if (trainerStatus != null)
                {
                    trainerStatus.Enable();
                }
            }
            else
            {
                pokeStatus.Enable();
            }
        }
    }
}
