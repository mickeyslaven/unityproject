/* Written by Kaz Crowe */
/* UltimateJoystickEditorJAVA.js ver. 1.2.2 */
#pragma strict
import UnityEngine.UI;
import UnityEditor.AnimatedValues;

@CustomEditor( UltimateJoystickJAVA )
public class UltimateJoystickEditorJAVA extends Editor
{
	/* -----< ASSIGNED VARIABLES >----- */
	var joystick : SerializedProperty;
	var joystickSizeFolder : SerializedProperty;
	var highlightBase : SerializedProperty;
	var highlightJoystick : SerializedProperty;
	var tensionAccentUp : SerializedProperty;
	var tensionAccentDown : SerializedProperty;
	var tensionAccentLeft : SerializedProperty;
	var tensionAccentRight : SerializedProperty;
	var joystickAnimator : SerializedProperty;
	var joystickBase : SerializedProperty;

	/* -----< SIZE AND PLACEMENT >----- */
	var scalingAxis : SerializedProperty;
	var anchor : SerializedProperty;
	var joystickTouchSize : SerializedProperty;
	var customTouchSize_X : SerializedProperty;
	var customTouchSize_Y : SerializedProperty;
	var customTouchSizePos_X : SerializedProperty;
	var customTouchSizePos_Y : SerializedProperty;
	var dynamicPositioning : SerializedProperty;
	var joystickSize : SerializedProperty;
	var radiusModifier : SerializedProperty;
	var customSpacing_X : SerializedProperty;
	var customSpacing_Y : SerializedProperty;

	/* -----< STYLES AND OPTIONS >----- */
	var touchPad : SerializedProperty;
	var throwable : SerializedProperty;
	var draggable : SerializedProperty;
	var throwDuration : SerializedProperty;
	var showHighlight : SerializedProperty;
	var showTension : SerializedProperty;
	var highlightColor : SerializedProperty;
	var tensionColorNone : SerializedProperty;
	var tensionColorFull : SerializedProperty;
	var axis : SerializedProperty;
	var boundary : SerializedProperty;
	var deadZoneOption : SerializedProperty;
	var xDeadZone : SerializedProperty;
	var yDeadZone : SerializedProperty;

	/* --------< TOUCH ACTION >-------- */
	var useAnimation : SerializedProperty;
	var useFade : SerializedProperty;
	var tapCountOption : SerializedProperty;
	var tapCountDuration : SerializedProperty;
	var tapCountEvent : SerializedProperty;
	var targetTapCount : SerializedProperty;
	var fadeUntouched : SerializedProperty;
	var fadeTouched : SerializedProperty;
	
	/* ----- > SCRIPTING REFERENCE < ----- */
	var joystickName : SerializedProperty;
	
	/* // ----< ANIMATED SECTIONS >---- \\ */
	var AssignedVariables : AnimBool;
	var SizeAndPlacement : AnimBool;
	var StyleAndOptions : AnimBool;
	var TouchActions : AnimBool;
	var ScriptReference : AnimBool;

	/* // ----< ANIMATED VARIABLE >---- \\ */
	var customTouchSizeOption : AnimBool;
	var throwableOption : AnimBool;
	var highlightOption : AnimBool;
	var tensionOption : AnimBool;
	var dzOneValueOption : AnimBool;
	var dzTwoValueOption : AnimBool;
	var tcOption : AnimBool;
	var tcTargetTapOption : AnimBool;
	var animationOption : AnimBool;
	var fadeOption : AnimBool;
	
	
	function OnEnable ()
	{
		// Store the references to all variables.
		StoreReferences();

		// Find parent canvas and add update script
		var myParent : GameObject = GetParentCanvas();
		if( myParent != null && !myParent.GetComponent( UltimateJoystickJAVAUpdater ) )
			myParent.AddComponent( typeof( UltimateJoystickJAVAUpdater ) );
	}

	function GetParentCanvas () : GameObject
	{
		// Store the current parent.
		var parent : Transform = Selection.activeGameObject.transform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{ 
			// If there is a Canvas component, return the component.
			if( parent.transform.GetComponent.<Canvas>() )
				return parent.transform.gameObject;
			
			// Else, shift to the next parent.
			parent = parent.transform.parent;
		}
		return null;
	}

