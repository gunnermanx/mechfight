using UnityEngine;
using System.Collections;

public class PersistenceManager : MonoBehaviour {


	// FOR NOW NO NETWORK STUFF, STORED LOCALLY, 
	//TODO HUGE TODO

	private readonly static string PLAYER_JSON_WWW_PATH = "file://" + Application.dataPath + "/TempPersistence/Player.json";

	public PlayerSerializedData LoadPlayerData() {

		Debug.Log ( "LoadPlayerData start" );

		PlayerSerializedData playerData = null;
		WWW www = new WWW( PLAYER_JSON_WWW_PATH );
		while ( !www.isDone ) {
			// update some progress bar later
		}
	
		if ( !string.IsNullOrEmpty( www.text ) ) {
			playerData = (PlayerSerializedData) Serializer.Deserialize( typeof(PlayerSerializedData), www.text );
		}

		Debug.Log ( "LoadPlayerData done!" );

		return playerData;
	}
}

