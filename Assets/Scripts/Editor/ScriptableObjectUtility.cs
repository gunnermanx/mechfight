using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectUtility {

	[MenuItem("Assets/Create/CombatantData/MechPartData")]
	public static void CreateMechPartData() {
		CreateScriptableObject<MechPartData>();
	}

	[MenuItem("Assets/Create/CombatantData/PilotData")]
	public static void CreatePilotData() {
		CreateScriptableObject<PilotData>();
	}

	[MenuItem("Assets/Create/Skills/SkillData")]
	public static void CreateSkillData() {
		CreateScriptableObject<SkillData>();
	}
	[MenuItem("Assets/Create/Skills/SkillEffectData")]
	public static void CreateSkillEffectData() {
		CreateScriptableObject<SkillEffectData>();
	}

	[MenuItem("Assets/Create/Settings/BattleConductorSettings")]
	public static void CreateBattleConductorSettings() {
		CreateScriptableObject<BattleManagerSettings>();
	}


	private static void CreateScriptableObject<T> () where T: ScriptableObject {
		T sObject = ScriptableObject.CreateInstance<T>();
		
		string path = "Assets";
		foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}
			break;
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath( path + "/New " + typeof(T).ToString() + ".asset" );
		
		AssetDatabase.CreateAsset( sObject, assetPathAndName );
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = sObject;
	}
}

