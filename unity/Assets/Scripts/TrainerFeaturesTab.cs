using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerFeaturesTab : MonoBehaviour
{
    private Trainer trainer;
    public GameObject FeaturePanel = default;
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
		foreach(TrainerFeature feature in this.trainer.trainer_feature)
        {
            GameObject featurePanel = Instantiate(this.FeaturePanel);
			featurePanel.transform.SetParent(content);
			featurePanel.GetComponent<FeaturePanel>().Set(feature.feature, trainer.id);
        }
    }
}
