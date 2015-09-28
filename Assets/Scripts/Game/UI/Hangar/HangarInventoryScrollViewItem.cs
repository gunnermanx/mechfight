using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HangarInventoryScrollViewItem : MonoBehaviour {

	public Button Button;
	[SerializeField]
	protected Text NameLabel;
	[SerializeField]
	protected Text CountLabel;
	[SerializeField]
	protected Image IconImage;

	private MechPartData _mechPartData = null;
	private PilotData _pilotData = null;

	private Action<HangarInventoryScrollViewItem> _callback;

	public void Initialize( MechPartData data, int ownedCount, Action<HangarInventoryScrollViewItem> callback ) {
		_mechPartData = data;
		_callback = callback;

		NameLabel.text = data.name;
		CountLabel.text = "x" + ownedCount.ToString();
		IconImage.sprite = data.icon;
	}

	public void Initialize( PilotSerializedData data, Action<HangarInventoryScrollViewItem> callback ) {
		_pilotData = GameManager.Database.GetPilotData( data._pilotId );
		_callback = callback;

		NameLabel.text = data._pilotId;
		CountLabel.text = data._pilotLevel.ToString();
		IconImage.sprite = _pilotData.icon;
	}

	public MechPartData GetMechPartData() {
		return _mechPartData;
	}

	public PilotData GetPilotData() {
		return _pilotData;
	}

	private void ButtonPressed() {
		if ( _callback != null ) {
			_callback( this );
		}
	}

	private void Awake() {
		Button.onClick.AddListener( ButtonPressed );
	}
	
	private void OnDestroy() {
		Button.onClick.RemoveAllListeners();
	}
}

