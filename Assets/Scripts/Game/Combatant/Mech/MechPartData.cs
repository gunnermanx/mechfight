using UnityEngine;
using System.Collections;

[System.Serializable]
public class MechPartData : ScriptableObject {

	public enum PartType {
		HEAD,
		ARMS,
		LEGS,
		CORE,
		WEAPON,
		GENERATOR
	}

	public PartType partType = PartType.ARMS;

	public int defense = 0;

	public int attack = 0;

	public int speed = 0;

	public int hp = 0;

	public int powerMax = 0;

	public int powerCharge = 0;

	public int powerInitial = 0;

	public Sprite icon = null;
}

