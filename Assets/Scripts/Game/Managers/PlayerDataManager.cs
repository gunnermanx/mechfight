using UnityEngine;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour {
	
	public PlayerSerializedData PlayerData {
		get; private set;
	}

	public void Initialize( PlayerSerializedData data ) {
		PlayerData = data;
	}

	public List<UnitSerializedData> GetUnitsData() {
		return PlayerData._units;
	}

	public PilotSerializedData GetPilotData( string id ) {
		PilotSerializedData data = null;
		PlayerData._pilotCollection.TryGetValue( id, out data );
#if DEBUG
		Debug.Assert( data != null, "Couldn't find pilot data with id " + id + " in PlayerData" );
#endif
		return data;
	}

	public Dictionary<string, PilotSerializedData> GetPilotCollection() {
		return PlayerData._pilotCollection;
	}

	public Dictionary<string,int> GetMechPartCollection() {
		return PlayerData._mechPartCollection;
	}

}

