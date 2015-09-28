using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PilotSkillItem : MonoBehaviour {
	public Text SkillNameLabel;
	public Text TargetLabel;
	public Text EffectLabel;
	public Text EnergyLabel;

	public void Initialize( SkillData data ) {
		SkillNameLabel.text = data.name;
		TargetLabel.text = data.target.ToString();
		// TODO
		EffectLabel.text = data.skillEffects[ 0 ].name;
		EnergyLabel.text = data.cost.ToString();
	}
}

