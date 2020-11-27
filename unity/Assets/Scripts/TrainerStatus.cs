using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TrainerStatus : Window
{
    private Trainer trainer;
    private GameObject statsTab;
    private GameObject skillsTab;
    private GameObject movesTab;
	private GameObject edgesTab;
	private GameObject featuresTab;
	private RectTransform panelsPanel;

	void Awake()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.transform.SetParent(canvas.transform);
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(490, -100);
    }

    internal void Set(Trainer trainer)
    {
        this.trainer = trainer;
        this.UpdateTrainer();
        this.setTokenSprite();
        this.ShowStats();
    }

    private void UpdateTrainer()
    {
        Transform content = this.gameObject.transform.Find("Viewport").Find("Content");
		this.panelsPanel = (RectTransform)content.Find("Panels");
		content.Find("Main").Find("Name").gameObject.GetComponent<Text>().text = this.trainer.name;
		content.Find("Level").Find("Level").gameObject.GetComponent<Text>().text = this.trainer.level.ToString();
        this.statsTab = content.Find("Panels").Find("StatsPanel").gameObject;
        this.statsTab.GetComponent<TrainerStatsTab>().Set(this.trainer);
        this.skillsTab = content.Find("Panels").Find("SkillsPanel").gameObject;
        this.skillsTab.GetComponent<TrainerSkillsTab>().Set(this.trainer);
        this.movesTab = content.Find("Panels").Find("MovesPanel").gameObject;
        this.movesTab.GetComponent<TrainerMovesTab>().Set(this.trainer);
		this.edgesTab = content.Find("Panels").Find("EdgesPanel").gameObject;
		this.edgesTab.GetComponent<TrainerEdgesTab>().Set(this.trainer);
		this.featuresTab = content.Find("Panels").Find("FeaturesPanel").gameObject;
		this.featuresTab.GetComponent<TrainerFeaturesTab>().Set(this.trainer);
	}

    private void setTokenSprite()
    {
        this.controller.SendTextureRequest(string.Format("api/image/{0}", this.trainer.token.image.id), (request) =>
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            this.gameObject.transform.Find("Viewport").Find("Content").Find("Buttons").Find("AddToken").GetComponent<Image>().sprite = sprite;
        }, (request) =>
        {

        });
    }

    public void AddToken()
    {
        this.controller.socket.AddToken(this.trainer.token.id, Camera.main.transform.position.x, Camera.main.transform.position.y, TokenType.trainer, this.trainer.id);
    }

    public void ShowStats()
    {
        this.HideTabs();
        this.statsTab.SetActive(true);
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, 200);
	}

    public void ShowSkills()
    {
        this.HideTabs();
        this.skillsTab.SetActive(true);
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, 220);
	}

    public void ShowMoves()
    {
        this.HideTabs();
        this.movesTab.SetActive(true);
		float size = this.trainer.trainer_attack.Length * 180;
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, size);
		RectTransform movesPanel = (RectTransform)this.panelsPanel.Find("MovesPanel");
		movesPanel.localPosition = new Vector2(movesPanel.localPosition.x, 0);
	}

	public void ShowEdges() {
		this.HideTabs();
		this.edgesTab.SetActive(true);
		float size = this.trainer.trainer_edge.Length * 80;
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, size);
		RectTransform edgesPanel = (RectTransform)this.panelsPanel.Find("EdgesPanel");
		edgesPanel.localPosition = new Vector2(edgesPanel.localPosition.x, 0);
	}

	public void ShowFeatures() {
		this.HideTabs();
		this.featuresTab.SetActive(true);
		float size = this.trainer.trainer_feature.Length * 155;
		this.panelsPanel.sizeDelta = new Vector2(this.panelsPanel.sizeDelta.x, size);
		RectTransform featuresPanel = (RectTransform)this.panelsPanel.Find("FeaturesPanel");
		featuresPanel.localPosition = new Vector2(featuresPanel.localPosition.x, 0);
	}

	private void HideTabs()
    {
        this.statsTab.SetActive(false);
        this.skillsTab.SetActive(false);
        this.movesTab.SetActive(false);
		this.edgesTab.SetActive(false);
		this.featuresTab.SetActive(false);
	}

    public void UpdateAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.attack_cs = newCS;
        this.UpdateTrainer();
		this.controller.SendPutRequest(string.Format("api/trainer/{0}", this.trainer.id), JsonUtility.ToJson(this.trainer), (request) => { }, (request) => { });
	}

    public void UpdateDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.defense_cs = newCS;
        this.UpdateTrainer();
		this.controller.SendPutRequest(string.Format("api/trainer/{0}", this.trainer.id), JsonUtility.ToJson(this.trainer), (request) => { }, (request) => { });
	}

    public void UpdateSpecialAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.special_attack_cs = newCS;
        this.UpdateTrainer();
		this.controller.SendPutRequest(string.Format("api/trainer/{0}", this.trainer.id), JsonUtility.ToJson(this.trainer), (request) => { }, (request) => { });
	}

    public void UpdateSpecialDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.special_defense_cs = newCS;
        this.UpdateTrainer();
		this.controller.SendPutRequest(string.Format("api/trainer/{0}", this.trainer.id), JsonUtility.ToJson(this.trainer), (request) => { }, (request) => { });
	}

    public void UpdateSpeed(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.speed_cs = newCS;
        this.UpdateTrainer();
		this.controller.SendPutRequest(string.Format("api/trainer/{0}", this.trainer.id), JsonUtility.ToJson(this.trainer), (request) => { }, (request) => { });
	}
}
