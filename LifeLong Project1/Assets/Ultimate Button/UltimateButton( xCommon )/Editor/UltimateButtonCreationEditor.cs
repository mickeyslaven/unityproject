/* Written by Kaz Crowe */
/* UltimateButtonCreationEditor.cs ver. 1.0.1 */
// 1.0.1 - 
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateButtonCreationEditor
{
	/* ---------< ULTIMATE BUTTON MENU >--------- */
	[ MenuItem( "GameObject/UI/Ultimate UI/Ultimate Button" ) ]
	private static void CreateUltimateButton ()
	{
		GameObject buttonPrefab = Resources.Load( "UltimateButton", typeof( GameObject ) ) as GameObject;
		
		if( buttonPrefab != null )
			CreateButton( buttonPrefab );
		else
			Debug.LogError( "Could not find 'UltimateButton.prefab' in any Resources folders." );
	}

	/* ---------< SIMPLE BUTTON MENU >--------- */
	[ MenuItem( "GameObject/UI/Ultimate UI/Simple Button" ) ]
	private static void CreateSimpleButton ()
	{
		GameObject buttonPrefab = Resources.Load( "SimpleButton", typeof( GameObject ) ) as GameObject;
		
		if( buttonPrefab != null )
			CreateButton( buttonPrefab );
		else
			Debug.LogError( "Could not find 'SimpleButton.prefab' in any Resources folders." );
	}
	
	private static void CreateButton ( Object buttonPrefab )
	{
		GameObject instBtn = ( GameObject )Object.Instantiate( buttonPrefab, Vector3.zero, Quaternion.identity );
		instBtn.name = buttonPrefab.name;
		Selection.activeGameObject = instBtn;
		CheckNeededObjects( instBtn );
	}
	
	private static void CheckNeededObjects ( GameObject button )
	{
		Canvas[] allCanvas = Object.FindObjectsOfType( typeof( Canvas ) ) as Canvas[];

		foreach( Canvas can in allCanvas )
		{
			if( can.renderMode == RenderMode.ScreenSpaceOverlay )
			{
				button.transform.SetParent( can.transform, false );
				CreateEventSystem( can.gameObject );
				return;
			}
		}
		CreateNewUI( button );
	}
	
	static public void CreateNewUI ( GameObject button )
	{
		GameObject root = new GameObject( "Canvas" );
		root.layer = LayerMask.NameToLayer( "UI" );
		Canvas canvas = root.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		root.AddComponent<CanvasScaler>();
		root.AddComponent<GraphicRaycaster>();
		Undo.RegisterCreatedObjectUndo( root, "Create " + root.name );
		button.transform.SetParent( root.transform, false );
		CreateEventSystem( root.gameObject );
	}
	
	private static void CreateEventSystem ( GameObject parent )
	{
		Object esys = Object.FindObjectOfType<EventSystem>();
		if( esys == null )
		{
			GameObject eventSystem = new GameObject( "EventSystem" );
			GameObjectUtility.SetParentAndAlign( eventSystem, parent );
			esys = eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
			eventSystem.AddComponent<TouchInputModule>();
			Undo.RegisterCreatedObjectUndo( eventSystem, "Create " + eventSystem.name );
		}
	}
}