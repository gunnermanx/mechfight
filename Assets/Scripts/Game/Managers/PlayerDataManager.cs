using UnityEngine;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour {

	private PlayerSerializedData _playerData;

	public void Initialize( PlayerSerializedData data ) {
		_playerData = data;
	}

	public List<UnitSerializedData> GetUnitsData() {
		return _playerData._units;
	}

	public PilotSerializedData GetPilotData( string id ) {
		PilotSerializedData data = null;
		_playerData._pilotCollection.TryGetValue( id, out data );
#if DEBUG
		Debug.Assert( data != null, "Couldn't find pilot data with id " + id + " in PlayerData" );
#endif
		return data;
	}
}

