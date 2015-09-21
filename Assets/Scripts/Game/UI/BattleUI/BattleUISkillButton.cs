using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleUISkillButton : MonoBehaviour
{
	public Text SkillText;

	public Button SkillButton;

	public Text EnergyCostLabel;


	private SkillData _data;

	public void Initialize( SkillData data, int currentPower ) {
		_data = data;

		// Populate button visuals
		if ( data != null ) {
			gameObject.SetActive( true );
			SkillText.text = data.skillNameId;
			SkillButton.interactable = ( data.cost <= currentPower );
			EnergyCostLabel.text = data.cost.ToString();
		} else {
			gameObject.SetActive( false );
		}
	}

	public void SetSelected( bool toggle ) {
		// do some ui toggling
	}
}

