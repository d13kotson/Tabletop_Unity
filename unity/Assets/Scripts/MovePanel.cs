using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanel : MonoBehaviour
{
	private GameController controller = default;

    void Start()
    {
        
    }

    internal void Set(Attack attack, int attackerID, TokenType attackerType) {
		this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Transform content = this.gameObject.transform;
        content.Find("Name").gameObject.GetComponent<Text>().text = attack.name;
        content.Find("Type").gameObject.GetComponent<Text>().text = attack.type.name;
        content.Find("Frequency").gameObject.GetComponent<Text>().text = attack.frequency;
        content.Find("AC").gameObject.GetComponent<Text>().text = attack.ac.ToString();
        content.Find("DB").gameObject.GetComponent<Text>().text = attack.damage_base.id.ToString();
        content.Find("Class").gameObject.GetComponent<Text>().text = attack.attack_class;
        content.Find("Range").gameObject.GetComponent<Text>().text = attack.range;
		content.Find("Effect").gameObject.GetComponent<Text>().text = attack.effect;
		content.Find("AttackButton").gameObject.GetComponent<Button>().onClick.AddListener(() => this.prepareAttack(attack, attackerID, attackerType));
    }

	private void prepareAttack(Attack attack, int attackerID, TokenType attackerType) {
		this.controller.IsAttacking = true;
		this.controller.attack = attack;
		this.controller.attacker = attackerID;
		this.controller.attackerType = attackerType;
		this.controller.CloseScreens();
	}
}
