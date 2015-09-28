using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HangarUnitScrollViewItem : MonoBehaviour {
	[SerializeField]
	protected Button Button;
	[SerializeField]
	protected Text NameLabel;
	[SerializeField]
	protected Image IconImage;
	
	private UnitSerializedData _unitData = null;
	private Action<HangarUnitScrollViewItem> _callback = null;
	
	public void Initialize( UnitSerializedData data, Action<HangarUnitScrollViewItem> callback ) {
		_unitData = data;
		_callback = callback;

		PilotData pilotData = GameManager.Database.GetPilotData( _unitData._pilotId );
		NameLabel.text = data._unitName;
		IconImage.sprite = pilotData.icon;
	}

	public void Refresh() {
		PilotData pilotData = GameManager.Database.GetPilotData( _unitData._pilotId );
		NameLabel.text = _unitData._unitName;
		IconImage.sprite = pilotData.icon;
	}

	public UnitSerializedData GetUnitSerializedData() {
		return _unitData;
	}

	public void SetSelected( bool isSelected ) {
		ItemSelected();
	}
	
	private void ItemSelected() {
		if ( _callback != null ) {
			_callback( this );
		}
	}

	private void Awake() {
		Button.onClick.AddListener( ItemSelected );
	}
	
	private void OnDestroy() {
		Button.onClick.RemoveAllListeners();
	}
}

