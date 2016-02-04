/* Written by Kaz Crowe */
/* UltimateButtonEditorJAVA.cs ver 1.5.2 */
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[ CanEditMultipleObjects ]
[CustomEditor( typeof( UltimateButtonJAVA ) )]
public class UltimateButtonEditorJAVA : Editor
{
	/* -----< ASSIGNED VARIABLES >----- */
	SerializedProperty sizeFolder, buttonHighlight, tensionAccent, buttonAnimator;
	/* ---< END ASSIGNED VARIABLES >--- */

	/* -----< SIZE AND PLACEMENT >----- */
	SerializedProperty scalingAxis, anchor, touchSize;
	SerializedProperty buttonSize, customSpacing_X, customSpacing_Y;
	/* ---< END SIZE AND PLACEMENT >--- */

	/* -----< STYLES AND OPTIONS >----- */
	SerializedProperty showHighlight, showTension;
	SerializedProperty highlightColor, tensionColorNone, tensionColorFull;
	SerializedProperty tensionFadeInDuration, tensionFadeOutDuration;
	/* ---< END STYLES AND OPTIONS >--- */

	/* --------< TOUCH ACTION >-------- */
	SerializedProperty tapCountOption;
	SerializedProperty tapCountDuration;
	SerializedProperty targetTapCount;
	SerializedProperty tapCountEvent;
	SerializedProperty useAnimation, useFade;
	SerializedProperty fadeUntouched, fadeTouched;
	SerializedProperty fadeInDuration, fadeOutDuration;
	/* ------< END TOUCH ACTION >------ */

	/* ------< SCRIPT REFERENCE >------ */
	SerializedProperty referenceOption;
	SerializedProperty buttonName;
	SerializedProperty onButtonDown;
	SerializedProperty onButtonUp;
	/* ----< END SCRIPT REFERENCE >---- */

	/* // ----< ANIMATED SECTIONS >---- \\ */
	AnimBool AssignedVariables, SizeAndPlacement, StyleAndOptions;
	AnimBool TouchActions, ScriptReference;
	/* \\ --< END ANIMATED SECTIONS >-- // */

	/* // ----< ANIMATED VARIABLE >---- \\ */
	AnimBool highlightOption, tensionOption;
	AnimBool animationOption, fadeOption, tapOption;
	/* // --< END ANIMATED VARIABLE >-- \\ */

	
	void OnEnable ()
	{
		// Store the Ultimate Button references as soon as this script is being viewed.
		StoreReferences();

		// Register the Undo function to be called for undo's.
		Undo.undoRedoPerformed += UndoRedoCallback;

		// Find parent canvas and add update script
		GameObject myParent = GetParentCanvas();
		if( myParent != null && !myParent.GetComponent<UltimateButtonJAVAUpdater>() )
			myParent.AddComponent( typeof( UltimateButtonJAVAUpdater ) );
	}

	GameObject GetParentCanvas ()
	{
		// Store the current parent.
		Transform parent = Selection.activeGameObject.transform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{ 
			// If there is a Canvas component, return the component.
			if( parent.transform.GetComponent<Canvas>() )
				return parent.transform.gameObject;
			
			// Else, shift to the next parent.
			parent = parent.transform.parent;
		}
		return null;
	}

	// Function for Undo/Redo operations.
	void UndoRedoCallback ()
	{
		// Re-reference all variables on undo/redo.
		StoreReferences();
	}

	// Function called to display an interactive header.
	void DisplayHeader ( string headerName, string editorPref, AnimBool targetAnim )
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
	in the UltimateButtonJAVA.js script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		UltimateButtonJAVA ult = ( UltimateButtonJAVA ) target;

		EditorGUILayout.Space();

		#region ASSIGNED VARIABLES
		/* ///// ---------------------------------------- < ASSIGNED VARIABLES > ---------------------------------------- \\\\\ */
		DisplayHeader( "Assigned Variables", "UUI_Variables", AssignedVariables );
		if( EditorGUILayout.BeginFadeGroup( AssignedVariables.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.PropertyField( sizeFolder );
				if( EditorGUILayout.BeginFadeGroup( highlightOption.faded ) )
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField( "Highlight Variables", EditorStyles.boldLabel );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.PropertyField( buttonHighlight );
					EditorGUI.indentLevel = 0;
				}
				if( AssignedVariables.faded == 1 )
					EditorGUILayout.EndFadeGroup();

				if( EditorGUILayout.BeginFadeGroup( tensionOption.faded ) )
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField( "Tension Variables", EditorStyles.boldLabel );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.PropertyField( tensionAccent );
					EditorGUI.indentLevel = 0;
				}
				if( AssignedVariables.faded == 1 )
					EditorGUILayout.EndFadeGroup();

