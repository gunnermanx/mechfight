using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextPopup : MonoBehaviour {

	public Text Text;
	public float Duration;

	public void Show( string text, Color textColor, Transform parent, Vector3 offset ) {
		Text.text = text;
		Text.color = textColor;
		transform.SetParent( parent, false );
		transform.localPosition = offset;

		GameObject.Destroy( gameObject, Duration );
	}
}

