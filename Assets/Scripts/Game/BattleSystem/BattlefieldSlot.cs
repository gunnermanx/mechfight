using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BattlefieldSlot : MonoBehaviour, IPointerClickHandler {

	public Unit attachedUnit;

	public delegate void BattlefieldSlotTappedDelegate( BattlefieldSlot slot );
	public BattlefieldSlotTappedDelegate BattlefieldSlotTapped;
 
	#region IPointerClickHandler implementation
	
	public void OnPointerClick( PointerEventData eventData ) {
		if ( BattlefieldSlotTapped != null ) {
			BattlefieldSlotTapped( this );
		}
	}
	
	#endregion

}

