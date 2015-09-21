using UnityEngine;
using System.Collections.Generic;

public enum Stat {
	ATTACK,
	DEFENSE,
	HP,
	SPEED,
	POWER_MAX,
	POWER_CHARGE
}

public class Unit : MonoBehaviour {

	public BattlefieldSlot Slot;

	public UnitStatus UnitStatus;

	private Pilot _pilot;
	private List<MechPartData> _mechParts = new List<MechPartData>();

	private UnitSerializedData _serializedData = null;

	private int _actionPoints = 0;

	private int _defense = 0;
	private int _attack = 0;
	private int _speed = 0;
	private int _maxHP = 0;
	private int _powerMax = 0;
	private int _powerCharge = 0;
	private int _powerInitial = 0;

	private int _currentHP = 0;
	private int _currentPower = 0;

	private List<SkillEffect> _appliedSkillEffects = new List<SkillEffect>();

	public void InitializeUnit( UnitSerializedData serializedData ) {
		_serializedData = serializedData;
		PilotSerializedData pilotSerializedData = GameManager.PlayerDataManager.GetPilotData( _serializedData._pilotId );
		_pilot = new Pilot( pilotSerializedData );

		_mechParts.Clear();
		for ( int i = 0, count = _serializedData._mechPartIds.Count; i < count; i++ ) {
			MechPartData part = GameManager.Database.GetMechPartData( _serializedData._mechPartIds[ i ] );
			_mechParts.Add( part );
		}      

		InitializeStats();

		UnitStatus.UpdateHPBar( _currentHP, _maxHP );
	}
	
	public void TargetWithSkill( Unit skillSource, int skillActualPower, SkillEffectData[] effects ) {
		for ( int i = 0, count = effects.Length; i < count; i++ ) {
			SkillEffect effect = new SkillEffect( skillSource, skillActualPower, effects[ i ] );
			
			bool refreshedEffect = false;
			for ( int j = 0, n = _appliedSkillEffects.Count; j < n; j++ ) {
				if ( _appliedSkillEffects[ j ].Equals( effect ) ) {
					_appliedSkillEffects[ j ].Refresh();
					refreshedEffect = true;
					break;
				}
			}
			
			if ( !refreshedEffect ) {
				if ( effect.Trigger == SkillEffectData.Trigger.IMMEDIATE ) {
					QueueSkillEffect( effect );
					ResolveSkillEffect( effect );
				} else if ( effect.Trigger == SkillEffectData.Trigger.QUEUED ) {
					QueueSkillEffect( effect );
				}
			}
		}
	}

	public void ResolveAppliedSkillEffects() {
		for ( int i = _appliedSkillEffects.Count-1; i >= 0; i-- ) {
			ResolveSkillEffect( _appliedSkillEffects[ i ] );
		}
	}

	public List<SkillData> GetAssignedSkills() {
		return _pilot.GetAssignedSkills();
	}

	public int GetStat( Stat stat ) {
		int statValue = -1;

		switch( stat ) {
		case Stat.ATTACK:
			int attackMultiplier = GetAttackMultiplier();
			statValue = _attack * attackMultiplier;
			break;
		case Stat.DEFENSE:
			int defenseMultiplier = GetDefenseMultiplier();
			statValue = _defense * defenseMultiplier;
			break;
		case Stat.HP:
			statValue = _maxHP;
			break;
		case Stat.SPEED:
			statValue = _speed;
			break;
		case Stat.POWER_CHARGE:
			statValue = _powerCharge;
			break;
		case Stat.POWER_MAX:
			statValue = _powerMax;
			break;
		}

		return statValue;
	}

	public int CurrentHP {
		get { return _currentHP; }
	}

	public int ActionPoints {
		get { return _actionPoints; }
	}

	public void IncrementActionPoints() {
		float speedToActionPoints = GameManager.BattleManager.Settings.speedToActionPointsConversion;
		_actionPoints += Mathf.FloorToInt( GetStat( Stat.SPEED ) * speedToActionPoints );
	}

	public void ResetActionPoints() {
		_actionPoints = 0;
	}

	public int CurrentPower {
		get { return _currentPower; }
	}

	public void ChargePower() {
		_currentPower = Mathf.Clamp( _currentPower + _powerCharge, 0, _powerMax );
	}

