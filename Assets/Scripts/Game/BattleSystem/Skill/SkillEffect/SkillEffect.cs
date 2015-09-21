using UnityEngine;
using System.Collections;

public class SkillEffect {

	private SkillEffectData _data;

	public SkillEffect( Unit skillSourceUnit, int skillPower, SkillEffectData data ) {
		_data = data;
		RemainingDuration = data.duration;
		SkillSourceUnit = skillSourceUnit;
		SkillPower = skillPower;
	}

	// TODO
	public string SkillEffectName {
		get { return _data.name; }
	}

	public SkillEffectData.Trigger Trigger {
		get { return _data.trigger; }
	}

	public SkillEffectData.Type Type {
		get { return _data.type; }
	}

	public int RemainingDuration {
		get; private set;
	}
	
	public Unit SkillSourceUnit {
		get; private set;
	}
	
	public int SkillPower {
		get; private set;
	}

	public void Refresh() {
		RemainingDuration = _data.duration;
	}

	public void DecrementDuration() {
		RemainingDuration--;
	}

	public bool Equals( SkillEffect other ) {
		if ( other == null ){
			return false;
		}
		return _data == other._data; 
	}
}
