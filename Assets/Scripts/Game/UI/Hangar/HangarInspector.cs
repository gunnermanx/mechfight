using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HangarInspector : MonoBehaviour {
	public Text AttackLabel;
	public Text DefenseLabel;
	public Text SpeedLabel;
	public Text HPLabel;
	public Text PowerMaxLabel;
	public Text PowerChargeLabel;
	public Text PowerInitialLabel;

	public Text PartNameLabel;
	public Image PartIconImage;

	public GameObject PartInspectorContainer;

	public GameObject PilotInspectorContainer;
	public Transform PilotSkillsContainer;
	public GameObject PilotSkillPrefab;

	public void UpdateInspector( MechPartData data ) {

		PartInspectorContainer.SetActive( true );
		PilotInspectorContainer.SetActive( false );

		AttackLabel.text = data.attack.ToString();
		DefenseLabel.text = data.defense.ToString();
		SpeedLabel.text = data.speed.ToString();
		HPLabel.text = data.hp.ToString();
		PowerMaxLabel.text = data.powerMax.ToString();
		PowerChargeLabel.text = data.powerCharge.ToString();
		PowerInitialLabel.text = data.powerInitial.ToString();

		PartNameLabel.text = data.name;
		PartIconImage.sprite = data.icon;
	}

	public void UpdateInspector( PilotData pilotData ) {
		
		PartInspectorContainer.SetActive( false );
		PilotInspectorContainer.SetActive( true );

		for ( int i = 0, count = PilotSkillsContainer.childCount; i < count; i++ ) {
			GameObject.Destroy( PilotSkillsContainer.GetChild( i ).gameObject );
		}
		for ( int i = 0, count = pilotData.skills.Count; i < count; i++ ) {
			SkillData skillData = pilotData.skills[ i ];

			GameObject skill = Instantiate( PilotSkillPrefab ) as GameObject;
			skill.transform.SetParent( PilotSkillsContainer );
			skill.GetComponent<PilotSkillItem>().Initialize( skillData );
		}

		PartNameLabel.text = pilotData.name;
		PartIconImage.sprite = pilotData.icon;
	}
}

