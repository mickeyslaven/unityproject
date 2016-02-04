/* Written by Kaz Crowe */
/* UltimateStatusBarUpdater.cs ver 1.0.1 */
using UnityEngine;
using System.Collections;

public class UltimateStatusBarUpdater : MonoBehaviour
{
	UltimateStatusBarController[] allControllers;
	bool initialChange = false;

	void OnRectTransformDimensionsChange()
	{
		if( initialChange == false )
		{
			initialChange = true;
			return;
		}

		allControllers = FindObjectsOfType( typeof( UltimateStatusBarController ) ) as UltimateStatusBarController[];
		foreach( UltimateStatusBarController cont in allControllers )
			cont.UpdatePositioning();
	}
}