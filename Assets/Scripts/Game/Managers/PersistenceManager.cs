using UnityEngine;
using System.Collections;

public class PersistenceManager : MonoBehaviour {


	// FOR NOW NO NETWORK STUFF, STORED LOCALLY, 
	//TODO HUGE TODO

	public PlayerSerializedData LoadPlayerData() {

		Debug.Log ( "LoadPlayerData start" );

//		string path = "file://" + Application.dataPath + "/TempPersistence/Player.json";
//
//		PlayerSerializedData playerData = null;
//		WWW www = new WWW( path );
//		while ( !www.isDone ) {
//			// update some progress bar later
//			Debug.Log ( "Progress " + www.progress.ToString("0.00") );
//		}
//
//		Debug.Log ( "Progress " + www.progress.ToString("0.00") );
	
		PlayerSerializedData playerData = null;

		TextAsset textData = (TextAsset) Resources.Load("TempPersistence/Player");
		string txt = textData.text;

		if ( !string.IsNullOrEmpty( txt ) ) {
			playerData = (PlayerSerializedData) Serializer.Deserialize( typeof(PlayerSerializedData), txt );
		}

		Debug.Log ( "LoadPlayerData done!" );

		return playerData;
	}
}

