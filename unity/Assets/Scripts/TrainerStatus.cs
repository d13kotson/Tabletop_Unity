using UnityEngine;
using UnityEngine.UI;

public class TrainerStatus : MonoBehaviour
{
    private Trainer trainer;
    private GameController controller;
    private GameObject statsTab;
    private GameObject skillsTab;
    private GameObject movesTab;

    void Start()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.transform.SetParent(canvas.transform);
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(470, -100);
    }

    void Update()
    {
        
    }

    internal void Set(Trainer trainer)
    {
        this.trainer = trainer;
        Transform content = this.gameObject.transform.Find("Viewport").Find("Content");
        content.Find("Name").gameObject.GetComponent<Text>().text = this.trainer.name;
        this.statsTab = content.Find("StatsPanel").gameObject;
        this.statsTab.GetComponent<TrainerStatsTab>().Set(this.trainer);
        this.skillsTab = content.Find("SkillsPanel").gameObject;
        this.skillsTab.GetComponent<TrainerSkillsTab>().Set(this.trainer);
        this.movesTab = content.Find("MovesPanel").gameObject;
        this.movesTab.GetComponent<TrainerMovesTab>().Set(this.trainer);
        this.ShowStats();
    }

    public void ShowStats()
    {
        this.HideTabs();
        this.statsTab.SetActive(true);
    }

    public void ShowSkills()
    {
        this.HideTabs();
        this.skillsTab.SetActive(true);
    }

    public void ShowMoves()
    {
        this.HideTabs();
        this.movesTab.SetActive(true);
    }

    private void HideTabs()
    {
        this.statsTab.SetActive(false);
        this.skillsTab.SetActive(false);
        this.movesTab.SetActive(false);
    }

    void UpdateAttack()
    {
        string newCS = this.gameObject.transform.Find("StatsPanel").Find("AttackCS").gameObject.GetComponentInChildren<Text>().text;
        float multiple = int.Parse(newCS) / 2;
    }

    public void Enable()
    {
        bool value = !this.gameObject.activeSelf;
        if (value)
        {
            this.controller.OpenScreens.Add(this.gameObject);
        }
        else
        {
            this.controller.OpenScreens.Remove(this.gameObject);
        }
        this.gameObject.SetActive(value);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
