using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleManagerSettings : ScriptableObject {
	public int actionPointsThreshold;
	public float speedToActionPointsConversion;
	public float DRPer100Defense;
}

