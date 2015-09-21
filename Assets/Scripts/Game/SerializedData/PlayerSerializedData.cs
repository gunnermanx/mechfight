using UnityEngine;
using System.Collections.Generic;
using FullSerializer;

public class PlayerSerializedData {

	[fsProperty]
	public Dictionary<string,PilotSerializedData> _pilotCollection;

	[fsProperty]
	public Dictionary<string,int> _mechPartCollection;

	[fsProperty]
	public List<UnitSerializedData> _units;
}

