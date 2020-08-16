﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TrainerStatus : Window
{
    private Trainer trainer;
    private GameObject statsTab;
    private GameObject skillsTab;
    private GameObject movesTab;

    void Awake()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        this.transform.SetParent(canvas.transform);
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(470, -100);
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
        content.Find("Name").gameObject.GetComponent<Text>().text = this.trainer.name;
        this.statsTab = content.Find("StatsPanel").gameObject;
        this.statsTab.GetComponent<TrainerStatsTab>().Set(this.trainer);
        this.skillsTab = content.Find("SkillsPanel").gameObject;
        this.skillsTab.GetComponent<TrainerSkillsTab>().Set(this.trainer);
        this.movesTab = content.Find("MovesPanel").gameObject;
        this.movesTab.GetComponent<TrainerMovesTab>().Set(this.trainer);
    }

    private void setTokenSprite()
    {
        this.controller.SendTextureRequest(string.Format("api/image/{0}", this.trainer.token.image.id), (request) =>
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            this.gameObject.transform.Find("Viewport").Find("Content").Find("AddToken").GetComponent<Image>().sprite = sprite;
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

    public void UpdateAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.attack_cs = newCS;
        this.UpdateTrainer();
    }

    public void UpdateDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.defense_cs = newCS;
        this.UpdateTrainer();
    }

    public void UpdateSpecialAttack(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.special_attack_cs = newCS;
        this.UpdateTrainer();
    }

    public void UpdateSpecialDefense(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.special_defense_cs = newCS;
        this.UpdateTrainer();
    }

    public void UpdateSpeed(InputField input)
    {
        int newCS = int.Parse(input.text);
        this.trainer.speed_cs = newCS;
        this.UpdateTrainer();
    }
}
