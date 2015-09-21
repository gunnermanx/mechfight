using UnityEngine;
using System.Collections;

public class PersistenceManager : MonoBehaviour {


	// FOR NOW NO NETWORK STUFF, STORED LOCALLY, 
	//TODO HUGE TODO

	public PlayerSerializedData LoadPlayerData() {

		Debug.Log ( "LoadPlayerData start" );

		string path = "file://" + Application.dataPath + "/TempPersistence/Player.json";

		PlayerSerializedData playerData = null;
		WWW www = new WWW( path );
		while ( !www.isDone ) {
			// update some progress bar later
			Debug.Log ( "Progress " + www.progress );
		}
	
		if ( !string.IsNullOrEmpty( www.text ) ) {
			playerData = (PlayerSerializedData) Serializer.Deserialize( typeof(PlayerSerializedData), www.text );
		}

		Debug.Log ( "LoadPlayerData done!" );

		return playerData;
	}
}

