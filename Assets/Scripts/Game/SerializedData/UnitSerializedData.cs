using UnityEngine;
using System.Collections.Generic;
using FullSerializer;

public class UnitSerializedData {
	
	[fsProperty]
	public string _pilotId;
	
	[fsProperty]
	public string _headMechPartId;
	[fsProperty]
	public string _coreMechPartId;
	[fsProperty]
	public string _armsMechPartId;
	[fsProperty]
	public string _legsMechPartId;
	[fsProperty]
	public string _weaponMechPartId;
	[fsProperty]
	public string _generatorMechPartId;

	[fsProperty]
	public string _unitName;

}

