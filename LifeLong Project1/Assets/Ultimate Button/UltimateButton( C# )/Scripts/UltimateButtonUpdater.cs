/* Written by Kaz Crowe */
/* UltimateButtonUpdater.cs ver 1.0 */
using UnityEngine.EventSystems;

public class UltimateButtonUpdater : UIBehaviour
{
	UltimateButton[] allButtons;
	bool initialFrame = false;

	protected override void OnRectTransformDimensionsChange ()
	{
		if( initialFrame == false )
		{
			initialFrame = true;
			return;
		}

		allButtons = FindObjectsOfType( typeof( UltimateButton ) ) as UltimateButton[];
		foreach( UltimateButton button in allButtons )
			button.UpdatePositioning();
	}
}