				if( EditorGUILayout.BeginFadeGroup( animationOption.faded ) )
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField( "Touch Action Variables", EditorStyles.boldLabel );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.PropertyField( buttonAnimator );
					EditorGUI.indentLevel = 0;
				}
				if( AssignedVariables.faded == 1 )
					EditorGUILayout.EndFadeGroup();
			}
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndFadeGroup();
		/* \\\\\ -------------------------------------- < END ASSIGNED VARIABLES > -------------------------------------- ///// */
		#endregion

		EditorGUILayout.Space();

		#region SIZE AND PLACEMENT
		/* ///// ---------------------------------------- < SIZE AND PLACEMENT > ---------------------------------------- \\\\\ */
		DisplayHeader( "Size and Placement", "UUI_SizeAndPlacement", SizeAndPlacement );
		
		if( EditorGUILayout.BeginFadeGroup( SizeAndPlacement.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.PropertyField( scalingAxis );
				EditorGUILayout.PropertyField( anchor );
				EditorGUILayout.PropertyField( touchSize, new GUIContent( "Touch Size", "The size of the area in which the touch can start" ) );
				EditorGUILayout.Slider( buttonSize, 1.0f, 4.0f, new GUIContent( "Button Size" ) );
			}
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			EditorGUILayout.BeginVertical( "Box" );
			{
				EditorGUILayout.LabelField( "Button Position" );
				EditorGUI.indentLevel = 1;
				{
					EditorGUI.BeginChangeCheck();
					{
						EditorGUILayout.Slider( customSpacing_X, 0.0f, 50.0f, new GUIContent( "X Position:" ) );
						EditorGUILayout.Slider( customSpacing_Y, 0.0f, 100.0f, new GUIContent( "Y Position:" ) );
					}
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();
				}
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndFadeGroup();
		/* \\\\\ -------------------------------------- < END SIZE AND PLACEMENT > -------------------------------------- ///// */
		#endregion

		EditorGUILayout.Space();

		#region STYLE AND OPTIONS
		/* ///// ---------------------------------------- < STYLES AND OPTIONS > ---------------------------------------- \\\\\ */
		DisplayHeader( "Style and Options", "UUI_StyleAndOptions", StyleAndOptions );
		if( EditorGUILayout.BeginFadeGroup( StyleAndOptions.faded ) )
		{
			EditorGUILayout.Space();

			// --------------------------< HIGHLIGHT >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showHighlight, new GUIContent( "Show Highlight", "Displays the highlight images with the Highlight Color variable." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetHighlight( ult );
				highlightOption.target = ult.showHighlight;
			}
			if( EditorGUILayout.BeginFadeGroup( highlightOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				{
					if( ult.buttonHighlight != null )
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField( highlightColor );
						if( EditorGUI.EndChangeCheck() )
						{
							serializedObject.ApplyModifiedProperties();
							ult.UpdateHighlight();
						}
					}
					else
						EditorGUILayout.HelpBox( "Button Highlight is not assigned in 'Variables'. Button Highlight will not be displayed", MessageType.Warning );
				}
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			// ------------------------< END HIGHLIGHT >------------------------ //
			
			// ---------------------------< TENSION >--------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showTension, new GUIContent( "Show Tension", "Displays the visual state of the button using the tension color options." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetTensionAccent( ult );
				tensionOption.target = ult.showTension;
			}

			if( EditorGUILayout.BeginFadeGroup( tensionOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				{
					if( ult.tensionAccent != null )
					{
						EditorGUI.BeginChangeCheck();
						{
							EditorGUILayout.PropertyField( tensionColorNone, new GUIContent( "Tension None", "The Color of the Tension with no input." ) );
							EditorGUILayout.PropertyField( tensionColorFull, new GUIContent( "Tension Full", "The Color of the Tension when there is input." ) );
							EditorGUILayout.PropertyField( tensionFadeInDuration, new GUIContent( "Fade In Duration", "Time is seconds for the tension to fade in, with 0 being instant." ) );
							EditorGUILayout.PropertyField( tensionFadeOutDuration, new GUIContent( "Fade Out Duration", "Time is seconds for the tension to fade out, with 0 being instant." ) );
						}
						if( EditorGUI.EndChangeCheck() )
						{
							serializedObject.ApplyModifiedProperties();
							ult.tensionAccent.color = ult.tensionColorNone;
						}
					}
					else
						EditorGUILayout.HelpBox( "The Tension Accent variable is not assigned. Please assign the image before continuing.", MessageType.Error );
				}
				EditorGUI.indentLevel = 0;
			}
			if( StyleAndOptions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
			// -------------------------< END TENSION >------------------------- //
		}
		EditorGUILayout.EndFadeGroup();
		/* \\\\\ -------------------------------------- < END STYLES AND OPTIONS > -------------------------------------- ///// */
		#endregion

		EditorGUILayout.Space();

		#region TOUCH ACTIONS
		/* ///// --------------------------------------- < TOUCH ACTION SECTION > --------------------------------------- \\\\\ */
		DisplayHeader( "Touch Actions", "UUI_TouchActions", TouchActions );
		if( EditorGUILayout.BeginFadeGroup( TouchActions.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( tapCountOption );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				tapOption.target = ult.tapCountOption != UltimateButtonJAVA.TapCountOption.NoCount ? true : false;
			}
			if( EditorGUILayout.BeginFadeGroup( tapOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				{
					EditorGUI.BeginChangeCheck();
					{
						EditorGUILayout.PropertyField( tapCountEvent );
						EditorGUILayout.Slider( tapCountDuration, 0.0f, 1.0f, new GUIContent( "Tap Time Window" ) );
						if( ult.tapCountOption == UltimateButtonJAVA.TapCountOption.Accumulate )
							EditorGUILayout.IntSlider( targetTapCount, 1, 5, new GUIContent( "Target Tap Count" ) );
					}
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();
				}
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( TouchActions.faded == 1 )
				EditorGUILayout.EndFadeGroup();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useAnimation );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetAnimation( ult );
			}

			if( ult.useAnimation == true && ult.buttonAnimator == null )
				EditorGUILayout.HelpBox( "Button Animator needs to be assigned.", MessageType.Error );
				
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useFade );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				// Apply the current fade untouched on changing this value if the fade is enabled
				fadeOption.target = ult.useFade;
				if( ult.useFade == true )
					ult.gameObject.GetComponent<CanvasGroup>().alpha = ult.fadeUntouched;
				else
					ult.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
			}
			if( EditorGUILayout.BeginFadeGroup( fadeOption.faded ) )
			{
				EditorGUI.indentLevel = 1;
				{
					EditorGUI.BeginChangeCheck();
					{
						EditorGUILayout.Slider( fadeUntouched, 0.0f, 1.0f, new GUIContent( "Fade Untouched", "This controls the alpha of the button when it is NOT receiving any input." ) );
						EditorGUILayout.Slider( fadeTouched, 0.0f, 1.0f, new GUIContent( "Fade Touched", "This controls the alpha of the button when it is receiving input." ) );
					}
					if( EditorGUI.EndChangeCheck() )
					{
						serializedObject.ApplyModifiedProperties();
						if( ult.useFade == true )
							ult.gameObject.GetComponent<CanvasGroup>().alpha = ult.fadeUntouched;
						else
							ult.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
					}
					EditorGUI.BeginChangeCheck();
					{
						EditorGUILayout.PropertyField( fadeInDuration, new GUIContent( "Fade In Duration", "Time is seconds for the button to fade in, with 0 being instant." ) );
						EditorGUILayout.PropertyField( fadeOutDuration, new GUIContent( "Fade Out Duration", "Time is seconds for the button to fade out, with 0 being instant." ) );
					}
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();
				}
				EditorGUI.indentLevel = 0;
			}
			if( TouchActions.faded == 1 )
				EditorGUILayout.EndFadeGroup();
		}
		EditorGUILayout.EndFadeGroup();
		/* ///// ------------------------------------- < END TOUCH ACTION SECTION > ------------------------------------- \\\\\ */
		#endregion

		EditorGUILayout.Space();

		#region SCRIPT REFERENCE
		/* \\\\\ ----------------------------------------- < SCRIPT REFERENCE > ----------------------------------------- ///// */
		DisplayHeader( "Script Reference", "UUI_ScriptReference", ScriptReference );
		if( EditorGUILayout.BeginFadeGroup( ScriptReference.faded ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( referenceOption );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( ult.referenceOption == UltimateButtonJAVA.ReferenceOption.GetButtonStates )
			{
				EditorGUILayout.Space();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( buttonName );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
			
				if( ult.buttonName == string.Empty )
					EditorGUILayout.HelpBox( "Please assign a Button Name in order to be able to get this button's state dynamically.", MessageType.Warning );
				else
				{
					EditorGUILayout.LabelField( "Get Button Down:" );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.TextField( "UltimateButtonJAVA.GetButtonDown( \"" + ult.buttonName + "\" )" );
					EditorGUI.indentLevel = 0;

					EditorGUILayout.LabelField( "Get Button:" );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.TextField( "UltimateButtonJAVA.GetButton( \"" + ult.buttonName + "\" )" );
					EditorGUI.indentLevel = 0;

					EditorGUILayout.LabelField( "Get Button Up:" );
					EditorGUI.indentLevel = 1;
					EditorGUILayout.TextField( "UltimateButtonJAVA.GetButtonUp( \"" + ult.buttonName + "\" )" );
					EditorGUI.indentLevel = 0;
				}
			}
			else
			{
				EditorGUILayout.Space();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( onButtonDown );
				EditorGUILayout.PropertyField( onButtonUp );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
			}
		}
		EditorGUILayout.EndFadeGroup();
		/* ///// --------------------------------------- < END SCRIPT REFERENCE > --------------------------------------- \\\\\ */
		#endregion
		EditorGUILayout.Space();

		if( ult.sizeFolder == null )
		{
			foreach( RectTransform trans in ult.GetComponentsInChildren<RectTransform>() )
				if( trans.name == "ButtonSizeFolder" )
					ult.sizeFolder = trans;

			if( ult.sizeFolder == null )
				EditorGUILayout.HelpBox( "Size Folder needs to be assigned in 'Assigned Variables'!", MessageType.Error );
		}

		Repaint();
	}

	// This function stores the references to the variables of the target.
	void StoreReferences ()
	{
		/* -----< ASSIGNED VARIABLES >----- */
		sizeFolder = serializedObject.FindProperty( "sizeFolder" );
		buttonHighlight = serializedObject.FindProperty( "buttonHighlight" );
		tensionAccent = serializedObject.FindProperty( "tensionAccent" );
		buttonAnimator = serializedObject.FindProperty( "buttonAnimator" );
		/* ---< END ASSIGNED VARIABLES >--- */

		/* -----< SIZE AND PLACEMENT >----- */
		scalingAxis = serializedObject.FindProperty( "scalingAxis" );
		anchor = serializedObject.FindProperty( "anchor" );
		touchSize = serializedObject.FindProperty( "touchSize" );
		buttonSize = serializedObject.FindProperty( "buttonSize" );
		customSpacing_X = serializedObject.FindProperty( "customSpacing_X" );
		customSpacing_Y = serializedObject.FindProperty( "customSpacing_Y" );
		/* ---< END SIZE AND PLACEMENT >--- */

		/* -----< STYLES AND OPTIONS >----- */
		showHighlight = serializedObject.FindProperty( "showHighlight" );
		showTension = serializedObject.FindProperty( "showTension" );
		highlightColor = serializedObject.FindProperty( "highlightColor" );
		tensionColorNone = serializedObject.FindProperty( "tensionColorNone" );
		tensionColorFull = serializedObject.FindProperty( "tensionColorFull" );
		tensionFadeInDuration = serializedObject.FindProperty( "tensionFadeInDuration" );
		tensionFadeOutDuration = serializedObject.FindProperty( "tensionFadeOutDuration" );
		/* ---< END STYLES AND OPTIONS >--- */

		/* --------< TOUCH ACTION >-------- */
		tapCountOption = serializedObject.FindProperty( "tapCountOption" );
		tapCountDuration = serializedObject.FindProperty( "tapCountDuration" );
		targetTapCount = serializedObject.FindProperty( "targetTapCount" );
		tapCountEvent = serializedObject.FindProperty( "tapCountEvent" );
		useAnimation = serializedObject.FindProperty( "useAnimation" );
		useFade = serializedObject.FindProperty( "useFade" );
		fadeUntouched = serializedObject.FindProperty( "fadeUntouched" );
		fadeTouched = serializedObject.FindProperty( "fadeTouched" );
		fadeInDuration = serializedObject.FindProperty( "fadeInDuration" );
		fadeOutDuration = serializedObject.FindProperty( "fadeOutDuration" );
		/* ------< END TOUCH ACTION >------ */

		/* ------< SCRIPT REFERENCE >------ */
		referenceOption = serializedObject.FindProperty( "referenceOption" );
		buttonName = serializedObject.FindProperty( "buttonName" );
		onButtonDown = serializedObject.FindProperty( "onButtonDown" );
		onButtonUp = serializedObject.FindProperty( "onButtonUp" );
		/* ----< END SCRIPT REFERENCE >---- */

		/* // ----< ANIMATED SECTIONS >---- \\ */
		AssignedVariables = new AnimBool( EditorPrefs.GetBool( "UUI_Variables" ) );
		SizeAndPlacement = new AnimBool( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) );
		StyleAndOptions = new AnimBool( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) );
		TouchActions = new AnimBool( EditorPrefs.GetBool( "UUI_TouchActions" ) );
		ScriptReference = new AnimBool( EditorPrefs.GetBool( "UUI_ScriptReference" ) );
		/* \\ --< END ANIMATED SECTIONS >-- // */

		/* // ----< ANIMATED VARIABLES >---- \\ */
		UltimateButtonJAVA ult = ( UltimateButtonJAVA ) target;
		highlightOption = new AnimBool( ult.showHighlight );
		tensionOption = new AnimBool( ult.showTension );
		animationOption = new AnimBool( ult.useAnimation );
		fadeOption = new AnimBool( ult.useFade );
		tapOption = new AnimBool( ult.tapCountOption != UltimateButtonJAVA.TapCountOption.NoCount ? true : false );
		/* // --< END ANIMATED VARIABLES >-- \\ */

		SetHighlight( ult );
		SetAnimation( ult );
		SetTensionAccent( ult );

		if( !ult.GetComponent<CanvasGroup>() )
			ult.gameObject.AddComponent<CanvasGroup>();

		if( ult.useFade == true )
			ult.gameObject.GetComponent<CanvasGroup>().alpha = ult.fadeUntouched;
		else
			ult.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
	}

	void SetHighlight ( UltimateButtonJAVA ult )
	{
		if( ult.showHighlight == true )
		{
			if( ult.buttonHighlight != null && ult.buttonHighlight.gameObject.activeInHierarchy == false )
				ult.buttonHighlight.gameObject.SetActive( true );
			
			ult.UpdateHighlight();
		}
		else
		{
			if( ult.buttonHighlight != null && ult.buttonHighlight.gameObject.activeInHierarchy == true )
				ult.buttonHighlight.gameObject.SetActive( false );
		}
	}
	
	void SetTensionAccent ( UltimateButtonJAVA ult )
	{
		if( ult.showTension == true )
		{
			if( ult.tensionAccent == null )
				return;
			
			if( ult.tensionAccent != null && ult.tensionAccent.gameObject.activeInHierarchy == false )
				ult.tensionAccent.gameObject.SetActive( true );

			ult.tensionAccent.color = ult.tensionColorNone;
		}
		else
		{
			if( ult.tensionAccent != null && ult.tensionAccent.gameObject.activeInHierarchy == true )
				ult.tensionAccent.gameObject.SetActive( false );
		}
	}

	void SetAnimation ( UltimateButtonJAVA ult )
	{
		if( ult.useAnimation == true )
		{
			if( ult.buttonAnimator != null )
				if( ult.buttonAnimator.enabled == false )
					ult.buttonAnimator.enabled = true;
		}
		else
		{
			if( ult.buttonAnimator != null )
				if( ult.buttonAnimator.enabled == true )
					ult.buttonAnimator.enabled = false;
		}
	}
}