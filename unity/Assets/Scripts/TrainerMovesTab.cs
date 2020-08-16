using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerMovesTab : MonoBehaviour
{
    private Trainer trainer;
    public GameObject MovePanel = default;
    void Start()
    {

    }

    internal void Set(Trainer trainer)
    {
        this.trainer = trainer;
        Transform content = this.gameObject.transform;
        int position = -50;
        foreach(TrainerAttack attack in this.trainer.trainer_attack)
        {
            GameObject movePanel = Instantiate(this.MovePanel);
            movePanel.transform.SetParent(content);
            RectTransform rect = movePanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, position);
            rect.sizeDelta = new Vector2(0, 120);
            movePanel.GetComponentInChildren<Text>().text = name;
            movePanel.GetComponent<MovePanel>().Set(attack.attack, trainer.id, TokenType.trainer);
            position -= 120;
        }
    }
}
