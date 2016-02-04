/* Written by Kaz Crowe */
/* UltimateStatusBarControllerEditor.cs ver 1.0.1a */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;

[ CanEditMultipleObjects ]
[ CustomEditor( typeof( UltimateStatusBarController ) ) ]
public class UltimateStatusBarControllerEditor : Editor
{
	Dictionary<int, UltimateStatusBar> myStatusBars = new Dictionary<int, UltimateStatusBar>();
	string statusBarName = "Enter Name";

	/* ASSIGNED COMPONENTS */
	SerializedProperty statusBarText;
	SerializedProperty statusBarIcon;

	/* SCALE AND POSITIONING */
	SerializedProperty usePositioning;
	SerializedProperty scalingAxis;
	SerializedProperty statusBarSize;
	SerializedProperty spacingX;
	SerializedProperty spacingY;
	SerializedProperty preserveAspect;
	SerializedProperty targetImage;
	SerializedProperty xRatio;
	SerializedProperty yRatio;

	/* TIMEOUT OPTIONS */
	SerializedProperty enableIdleTimeout;
	SerializedProperty idleTimeout;
	SerializedProperty statusBarEnabled;
	SerializedProperty enabledSpeed;
	SerializedProperty statusBarDisabled;
	SerializedProperty disabledSpeed;

	SerializedProperty defaultState;

	/* Animated Values */
	AnimBool AssignedVariables;
	AnimBool SizeAndPlacement;
	AnimBool StyleAndOptions;
	AnimBool UltimateStatusBars;
	AnimBool customAspectRatio;


