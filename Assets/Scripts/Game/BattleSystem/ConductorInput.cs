using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConductorInput {

	public List<Unit> TargetUnits {
		get; private set;
	}

	public SkillData TargetSkill {
		get; private set;
	}

	public Unit SourceUnit {
		get; private set;
	}

	public ConductorInput( Unit source, SkillData skill, List<Unit> targets ) {
		TargetUnits = targets;
		TargetSkill = skill;
		SourceUnit = source;
	}
}

