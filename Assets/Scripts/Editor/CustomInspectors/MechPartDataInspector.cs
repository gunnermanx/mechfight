using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MechPartData))]
public class MechPartDataInspector : Editor {

	private MechPartData _mechPartData;

	private SerializedProperty _partTypeProperty;
	private SerializedProperty _defenseProperty;
	private SerializedProperty _attackProperty;
	private SerializedProperty _speedProperty;
	private SerializedProperty _lifeProperty;
	private SerializedProperty _powerMaxProperty;
	private SerializedProperty _powerChargeProperty;
	private SerializedProperty _powerInitialProperty;

	private MechPartData.PartType _lastPartTypeValue;

	void Awake() {
		_mechPartData=(MechPartData)target;

		_partTypeProperty = serializedObject.FindProperty( "partType" );
		_defenseProperty = serializedObject.FindProperty( "defense" );
		_attackProperty = serializedObject.FindProperty( "attack" );
		_speedProperty = serializedObject.FindProperty( "speed" );
		_lifeProperty = serializedObject.FindProperty( "hp" );
		_powerMaxProperty = serializedObject.FindProperty( "powerMax" );
		_powerChargeProperty = serializedObject.FindProperty( "powerCharge" );
		_powerInitialProperty = serializedObject.FindProperty( "powerInitial" );

		_lastPartTypeValue = ( MechPartData.PartType ) _partTypeProperty.enumValueIndex;
	}
	
	public override void OnInspectorGUI() {
		// Update
		serializedObject.Update();

		// The mech part type dropdown
		EditorGUILayout.BeginVertical( "Box" );
		{
			EditorGUILayout.PropertyField( _partTypeProperty );

			// clear all values if part type changed
			MechPartData.PartType newValue = ( MechPartData.PartType ) _partTypeProperty.enumValueIndex;
			if ( _lastPartTypeValue != newValue ) {
				_lastPartTypeValue = newValue;
				ClearAllProperties();
			}
		}
		EditorGUILayout.EndVertical();

		// Properties drawn depending on the type selected
		EditorGUILayout.BeginVertical( "Box" );
		{
			MechPartData.PartType enumValue = (MechPartData.PartType)_partTypeProperty.enumValueIndex;
			switch( enumValue ) {
			case MechPartData.PartType.HEAD:
				DrawHeadProperties();
				break;
			case MechPartData.PartType.ARMS:
				DrawArmsProperties();
				break;
			case MechPartData.PartType.LEGS:
				DrawLegsProperties();
				break;
			case MechPartData.PartType.CORE:
				DrawCoreProperties();
				break;
			case MechPartData.PartType.WEAPON:
				DrawWeaponProperties();
				break;
			case MechPartData.PartType.GENERATOR:
				DrawGeneratorProperties();
				break;
			}
		}
		EditorGUILayout.EndVertical();

		// Serialize changes
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawHeadProperties() {
		EditorGUILayout.PropertyField( _defenseProperty );
		EditorGUILayout.PropertyField( _attackProperty );
		EditorGUILayout.PropertyField( _speedProperty );
		EditorGUILayout.PropertyField( _lifeProperty );
	}

	private void DrawArmsProperties() {
		EditorGUILayout.PropertyField( _defenseProperty );
		EditorGUILayout.PropertyField( _lifeProperty );
	}

	private void DrawLegsProperties() {
		EditorGUILayout.PropertyField( _defenseProperty );
		EditorGUILayout.PropertyField( _speedProperty );
		EditorGUILayout.PropertyField( _lifeProperty );
	}

	private void DrawCoreProperties() {
		EditorGUILayout.PropertyField( _defenseProperty );
		EditorGUILayout.PropertyField( _lifeProperty );
	}

	private void DrawWeaponProperties() {
		EditorGUILayout.PropertyField( _defenseProperty );
		EditorGUILayout.PropertyField( _attackProperty );
	}

	private void DrawGeneratorProperties() {
		EditorGUILayout.PropertyField( _powerMaxProperty );
		EditorGUILayout.PropertyField( _powerChargeProperty );
		EditorGUILayout.PropertyField( _powerInitialProperty );
	}

	private void ClearAllProperties() {
		_defenseProperty.intValue = 0;
		_attackProperty.intValue = 0;
		_speedProperty.intValue = 0;
		_lifeProperty.intValue = 0;
		_powerMaxProperty.intValue = 0;
		_powerChargeProperty.intValue = 0;
		_powerInitialProperty.intValue = 0;
	}
}