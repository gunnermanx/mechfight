using System;
using UnityEngine;

public delegate void GetConductorInputDelegate( ConductorInput input );

public class BaseBattleConductor {

	protected Team _allyTeam = null;
	protected Team _enemyTeam = null;
	protected GetConductorInputDelegate _callback = null;

	public BaseBattleConductor( Team allyTeam, Team enemyTeam, GetConductorInputDelegate callback ) {
		_allyTeam = allyTeam;
		_enemyTeam = enemyTeam;
		_callback = callback;
	}

	public virtual void GetConductorInput( Unit source ) {
	}
}

