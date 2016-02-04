/* Written by Kaz Crowe */
/* CreateStatusBarEditor.cs ver 1.0.1 */
// Removed unnecessary create functions.
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateStatusBarEditor
{
	/* ---------< ULTIMATE STATUS BAR >--------- */
	[MenuItem( "GameObject/UI/Ultimate UI/Ultimate Status Bar", true )]
	static bool CheckUltimateStatusBarUltimate ()
	{
		GameObject statusBarPrefab = Resources.Load( "UltimateStatusBar_Ultimate", typeof( GameObject ) ) as GameObject;
		if( statusBarPrefab != null )
			return true;
		return false;
	}
	
	[MenuItem( "GameObject/UI/Ultimate UI/Ultimate Status Bar" )]
	private static void CreateUltimateStatusBarUltimate ()
	{
		GameObject statusBarPrefab = Resources.Load( "UltimateStatusBar_Ultimate", typeof( GameObject ) ) as GameObject;
		if( statusBarPrefab != null )
			CreateStatusBar( statusBarPrefab );
		else
			Debug.LogError( "Could not find 'UltimateStatusBar_Ultimate.prefab' in any Resources folders." );
	}

	private static void CreateStatusBar ( Object statusBarPrefab )
	{
		// create our prefab in our scene
		GameObject instJoy = ( GameObject )Object.Instantiate( statusBarPrefab, Vector3.zero, Quaternion.identity );
		
		// Our instJoy.name currently has (Clone) at the end, so rename it to our original
		instJoy.name = statusBarPrefab.name;
		
		// Focus on the new GameObject
		Selection.activeGameObject = instJoy;
		
		// Check if we need anything else created( Canvas, EventSystem )
		CheckNeededObjects( instJoy );
	}

	private static void CheckNeededObjects ( GameObject statusBar )
	{
		Canvas[] allCanvas = Object.FindObjectsOfType( typeof( Canvas ) ) as Canvas[];

		foreach( Canvas can in allCanvas )
		{
			if( can.renderMode == RenderMode.ScreenSpaceOverlay )
			{
				statusBar.transform.SetParent( can.transform, false );
				return;
			}
		}
		CreateNewUI( statusBar );
	}
	
	static public void CreateNewUI ( GameObject statusBar )
	{
		// Root for the UI
		GameObject root = new GameObject( "Canvas" );
		root.layer = LayerMask.NameToLayer( "UI" );
		Canvas canvas = root.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		root.AddComponent<CanvasScaler>();
		root.AddComponent<GraphicRaycaster>();
		Undo.RegisterCreatedObjectUndo( root, "Create " + root.name );
		
		// Here we set the joystick to be a child of the canvas
		statusBar.transform.SetParent( root.transform, false );
	}
}