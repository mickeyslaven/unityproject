/* Written by Kaz Crowe */
/* UltimateJoystick.cs ver. 1.6.7 */
/* ---< Pre 1.6.2 Updates >--- */
	// 1.6.3 - Adding better update positioning options line - 506...also created new script UltimateJoystickUpdater.
/* ---< Version 1.6.2 Updates >--- */
	// 1.6.4 - Fixed throwable option snag on line 205.
	// 1.6.5 - Simple script updates.
	// 1.6.6 - Fixed Draggable option to accept axis constraints.
	// 1.6.7 - Condensed the tap count functionality down to one countdown function.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/*
 * First off, the script is using [ExecuteInEditMode] to be able to show changes in real time.
 * This will not affect anything within a build or play mode. This simply makes the script
 * able to be run while in the Editor in Edit Mode.
*/
[ExecuteInEditMode]
public class UltimateJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	/* ----- > ASSIGNED VARIABLES < ----- */
	public RectTransform joystick, joystickSizeFolder, joystickBase;
	RectTransform baseTrans;
	Vector2 textureCenter = Vector2.zero, defaultPos = Vector2.zero;
	Vector3 joystickCenter = Vector3.zero;
	public Image highlightBase, highlightJoystick;
	public Image tensionAccentUp, tensionAccentDown;
	public Image tensionAccentLeft, tensionAccentRight;

	/* ----- > SIZE AND PLACEMENT < ----- */
	public enum ScalingAxis{ Width, Height }
	public ScalingAxis scalingAxis = ScalingAxis.Height;
	public enum Anchor{ Left, Right }
	public Anchor anchor = Anchor.Left;
	public enum JoystickTouchSize{ Default, Medium, Large, Custom }
	public JoystickTouchSize joystickTouchSize = JoystickTouchSize.Default;
	public float joystickSize = 1.75f, radiusModifier = 4.5f;
	float radius = 1.0f;
	public bool dynamicPositioning = false;
	public float customTouchSize_X = 50.0f, customTouchSize_Y = 75.0f;
	public float customTouchSizePos_X = 0.0f, customTouchSizePos_Y = 0.0f;
	public float customSpacing_X = 5.0f, customSpacing_Y = 20.0f;
	Canvas parentCanvas;
	Vector2 canvasSize = Vector2.zero;

	/* ----- > STYLE AND OPTIONS < ----- */
	public bool touchPad = false, throwable = false, draggable = false;
	public float throwDuration = 0.5f;
	bool isThrowing = false;
	public bool showHighlight = false;
	public Color highlightColor = new Color( 1, 1, 1, 1 );
	public bool showTension = false;
	public Color tensionColorNone = new Color( 1, 1, 1, 1 ), tensionColorFull = new Color( 1, 1, 1, 1 );
	public enum Axis{ Both, X, Y }
	public Axis axis = Axis.Both;
	public enum Boundary{ Circular, Square }
	public Boundary boundary = Boundary.Circular;
	public enum DeadZoneOption{ DoNotUse, OneValue, TwoValues }
	public DeadZoneOption deadZoneOption = DeadZoneOption.DoNotUse;
	public float xDeadZone = 0.1f, yDeadZone = 0.1f;

	/* ----- > TOUCH ACTIONS < ----- */
	public enum TapCountOption{ NoCount, Accumulate, TouchRelease }
	public TapCountOption tapCountOption = TapCountOption.NoCount;
	public float tapCountDuration = 0.5f;
	public UnityEvent tapCountEvent;
	public int targetTapCount = 2;
	float currentTapTime = 0.0f;
	int tapCount = 0;
	public Animator joystickAnimator;
	public bool useAnimation = false, useFade = false;
	int animationID = 0;
	CanvasGroup joystickGroup;
	public float fadeUntouched = 1.0f, fadeTouched = 0.5f;
	public string joystickName;

	/* ----- > JOYSTICK POSITIONS < ----- */
	static Dictionary<string,Vector2> JoystickPositions = new Dictionary<string, Vector2>();


	void Awake ()
	{
		if( joystickName != string.Empty )
			RegisterJoystick( joystickName );
	}

	void Start ()
	{
		if( Application.isPlaying == true )
		{
			// First off, UpdatePositioning of the joystick.
			UpdatePositioning();

			if( showHighlight == true )
				UpdateHighlightColor();

			if( showTension == true )
				TensionAccentReset();

			if( useAnimation == true )
				animationID = Animator.StringToHash( "Touch" );
		}
	}

	// This function is called on the initial touch down.
	public void OnPointerDown ( PointerEventData touchInfo )
	{
		// If the throwable option is selected and isThrowing, then stop the current movement.
		if( throwable == true && isThrowing == true )
			StopCoroutine( "ThrowableMovement" );

		// If dynamicPositioning or touchpad are enabled...
		if( dynamicPositioning == true || touchPad == true )
		{
			// Then move the joystickSizeFolder to the position of the touch.
			joystickSizeFolder.position = touchInfo.position - textureCenter;

			// Set the joystickCenter so that the position can be calculated correctly.
			joystickCenter = touchInfo.position;
		}

		// If the user wants animation to be shown, do that here.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, true );

		// If the user wants the joystick to fade, do that here.
		if( useFade == true && joystickGroup != null )
			joystickGroup.alpha = fadeTouched;

		// If the user is wanting to use any tap count...
		if( tapCountOption != TapCountOption.NoCount )
		{
			// If the user is accumulating taps...
			if( tapCountOption == TapCountOption.Accumulate )
			{
				// If the TapCountdown is not counting down...
				if( currentTapTime <= 0 )
				{
					// Set tapCount to 1 since this is the initial touch and start the TapCountdown.
					tapCount = 1;
					StartCoroutine( "TapCountdown" );
				}
				// Else the TapCountdown is currently counting down, so increase the current tapCount.
				else
					++tapCount;
			}
			// Else the user wants to touch and release, so start the TapCountdown timer.
			else
				StartCoroutine( "TapCountdown" );
		}

		// Call UpdateJoystick with the info from the current PointerEventData.
		UpdateJoystick( touchInfo );
	}

	// This function is called when the user is dragging the joystick.
	public void OnDrag ( PointerEventData touchInfo )
	{
		// Call UpdateJoystick with the info from the current PointerEventData.
		UpdateJoystick( touchInfo );
	}

	// This function is called when the user has released the touch.
	public void OnPointerUp ( PointerEventData touchInfo )
	{
		// If dynamicPositioning, touchpad, or draggable are enabled...
		if( dynamicPositioning == true || touchPad == true || draggable == true )
		{
			// The joystickSizeFolder needs to be reset back to the default position.
			joystickSizeFolder.position = defaultPos;

			// Reset the joystickCenter since the touch has been released.
			joystickCenter = joystickBase.position;
		}

		// If the user has the throwable option enable, begin ThrowableMovement().
		if( throwable == true )
			StartCoroutine( "ThrowableMovement" );
		else
		{
			// Reset the joystick's position back to center.
			joystick.position = joystickCenter;

			// If the user has showHighlight enabled, and the highlightJoystick variable is assigned, reset it too.
			if( showHighlight == true && highlightJoystick != null )
				highlightJoystick.transform.position = joystickCenter;
		}

		// If the user has showTension enabled, then reset the tension if throwable is disabled.
		if( showTension == true && throwable == false )
			TensionAccentReset();

		// If the user has useAnimation enabled, set that here.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, false );
		
		// If the user has useFade enabled, set joystickGroup.alpha to reset back to an untouched state.
		if( useFade == true && joystickGroup != null )
			joystickGroup.alpha = fadeUntouched;
		
		// If the user is wanting to use the TouchAndRelease tapcount, and the tapTime is still above zero...
		if( tapCountOption == TapCountOption.TouchRelease )
		{
			if( currentTapTime > 0 )
				tapCountEvent.Invoke();

			currentTapTime = 0;
		}

		// If the user has set up the joystickName string, then set this joystick's position back to zero.
		if( joystickName != string.Empty && throwable == false )
			SetPosition( joystickName, Vector2.zero );
	}

	// This function updates the Ultimate Joystick according to the current touch.
	void UpdateJoystick ( PointerEventData touchInfo )
	{
		// Create a new Vector2 to equal the vector from the curret touch to the center of joystick.
		Vector2 tempVector = touchInfo.position - ( Vector2 )joystickCenter;

		// If the user wants only one axis, then zero out the opposite value.
		if( axis == Axis.X )
			tempVector.y = 0;
		else if( axis == Axis.Y )
			tempVector.x = 0;

		// If the user wants a circular boundary for the joystick, then clamp the magnitude by the radius.
		if( boundary == Boundary.Circular )
			tempVector = Vector2.ClampMagnitude( tempVector, radius );
		// Else the user wants a square boundry, so clamp X and Y individually.
		else if( boundary == Boundary.Square )
		{
			tempVector.x = Mathf.Clamp( tempVector.x,  -radius,  radius );
			tempVector.y = Mathf.Clamp( tempVector.y,  -radius,  radius );
		}

		// Apply the tempVector to the joystick's position.
		joystick.transform.position = ( Vector2 )joystickCenter + tempVector;

		// If the user is showing highlight and the highlightJoystick is assigned, then move the highlight to match the joystick's position.
		if( showHighlight == true && highlightJoystick != null )
			highlightJoystick.transform.position = joystick.transform.position;

		// If the user has showTension enabled, then display the Tension.
		if( showTension == true )
			TensionAccentDisplay();

		// If the user wants to drag the joystick along with the touch...
		if( draggable == true )
		{
			Vector3 currentTouchPosition = touchInfo.position;
			if( axis != Axis.Both )
			{
				if( axis == Axis.X )
					currentTouchPosition.y = joystickCenter.y;
				else
					currentTouchPosition.x = joystickCenter.x;
			}
			// Then find the distance that the touch is from the center of the joystick.
			float touchDistance = Vector3.Distance( joystickCenter, currentTouchPosition );

			// If the touchDistance is greater than the set radius...
			if( touchDistance >= radius )
			{
				// Figure out the current position of the joystick.
				Vector2 joystickPosition = ( joystick.position - joystickCenter ) / radius;

				// Move the joystickSizeFolder in the direction that the joystick is, multiplied by the difference in distance of the max radius.
				joystickSizeFolder.position += new Vector3( joystickPosition.x, joystickPosition.y, 0 ) * ( touchDistance - radius );

				// Reconfigure the joystickCenter since the joystick has now moved it position.
				joystickCenter = joystickBase.position;
			}
		}

		// If the user has set up the joystickName string, then set this joystick's position.
		if( joystickName != string.Empty )
			SetPosition( joystickName, JoystickPosition );
	}

	// This function will configure the position of an image based on the size and custom spacing selected.
	Vector2 ConfigureImagePosition ( Vector2 textureSize, Vector2 customSpacing )
	{
		// First, fix the customSpacing to be a value between 0.0f and 1.0f.
		Vector2 fixedCustomSpacing = customSpacing / 100;

		// Then configure position spacers according to canvasSize, the fixed spacing and texture size.
		float positionSpacerX = canvasSize.x * fixedCustomSpacing.x - ( textureSize.x * fixedCustomSpacing.x );
		float positionSpacerY = canvasSize.y * fixedCustomSpacing.y - ( textureSize.y * fixedCustomSpacing.y );

		// Create a temporary Vector2 to modify and return.
		Vector2 tempVector;

		// If it's left, simply apply the positionxSpacerX, else calculate out from the right side and apply the positionSpaceX.
		tempVector.x = anchor == Anchor.Left ? positionSpacerX : ( canvasSize.x - textureSize.x ) - positionSpacerX;

		// Apply the positionSpacerY variable.
		tempVector.y = positionSpacerY;

		// Return the updated temporary Vector.
		return tempVector;
	}

	// This function is called only when showTension is true, and only when the joystick is moving.
	void TensionAccentDisplay ()
	{
		// Create a temporary Vector2 for the joystick current position.
		Vector2 tension = ( joystick.position - joystickCenter ) / radius;

		// If the joystick is to the right...
		if( tension.x > 0 )
		{
			// Then lerp the color according to tension's X position.
			tensionAccentRight.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			
			// If the opposite tension is not tensionColorNone, the make it so.
			if( tensionAccentLeft.color != tensionColorNone )
				tensionAccentLeft.color = tensionColorNone;
		}
		// Else the joystick is to the left...
		else
		{
			// Mathf.Abs gives a positive number to lerp with.
			tension.x = Mathf.Abs( tension.x );

			// Repeat above steps...
			tensionAccentLeft.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			if( tensionAccentRight.color != tensionColorNone )
				tensionAccentRight.color = tensionColorNone;
		}

		// If the joystick is up...
		if( tension.y > 0 )
		{
			// Then lerp the color according to tension's Y position.
			tensionAccentUp.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// If the opposite tension is not tensionColorNone, the make it so.
			if( tensionAccentDown.color != tensionColorNone )
				tensionAccentDown.color = tensionColorNone;
		}
		// Else the joystick is down...
		else
		{
			// Mathf.Abs gives a positive number to lerp with.
			tension.y = Mathf.Abs( tension.y );
			tensionAccentDown.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// Repeat above steps...
			if( tensionAccentUp.color != tensionColorNone )
				tensionAccentUp.color = tensionColorNone;
		}
	}

	// This function resets the tension image's colors back to default.
	void TensionAccentReset ()
	{
		tensionAccentUp.color = tensionColorNone;
		tensionAccentDown.color = tensionColorNone;
		tensionAccentLeft.color = tensionColorNone;
		tensionAccentRight.color = tensionColorNone;
	}

	IEnumerator TapCountdown ()
	{
		currentTapTime = tapCountDuration;
		while( currentTapTime > 0 )
		{
			currentTapTime -= Time.deltaTime;

			if( tapCountOption == TapCountOption.Accumulate )
			{
				if( currentTapTime > 0 && tapCount >= targetTapCount )
				{
					currentTapTime = 0;
					tapCountEvent.Invoke();
				}
			}

			yield return null;
		}
	}

	// This function is for returning the joystick back to center of a set amount of time.
	IEnumerator ThrowableMovement ()
	{
		// Start throwing...
		isThrowing = true;

		// Fix the throwDuration into a speed.
		float throwSpeed = 1.0f / throwDuration;

		// Store the position of where the joystick is currently.
		Vector3 startJoyPos = joystick.position;

		for( float i = 0.0f; i < 1.0f; i += Time.deltaTime * throwSpeed )
		{
			// Lerp the joystick's position from where this coroutine started to the center.
			joystick.position = Vector3.Lerp( startJoyPos, joystickCenter, i );

			// If the user is using highlight and the highlightJoystick is assigned, move that as well.
			if( showHighlight == true && highlightJoystick != null )
				highlightJoystick.transform.position = joystick.position;

			// If the user has showTension enabled, then display the tension as the joystick moves.
			if( showTension == true )
				TensionAccentDisplay();

			// If the user has set up the joystickName string, then set this joystick's position.
			if( joystickName != string.Empty )
				SetPosition( joystickName, JoystickPosition );

			yield return null;
		}

		isThrowing = false;

		// Finalize the joystick's position.
		joystick.position = joystickCenter;

		// If the user is using highlight and the highlightJoystick is assigned, finalize that as well.
		if( showHighlight == true && highlightJoystick != null )
			highlightJoystick.transform.position = joystick.position;

		// Here at the end, reset the tension.
		if( showTension == true )
			TensionAccentReset();

		// If the user has set up the joystickName string, then set this joystick's position back to zero.
		if( joystickName != string.Empty )
			SetPosition( joystickName, Vector2.zero );
	}

	// This function is used only to find the canvas parent if its not the root object.
	Canvas GetParentCanvas ()
	{
		Transform parent = transform.parent;
		while( parent != null )
		{ 
			if( parent.transform.GetComponent<Canvas>() )
				return parent.transform.GetComponent<Canvas>();

			parent = parent.transform.parent;
		}
		return null;
	}

	CanvasGroup GetCanvasGroup ()
	{
		if( GetComponent<CanvasGroup>() )
			return GetComponent<CanvasGroup>();
		else
		{
			gameObject.AddComponent<CanvasGroup>();
			return GetComponent<CanvasGroup>();
		}
	}

	Vector2 JoystickPositionDeadZone
	{
		get
		{
			Vector2 tempVec = ( joystick.position - joystickCenter ) / radius;
			
			// If the X value is to the LEFT, then update the deadZone vector2.x to -1 if it is.
			if( tempVec.x < -xDeadZone )
				tempVec.x = -1;
			// Else check if it is to the RIGHT, then update the deadZone vector2.x to 1 if it is.
			else if( tempVec.x > xDeadZone )
				tempVec.x = 1;
			// Else it is not past the deadZone values, so set it to zero.
			else
				tempVec.x = 0;
			
			// If the Y value is DOWN and then update the deadZone vector2.y to -1 if it is.
			if( tempVec.y < -yDeadZone )
				tempVec.y = -1;
			// Else check if it is UP, then update the deadZone vector2.y to 1 if it is.
			else if( tempVec.y > yDeadZone )
				tempVec.y = 1;
			// Else it is not past the deadZone values, so set it to zero.
			else
				tempVec.y = 0;
			
			return tempVec;
		}
	}

	void RegisterJoystick ( string joystickName )
	{
		if( !JoystickPositions.ContainsKey( joystickName ) )
			JoystickPositions.Add( joystickName, Vector2.zero );
	}

	void SetPosition ( string joystickName, Vector2 joystickPosition )
	{
		if( !JoystickPositions.ContainsKey( joystickName ) )
			JoystickPositions.Add( joystickName, Vector2.zero );
		
		JoystickPositions[ joystickName ] = joystickPosition;
	}

	#if UNITY_EDITOR
	void Update ()
	{
		// Keep the joystick updated while the game is not being played.
		if( Application.isPlaying == false )
			UpdatePositioning();
	}
	#endif

	/* --------------------------------------------- *** PUBLIC FUNCTIONS FOR THE USER *** --------------------------------------------- */
	/// <summary>
	/// Updates the size and placement of the Ultimate Joystick. Useful for screen rotations or changing of screen size.
	/// </summary>
	public void UpdatePositioning ()
	{
		// If the root contains a Canvas component, then store it for refernece.
		if( transform.root.GetComponent<Canvas>() )
			parentCanvas = transform.root.GetComponent<Canvas>();
		// Else the root doesn't have a Canvas, so search through all parents to find one.
		else
			parentCanvas = GetParentCanvas();
		
		// The canvas size is determined by the size of the parentCanvas transform * it's local scale to allow different CanvasScaler options.
		canvasSize = parentCanvas.GetComponent<RectTransform>().sizeDelta * parentCanvas.transform.localScale.x;
		
		float referenceSize = scalingAxis == ScalingAxis.Height ? canvasSize.y : canvasSize.x;
		
		// Configure a size for the image based on the Canvas's size and scale.
		float textureSize = referenceSize * ( joystickSize / 10 ) / parentCanvas.transform.localScale.x;
		
		// If baseTrans is null, store this object's RectTrans so that it can be positioned.
		if( baseTrans == null )
			baseTrans = GetComponent<RectTransform>();
		
		// Get a position for the joystick based on the position variables.
		Vector2 imagePosition = ConfigureImagePosition( new Vector2( textureSize * parentCanvas.transform.localScale.x, textureSize * parentCanvas.transform.localScale.x ), new Vector2( customSpacing_X, customSpacing_Y ) );
		
		// If the user wants a custom touch size...
		if( joystickTouchSize == JoystickTouchSize.Custom )
		{
			// Fix the custom size variables.
			float fixedFBPX = customTouchSize_X / 100;
			float fixedFBPY = customTouchSize_Y / 100;
			
			// Depending on the joystickTouchSize options, configure the size.
			baseTrans.sizeDelta = new Vector2( canvasSize.x * fixedFBPX, canvasSize.y * fixedFBPY ) / parentCanvas.transform.localScale.x;
			
			// Send the size and custom positioning to the ConfigureImagePosition function to get the exact position.
			Vector2 imagePos = ConfigureImagePosition( baseTrans.sizeDelta * parentCanvas.transform.localScale.x, new Vector2( customTouchSizePos_X, customTouchSizePos_Y ) );

			// Apply the new position.
			baseTrans.position = imagePos;
		}
		else
		{
			// Temporary float to store a modifier for the touch area size.
			float fixedTouchSize = joystickTouchSize == JoystickTouchSize.Large ? 2.0f : joystickTouchSize == JoystickTouchSize.Medium ? 1.51f : 1.01f;
			
			// Temporary Vector2 to store the default size of the joystick.
			Vector2 tempVector = new Vector2( textureSize, textureSize );
			
			// Apply the joystick size multiplied by the fixedTouchSize.
			baseTrans.sizeDelta = tempVector * fixedTouchSize;
			
			// Apply the imagePosition modified with the difference of the sizeDelta divided by 2, multiplied by the scale of the parent canvas.
			baseTrans.position = imagePosition - ( ( baseTrans.sizeDelta - tempVector ) / 2 ) * parentCanvas.transform.localScale.x;
		}

		// If the options dictate that the default position needs to be stored...
		if( dynamicPositioning == true || touchPad == true || draggable == true )
		{
			// Set the texture center so that the joystick can move to the touch position correctly.
			textureCenter = new Vector2( textureSize / 2, textureSize / 2 ) * parentCanvas.transform.localScale.x;
			
			// Also need to store the default position so that it can return after the touch has been lifted.
			defaultPos = imagePosition;
		}

		// Store the pivot of the baseTrans so that the position will be correct no matter what the user has set for pivot.
		Vector2 pivotSpacer = new Vector2( baseTrans.sizeDelta.x * baseTrans.pivot.x * parentCanvas.transform.localScale.x, baseTrans.sizeDelta.y * baseTrans.pivot.y * parentCanvas.transform.localScale.y );
		
		// Apply the size and position to the joystickSizeFolder.
		joystickSizeFolder.sizeDelta = new Vector2( textureSize, textureSize );
		joystickSizeFolder.position = imagePosition + pivotSpacer;
		
		// Configure the size of the Ultimate Joystick's radius.
		radius = ( joystickSizeFolder.sizeDelta.x * parentCanvas.transform.localScale.x ) * ( radiusModifier / 10 );
		
		// Store the joystick's center so that JoystickPosition can be configured correctly.
		joystickCenter = joystick.position;

		// If the user wants to fade, and the joystickGroup is unassigned, find the CanvasGroup.
		if( useFade == true && joystickGroup == null )
			joystickGroup = GetCanvasGroup();
	}

	/// <summary>
	/// Returns the position of the Ultimate Joystick in a Vector2 Variable. X = Horizontal, Y = Vertical.
	/// </summary>
	/// <value>The Ultimate Joystick's Position.</value>
	public Vector2 JoystickPosition
	{
		get
		{
			if( deadZoneOption != DeadZoneOption.DoNotUse )
				return JoystickPositionDeadZone;

			return ( joystick.position - joystickCenter ) / radius;
		}
	}

	/// <summary>
	/// Returns the distance of the Ultimate Joystick from center.
	/// </summary>
	/// <value>The Ultimate Joystick's Distance.</value>
	public float JoystickDistance
	{
		get{ return Vector3.Distance( joystick.position, joystickCenter ) / radius; }
	}

	/// <summary>
	/// Resets the Ultimate Joystick if it becomes stuck or needs to be released for some other reason.
	/// </summary>
	public void ResetUltimateJoystick ()
	{
		OnPointerUp( null );
	}

	/// <summary>
	/// If showHighlight is true, the this function will update the color of the highlights attached to the Ultimate Joystick.
	/// </summary>
	public void UpdateHighlightColor ()
	{
		if( showHighlight == false )
			return;
		
		// Check if each variable is assigned so there is not a null reference exception when applying color.
		if( highlightBase != null )
			highlightBase.color = highlightColor;
		if( highlightJoystick != null )
			highlightJoystick.color = highlightColor;
	}

	/// <summary>
	/// Gets the joystick position.
	/// </summary>
	/// <returns>The joystick position.</returns>
	/// <param name="joystickName">Joystick name.</param>
	static public Vector2 GetPosition ( string joystickName )
	{
		if( !JoystickPositions.ContainsKey( joystickName ) )
		{
			Debug.Log( "No Ultimate Joystick has been registered with the name: " + joystickName + "." );
			JoystickPositions.Add( joystickName, Vector2.zero );
		}
		
		return JoystickPositions[ joystickName ];
	}
	/* ------------------------------------------- *** END PUBLIC FUNCTIONS FOR THE USER *** ------------------------------------------- */
}