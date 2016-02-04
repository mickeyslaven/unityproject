/* Written by Kaz Crowe */
/* UltimateButtonJAVAUpdater.js ver 1.0 */
#pragma strict
import UnityEngine.EventSystems;

private var allButtons : UltimateButtonJAVA[];
private var initialFrame : boolean = false;

public class UltimateButtonJAVAUpdater extends UIBehaviour
{
	function OnRectTransformDimensionsChange ()
	{
		if( initialFrame == false )
		{
			initialFrame = true;
			return;
		}
		
		allButtons = FindObjectsOfType( typeof( UltimateButtonJAVA ) ) as UltimateButtonJAVA[];
		for( var btn : UltimateButtonJAVA in allButtons )
			btn.UpdatePositioning();
	}
}