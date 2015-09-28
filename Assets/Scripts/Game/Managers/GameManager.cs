using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

#region Managers
	// ============ MANAGERS ============ //
	private PersistenceManager _persistenceManager = null;
	public static PersistenceManager PersistenceManager {
		get { return _instance._persistenceManager; }
	}

	private PlayerDataManager _playerDataManager = null;
	public static PlayerDataManager PlayerDataManager {
		get { return _instance._playerDataManager; }
	}

	private BattleManager _battleManager = null;
	public static BattleManager BattleManager {
		get { return _instance._battleManager; }
	}
#endregion


#region Databases
	// ============ DATABASE ============ //
	private Database _database = null;
	public static Database Database {
		get { return _instance._database; }
	}
#endregion


	// Singleton Instance
	private static GameManager _instance = null;
	public static GameManager Instance {
		get { return _instance; }
	}

	private void Awake() {
		_instance = this;

		_persistenceManager = gameObject.GetComponent<PersistenceManager>();
		_playerDataManager = gameObject.GetComponent<PlayerDataManager>();
		_battleManager = gameObject.GetComponent<BattleManager>();

		_database = gameObject.GetComponent<Database>();

		DontDestroyOnLoad( gameObject );
	}

	private void Start() {
		_persistenceManager.LoadPlayerData( OnPlayerDataLoaded );
	}

	private void OnPlayerDataLoaded( PlayerSerializedData playerData ) {
		_playerDataManager.Initialize( playerData );
		//TODO test
		LoadHangarScene();
		//LoadBattleScene();
	}

	public void LoadHangarScene() {
		Debug.Log ( "Starting hangar scene" );
		Application.LoadLevel( "HangarScene" );
	}

	public void LoadBattleScene() {
		Debug.Log ( "Starting battle scene" );
		Application.LoadLevel( "BattleScene" );
	}

	private void OnLevelWasLoaded( int level ) {
		if ( level == 1 ) {
			List<UnitSerializedData> unitsData = GameManager.PlayerDataManager.GetUnitsData();
			_battleManager.StartBattle( BattleManager.BattleType.THREE_ON_THREE, unitsData, unitsData );
		}
	}
}

