using UnityEngine;
using System.Collections.Generic;

public class Database : MonoBehaviour {

#region Path Constants
	private const string SKILLDATA_PATH = "Data/ScriptableObjects/Pilots/";
	private const string SKILLS_FOLDER = "/Skills/";

	private const string PILOTDATA_PATH = "Data/ScriptableObjects/Pilots/";

	private const string MECHPARTDATA_PATH = "Data/ScriptableObjects/MechParts/";
#endregion

#region Database Dictionaries
	private Dictionary<string,SkillData> _skillDataSet = new Dictionary<string, SkillData>();
	private Dictionary<string,PilotData> _pilotDataSet = new Dictionary<string, PilotData>();
	private Dictionary<string,MechPartData> _mechPartDatabase = new Dictionary<string, MechPartData>();
#endregion
	
	public SkillData GetSkillData( string pilotId, string id ) {
		SkillData data = null;
		string path = null;
		if ( !_skillDataSet.TryGetValue( id, out data ) ) {
			path = SKILLDATA_PATH + pilotId + SKILLS_FOLDER + id;
			data = Resources.Load( path ) as SkillData;
		}
		
		Debug.Assert( data != null, "No SkillData found with id " + id + " at path " + path );
		
		return data;
	}

	public MechPartData GetMechPartData( string id ) {
		MechPartData data = null;
		string path = null;
		if ( !_mechPartDatabase.TryGetValue( id, out data ) ) {
			path = MECHPARTDATA_PATH + id;
			data = Resources.Load( MECHPARTDATA_PATH + id ) as MechPartData;
		}
		
		Debug.Assert( data != null, "No MechPartData found with id " + id + " at path " + path  );	
		return data;
	}

	public PilotData GetPilotData( string id ) {
		PilotData data = null;
		string path = null;
		if ( !_pilotDataSet.TryGetValue( id, out data ) ) {
			//TODO has science gone too far
			path = PILOTDATA_PATH + id + "/" + id;
			data = Resources.Load( path ) as PilotData;
		}
		
		Debug.Assert( data != null, "No PilotData found with id " + id + " at path " + path  );	
		return data;
	}
}

