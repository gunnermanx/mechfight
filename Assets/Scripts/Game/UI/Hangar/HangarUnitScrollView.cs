using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HangarUnitScrollView : MonoBehaviour {

	public GameObject HangarUnitScrollViewItemPrefab;
	public Transform ContentPanel;

	private PlayerSerializedData _playerData;
	private List<UnitSerializedData> _units;
	
	public void Initialize( Action<HangarUnitScrollViewItem> itemClicked ) {
		_units = GameManager.PlayerDataManager.GetUnitsData();

		UnitSerializedData unitData = null;
		for ( int i = 0, count = _units.Count; i < count; i++ ) {
			unitData = _units[ i ];

			GameObject hangarButton = Instantiate( HangarUnitScrollViewItemPrefab ) as GameObject;
			HangarUnitScrollViewItem item = hangarButton.GetComponent<HangarUnitScrollViewItem>();
			item.Initialize( unitData, itemClicked );
			item.transform.SetParent( ContentPanel );
		}

		Transform firstItem = ContentPanel.GetChild( 0 );
		firstItem.GetComponent<HangarUnitScrollViewItem>().SetSelected( true );

	}

}

