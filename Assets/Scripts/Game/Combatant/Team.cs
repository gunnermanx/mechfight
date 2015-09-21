using UnityEngine;
using System.Collections.Generic;

public class Team {

	private List<Unit> _units;
	public IEnumerator<Unit> Units {
		get { return _units.GetEnumerator(); }
	}

	public Team( List<Unit> units ) {
		_units = units;
	}

	public bool IsUnitInTeam( Unit unit ) {
		return _units.Contains( unit );
	}
	
	public bool IsAlive() {
		bool isAlive = false;
		for ( int i = 0, count = _units.Count; i < count; i++ ) {
			if ( _units[ i ].CurrentHP > 0 ) {
				isAlive = true;
				break;
			}
		}
		return isAlive;
	}

	public Unit GetRandomUnit() {
		int randomIndex = Random.Range( 0, _units.Count );
		return _units[ randomIndex ];
	}
}

