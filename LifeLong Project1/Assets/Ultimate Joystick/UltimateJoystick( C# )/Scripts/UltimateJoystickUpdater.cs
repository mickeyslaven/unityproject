/* Written by Kaz Crowe */
/* UltimateJoystickUpdatePositioning.cs ver 1.0.1 */
using UnityEngine.EventSystems;

public class UltimateJoystickUpdater : UIBehaviour
{
	UltimateJoystick[] allJoysticks;
	bool initialFrame = false;

	protected override void OnRectTransformDimensionsChange ()
	{
		if( initialFrame == false )
		{
			initialFrame = true;
			return;
		}

		allJoysticks = FindObjectsOfType( typeof( UltimateJoystick ) ) as UltimateJoystick[];
		foreach( UltimateJoystick joy in allJoysticks )
			joy.UpdatePositioning();
	}
}