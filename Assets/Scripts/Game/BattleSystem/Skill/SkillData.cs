using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillData : ScriptableObject {

	public enum Target {
		SELF,
		ALLY,
		ALL_ALLIES,
		ENEMY,
		ALL_ENEMIES
	}

	public Target target;

	public Stat sourceStat;
	
	public SkillEffectData[] skillEffects;

	// divide by 100 to get multiplier, ie: 100 => 100% of attack
	public int power;
	
	public int cost;

	public string skillNameId;
	
	public string skillDescriptionId;
	                    
}