using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class HangarInventoryScrollView : MonoBehaviour {

	public GameObject HangarScrollViewItemPrefab;
	public Transform ContentPanel;
	
	private PlayerSerializedData _playerData;

	public Dictionary<string,int> _mechPartCollection = null;
	public Dictionary<string, PilotSerializedData> _pilotCollection = null;
	public Action<HangarInventoryScrollViewItem> _itemClicked;

	public void Initialize( Action<HangarInventoryScrollViewItem> itemClicked ) {
		_mechPartCollection = GameManager.PlayerDataManager.GetMechPartCollection();
		_pilotCollection = GameManager.PlayerDataManager.GetPilotCollection();
		_itemClicked = itemClicked;
	}

	public HangarInventoryScrollViewItem DisplayMechPartsOfType( MechPartData.PartType type, string mechPartId ) {
		ResetList();
		return InitializeMechPartDataList( type, mechPartId );
	}

	public HangarInventoryScrollViewItem DisplayPilots( string pilotId ) {
		ResetList();
		return InitializePilotsList( pilotId );
	}

	private HangarInventoryScrollViewItem InitializeMechPartDataList( MechPartData.PartType type, string mechPartId ) {
		HangarInventoryScrollViewItem matchingItem = null;

		MechPartData mechPartData = null;
		foreach( KeyValuePair<string,int> kvp in _mechPartCollection ) {
			mechPartData = GameManager.Database.GetMechPartData( kvp.Key );

			if ( mechPartData.partType == type ) {	
				GameObject hangarButton = Instantiate( HangarScrollViewItemPrefab ) as GameObject;
				HangarInventoryScrollViewItem item = hangarButton.GetComponent<HangarInventoryScrollViewItem>();
				item.Initialize( mechPartData, kvp.Value, _itemClicked );
				item.transform.SetParent( ContentPanel );

				if ( matchingItem == null && kvp.Key == mechPartId ) {
					matchingItem = item;
				}
			}
		}

		return matchingItem;
	}

	private HangarInventoryScrollViewItem InitializePilotsList( string pilotId ) {
		HangarInventoryScrollViewItem matchingItem = null;

		foreach( KeyValuePair<string,PilotSerializedData> kvp in _pilotCollection ) {
			GameObject hangarButton = Instantiate( HangarScrollViewItemPrefab ) as GameObject;
			HangarInventoryScrollViewItem item = hangarButton.GetComponent<HangarInventoryScrollViewItem>();
			item.Initialize( kvp.Value, _itemClicked );
			item.transform.SetParent( ContentPanel );

			if ( matchingItem == null && kvp.Key == pilotId ) {
				matchingItem = item;
			}
		}

		return matchingItem;
	}

	private void ResetList() {
		for ( int i = 0, count = ContentPanel.childCount; i < count; i++ ) {
			GameObject.Destroy( ContentPanel.GetChild( i ).gameObject );
		}
	}
	                                     
}