	void OnEnable ()
	{
		/* --------------------- > ASSIGNED COMPONENTS < --------------------- */
		statusBarText = serializedObject.FindProperty( "statusBarText" );
		statusBarIcon = serializedObject.FindProperty( "statusBarIcon" );
		/* ------------------- > END ASSIGNED COMPONENTS < ------------------- */

		/* -------------------- > SCALE AND POSITIONING < -------------------- */
		usePositioning = serializedObject.FindProperty( "usePositioning" );
		scalingAxis = serializedObject.FindProperty( "scalingAxis" );
		statusBarSize = serializedObject.FindProperty( "statusBarSize" );
		spacingX = serializedObject.FindProperty( "spacingX" );
		spacingY = serializedObject.FindProperty( "spacingY" );
		preserveAspect = serializedObject.FindProperty( "preserveAspect" );
		targetImage = serializedObject.FindProperty( "targetImage" );
		xRatio = serializedObject.FindProperty( "xRatio" );
		yRatio = serializedObject.FindProperty( "yRatio" );
		/* ------------------ > END SCALE AND POSITIONING < ------------------ */

		/* ----------------------- > TIMEOUT OPTIONS < ----------------------- */
		enableIdleTimeout = serializedObject.FindProperty( "enableIdleTimeout" );
		idleTimeout = serializedObject.FindProperty( "idleTimeout" );
		statusBarEnabled = serializedObject.FindProperty( "statusBarEnabled" );
		enabledSpeed = serializedObject.FindProperty( "enabledSpeed" );
		statusBarDisabled = serializedObject.FindProperty( "statusBarDisabled" );
		disabledSpeed = serializedObject.FindProperty( "disabledSpeed" );
		/* --------------------- > END TIMEOUT OPTIONS < --------------------- */

		defaultState = serializedObject.FindProperty( "defaultState" );

		UltimateStatusBarController cont = ( UltimateStatusBarController )target;
		if( cont.statusBarText != null && cont.statusBarText.text != "New Text" && cont.statusBarText.text != string.Empty )
			statusBarName = cont.statusBarText.text;

		StoreChildBars();

		AssignedVariables = new AnimBool( EditorPrefs.GetBool( "UUI_Variables" ) );
		SizeAndPlacement = new AnimBool( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) );
		StyleAndOptions = new AnimBool( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) );
		UltimateStatusBars = new AnimBool( EditorPrefs.GetBool( "UUI_ExtraOption_01" ) );

		customAspectRatio = new AnimBool( cont.preserveAspect == true ? false : true );

		// Find parent canvas and add update script
		GameObject myParent = GetParentCanvas();
		if( myParent != null && !myParent.GetComponent<UltimateStatusBarUpdater>() )
			myParent.AddComponent( typeof( UltimateStatusBarUpdater ) );
	}

	GameObject GetParentCanvas ()
	{
		// Store the current parent.
		Transform parent = Selection.activeGameObject.transform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{ 
			// If there is a CanvasScaler component, return the component.
			if( parent.transform.GetComponent<CanvasScaler>() )
				return parent.transform.gameObject;
			
			// Else, shift to the next parent.
			parent = parent.transform.parent;
		}
		return null;
	}

	void StoreChildBars ()
	{
		//Reset the dictionary and restore all the new status bars.
		myStatusBars = new Dictionary<int, UltimateStatusBar>();

		UltimateStatusBar[] myBars = Selection.activeGameObject.GetComponentsInChildren<UltimateStatusBar>();
		int current = 0;
		foreach( UltimateStatusBar status in myBars )
		{
			myStatusBars.Add( current, status );
			current++;
		}
	}
	
	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateStatusBarControllerJAVA.cs script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		UltimateStatusBarController usbController = ( UltimateStatusBarController )target;

		EditorGUILayout.Space();

		/* ---------------------------------------- > ASSIGNED COMPONENTS < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Assigned Variables", EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( "UUI_Variables" ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			EditorPrefs.SetBool( "UUI_Variables", EditorPrefs.GetBool( "UUI_Variables" ) == true ? false : true );
			AssignedVariables.target = EditorPrefs.GetBool( "UUI_Variables" );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if( EditorGUILayout.BeginFadeGroup( AssignedVariables.faded ) )
		{
			EditorGUILayout.Space();

			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( statusBarText, new GUIContent( "Status Bar Text", "The main text of the status bar. Commonly showing the name or function." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( usbController.statusBarText == null )
				EditorGUILayout.HelpBox( "Status Bar Name variable needs to be assigned in order for the UpdateStatusBarName() function to work.", MessageType.Warning );
			else
			{
				EditorGUI.BeginChangeCheck();
				statusBarName = EditorGUILayout.TextField( "Status Bar Name", statusBarName );
				if( EditorGUI.EndChangeCheck() )
				{
					usbController.statusBarText.enabled = false;
					usbController.UpdateStatusBarName( statusBarName );
					usbController.statusBarText.enabled = true;
				}
			}

			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( statusBarIcon, new GUIContent( "Status Bar Icon", "The icon of the status bar." ) );
			if( usbController.statusBarIcon == null )
				EditorGUILayout.HelpBox( "Status Bar Icon variable needs to be assigned in order for the UpdateStatusBarIcon() function to work.", MessageType.Warning );

			EditorGUI.indentLevel = 0;
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndFadeGroup();
		/* -------------------------------------- > END ASSIGNED COMPONENTS < -------------------------------------- */
		
		EditorGUILayout.Space();

		/* ---------------------------------------- > SIZE AND PLACEMENT < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Size and Placement", EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			EditorPrefs.SetBool( "UUI_SizeAndPlacement", EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) == true ? false : true );
			SizeAndPlacement.target = EditorPrefs.GetBool( "UUI_SizeAndPlacement" );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if( EditorGUILayout.BeginFadeGroup( SizeAndPlacement.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( usePositioning, new GUIContent( "Use Positioning", "Should the positioning of the Ultimate Status Bar be enabled or disabled?" ) );
			EditorGUI.BeginDisabledGroup( usbController.usePositioning == UltimateStatusBarController.SwitchState.Disabled );
			EditorGUILayout.PropertyField( scalingAxis, new GUIContent( "Scaling Axis", "Should the Ultimate Status Bar be sized according to screen Height or Width?" ) );
			EditorGUILayout.Slider( statusBarSize, 0.0f, 10.0f, new GUIContent( "Status Bar Size", "Determines the overall size of the status bar." ) );
			EditorGUILayout.PropertyField( preserveAspect, new GUIContent( "Preserve Aspect", "Should the Ultimate Status Bar preserve the aspect ratio of the targeted image?" ) );
			EditorGUI.BeginDisabledGroup( usbController.preserveAspect == false );
			EditorGUI.indentLevel = 2;
			EditorGUILayout.PropertyField( targetImage, new GUIContent( "Target Image", "The targeted image to preserve the aspect ratio of." ) );
			if( usbController.preserveAspect == true && usbController.targetImage == null )
				EditorGUILayout.HelpBox( "Target Image needs to be assigned for the Preserve Aspect option to work.", MessageType.Error );
			EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel = 1;
			if( EditorGUILayout.BeginFadeGroup( customAspectRatio.faded ) )
			{
				EditorGUI.indentLevel = 2;
				EditorGUILayout.Slider( xRatio, 0.0f, 1.0f, new GUIContent( "X Ratio", "The desired width of the image." ) );
				EditorGUILayout.Slider( yRatio, 0.0f, 1.0f, new GUIContent( "Y Ratio", "The desired height of the image." ) );
				EditorGUI.indentLevel = 1;
			}
			if( SizeAndPlacement.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				customAspectRatio.target = usbController.preserveAspect == true ? false : true;
			}

			EditorGUILayout.Space();

			EditorGUILayout.BeginVertical( "Box" );
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( new GUIContent( "Status Bar Position", "Customize the position of the status bar." ), EditorStyles.boldLabel );
			GUILayout.EndHorizontal();
			EditorGUI.indentLevel = 1;
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.Slider( spacingX, 0.0f, 100.0f, new GUIContent( "X Position", "The horizontal position of the image." ) );
			EditorGUILayout.Slider( spacingY, 0.0f, 100.0f, new GUIContent( "Y Position", "The vertical position of the image." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel = 0;

			EditorGUI.EndDisabledGroup();
		}
		EditorGUILayout.EndFadeGroup();
		/* -------------------------------------- > END SIZE AND PLACEMENT < -------------------------------------- */

		EditorGUILayout.Space();

		/* ----------------------------------------- > STYLE AND OPTIONS < ----------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Style and Options", EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )//
		{
			EditorPrefs.SetBool( "UUI_StyleAndOptions", EditorPrefs.GetBool( "UUI_StyleAndOptions" ) == true ? false : true );
			StyleAndOptions.target = EditorPrefs.GetBool( "UUI_StyleAndOptions" );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if( EditorGUILayout.BeginFadeGroup( StyleAndOptions.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( defaultState, new GUIContent( "Default State", "Determines if the Ultimate Status Bar Controller should be enabled or disabled from start." ) );
			EditorGUILayout.PropertyField( enableIdleTimeout, new GUIContent( "Enable Timeout", "Should the Ultimate Status Bar be disabled visually after being idle for a designated time?" ) );
			EditorGUI.indentLevel = 2;
			EditorGUI.BeginDisabledGroup( usbController.enableIdleTimeout == false );
			EditorGUILayout.PropertyField( idleTimeout, new GUIContent( "Idle Seconds", "Time in seconds after being idle for the status bar to be disabled." ) );
			EditorGUILayout.PropertyField( statusBarEnabled, new GUIContent( "Enabled", "How should the Ultimate Status Bar become enabled visually?" ) );
			if( usbController.statusBarEnabled == UltimateStatusBarController.TimeoutOption.Fade )
			{
				EditorGUI.indentLevel = 3;
				EditorGUILayout.Slider( enabledSpeed, 0.0f, 2.0f, new GUIContent( "Speed", "The speed at which to fade in." ) );
				EditorGUI.indentLevel = 2;
			}
			EditorGUILayout.PropertyField( statusBarDisabled, new GUIContent( "Disabled", "How should the Ultimate Status Bar become disabled visually?" ) );
			if( usbController.statusBarDisabled == UltimateStatusBarController.TimeoutOption.Fade )
			{
				EditorGUI.indentLevel = 3;
				EditorGUILayout.Slider( disabledSpeed, 0.0f, 2.0f, new GUIContent( "Speed", "The speed at which to fade out." ) );
				EditorGUI.indentLevel = 2;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel = 0;
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndFadeGroup();
		/* --------------------------------------- > END STYLE AND OPTIONS < --------------------------------------- */
		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Ultimate Status Bars", EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( "UUI_ExtraOption_01" ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			EditorPrefs.SetBool( "UUI_ExtraOption_01", EditorPrefs.GetBool( "UUI_ExtraOption_01" ) == true ? false : true );
			UltimateStatusBars.target = EditorPrefs.GetBool( "UUI_ExtraOption_01" );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if( EditorGUILayout.BeginFadeGroup( UltimateStatusBars.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.indentLevel = 1;

			if( myStatusBars.Count == 0 )
				EditorGUILayout.HelpBox( "There are no Ultimate Status Bar scripts attached to any children of this object. " +
					"Please be sure there is at least one Ultimate Status Bar before attempting to make changes in this section.", MessageType.Warning );
			else
			{
				for( int i = 0; i < myStatusBars.Count; i++ )
				{
					EditorGUILayout.LabelField( "Name: " + myStatusBars[ i ].gameObject.name, EditorStyles.boldLabel );
					EditorGUI.indentLevel = 2;
					myStatusBars[ i ].statusBarName = EditorGUILayout.TextField( new GUIContent( "Status Name:", "The name to be used for reference from scripts." ), myStatusBars[ i ].statusBarName );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.Space();
				}
				bool hasDuplicates = false;
				EditorGUI.indentLevel = 0;
				UltimateStatusBar[] myBars = Selection.activeGameObject.GetComponentsInChildren<UltimateStatusBar>();
				foreach( UltimateStatusBar status in myBars )
				{
					foreach( UltimateStatusBar status2 in myBars )
					{
						if( status != status2 && status.statusBarName == status2.statusBarName )
							hasDuplicates = true;
					}
				}
				if( hasDuplicates == true )
					EditorGUILayout.HelpBox( "Some statusBarName references are the same. Please be sure to make every Ultimate Status Bar name unique.", MessageType.Error );
			}
		}
		EditorGUILayout.EndFadeGroup();

		EditorGUILayout.Space();

		base.Repaint();
	}
}