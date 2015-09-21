using UnityEngine;
using System.Collections.Generic;

public class Pilot {

	private PilotSerializedData _serializedData = null;
	private PilotData _data = null;

	private List<SkillData> _assignedSkills = new List<SkillData>();

	public Pilot( PilotSerializedData serializedData ) {
		_serializedData = serializedData;
		_data = GameManager.Database.GetPilotData( _serializedData._pilotId );

		for ( int i = 0, count = _serializedData._pilotSkillIds.Count; i < count; i++ ) {
			string skillId = _serializedData._pilotSkillIds[i];
			SkillData skillData = GameManager.Database.GetSkillData( _data.name, skillId );
#if DEBUG
			Debug.Assert( skillData != null, "Skill with id " + skillId + " does not exist!" );
#endif
			_assignedSkills.Add( skillData );
		}
	}

	public List<SkillData> GetAssignedSkills() {
		return _assignedSkills;
	}

//	public List<SkillData> GetLearnedSkills() {
//	}

//	public List<SkillData> GetAllSkills() {
//	}
}

