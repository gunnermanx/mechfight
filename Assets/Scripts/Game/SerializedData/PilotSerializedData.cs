using UnityEngine;
using FullSerializer;
using System.Collections.Generic;

public class PilotSerializedData {

	[fsProperty]
	public string _pilotId;

	[fsProperty]
	public int _pilotLevel;

	[fsProperty]
	public List<string> _pilotSkillIds;

	[fsProperty]
	public int _pilotXp;
	
}

