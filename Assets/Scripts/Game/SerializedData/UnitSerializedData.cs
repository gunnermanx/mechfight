using UnityEngine;
using System.Collections.Generic;
using FullSerializer;

public class UnitSerializedData {
	
	[fsProperty]
	public string _pilotId;
	
	[fsProperty]
	public List<string> _mechPartIds;
}

