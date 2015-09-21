using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBattleConductor : BaseBattleConductor {

	private Unit _source = null;

	// algorithms that determine move go in this class
	public AIBattleConductor( Team allyTeam, Team enemyTeam, GetConductorInputDelegate callback ) : base( allyTeam, enemyTeam, callback ) {
	}

	public override void GetConductorInput( Unit source ) {
		_source = source;

		// for now, its rngesus logic
		ConductorInput input = RandomStrategy();
		if ( _callback != null ) {
			_callback( input );
		}
	}

	private ConductorInput RandomStrategy() {
		List<SkillData> skills = _source.GetAssignedSkills();
		
		int randomIndex = Random.Range( 0, skills.Count );
		SkillData randomSkill = null; 
		while ( randomSkill == null ) {
			if ( skills[ randomIndex ].cost <= _source.CurrentPower ) {
				randomSkill = skills[ randomIndex ];
			} else {
				randomIndex = Random.Range( 0, skills.Count );
			}
		}

		List<Unit> targets = new List<Unit>();

		switch( randomSkill.target ) {
		case SkillData.Target.SELF:
			targets.Add( _source );
			break;
		case SkillData.Target.ALLY:
			targets.Add( _allyTeam.GetRandomUnit() );
			break;
		case SkillData.Target.ALL_ALLIES:
			IEnumerator<Unit> teamIterator = _allyTeam.Units;
			while( teamIterator.MoveNext() ) {
				targets.Add( teamIterator.Current );
			}
			break;
		case SkillData.Target.ENEMY:
			targets.Add( _enemyTeam.GetRandomUnit() );
			break;
		case SkillData.Target.ALL_ENEMIES:
			IEnumerator<Unit> enemyIterator = _enemyTeam.Units;
			while( enemyIterator.MoveNext() ) {
				targets.Add( enemyIterator.Current );
			}
			break;
		}

		return new ConductorInput( _source, randomSkill, targets );
	}
}

