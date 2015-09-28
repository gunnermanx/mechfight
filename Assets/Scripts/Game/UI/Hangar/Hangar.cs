using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hangar : MonoBehaviour {

	public HangarInventoryScrollView InventoryScrollView;
	public HangarUnitScrollView UnitScrollView;
	public HangarInspector Inspector;

	public Text PilotLabel;
	public Image PilotImage;

	public Text HeadLabel;
	public Image HeadImage;

	public Text CoreLabel;
	public Image CoreImage;

	public Text ArmsLabel;
	public Image ArmsImage;

	public Text LegsLabel;
	public Image LegsImage;

	public Text WeaponLabel;
	public Image WeaponImage;

	public Text GeneratorLabel;
	public Image GeneratorImage;

	private HangarUnitScrollViewItem _selectedHangarUnitScrollViewItem = null;

	public void Start() {
		//TODO:
		InventoryScrollView.Initialize( InventoryScrollViewItemChosen );
		UnitScrollView.Initialize( UnitScrollViewItemChosen );
		ShowPilotsInventory();
	}

	public void ShowHeadPartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._headMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.HEAD, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowCorePartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._coreMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.CORE, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowArmPartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._armsMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.ARMS, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowLegPartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._legsMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.LEGS, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowGeneratorPartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._generatorMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.GENERATOR, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowWeaponPartsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPartId = selectedUnitSerializedData._weaponMechPartId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayMechPartsOfType( MechPartData.PartType.WEAPON, currentPartId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}
	public void ShowPilotsInventory() {
		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();
		string currentPilotId = selectedUnitSerializedData._pilotId;

		HangarInventoryScrollViewItem item = InventoryScrollView.DisplayPilots( currentPilotId );
		if ( item != null ) {
			InventoryScrollViewItemChosen( item );
		}
	}

	public void Save() {
		// TODO we dont want to save that often, make a thing to set the blob dirty and save when we really need to
		GameManager.PersistenceManager.SavePlayerData();
	}

	public void Battle() {
		GameManager.Instance.LoadBattleScene();
	}

	public void InventoryScrollViewItemChosen( HangarInventoryScrollViewItem item ) {
		MechPartData mechPartData = item.GetMechPartData();
		PilotData pilotData = item.GetPilotData();

		UnitSerializedData selectedUnitSerializedData = _selectedHangarUnitScrollViewItem.GetUnitSerializedData();

		if ( pilotData != null ) {
			selectedUnitSerializedData._pilotId = pilotData.name;
			PilotLabel.text = pilotData.name;
			_selectedHangarUnitScrollViewItem.Refresh();
			Inspector.UpdateInspector( pilotData );
		}
		else if ( mechPartData != null ) {
			switch( mechPartData.partType ) {
			case MechPartData.PartType.HEAD:
				HeadLabel.text = mechPartData.name;
				selectedUnitSerializedData._headMechPartId = mechPartData.name;
				break;
			case MechPartData.PartType.ARMS:
				ArmsLabel.text = mechPartData.name;
				selectedUnitSerializedData._armsMechPartId = mechPartData.name;
				break;
			case MechPartData.PartType.LEGS:
				LegsLabel.text = mechPartData.name;
				selectedUnitSerializedData._legsMechPartId = mechPartData.name;
				break;
			case MechPartData.PartType.CORE:
				CoreLabel.text = mechPartData.name;
				selectedUnitSerializedData._coreMechPartId = mechPartData.name;
				break;
			case MechPartData.PartType.WEAPON:
				WeaponLabel.text = mechPartData.name;
				selectedUnitSerializedData._weaponMechPartId = mechPartData.name;
				break;
			case MechPartData.PartType.GENERATOR:
				GeneratorLabel.text = mechPartData.name;
				selectedUnitSerializedData._generatorMechPartId = mechPartData.name;
				break;
			}
			Inspector.UpdateInspector( mechPartData );
		}


	}
	
	public void UnitScrollViewItemChosen( HangarUnitScrollViewItem item ) {
		_selectedHangarUnitScrollViewItem = item;
		UnitSerializedData selectedUnitSerializedData = item.GetUnitSerializedData();

		PilotData pilotData = GameManager.Database.GetPilotData( selectedUnitSerializedData._pilotId );

		// TODO parts need unique names
		// TODO shouldnt be using icon

		// MechPart labels
		MechPartData partData;
		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._headMechPartId );
		HeadLabel.text = partData.name;
		//HeadImage.sprite = partData.icon;

		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._coreMechPartId );
		CoreLabel.text = partData.name;

		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._armsMechPartId );
		ArmsLabel.text = partData.name;

		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._legsMechPartId );
		LegsLabel.text = partData.name;

		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._weaponMechPartId );
		WeaponLabel.text = partData.name;

		partData = GameManager.Database.GetMechPartData( selectedUnitSerializedData._generatorMechPartId );
		GeneratorLabel.text = partData.name;

		// Pilot label
		PilotLabel.text = pilotData.name;

		ShowPilotsInventory();
	}
}

