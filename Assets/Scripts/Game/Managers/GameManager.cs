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
		InitializeManagers();

		//TODO test
		LoadBattleScene();
	}

	private void InitializeManagers() {
		_playerDataManager.Initialize( _persistenceManager.LoadPlayerData() );
	}

	private void InitializeDatabases() {
	}

	private void LoadBattleScene() {
		Application.LoadLevel( "BattleScene" );


	}

	private void OnLevelWasLoaded( int level ) {
		if ( level == 1 ) {
			List<UnitSerializedData> unitsData = GameManager.PlayerDataManager.GetUnitsData();
			_battleManager.StartBattle( BattleManager.BattleType.THREE_ON_THREE, unitsData, unitsData );
		}
	}
}
