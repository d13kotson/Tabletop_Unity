using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerEdgesTab : MonoBehaviour
{
    private Trainer trainer;
    public GameObject EdgePanel = default;
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
		foreach(TrainerEdge edge in this.trainer.trainer_edge)
        {
            GameObject edgePanel = Instantiate(this.EdgePanel);
			edgePanel.transform.SetParent(content);
			edgePanel.GetComponent<EdgePanel>().Set(edge.edge, trainer.id);
        }
    }
}
