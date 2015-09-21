using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {

	public BattleUISkillButton[] SkillButtons;

	public GameObject SkillsPanel;

	public Slider GeneratorPowerSlider;

	public Text GeneratorPowerLabel;


	private List<SkillData> _skillData;

	public delegate void SkillUsedDelegate( Unit target, SkillData data );
	public SkillUsedDelegate UnitTappedCallback;

	private Battlefield _battlefield;

	private SkillData _tappedSkill = null;

	public void Initialize( Battlefield battlefield ) {
		_battlefield = battlefield;
	}

	public void SkillButtonDelegate( int index ) {
		// If we have not yet chosen a skill, we need to register our UnitTapped listener
		if ( _tappedSkill == null ) {
			// No further action is taken until a unit is tapped
			_battlefield.UnitTapped += UnitTapped;
		}

		// Now we selected a skill, we wait to select the target
		_tappedSkill = _skillData[ index ];

		Debug.Log ( "Button pressed for skill " + _tappedSkill.name );

		// highlight possible targets

		// change the state of the button
	}

	public void InitializeForUnit( Unit sourceUnit ) {
		_skillData = sourceUnit.GetAssignedSkills();

		// Update the generator bar
		int powerMax = sourceUnit.GetStat( Stat.POWER_MAX );
		int powerCurrent = sourceUnit.CurrentPower;
		GeneratorPowerLabel.text = powerCurrent.ToString() + "/" + powerMax.ToString();
		GeneratorPowerSlider.value = (float)powerCurrent / (float)powerMax;

		// Update the skills panel
		SkillsPanel.SetActive( true );	
		for ( int i = 0, count = SkillButtons.Length; i < count; i++ ) {
			if ( i < _skillData.Count ) {
				SkillButtons[ i ].Initialize( _skillData[ i ], powerCurrent );
			} else {
				SkillButtons[ i ].Initialize( null, powerCurrent );
			}
		}
	}

	public void PickSkillFinish() {
		SkillsPanel.SetActive( false );

		_skillData = null;
		_tappedSkill = null;
		_battlefield.UnitTapped -= UnitTapped;
	}

	private void UnitTapped( Unit unit ) {
		Debug.Log ( "Unit Tapped" + unit.name );

		if ( UnitTappedCallback != null ) {
			UnitTappedCallback( unit, _tappedSkill );
		}
	}
}

