/* Written by Kaz Crowe */
/* UltimateStatusBarUpdaterJAVA.js ver 1.0 */
#pragma strict

private var allControllers : UltimateStatusBarControllerJAVA[];
private var initialChange : boolean = false;

function OnRectTransformDimensionsChange()
{
	if( initialChange == false )
	{
		initialChange = true;
		return;
	}

	allControllers = FindObjectsOfType( UltimateStatusBarControllerJAVA ) as UltimateStatusBarControllerJAVA[];
	for( var cont : UltimateStatusBarControllerJAVA in allControllers )
		cont.UpdatePositioning();
}