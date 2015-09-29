using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour {

	private const string ONE_ON_ONE_PREFAB = "Prefabs/Battlefields/Battle_1v1";
	private const string THREE_ON_THREE_PREFAB = "Prefabs/Battlefields/Battle_3v3";

	public enum BattleType {
		ONE_ON_ONE,
		THREE_ON_THREE
	}

	public BattleManagerSettings Settings;
	public GameObject BattleUIPrefab;


	private List<Unit> _allUnits = new List<Unit>();
	private Team _humanTeam;
	private Team _aiTeam;
	private Battlefield _battlefield = null;
	private BattleType _battleType;
	private BattleUI _battleUI = null;

	private InputBattleConductor _inputConductor = null;
	private AIBattleConductor _aiConductor = null;

	private bool _waitingForConductor = false;


	public void StartBattle( BattleType type, List<UnitSerializedData> humanTeamUnitData, List<UnitSerializedData> aiTeamUnitData ) {
		_battleType = type;


		InitializeBattlefield( humanTeamUnitData, aiTeamUnitData );
		InitializeTeams();

		InitializeBattleUI();
		InitalizeConductors();

		StartCoroutine( BattleCoroutine() );
	}

	private void InitializeBattleUI() {
		GameObject battleUIGO = Instantiate( BattleUIPrefab ) as GameObject;
		battleUIGO.transform.SetParent( GameObject.Find("Canvas").gameObject.transform, false );
		battleUIGO.transform.localScale = Vector3.one;
		_battleUI = battleUIGO.GetComponent<BattleUI>();
		_battleUI.Initialize( _battlefield );
	}
	
	private void InitializeBattlefield( List<UnitSerializedData> humanTeamUnitData, List<UnitSerializedData> aiTeamUnitData ) {
		string prefabPath = null;
		switch( _battleType ) {
		case BattleType.ONE_ON_ONE:
			prefabPath = ONE_ON_ONE_PREFAB;
			break;
		case BattleType.THREE_ON_THREE:
			prefabPath = THREE_ON_THREE_PREFAB;
			break;
		}
		GameObject battlefieldGO = Instantiate( Resources.Load( prefabPath ) ) as GameObject;
		_battlefield = battlefieldGO.GetComponent<Battlefield>();
		_battlefield.Initialize( humanTeamUnitData, aiTeamUnitData );
	}
	
	private void InitializeTeams() {
		_humanTeam = new Team( _battlefield.Team1Units );
		_aiTeam = new Team( _battlefield.Team2Units );
		
		IEnumerator<Unit> humanTeamEnumerator = _humanTeam.Units;
		while( humanTeamEnumerator.MoveNext() ) {
			_allUnits.Add( humanTeamEnumerator.Current );
		}
		
		IEnumerator<Unit> aiTeamEnumerator = _aiTeam.Units;
		while( aiTeamEnumerator.MoveNext() ) {
			_allUnits.Add( aiTeamEnumerator.Current );
		}
	}
	
	private void InitalizeConductors() {
		_inputConductor = new InputBattleConductor( _battleUI, _humanTeam, _aiTeam, RecieveConductorInput );
		_aiConductor = new AIBattleConductor( _aiTeam, _humanTeam, RecieveConductorInput );
	}


	private IEnumerator BattleCoroutine() {
		Unit unitToMove = null;

		// The battle isnt over... until the battle is over
		while ( !IsBattleOver() ) {
			// Get the next unit that should move
			unitToMove = GetNextUnitToMove();

			unitToMove.UnitStatus.SetSelected( true );

			// Resolve all applied skill effects that are on the unit
			unitToMove.ResolveAppliedSkillEffects();

			// Power Charge
			unitToMove.ChargePower();
	
			// We are waiting for the conductor now
			_waitingForConductor = true;





			// Depending on the unit, either get input from the input conductor or the ai conductor
			if ( _humanTeam.IsUnitInTeam( unitToMove ) ) {
				Debug.Log ( "======= Human input =======" );
				_inputConductor.GetConductorInput( unitToMove );
			} 
			else if ( _aiTeam.IsUnitInTeam( unitToMove ) ) {
				Debug.Log ( "======= AI input =======" );
				_aiConductor.GetConductorInput( unitToMove );
			} 
			else {
#if DEBUG
				Debug.Assert( false, "Unit not in any team?" );
#endif
			}

			// Keep yielding until the conductor has finished the current turn
			while( _waitingForConductor ) {
				yield return null;
			}

			// TODO: simulate some animation delay time, for clarity
			// just for clarity, add a small delay for now ( to simulate thinking )
			yield return new WaitForSeconds( 1f );

			unitToMove.UnitStatus.SetSelected( false );

			// The unit has finished its turn, reset its action points
			unitToMove.ResetActionPoints();
		}
	}

	private void RecieveConductorInput( ConductorInput conductorInput ) {
		Debug.Log( "Input recieved! " + conductorInput.TargetSkill.name + " used on " + conductorInput.TargetSkill.target.ToString() );

		// TODO: do some animation before anything else happens, everything below should be on callback
	
		// Get the skill power
		SkillData skillData = conductorInput.TargetSkill;
		float skillPower = skillData.power;
		int stat = conductorInput.SourceUnit.GetStat( skillData.sourceStat );
		int actualPower = Mathf.FloorToInt( ((float)stat)* ((float)skillPower) * 0.01f );

		// Deduct power cost
		conductorInput.SourceUnit.DeductPower( skillData.cost );

		// Target each target with the skill
		Unit targetUnit = null;
		for ( int i = 0, count = conductorInput.TargetUnits.Count; i < count; i++ ) {
			targetUnit = conductorInput.TargetUnits[ i ];
			targetUnit.TargetWithSkill( conductorInput.SourceUnit, actualPower, skillData.skillEffects );		
		}

		_waitingForConductor = false;
	}

	/// <summary>
	/// Determines whether the battle is done
	/// </summary>
	/// <returns><c>true</c> if this instance is battle over; otherwise, <c>false</c>.</returns>
	private bool IsBattleOver() {
		bool battleOver = false;
		if ( !_humanTeam.IsAlive() || !_aiTeam.IsAlive() ) {
			battleOver = true;
		}
		return battleOver;
	}

	/// <summary>
	/// Gets the next unit to move.
	/// </summary>
	/// <returns>The next unit to move.</returns>
	private Unit GetNextUnitToMove() {

		Unit unitToMove = null;
		while( unitToMove == null ) {
			// Increment all unit's action points
			foreach( Unit unit in _allUnits ) {
				unit.IncrementActionPoints();
			}

			// Sort based on action points
			SortUnitsByActionPoints();

			// Get the unit that meets the action point requirement
			unitToMove = GetUnitMeetingActionPointRequirement();
		}

		return unitToMove;
	}

	/// <summary>
	/// Gets a unit that has met the action point requirements
	/// In the case of tie(s), a random creature will be picked from the eligible cast
	/// </summary>
	/// <returns>The unit meeting action point requirement.</returns>
	private Unit GetUnitMeetingActionPointRequirement() {
		Unit currentUnit = null;
		int i, count;
		for ( i = 0, count = _allUnits.Count; i < count; i++ ) {
			
			// If we've previously picked a unit
			if ( currentUnit != null ) {
				// Checking for units with the same action points and same speed
				if ( currentUnit.ActionPoints == _allUnits[ i ].ActionPoints && 
				    currentUnit.GetStat( Stat.SPEED ) == _allUnits[ i ].GetStat( Stat.SPEED ) ) {
					continue;
				}
				// Otherwise there are no ties so we are done
				else {
					break;
				}
			}
			// Otherwise start the process
			else {
				currentUnit = _allUnits[ i ];
				// If the first unit doesnt exceed the threshold needed for action points, we're done
				if ( currentUnit.ActionPoints < Settings.actionPointsThreshold ) {
					currentUnit = null;
					break;
				}
			}
		}
		
		// There was a tie
		if ( i > 1 ) {
			int randomIndex = Random.Range( 0, i );
			currentUnit = _allUnits[ randomIndex ];
		}

		return currentUnit;
	}

	/// <summary>
	/// Sorts the units by their action points.
	/// </summary>
	private void SortUnitsByActionPoints() {
		// For a descending sort, we need to reverse the results, thus the * -1
		_allUnits.Sort( delegate( Unit a, Unit b ) {

			// We want to sort the units that are dead, last
			if ( a.CurrentHP <= 0 && b.CurrentHP <= 0 ) {
				return 0;
			} else if ( a.CurrentHP <= 0 ) {
				return 1;
			} else if ( b.CurrentHP <= 0 ) {
				return -1;
			} 

			int result = a.ActionPoints.CompareTo( b.ActionPoints ) * -1; 
			return result;
		});
	}
}