	public void DeductPower( int cost )  {
		_currentPower = Mathf.Clamp( _currentPower - cost, 0, _powerMax );
	}

	private void InitializeStats() {
		MechPartData part = null;
		for ( int i = 0, count = _mechParts.Count; i < count; i++ ) {
			part = _mechParts[ i ];
			_defense += part.defense;
			_attack += part.attack;
			_speed += part.speed;
			_maxHP += part.hp;
			_powerMax += part.powerMax;
			_powerCharge += part.powerCharge;
			_powerInitial += part.powerInitial;
		}

		_currentHP = _maxHP;
		_currentPower = _powerInitial;
	}

	private void QueueSkillEffect( SkillEffect effect ) {
		_appliedSkillEffects.Add( effect );
	}

	private void ResolveSkillEffect( SkillEffect effect ) {
		
		switch( effect.Type ) {
		case SkillEffectData.Type.CHARGE:
			ResolveChargeSkillEffect( effect );
			break;
		case SkillEffectData.Type.HEAL:
			ResolveHealSkillEffect( effect );
			break;
		case SkillEffectData.Type.DAMAGE:
			ResolveDamageSkillEffect( effect );
			break;
		case SkillEffectData.Type.BUFF_DEFENSE:
		case SkillEffectData.Type.BUFF_ATTACK:
		case SkillEffectData.Type.DEBUFF_DEFENSE:
		case SkillEffectData.Type.DEBUFF_ATTACK:
			// nothing additional needs to happen
			break;
		}

		effect.DecrementDuration();
		if ( effect.RemainingDuration <= 0 ) {
			_appliedSkillEffects.Remove( effect );
		}

		UnitStatus.UpdateHPBar( _currentHP, _maxHP );
	}

	private void ResolveChargeSkillEffect( SkillEffect effect ) {
		_currentPower = Mathf.Clamp( _currentPower + effect.SkillPower, 0, _powerMax );
	}

	private void ResolveDamageSkillEffect( SkillEffect effect ) {
		int defense = GetStat( Stat.DEFENSE );
		float DR = ((float)defense) / 100f * GameManager.BattleManager.Settings.DRPer100Defense;

		int damage = Mathf.FloorToInt( effect.SkillPower * ( 1f - DR ) );
		_currentHP = Mathf.Clamp( _currentHP - damage, 0, _currentHP );

		// Show some text for now
		UnitStatus.ShowDamagePopup( damage );
	}

	private void ResolveHealSkillEffect( SkillEffect effect ) {
		_currentHP = Mathf.Clamp( _currentHP + effect.SkillPower, _currentHP, _maxHP );

		// Show some text for now
		UnitStatus.ShowHealingPopup( effect.SkillPower );
	}

	private int GetAttackMultiplier() {
		int multiplier = 1;
		for ( int i = 0, count = _appliedSkillEffects.Count; i < count; i++ ) {
			if ( _appliedSkillEffects[ i ].Type == SkillEffectData.Type.BUFF_ATTACK ) {
				multiplier *= 2;
			} else if ( _appliedSkillEffects[ i ].Type == SkillEffectData.Type.DEBUFF_ATTACK ) {
				multiplier /= 2;
			}
		}
		return multiplier;
	}

	private int GetDefenseMultiplier() {
		int multiplier = 1;
		for ( int i = 0, count = _appliedSkillEffects.Count; i < count; i++ ) {
			if ( _appliedSkillEffects[ i ].Type == SkillEffectData.Type.BUFF_DEFENSE ) {
				multiplier *= 2;
			} else if ( _appliedSkillEffects[ i ].Type == SkillEffectData.Type.DEBUFF_DEFENSE ) {
				multiplier /= 2;
			}
		}
		return multiplier;
	}

	private List<SkillEffect> GetAppliedSkillEffectsOfType( SkillEffectData.Type type ) {
		List<SkillEffect> skills = new List<SkillEffect>();
		for ( int i = 0, count = _appliedSkillEffects.Count; i < count; i++ ) {
			if ( _appliedSkillEffects[ i ].Type == type ) {
				skills.Add( _appliedSkillEffects[ i ] );
			}
		}
		return skills;
	}
}

