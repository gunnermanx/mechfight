using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputBattleConductor : BaseBattleConductor {

	private BattleUI _battleUI = null;

	private Unit _source = null;

	public InputBattleConductor( BattleUI battleUI, Team allyTeam, Team enemyTeam, GetConductorInputDelegate callback ) : base( allyTeam, enemyTeam, callback ) {
		_battleUI = battleUI;
		_battleUI.UnitTappedCallback += SkillUsed;
	}

	public override void GetConductorInput( Unit source ) {
		_source = source;

		// Populate BattleUI with skills
		_battleUI.InitializeForUnit( source );
	}

	private void SkillUsed( Unit target, SkillData data ) {
		bool isValid = ValidateTarget( target, data );
		if( isValid ) {
			Debug.Log( "Skill used! " + data.name + " on " + data.target.ToString() );

			// The target is valid, but we need to account for skills that target all allies/enemies
			List<Unit> targets = new List<Unit>();
			if ( data.target == SkillData.Target.ALL_ALLIES ) {
				IEnumerator<Unit> teamIterator = _allyTeam.Units;
				while( teamIterator.MoveNext() ) {
					targets.Add( teamIterator.Current );
				}
			} else if ( data.target == SkillData.Target.ALL_ENEMIES ) {
				IEnumerator<Unit> enemyIterator = _enemyTeam.Units;
				while( enemyIterator.MoveNext() ) {
					targets.Add( enemyIterator.Current );
				}
			} else {
				targets.Add( target );
			}

			ConductorInput input = new ConductorInput( _source, data, targets );
			if ( _callback != null ) {
				_callback( input );
			}

			_battleUI.PickSkillFinish();
		}
	}

	private bool ValidateTarget( Unit target, SkillData data ) {	
		bool isValid = false;
		
		switch( data.target ) {
		case SkillData.Target.SELF:
			isValid = _source == target;
			break;
		case SkillData.Target.ALLY:
		case SkillData.Target.ALL_ALLIES:
			isValid = _allyTeam.IsUnitInTeam( target );
			break;
		case SkillData.Target.ENEMY:
		case SkillData.Target.ALL_ENEMIES:
			isValid = _enemyTeam.IsUnitInTeam( target );
			break;
		}
		
		return isValid;
	}

}

