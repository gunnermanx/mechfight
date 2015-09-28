using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PersistenceManager : MonoBehaviour {

	// FOR NOW NO NETWORK STUFF, STORED LOCALLY, 
	//TODO HUGE TODO

	public delegate void PlayerDataLoadedDelegate( PlayerSerializedData data );

	private WWW _fileWWW = null;
	private PlayerDataLoadedDelegate _callback;

	public void LoadPlayerData( PlayerDataLoadedDelegate playerDataLoadedCallback ) {
		PlayerSerializedData playerData = null;
		_callback = playerDataLoadedCallback;

		StartCoroutine( LoadPlayerDataFile( playerData ) );
	}

	private IEnumerator LoadPlayerDataFile( PlayerSerializedData playerData ){

		Debug.Log ( "LoadPlayerData start" );

		string playerDataText = null;

		// loading from persistent data
		string filePath = Application.persistentDataPath + "/Player.json";
		if( File.Exists( filePath ) ) {
			_fileWWW = new WWW( "file://" + filePath );
			yield return _fileWWW;
			if( _fileWWW.bytes.Length > 0 ){
				playerDataText = _fileWWW.text;
			}
		}
		// first time load
		else {
			TextAsset textData = (TextAsset) Resources.Load("TempPersistence/Player");
			playerDataText = textData.text;
		}

		if ( !string.IsNullOrEmpty( playerDataText ) ) {
			playerData = (PlayerSerializedData) Serializer.Deserialize( typeof(PlayerSerializedData), playerDataText );
		}

		_callback( playerData );

		Debug.Log ( "LoadPlayerData done!" );
	}

	public void SavePlayerData() {
		string playerDataText = Serializer.Serialize( typeof(PlayerSerializedData), GameManager.PlayerDataManager.PlayerData );
		System.IO.File.WriteAllText( Application.persistentDataPath + "/Player.json", playerDataText );
	}
}

