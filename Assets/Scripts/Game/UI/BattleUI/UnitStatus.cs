using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitStatus : MonoBehaviour {

	public Slider HPBar;
	public Text HPBarText;

	public GameObject NumberPopupPrefab;

	public void UpdateHPBar( int currentHP, int maxHP ) {
		HPBarText.text = currentHP + "/" + maxHP;
		HPBar.value = (float)currentHP / (float)maxHP;

		//TODO
		Vector3 cameraForward = Camera.main.transform.forward;
		Vector3 lookPosition = gameObject.transform.position + cameraForward.normalized;
		gameObject.transform.LookAt( lookPosition );
	}

	// TODO temp
	public void SetSelected( bool selected ) {
		Color color = selected ? Color.green : Color.white;
		HPBarText.color = color;
	}

	public void ShowDamagePopup( int damage ) {
		GameObject numberPopupGO = GameObject.Instantiate( NumberPopupPrefab ) as GameObject;
		numberPopupGO.GetComponent<TextPopup>().Show( damage.ToString(), Color.red, transform, Vector3.zero );
	}

	public void ShowHealingPopup( int healing ) {
		GameObject numberPopupGO = GameObject.Instantiate( NumberPopupPrefab ) as GameObject;
		numberPopupGO.GetComponent<TextPopup>().Show( healing.ToString(), Color.green, transform, Vector3.zero );
	}
}

