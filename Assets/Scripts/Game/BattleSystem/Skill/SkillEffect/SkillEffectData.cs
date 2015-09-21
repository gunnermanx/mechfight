using UnityEngine;
using System.Collections;

public class SkillEffectData : ScriptableObject {

	public enum Trigger {
		IMMEDIATE,
		QUEUED
	}

	public enum Type {
		CHARGE,
		HEAL,
		DAMAGE,
		BUFF_DEFENSE,
		BUFF_ATTACK,
		DEBUFF_DEFENSE,
		DEBUFF_ATTACK
	}

	
	public Type type;

	public Trigger trigger;

	public int duration;
}

