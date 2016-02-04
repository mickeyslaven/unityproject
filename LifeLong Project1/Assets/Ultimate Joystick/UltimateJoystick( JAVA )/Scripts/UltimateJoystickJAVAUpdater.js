/* Written by Kaz Crowe */
/* UltimateJoystickJAVAUpdater.js ver 1.0 */
#pragma strict
import UnityEngine.EventSystems;

public class UltimateJoystickJAVAUpdater extends UIBehaviour
{
	private var allJoysticks : UltimateJoystickJAVA[];
	private var initialFrame : boolean = false;

	function OnRectTransformDimensionsChange ()
	{
		if( initialFrame == false )
		{
			initialFrame = true;
			return;
		}

		allJoysticks = FindObjectsOfType( typeof( UltimateJoystickJAVA ) ) as UltimateJoystickJAVA[];
		for( var joy : UltimateJoystickJAVA in allJoysticks )
			joy.UpdatePositioning();
	}
}