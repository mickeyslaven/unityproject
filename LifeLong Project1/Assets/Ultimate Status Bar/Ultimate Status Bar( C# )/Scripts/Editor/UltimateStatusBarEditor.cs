/* Written by Kaz Crowe */
/* UltimateStatusBarEditor.cs ver 1.0.2a */
using UnityEngine;
using UnityEditor;

[ CanEditMultipleObjects ]
[ CustomEditor( typeof( UltimateStatusBar ) ) ]
public class UltimateStatusBarEditor : Editor
{
	/* Variables */
	float testValue = 1.0f;
	
	SerializedProperty statusBar;
	SerializedProperty statusBarColor;
	
	SerializedProperty showText;
	SerializedProperty statusBarText;
	SerializedProperty statusBarTextColor;
	SerializedProperty usePercentage;
	SerializedProperty additionalText;

	SerializedProperty alternateState;
	SerializedProperty stateTrigger;
	SerializedProperty triggerValue;
	SerializedProperty flashing;
	SerializedProperty flashingSpeed;
	SerializedProperty alternateStateColorMid;
	SerializedProperty alternateStateColor;

	SerializedProperty statusBarName;
	

	void OnEnable ()
	{
		UltimateStatusBar status = ( UltimateStatusBar )target;
		if( status.statusBar != null )
			testValue = status.statusBar.fillAmount;

		/* --------------------- > STATUS BAR SECTION < --------------------- */
		statusBar = serializedObject.FindProperty( "statusBar" );
		statusBarColor = serializedObject.FindProperty( "statusBarColor" );
		/* ------------------- > END STATUS BAR SECTION < ------------------- */

		/* ------------------------ > TEXT SECTION < ------------------------ */
		showText = serializedObject.FindProperty( "showText" );
		statusBarText = serializedObject.FindProperty( "statusBarText" );
		statusBarTextColor = serializedObject.FindProperty( "statusBarTextColor" );
		usePercentage = serializedObject.FindProperty( "usePercentage" );
		additionalText = serializedObject.FindProperty( "additionalText" );
		/* ---------------------- > END TEXT SECTION < ---------------------- */

		/* ---------------------- > ALTERNATE STATE < ----------------------- */
		alternateState = serializedObject.FindProperty( "alternateState" );
		stateTrigger = serializedObject.FindProperty( "stateTrigger" );
		triggerValue = serializedObject.FindProperty( "triggerValue" );
		flashing = serializedObject.FindProperty( "flashing" );
		flashingSpeed = serializedObject.FindProperty( "flashingSpeed" );
		alternateStateColor = serializedObject.FindProperty( "alternateStateColor" );
		alternateStateColorMid = serializedObject.FindProperty( "alternateStateColorMid" );
		/* -------------------- > END ALTERNATE STATE < --------------------- */

		statusBarName = serializedObject.FindProperty( "statusBarName" );
	}
	
	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateStatusBar.cs script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		UltimateStatusBar usbLogic = ( UltimateStatusBar )target;
		
		EditorGUILayout.Space();

		/* -------------------------------------- > REFERENCE < -------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		EditorGUILayout.LabelField( "Script Reference", EditorStyles.boldLabel );
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		EditorGUI.indentLevel = 1;
		
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( statusBarName, new GUIContent( "Status Bar Name", "The name to be used for reference from scripts." ) );
		if( EditorGUI.EndChangeCheck() )
			serializedObject.ApplyModifiedProperties();

		if( usbLogic.statusBarName == string.Empty )
			EditorGUILayout.HelpBox( "Please make sure to assign a name so that this status bar can be referenced from your scripts.", MessageType.Warning );
		else
		{
			EditorGUILayout.BeginVertical( "Box" );
			EditorGUILayout.Space();
			EditorGUILayout.TextArea( "UltimateStatusBar.UpdateStatus( \"" + usbLogic.statusBarName + "\", currentValue, maxValue );", EditorStyles.wordWrappedLabel );
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}
		EditorGUI.indentLevel = 0;
		/* ------------------------------------ > END REFERENCE < ------------------------------------ */
		
		EditorGUILayout.Space();

		/* ------------------------------------ > CUSTOMIZATION < ------------------------------------ */
		EditorGUILayout.BeginVertical( "Toolbar" );
		EditorGUILayout.LabelField( "Customization", EditorStyles.boldLabel );
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		EditorGUI.indentLevel = 1;

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( statusBar, new GUIContent( "Status Bar", "The targeted image to display the status." ) );
		EditorGUI.BeginDisabledGroup( usbLogic.statusBar == null );
		EditorGUILayout.PropertyField( statusBarColor, new GUIContent( "Status Bar Color", "The default color of the status bar." ) );
		EditorGUI.EndDisabledGroup();
		if( usbLogic.statusBar == null )
			EditorGUILayout.HelpBox( "Status Bar variable needs to be assigned.", MessageType.Error );
		else if( usbLogic.statusBar.type != UnityEngine.UI.Image.Type.Filled )
		{
			EditorGUILayout.HelpBox( "Status Bar image needs to be adjusted to type: Filled.", MessageType.Warning );
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Adjust Image" ) )
			{
				usbLogic.statusBar.type = UnityEngine.UI.Image.Type.Filled;
				usbLogic.statusBar.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
			}

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUI.indentLevel = 0;
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			usbLogic.UpdateStatusBarColor( usbLogic.statusBarColor );
		}
		/* ------------------- > END STATUS BAR SECTION < ------------------- */

		EditorGUILayout.Space();

		/* ------------------------ > TEXT SECTION < ------------------------ */
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( showText, new GUIContent( "Show Text", "Should the text of the status be shown?" ) );
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			if( usbLogic.showText == false && usbLogic.statusBarText != null )
				usbLogic.statusBarText.gameObject.SetActive( false );
			else if( usbLogic.showText == true && usbLogic.statusBarText != null )
				usbLogic.statusBarText.gameObject.SetActive( true );
		}

		EditorGUI.BeginDisabledGroup( usbLogic.showText == false );
		EditorGUI.indentLevel = 1;

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( statusBarText, new GUIContent( "Status Bar Text", "The text component used to display the values of the status." ) );
		EditorGUI.indentLevel = 2;
		EditorGUILayout.PropertyField( statusBarTextColor, new GUIContent( "Text Color", "The color of the text component." ) );
		EditorGUI.indentLevel = 1;
		if( usbLogic.statusBarTextColor != usbLogic.statusBarText.color )
		{
			EditorGUILayout.HelpBox( "Please assign the color of the text component using the color property above.", MessageType.Warning );
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if( GUILayout.Button( new GUIContent( "Update Color", "Updates the Text Color property to match the color of the text component." ) ) )
					usbLogic.statusBarTextColor = usbLogic.statusBarText.color;
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();
		}
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			usbLogic.UpdateStatusBarTextColor( usbLogic.statusBarTextColor );
		}

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( usePercentage, new GUIContent( "Use Percentage", "Should the status bar values be displayed as a percentage?" ) );
		EditorGUILayout.PropertyField( additionalText, new GUIContent( "Additional Text", "Determines what additional text is displayed before the values." ) );
		EditorGUI.indentLevel = 0;
		EditorGUI.EndDisabledGroup();
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			usbLogic.UpdateStatusBar( testValue, 1.0f );
		}
		/* ---------------------- > END TEXT SECTION < ---------------------- */

		EditorGUILayout.Space();

		/* ---------------------- > ALTERNATE STATES < ---------------------- */
		EditorGUI.BeginChangeCheck();
		{
			EditorGUILayout.PropertyField( alternateState, new GUIContent( "Alternate State", "Does this status require having an alternate state to display?" ) );
			EditorGUI.BeginDisabledGroup( usbLogic.alternateState == false );
			EditorGUI.indentLevel = 1;
			EditorGUILayout.PropertyField( stateTrigger, new GUIContent( "Trigger", "How should the alternate state trigger?" ) );
		}
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			if( usbLogic.alternateState == false )
				usbLogic.UpdateStatusBarColor( usbLogic.statusBarColor );
			else
				usbLogic.UpdateStatusBar( testValue, 1.0f );
		}

		EditorGUI.BeginChangeCheck();
		{
			if( usbLogic.stateTrigger != UltimateStatusBar.StateTrigger.ColorBlended )
				EditorGUILayout.PropertyField( flashing );
			if( usbLogic.flashing == true )
			{
				EditorGUI.indentLevel = 2;
				EditorGUILayout.Slider( flashingSpeed, 0.0f, 10.0f, new GUIContent( "Flashing Speed", "" ) );
				EditorGUI.indentLevel = 1;
			}

			if( usbLogic.stateTrigger == UltimateStatusBar.StateTrigger.Percentage )
			{
				EditorGUILayout.Slider( triggerValue, 0.0f, 1.0f, new GUIContent( "Percentage", "The percentage at which the state will trigger." ) );
				EditorGUILayout.PropertyField( alternateStateColor, new GUIContent( "Alt State Color", "The color of the alternate state." ) );
				if( usbLogic.flashing == true )
					EditorGUILayout.PropertyField( alternateStateColorMid, new GUIContent( "Alt State Color2", "The color of the alternate state." ) );
			}
			else if( usbLogic.stateTrigger == UltimateStatusBar.StateTrigger.ColorBlended )
			{
				EditorGUILayout.PropertyField( statusBarColor, new GUIContent( "Alt State Full", "The color of the alternate state." ) );
				EditorGUILayout.PropertyField( alternateStateColorMid, new GUIContent( "Alt State Mid", "The color of the alternate state." ) );
				EditorGUILayout.PropertyField( alternateStateColor, new GUIContent( "Alt State Low", "The color of the alternate state." ) );
			}
			EditorGUI.indentLevel = 0;
		}
		if( EditorGUI.EndChangeCheck() )
		{
			serializedObject.ApplyModifiedProperties();
			usbLogic.UpdateStatusBar( testValue, 1.0f );
		}
		EditorGUI.EndDisabledGroup();
		/* -------------------- > END ALTERNATE STATES < -------------------- */
		/* ---------------------------------- > END CUSTOMIZATION < ---------------------------------- */

		EditorGUILayout.Space();
		
		/* -------------------------------------- > DEBUGGING < -------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		EditorGUILayout.LabelField( "Debugging", EditorStyles.boldLabel );
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		EditorGUI.indentLevel = 1;

		if( usbLogic.statusBar != null )
			EditorGUILayout.LabelField( new GUIContent( "Fill Method: " + usbLogic.statusBar.fillMethod, "The Fill Method as determined by the Image Component on the Status Bar." ) );

		EditorGUI.BeginChangeCheck();
		testValue = EditorGUILayout.Slider( new GUIContent( "Test Value", "A test value to preview the Ultimate Status Bar when values change." ), testValue, 0.0f, 1.0f );
		EditorGUI.indentLevel = 0;
		if( EditorGUI.EndChangeCheck() )
		{
			usbLogic.statusBar.enabled = false;
			usbLogic.UpdateStatusBar( testValue, 1.0f );
			usbLogic.statusBar.enabled = true;
		}
		/* ------------------------------------ > END DEBUGGING < ------------------------------------ */

		EditorGUILayout.Space();
	}
}