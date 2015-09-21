using UnityEngine;
using System.Collections.Generic;

public class Battlefield : MonoBehaviour {

	public List<BattlefieldSlot> _team1Slots;
	public List<BattlefieldSlot> _team2Slots;

	public delegate void UnitTappedDelegate( Unit unit );
	public UnitTappedDelegate UnitTapped;

	private List<Unit> _team1Units = new List<Unit>();
	public List<Unit> Team1Units {
		get { return _team1Units; }
	}

	private List<Unit> _team2Units = new List<Unit>();
	public List<Unit> Team2Units {
		get { return _team2Units; }
	}

	public void Initialize( List<UnitSerializedData> team1UnitsData, List<UnitSerializedData> team2UnitsData ) {

#if DEBUG
		Debug.Assert( team1UnitsData.Count <=_team1Slots.Count, "Num of team1 unit data exceeds num of battlefield slots, use another battlefield!" );
		Debug.Assert( team2UnitsData.Count <=_team2Slots.Count, "Num of team2 unit data exceeds num of battlefield slots, use another battlefield!" );
#endif
		for ( int i = 0, count = team1UnitsData.Count; i < count; i++ ){
			_team1Units.Add( _team1Slots[ i ].attachedUnit );
			_team1Units[ i ].InitializeUnit( team1UnitsData[ i ] );
			_team1Slots[ i ].BattlefieldSlotTapped += BattlefieldSlotTapped;

			CreateTempBox( _team1Slots[ i ].transform );
		}

		for ( int i = 0, count = team2UnitsData.Count; i < count; i++ ){
			_team2Units.Add( _team2Slots[ i ].attachedUnit );
			_team2Units[ i ].InitializeUnit( team2UnitsData[ i ] );
			_team2Slots[ i ].BattlefieldSlotTapped += BattlefieldSlotTapped;

			CreateTempBox( _team2Slots[ i ].transform );
		}
	}

	private void BattlefieldSlotTapped( BattlefieldSlot slot ) {
		if ( UnitTapped != null ) {
			UnitTapped( slot.attachedUnit );
		}
	}

	private void OnDestroy() {
		// 
	}

	private void CreateTempBox( Transform transform ) {
		GameObject cube  = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Transform cubeTransform = cube.transform;

		cubeTransform.parent = transform;
		cubeTransform.localScale = Vector3.one * 5f;
		cubeTransform.localPosition = Vector3.zero;
		cubeTransform.localRotation = Quaternion.identity;
	}
}

