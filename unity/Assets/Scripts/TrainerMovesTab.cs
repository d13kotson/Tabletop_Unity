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
		foreach(Transform child in content) {
			Destroy(child.gameObject);
		}
		foreach(TrainerAttack attack in this.trainer.trainer_attack)
        {
            GameObject movePanel = Instantiate(this.MovePanel);
            movePanel.transform.SetParent(content);
            movePanel.GetComponent<MovePanel>().Set(attack.attack, trainer.id, TokenType.trainer);
        }
    }
}