	// Function called to display an interactive header.
	function DisplayHeader ( headerName : String , editorPref : String, targetAnim : AnimBool )
	{
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( headerName, EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( editorPref ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			EditorPrefs.SetBool( editorPref, EditorPrefs.GetBool( editorPref ) == true ? false : true );
			targetAnim.target = EditorPrefs.GetBool( editorPref );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
	
	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateJoystickJAVA.js script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	function OnInspectorGUI ()
	{
		serializedObject.Update();

		var uj : UltimateJoystickJAVA = target as UltimateJoystickJAVA;
		
		EditorGUILayout.Space();
		
		/* ----------------------------------------< ** ASSIGNED VARIABLES ** >---------------------------------------- */
		DisplayHeader( "Assigned Variables", "UUI_Variables", AssignedVariables );
		if( EditorGUILayout.BeginFadeGroup( AssignedVariables.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( joystick );
			EditorGUILayout.PropertyField( joystickSizeFolder, new GUIContent( "Size Folder" ) );
			EditorGUILayout.PropertyField( joystickBase );
			EditorGUI.indentLevel = 0;
			
			if( EditorGUILayout.BeginFadeGroup( highlightOption.faded ) )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Highlight Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField( highlightBase );
				EditorGUILayout.PropertyField( highlightJoystick );
				EditorGUI.indentLevel = 0;
			}
			if( AssignedVariables.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			
			if( EditorGUILayout.BeginFadeGroup( tensionOption.faded ) )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Tension Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField( tensionAccentUp, new GUIContent( "Tension Up" ) );
				EditorGUILayout.PropertyField( tensionAccentDown, new GUIContent( "Tension Down" ) );
				EditorGUILayout.PropertyField( tensionAccentLeft, new GUIContent( "Tension Left" ) );
				EditorGUILayout.PropertyField( tensionAccentRight, new GUIContent( "Tension Right" ) );
				EditorGUI.indentLevel = 0;
			}
			if( AssignedVariables.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			
			if( EditorGUILayout.BeginFadeGroup( animationOption.faded ) )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Touch Action Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField( joystickAnimator );
				EditorGUI.indentLevel = 0;
			}
			if( AssignedVariables.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndFadeGroup();
		/* --------------------------------------< ** END ASSIGNED VARIABLES ** >-------------------------------------- */
		
		EditorGUILayout.Space();
		
		/* ----------------------------------------< ** SIZE AND PLACEMENT ** >---------------------------------------- */
		DisplayHeader( "Size and Placement", "UUI_SizeAndPlacement", SizeAndPlacement );
		if( EditorGUILayout.BeginFadeGroup( SizeAndPlacement.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( scalingAxis, new GUIContent( "Scaling Axis", "The axis to scale the Ultimate Joystick from." ) );
			EditorGUILayout.PropertyField( anchor, new GUIContent( "Anchor", "The side of the screen that the\njoystick will be anchored to." ) );
			EditorGUILayout.PropertyField( joystickTouchSize, new GUIContent( "Touch Size", "The size of the area in which\nthe touch can be initiated." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				if( uj.joystickTouchSize == UltimateJoystickJAVA.JoystickTouchSize.Custom )
					customTouchSizeOption.target = true;
				else
					customTouchSizeOption.target = false;
			}
			if( EditorGUILayout.BeginFadeGroup( customTouchSizeOption.faded ) )
			{
				EditorGUILayout.BeginVertical( "Box" );
				EditorGUILayout.LabelField( "Touch Size Customization" );
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( customTouchSize_X, 0.0f, 100.0f, new GUIContent( "Width", "The width of the Joystick Touch Area." ) );
				EditorGUILayout.Slider( customTouchSize_Y, 0.0f, 100.0f, new GUIContent( "Height", "The height of the Joystick Touch Area." ) );
				EditorGUILayout.Slider( customTouchSizePos_X, 0.0f, 100.0f, new GUIContent( "X Position", "The x position of the Joystick Touch Area." ) );
				EditorGUILayout.Slider( customTouchSizePos_Y, 0.0f, 100.0f, new GUIContent( "Y Position", "The y position of the Joystick Touch Area." ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( SizeAndPlacement.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( dynamicPositioning, new GUIContent( "Dynamic Positioning", "Moves the joystick to the position of the initial touch." ) );
			EditorGUILayout.Slider( joystickSize, 1.0f, 4.0f, new GUIContent( "Joystick Size", "The overall size of the joystick." ) );
			EditorGUILayout.Slider( radiusModifier, 2.0f, 7.0f, new GUIContent( "Radius", "Determines how far the joystick can\nmove visually from the center." ) );
			EditorGUILayout.BeginVertical( "Box" );
			EditorGUILayout.LabelField( "Joystick Position" );
			EditorGUI.indentLevel = 1;
			EditorGUILayout.Slider( customSpacing_X, 0.0f, 50.0f, new GUIContent( "X Position:" ) );
			EditorGUILayout.Slider( customSpacing_Y, 0.0f, 100.0f, new GUIContent( "Y Position:" ) );
			EditorGUI.indentLevel = 0;
			EditorGUILayout.EndVertical();
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndFadeGroup();
		/* --------------------------------------< ** END SIZE AND PLACEMENT ** >-------------------------------------- */
		
		EditorGUILayout.Space();
		
		/* ----------------------------------------< ** STYLE AND OPTIONS ** >----------------------------------------- */
		DisplayHeader( "Style and Options", "UUI_StyleAndOptions", StyleAndOptions );
		if( EditorGUILayout.BeginFadeGroup( StyleAndOptions.faded ) )
		{
			EditorGUILayout.Space();
			
			// --------------------------< TOUCH PAD >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( touchPad, new GUIContent( "Touch Pad", "Disables the visuals of the joystick." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				if( uj.touchPad == true )
				{
					highlightOption.target = false;
					tensionOption.target = false;
				}

				SetTouchPad( uj );
				SetHighlight( uj );
				SetTensionAccent( uj );
			}
			
			if( uj.touchPad == true && uj.joystickBase == null )
				EditorGUILayout.HelpBox( "Joystick Base needs to be assigned in the Assigned Variables section.", MessageType.Error );
			// ------------------------< END TOUCH PAD >------------------------ //
			
			// --------------------------< THROWABLE >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( throwable, new GUIContent( "Throwable", "Smoothly transitions the joystick back to\ncenter when the input is released." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				throwableOption.target = uj.throwable;
			}
			
			if( EditorGUILayout.BeginFadeGroup( throwableOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( throwDuration, 0.05f, 1.0f, new GUIContent( "Throw Duration", "Time in seconds to return to center." ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			// ------------------------< END THROWABLE >------------------------ //
			
			// --------------------------< DRAGGABLE >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( draggable, new GUIContent( "Draggable", "Drags the joystick to follow the touch if it is farther than the radius." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			
			if( uj.draggable == true && uj.boundary == UltimateJoystickJAVA.Boundary.Square )
				EditorGUILayout.HelpBox( "Draggable option will force the boundary to being circular. " +
				                        "Please use a circular boundary when using the draggable option.", MessageType.Warning );
			// ------------------------< END DRAGGABLE >------------------------ //
			
			EditorGUI.BeginDisabledGroup( uj.touchPad == true );// This is the start of the disabled fields if the user is using the touchPad option.

			// --------------------------< HIGHLIGHT >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showHighlight, new GUIContent( "Show Highlight", "Displays the highlight images with the Highlight Color variable." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetHighlight( uj );
				highlightOption.target = uj.showHighlight;
			}
			
			if( EditorGUILayout.BeginFadeGroup( highlightOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( highlightColor );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					uj.UpdateHighlightColor();
				}
				
				if( uj.highlightBase == null && uj.highlightJoystick == null )
					EditorGUILayout.HelpBox( "No highlight images have been assigned. Please assign some highlight images before continuing.", MessageType.Error );
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			// ------------------------< END HIGHLIGHT >------------------------ //
			
			// ---------------------------< TENSION >--------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showTension, new GUIContent( "Show Tension", "Displays the visual direction of the joystick using the tension color options." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetTensionAccent( uj );
				tensionOption.target = uj.showTension;
			}
			
			if( EditorGUILayout.BeginFadeGroup( tensionOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( tensionColorNone, new GUIContent( "Tension None", "The color displayed when the joystick\nis closest to center." ) );
				EditorGUILayout.PropertyField( tensionColorFull, new GUIContent( "Tension Full", "The color displayed when the joystick\nis at the furthest distance." ) );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					TensionAccentReset( uj );
				}
				
				if( uj.tensionAccentUp == null || uj.tensionAccentDown == null || uj.tensionAccentLeft == null || uj.tensionAccentRight == null )
					EditorGUILayout.HelpBox( "Some tension accents are unassigned. Please assign all images before continuing.", MessageType.Error );
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			// -------------------------< END TENSION >------------------------- //
			
			EditorGUI.EndDisabledGroup();// This is the end for the Touch Pad option.
			
			// -----------------------------< AXIS >---------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( axis, new GUIContent( "Axis", "Contrains the joystick to a certain axis." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			// ---------------------------< END AXIS >-------------------------- //
			
			// ---------------------------< BOUNDARY >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( boundary, new GUIContent( "Boundry", "Determines how the joystick's position is clamped." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			// -------------------------< END BOUNDARY >------------------------ //
			
			// --------------------------< DEAD ZONE >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( deadZoneOption, new GUIContent( "Dead Zone", "Forces the joystick position to being only values of -1, 0, and 1." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				dzOneValueOption.target = uj.deadZoneOption == UltimateJoystickJAVA.DeadZoneOption.OneValue ? true : false;
				dzTwoValueOption.target = uj.deadZoneOption == UltimateJoystickJAVA.DeadZoneOption.TwoValues ? true : false;
			}
			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			if( EditorGUILayout.BeginFadeGroup( dzTwoValueOption.faded ) )
			{
				EditorGUILayout.Slider( xDeadZone, 0.0f, 1.0f, new GUIContent( "X Dead Zone", "X values within this range will be forced to 0." ) );
				EditorGUILayout.Slider( yDeadZone, 0.0f, 1.0f, new GUIContent( "Y Dead Zone", "Y values within this range will be forced to 0." ) );
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			
			if( EditorGUILayout.BeginFadeGroup( dzOneValueOption.faded ) )
			{
				EditorGUILayout.Slider( xDeadZone, 0.0f, 1.0f, new GUIContent( "Dead Zone", "Values within this range will be forced to 0." ) );
				uj.yDeadZone = uj.xDeadZone;
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			EditorGUI.indentLevel = 0;
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			// ------------------------< END DEAD ZONE >------------------------ //
		}
		EditorGUILayout.EndFadeGroup();
		/* --------------------------------------< ** END STYLE AND OPTIONS ** >------------------------------------- */
		
		EditorGUILayout.Space();
		
		/* ------------------------------------------< ** TOUCH ACTIONS ** >----------------------------------------- */
		DisplayHeader( "Touch Actions", "UUI_TouchActions", TouchActions );
		if( EditorGUILayout.BeginFadeGroup( TouchActions.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( tapCountOption, new GUIContent( "Tap Count", "Allows the joystick to calculate double taps and a touch and release within a certain time window." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				tcOption.target = uj.tapCountOption != UltimateJoystickJAVA.TapCountOption.NoCount ? true : false;
				tcTargetTapOption.target = uj.tapCountOption == UltimateJoystickJAVA.TapCountOption.Accumulate ? true : false;
			}

			if( EditorGUILayout.BeginFadeGroup( tcOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( tapCountEvent );
				EditorGUILayout.Slider( tapCountDuration, 0.0f, 1.0f, new GUIContent( "Tap Time Window", "Time in seconds that the joystick can recieve taps." ) );
				if( EditorGUILayout.BeginFadeGroup( tcTargetTapOption.faded ) )
					EditorGUILayout.IntSlider( targetTapCount, 1, 5, new GUIContent( "Target Tap Count", "How many taps to activate the Tap Count Event?" ) );
				if( TouchActions.faded == 1 )
					EditorGUILayout.EndFadeGroup();

				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( TouchActions.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useAnimation, new GUIContent( "Use Animation", "Play animation in reaction to input." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetAnimation( uj );
				animationOption.target = uj.useAnimation;
			}
			if( uj.useAnimation == true )
			{
				EditorGUI.indentLevel = 1;
				if( uj.joystickAnimator == null )
					EditorGUILayout.HelpBox( "Joystick Animator needs to be assigned.", MessageType.Error );
				EditorGUI.indentLevel = 0;
			}
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useFade, new GUIContent( "Use Fade", "Fade joystick visuals when touched,\nand released?" ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				if( uj.useFade == true )
					uj.gameObject.GetComponent.<CanvasGroup>().alpha = uj.fadeUntouched;
				else
					uj.gameObject.GetComponent.<CanvasGroup>().alpha = 1.0f;

				fadeOption.target = uj.useFade;
			}
			if( EditorGUILayout.BeginFadeGroup( fadeOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( fadeUntouched, 0.0f, 1.0f, new GUIContent( "Fade Untouched", "The alpha of the joystick when it is NOT receiving input." ) );
				EditorGUILayout.Slider( fadeTouched, 0.0f, 1.0f, new GUIContent( "Fade Touched", "The alpha of the joystick when receiving input." ) );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					uj.gameObject.GetComponent.<CanvasGroup>().alpha = uj.fadeUntouched;
				}
				
				EditorGUI.indentLevel = 0;
			}
			if( TouchActions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
		}
		EditorGUILayout.EndFadeGroup();
		/* ------------------------------------------< ** END TOUCH ACTIONS ** >------------------------------------------ */
		
		EditorGUILayout.Space();
		
		/* ------------------------------------------< ** SCRIPT REFERENCE ** >------------------------------------------- */
		DisplayHeader( "Script Reference", "UUI_ScriptReference", ScriptReference );
		if( EditorGUILayout.BeginFadeGroup( ScriptReference.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( joystickName );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
			
			if( uj.joystickName == String.Empty )
				EditorGUILayout.HelpBox( "Please assign a Joystick Name in order to be able to get this joystick's position dynamically.", MessageType.Warning );
			else
			{
				EditorGUILayout.LabelField( "Script Reference:" );
				EditorGUILayout.TextField( "UltimateJoystickJAVA.GetPosition( \"" + uj.joystickName + "\" )" );
			}
		}
		EditorGUILayout.EndFadeGroup();
		/* -----------------------------------------< ** END SCRIPT REFERENCE ** >---------------------------------------- */
		
		EditorGUILayout.Space();
		
		/* ----------------------------------------------< ** HELP TIPS ** >---------------------------------------------- */
		if( uj.joystick == null )
			EditorGUILayout.HelpBox( "Joystick needs to be assigned in 'Assigned Variables'!", MessageType.Error );
		if( uj.joystickSizeFolder == null )
			EditorGUILayout.HelpBox( "Joystick Size Folder needs to be assigned in 'Assigned Variables'!", MessageType.Error );
		if( uj.joystickBase == null )
			EditorGUILayout.HelpBox( "Joystick Base needs to be assigned in 'Assigned Variables'!", MessageType.Error );
		/* --------------------------------------------< ** END HELP TIPS ** >-------------------------------------------- */

		Repaint();
	}
	
	// This function stores the references to the variables of the target.
	function StoreReferences ()
	{
		/* -----< ASSIGNED VARIABLES >----- */
		joystick = serializedObject.FindProperty( "joystick" );
		joystickSizeFolder = serializedObject.FindProperty( "joystickSizeFolder" );
		joystickBase = serializedObject.FindProperty( "joystickBase" );
		highlightBase = serializedObject.FindProperty( "highlightBase" );
		highlightJoystick = serializedObject.FindProperty( "highlightJoystick" );
		tensionAccentUp = serializedObject.FindProperty( "tensionAccentUp" );
		tensionAccentDown = serializedObject.FindProperty( "tensionAccentDown" );
		tensionAccentLeft = serializedObject.FindProperty( "tensionAccentLeft" );
		tensionAccentRight = serializedObject.FindProperty( "tensionAccentRight" );
		joystickAnimator = serializedObject.FindProperty( "joystickAnimator" );
		
		/* -----< SIZE AND PLACEMENT >----- */
		scalingAxis = serializedObject.FindProperty( "scalingAxis" );
		anchor = serializedObject.FindProperty( "anchor" );
		joystickTouchSize = serializedObject.FindProperty( "joystickTouchSize" );
		customTouchSize_X = serializedObject.FindProperty( "customTouchSize_X" );
		customTouchSize_Y = serializedObject.FindProperty( "customTouchSize_Y" );
		customTouchSizePos_X = serializedObject.FindProperty( "customTouchSizePos_X" );
		customTouchSizePos_Y = serializedObject.FindProperty( "customTouchSizePos_Y" );
		dynamicPositioning = serializedObject.FindProperty( "dynamicPositioning" );
		joystickSize = serializedObject.FindProperty( "joystickSize" );
		radiusModifier = serializedObject.FindProperty( "radiusModifier" );
		customSpacing_X = serializedObject.FindProperty( "customSpacing_X" );
		customSpacing_Y = serializedObject.FindProperty( "customSpacing_Y" );
		
		/* -----< STYLES AND OPTIONS >----- */
		touchPad = serializedObject.FindProperty( "touchPad" );
		throwable = serializedObject.FindProperty( "throwable" );
		draggable = serializedObject.FindProperty( "draggable" );
		throwDuration = serializedObject.FindProperty( "throwDuration" );
		showHighlight = serializedObject.FindProperty( "showHighlight" );
		highlightColor = serializedObject.FindProperty( "highlightColor" );
		showTension = serializedObject.FindProperty( "showTension" );
		tensionColorNone = serializedObject.FindProperty( "tensionColorNone" );
		tensionColorFull = serializedObject.FindProperty( "tensionColorFull" );
		axis = serializedObject.FindProperty( "axis" );
		boundary = serializedObject.FindProperty( "boundary" );
		deadZoneOption = serializedObject.FindProperty( "deadZoneOption" );
		xDeadZone = serializedObject.FindProperty( "xDeadZone" );
		yDeadZone = serializedObject.FindProperty( "yDeadZone" );
		
		/* --------< TOUCH ACTION >-------- */
		useAnimation = serializedObject.FindProperty( "useAnimation" );
		useFade = serializedObject.FindProperty( "useFade" );
		tapCountOption = serializedObject.FindProperty( "tapCountOption" );
		tapCountDuration = serializedObject.FindProperty( "tapCountDuration" );
		targetTapCount = serializedObject.FindProperty( "targetTapCount" );
		tapCountEvent = serializedObject.FindProperty( "tapCountEvent" );
		fadeUntouched = serializedObject.FindProperty( "fadeUntouched" );
		fadeTouched = serializedObject.FindProperty( "fadeTouched" );

		/* ------< SCRIPT REFERENCE >------ */
		joystickName = serializedObject.FindProperty( "joystickName" );
		
		/* // ----< ANIMATED SECTIONS >---- \\ */
		AssignedVariables = new AnimBool( EditorPrefs.GetBool( "UUI_Variables" ) );
		SizeAndPlacement = new AnimBool( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) );
		StyleAndOptions = new AnimBool( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) );
		TouchActions = new AnimBool( EditorPrefs.GetBool( "UUI_TouchActions" ) );
		ScriptReference = new AnimBool( EditorPrefs.GetBool( "UUI_ScriptReference" ) );
		
		/* // ----< ANIMATED VARIABLES >---- \\ */
		var ult : UltimateJoystickJAVA = target as UltimateJoystickJAVA;
		customTouchSizeOption = new AnimBool( ult.joystickTouchSize == UltimateJoystickJAVA.JoystickTouchSize.Custom ? true : false );
		throwableOption = new AnimBool( ult.throwable );
		highlightOption = new AnimBool( ult.showHighlight );
		tensionOption = new AnimBool( ult.showTension );
		dzOneValueOption = new AnimBool( ult.deadZoneOption == UltimateJoystickJAVA.DeadZoneOption.OneValue ? true : false );
		dzTwoValueOption = new AnimBool( ult.deadZoneOption == UltimateJoystickJAVA.DeadZoneOption.TwoValues ? true : false );
		tcOption = new AnimBool( ult.tapCountOption != UltimateJoystickJAVA.TapCountOption.NoCount ? true : false );
		tcTargetTapOption = new AnimBool( ult.tapCountOption == UltimateJoystickJAVA.TapCountOption.Accumulate ? true : false );
		animationOption = new AnimBool( ult.useAnimation );
		fadeOption = new AnimBool( ult.useFade );

		SetTouchPad( ult );
		SetHighlight( ult );
		SetAnimation( ult );
		SetTensionAccent( ult );
		
		
		
		if( ult.useFade == true )
		{
			if( !ult.GetComponent.<CanvasGroup>() )
				ult.gameObject.AddComponent.<CanvasGroup>();
			
			ult.gameObject.GetComponent.<CanvasGroup>().alpha = ult.fadeUntouched;
		}
		else
		{
			if( !ult.GetComponent.<CanvasGroup>() )
				ult.gameObject.AddComponent.<CanvasGroup>();
			
			ult.gameObject.GetComponent.<CanvasGroup>().alpha = 1.0f;
		}
	}

	function SetTouchPad ( ult : UltimateJoystickJAVA )
	{
		if( ult.touchPad == true )
		{
			if( ult.showHighlight == true )
				ult.showHighlight = false;
			if( ult.showTension == true )
				ult.showTension = false;

			if( ult.dynamicPositioning == false )
				ult.dynamicPositioning = true;

			if( ult.joystickBase != null && ult.joystickBase.GetComponent.<Image>().enabled == true )
				ult.joystickBase.GetComponent.<Image>().enabled = false;
				
			if( ult.joystick.GetComponent.<Image>().enabled == true )
				ult.joystick.GetComponent.<Image>().enabled = false;
		}
		else
		{
			if( ult.joystickBase != null )
			{
				if( ult.joystickBase.GetComponent.<Image>().enabled == false )
					ult.joystickBase.GetComponent.<Image>().enabled = true;
			}
			if( ult.joystick.GetComponent.<Image>().enabled == false )
				ult.joystick.GetComponent.<Image>().enabled = true;
		}
	}

	function SetHighlight ( uj : UltimateJoystickJAVA )
	{
		if( uj.showHighlight == true )
		{
			if( uj.highlightBase != null && uj.highlightBase.gameObject.activeInHierarchy == false )
				uj.highlightBase.gameObject.SetActive( true );
			if( uj.highlightJoystick != null && uj.highlightJoystick.gameObject.activeInHierarchy == false )
				uj.highlightJoystick.gameObject.SetActive( true );
			
			uj.UpdateHighlightColor();
		}
		else
		{
			if( uj.highlightBase != null && uj.highlightBase.gameObject.activeInHierarchy == true )
				uj.highlightBase.gameObject.SetActive( false );
			if( uj.highlightJoystick != null && uj.highlightJoystick.gameObject.activeInHierarchy == true )
				uj.highlightJoystick.gameObject.SetActive( false );
		}
	}
	
	function SetTensionAccent ( uj : UltimateJoystickJAVA )
	{
		if( uj.showTension == true )
		{
			if( uj.tensionAccentUp == null || uj.tensionAccentDown == null || uj.tensionAccentLeft == null || uj.tensionAccentRight == null )
				return;
			
			if( uj.tensionAccentUp != null && uj.tensionAccentUp.gameObject.activeInHierarchy == false )
				uj.tensionAccentUp.gameObject.SetActive( true );
			if( uj.tensionAccentDown != null && uj.tensionAccentDown.gameObject.activeInHierarchy == false )
				uj.tensionAccentDown.gameObject.SetActive( true );
			if( uj.tensionAccentLeft != null && uj.tensionAccentLeft.gameObject.activeInHierarchy == false )
				uj.tensionAccentLeft.gameObject.SetActive( true );
			if( uj.tensionAccentRight != null && uj.tensionAccentRight.gameObject.activeInHierarchy == false )
				uj.tensionAccentRight.gameObject.SetActive( true );
			
			TensionAccentReset( uj );
		}
		else
		{
			if( uj.tensionAccentUp != null && uj.tensionAccentUp.gameObject.activeInHierarchy == true )
				uj.tensionAccentUp.gameObject.SetActive( false );
			if( uj.tensionAccentDown != null && uj.tensionAccentDown.gameObject.activeInHierarchy == true )
				uj.tensionAccentDown.gameObject.SetActive( false );
			if( uj.tensionAccentLeft != null && uj.tensionAccentLeft.gameObject.activeInHierarchy == true )
				uj.tensionAccentLeft.gameObject.SetActive( false );
			if( uj.tensionAccentRight != null && uj.tensionAccentRight.gameObject.activeInHierarchy == true )
				uj.tensionAccentRight.gameObject.SetActive( false );
		}
	}
	
	function TensionAccentReset ( uj : UltimateJoystickJAVA )
	{
		uj.tensionAccentUp.color = uj.tensionColorNone;
		uj.tensionAccentDown.color = uj.tensionColorNone;
		uj.tensionAccentLeft.color = uj.tensionColorNone;
		uj.tensionAccentRight.color = uj.tensionColorNone;
	}
	
	function SetAnimation ( uj : UltimateJoystickJAVA )
	{
		if( uj.useAnimation == true )
		{
			if( uj.joystickAnimator != null )
				if( uj.joystickAnimator.enabled == false )
					uj.joystickAnimator.enabled = true;
		}
		else
		{
			if( uj.joystickAnimator != null )
				if( uj.joystickAnimator.enabled == true )
					uj.joystickAnimator.enabled = false;
		}
	}
}