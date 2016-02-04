/* Written by Kaz Crowe */
/* UltimateButtonJAVA.js ver. 1.5.2 */
#pragma strict
import UnityEngine.UI;
import UnityEngine.Events;
import UnityEngine.EventSystems;
import System.Collections.Generic;

/* 
 * First off, we are using [ExecuteInEditMode] to be able to show changes in real time.
 * This will not affect anything within a build or play mode. This simply makes the script
 * able to be run while in the Editor in Edit Mode.
*/
@script ExecuteInEditMode()
public class UltimateButtonJAVA extends MonoBehaviour implements IPointerDownHandler, IPointerUpHandler
{
	/* ----- > ASSIGNED VARIABLES < ----- */
	var sizeFolder : RectTransform;
	var buttonHighlight : Image;
	var tensionAccent : Image;
	private var baseTrans : RectTransform;
	var buttonAnimator : Animator;
	
	/* ----- > SIZE AND PLACEMENT < ----- */
	enum ScalingAxis{ Width, Height }
	var scalingAxis : ScalingAxis = ScalingAxis.Height;
	enum Anchor{ Left, Right }
	var anchor : Anchor = Anchor.Right;
	enum TouchSize{ Default, Medium, Large }
	var touchSize : TouchSize = TouchSize.Default;
	var buttonSize : float = 1.75;
	var customSpacing_X : float = 5;
	var customSpacing_Y : float = 20;
	private var canvasGroup : CanvasGroup;
	private var parentCanvas : Canvas;
	private var canvasSize : Vector2 = Vector2.zero;
	
	/* ----- > STYLE AND OPTIONS < ----- */
	private var isHovering : boolean = false;
	private var buttonCenter : Vector2 = Vector2.zero;
	private var buttonImageRadius : float = 0.0;
	var showHighlight : boolean = false;
	var showTension : boolean = false;
	var highlightColor : Color = Color.white;
	var tensionColorNone : Color = Color.white;
	var tensionColorFull : Color = Color.white;
	var tensionFadeInDuration : float = 0.1;
	var tensionFadeOutDuration : float = 0.1;
	private var tensionFadeInSpeed : float = 1.0;
	private var tensionFadeOutSpeed : float = 1.0;
	
	/* ----- > TOUCH ACTIONS < ----- */
	enum TapCountOption{ NoCount, Accumulate, TouchRelease }
	var tapCountOption : TapCountOption = TapCountOption.NoCount;
	var tapCountDuration : float = 0.5;
	var tapCountEvent : UnityEvent;
	var targetTapCount : int = 2;
	private var currentTapTime : float = 0.0;
	var tapCount : int = 0;
	private var animatorState : int = 0;
	var useAnimation : boolean = false;
	var useFade : boolean = false;
	var fadeUntouched : float = 1.0f;
	var fadeTouched : float = 0.5f;
	var fadeInDuration : float = 1.0;
	var fadeOutDuration : float = 1.0;
	private var fadeInSpeed : float = 1.0;
	private var fadeOutSpeed : float = 1.0;
	
	/* ----- > SCRIPT REFERENCE < ----- */
	enum ReferenceOption { UnityEvents, GetButtonStates }
	var referenceOption : ReferenceOption = ReferenceOption.UnityEvents;
	var onButtonDown : UnityEngine.Events.UnityEvent;
	var onButtonUp : UnityEngine.Events.UnityEvent;
	private class ButtonStates
	{
		var getButtonDown : boolean = false;
		var getButtonUp : boolean = false;
		var getButton : boolean = false;
		var tapCount : boolean = false;
	}
	private static var UltimateButtons : Dictionary.<String, ButtonStates> = new Dictionary.<String, ButtonStates>();
	private var buttonStates : ButtonStates = new ButtonStates();
	var buttonName : String;
	
	
	function Awake ()
	{
		// If the application is being run, then send this button name and states to our static dictionary for reference.
		if( Application.isPlaying == true )
			CreateButton( buttonName, buttonStates );
	}
	
	function Start ()
	{
		//  If the application is running...
		if( Application.isPlaying == true )
		{
			// Update the size and positioning of the button.
			UpdatePositioning();

			// If the user is wanting to show the highlight color of the button, update the highlight image.
			if( showHighlight == true )
				UpdateHighlight();

			// If the user is using tension fade options...
			if( showTension == true )
			{
				// Configure the speed variables for the fade.
				tensionFadeInSpeed = 1.0f / tensionFadeInDuration;
				tensionFadeOutSpeed = 1.0f / tensionFadeOutDuration;
			}

			// If the user is wanting to show animation, then reference the HashID of the animator parameter.
			if( useAnimation == true )
				animatorState = Animator.StringToHash( "Touch" );

			// If the user has useFade enabled...
			if( useFade == true )
			{
				// Get the CanvasGroup component for Enable/Disable options.
				canvasGroup = GetComponent.<CanvasGroup>();

				// If the canvasGroup is null, then this object needs a canvasGroup.
				if( canvasGroup == null )
				{
					// Add the CanvasGroup component.
					gameObject.AddComponent( typeof( CanvasGroup ) );

					// Re-reference the canvasGroup variable.
					canvasGroup = GetComponent.<CanvasGroup>();
				}

				// Configure the fade speeds.
				fadeInSpeed = 1.0 / fadeInDuration;
				fadeOutSpeed = 1.0 / fadeOutDuration;

				// And apply the default fade for the button.
				canvasGroup.alpha = fadeUntouched;
			}
		}
	}
	
	// This function is called when the user has touched down.
	function OnPointerDown ( touchInfo : PointerEventData )
	{
		// Set the buttons state to true.
		buttonStates.getButton = true;

		// If the user wants to reference the button states, then start the button down coroutine.
		if( referenceOption == ReferenceOption.GetButtonStates )
			StartCoroutine( "ButtonDownDelay" );
		// Else the user wants to use Unity Events, so call the function.
		else
			onButtonDown.Invoke();

		// If the user wants to show animations on Touch, set the 'Touch' parameter to true.
		if( useAnimation == true )
			buttonAnimator.SetBool( animatorState, true );

		// If the user is wanting to count taps on this button...
		if( tapCountOption != TapCountOption.NoCount )
		{
			// If the user is wanting to accumulate taps...
			if( tapCountOption == TapCountOption.Accumulate )
			{
				// If the timer is not currently counting down...
				if( currentTapTime <= 0 )
				{
					// Then start the count down timer, and set the current tapCount to 1.
					StartCoroutine( "TapCountdown" );
					tapCount = 1;
				}
				// Else the timer is already running, so increase tapCount by 1.
				else
					++tapCount;

				// If the timere is still going, and the target tap count has been reached...
				if( currentTapTime > 0 && tapCount >= targetTapCount )
				{
					// Stop the timer by setting the tap time to zero, start the one frame delay for the static reference of tap count, and call the tapCountEvent.
					currentTapTime = 0;
					StartCoroutine( "TapCountDelay" );
					tapCountEvent.Invoke();
				}
			}
			// Else the user is wanting to send tap counts by way of a quick touch and release...
			else
			{
				// If the timer is not currently counting down, then start the coroutine.
				if( currentTapTime <= 0 )
					StartCoroutine( "TapCountdown" );
				else
					currentTapTime = tapCountDuration;
			}
		}

		// If the user wants the button to fade, do that here.
		if( useFade == true && canvasGroup != null )
		{
			StartCoroutine( "ButtonFade" );
		}
		
		if( showTension == true )
			StartCoroutine( "TensionAccentFade" );

		isHovering = true;
	}
	
	// This function is called when the user is dragging the input.
	function OnDrag ( touchInfo : PointerEventData )
	{
		// Configure the current distance from the center of the button to the touch position.
		var maxDistance : float = Vector2.Distance( touchInfo.position, buttonCenter );

		// The the current distance is farther than the buttonImageRadius value, and it is hovering...
		if( maxDistance > buttonImageRadius && isHovering == true )
		{
			// Then the touch is no longer hovering over the image, and the button is no longer down.
			isHovering = false;
			buttonStates.getButton = false;

			// If the user is wanting to show animation, update the animator.
			if( useAnimation == true )
				buttonAnimator.SetBool( animatorState, false );
		}
		// Else if the current distance is smaller than the buttonImageRadius value, and the touch is not currently hovering...
		else if( maxDistance < buttonImageRadius && isHovering == false )
		{
			// Then the touch is now hovering and the button is now down.
			isHovering = true;
			buttonStates.getButton = true;

			// If the user is wanting to show tension, start the corresponding coroutine.
			if( showTension == true )
				StartCoroutine( "TensionAccentFade" );

			// If the user is wanting to show animation, then update the animator.
			if( useAnimation == true )
				buttonAnimator.SetBool( animatorState, true );

			// If the user is wanting to show fade on the button
			if( useFade == true && canvasGroup != null )
				StartCoroutine( "ButtonFade" );
		}
	}
	
	// This function is called when the user has let go of the input.
	function OnPointerUp ( touchInfo : PointerEventData )
	{
		// Set the buttons state to false.
		buttonStates.getButton = false;

		// If the input is not currently hovering over the button, then return.
		if( isHovering == false )
			return;

		// If the user wants to reference the button states, then start the ButtonUpDelay coroutine.
		if( referenceOption == ReferenceOption.GetButtonStates )
			StartCoroutine( "ButtonUpDelay" );
		// Else the user wants to use Unity Events, so call the event.
		else
			onButtonUp.Invoke();

		// If the user is wanting to count the amount of taps by Touch and Release...
		if( tapCountOption == TapCountOption.TouchRelease )
		{
			// Then check the current tap time to see if the release is happening within time.
			if( currentTapTime > 0 )
			{
				// Call the button events.
				StartCoroutine( "TapCountDelay" );
				tapCountEvent.Invoke();
			}

			// Set the tap time to 0 to reset the timer.
			currentTapTime = 0;
		}
		
		// If the user is wanting to show animations, then set the animator.
		if( useAnimation == true )
			buttonAnimator.SetBool( animatorState, false );

		// Set isHovering to false since the touch input has been released.
		isHovering = false;
	}
	
	// This function is used for counting down for the TapCount options.
	function TapCountdown ()
	{
		currentTapTime = tapCountDuration;
		while( currentTapTime > 0 )
		{
			currentTapTime -= Time.deltaTime;
			yield;
		}
	}
	
	#if UNITY_EDITOR
	function Update ()
	{
		// The button will be updated constantly when the game is not being run in Unity.
		if( Application.isPlaying == false )
			UpdatePositioning();
	}
	#endif
	
	/* ////// ----------------------< PUBLIC FUNCTIONS >---------------------- \\\\\\ */
	/// <summary> Updates the size and placement of the Ultimate Button. Useful for when applying any options changed at runtime. </summary>
	function UpdatePositioning ()
	{
		// If the root contains a Canvas component, then store it for refernece.
		if( transform.root.GetComponent.<Canvas>() )
			parentCanvas = transform.root.GetComponent.<Canvas>();
		// Else the root doesn't have a Canvas, so search through all parents to find one.
		else
			parentCanvas = GetParentCanvas();

		// If the parentCanvas is still null, show error and return.
		if( parentCanvas == null )
		{
			Debug.LogWarning( "Ultimate Button: The Canvas that this Ultimate Button is not a child of a Canvas. Please move the Ultimate Button to a Canvas." );
			return;
		}

		// The canvas size is determined by the size of the parentCanvas transform * it's local scale to allow different CanvasScaler options.
		canvasSize = parentCanvas.GetComponent.<RectTransform>().sizeDelta * parentCanvas.transform.localScale.x;

		// Find the reference size for the axis to size the button by.
		var referenceSize : float = scalingAxis == ScalingAxis.Height ? canvasSize.y : canvasSize.x;

		// Configure a size for the image based on the Canvas's size and scale.
		var textureSize : float = referenceSize * ( buttonSize / 10 ) / parentCanvas.transform.localScale.x;

		// If baseTrans is null, store this object's RectTrans so that it can be positioned.
		if( baseTrans == null )
			baseTrans = GetComponent.<RectTransform>();

		// Get a position for the button based on the position variables.
		var imagePosition : Vector2 = ConfigureImagePosition( new Vector2( textureSize * parentCanvas.transform.localScale.x, textureSize * parentCanvas.transform.localScale.x ), new Vector2( customSpacing_X, customSpacing_Y ) );

		// Temporary float to store a modifier for the touch area size.
		var fixedTouchSize : float = touchSize == TouchSize.Large ? 2.0 : touchSize == TouchSize.Medium ? 1.51 : 1.01;

		// Temporary Vector2 to store the default size of the button.
		var tempVector : Vector2 = new Vector2( textureSize, textureSize );

		// Apply the button size multiplied by the fixedTouchSize.
		baseTrans.sizeDelta = tempVector * fixedTouchSize;

		// Apply the imagePosition modified with the difference of the sizeDelta divided by 2, multiplied by the scale of the parent canvas.
		baseTrans.position = imagePosition - ( ( baseTrans.sizeDelta - tempVector ) / 2 ) * parentCanvas.transform.localScale.x;

		// Store the pivot of the baseTrans so that the position will be correct no matter what the user has set for pivot.
		var pivotSpacer : Vector2 = new Vector2( baseTrans.sizeDelta.x * baseTrans.pivot.x * parentCanvas.transform.localScale.x, baseTrans.sizeDelta.y * baseTrans.pivot.y * parentCanvas.transform.localScale.y );

		if( sizeFolder != null )
		{
			// Apply the size and position to the sizeFolder.
			sizeFolder.sizeDelta = new Vector2( textureSize, textureSize );
			sizeFolder.position = imagePosition + pivotSpacer;

			buttonCenter = sizeFolder.position;
			buttonCenter += new Vector2( baseTrans.sizeDelta.x * parentCanvas.transform.localScale.x, baseTrans.sizeDelta.y * parentCanvas.transform.localScale.y ) / 2;
			buttonImageRadius = ( baseTrans.sizeDelta.x * parentCanvas.transform.localScale.x ) / 2;
		}
		else
			Debug.LogError( "Ultimate Button: The SizeFolder variable needs to be assigned in the Assigned Variables section in order to size the Ultimate Button correctly. Please assign this variable before continuing." );

		// If the user wants to fade, and the canvasGroup is unassigned, find the CanvasGroup.
		if( useFade == true && canvasGroup == null )
			canvasGroup = GetCanvasGroup();
	}
	
	/// <summary> If showHighlight is true, the this function will update the color of the highlight image attached to the Ultimate Button. </summary>
	function UpdateHighlight ()
	{
		if( showHighlight == false )
			return;

		// Check if each variable is assigned so there is not a null reference exception when applying color.
		if( buttonHighlight != null )
			buttonHighlight.color = highlightColor;
	}
	/* \\\\\\ --------------------< END PUBLIC FUNCTIONS >-------------------- ////// */
	
	/* ////// ---------------------< PRIVATE FUNCTIONS >---------------------- \\\\\\ */
	function ButtonDownDelay ()
	{
		buttonStates.getButtonDown = true;
		yield new WaitForEndOfFrame();
		buttonStates.getButtonDown = false;
	}

	function ButtonUpDelay ()
	{
		buttonStates.getButtonUp = true;
		yield new WaitForEndOfFrame();
		buttonStates.getButtonUp = false;
	}

	function TapCountDelay ()
	{
		buttonStates.tapCount = true;
		yield new WaitForEndOfFrame();
		buttonStates.tapCount = false;
	}

	function CreateButton ( btnName : String, btnState : ButtonStates )
	{
		if( !UltimateButtons.ContainsKey( btnName ) )
			UltimateButtons.Add( btnName, btnState );

		UltimateButtons[ btnName ] = btnState;
	}

	function TensionAccentFade ()
	{
		var currentColor : Color = tensionAccent.color;
		// for loop for fade in with conditional for buttonstate true
		for( var fadeIn : float = 0.0; fadeIn < 1.0 && buttonStates.getButton == true; fadeIn += Time.deltaTime * tensionFadeInSpeed )
		{
			tensionAccent.color = Color.Lerp( currentColor, tensionColorFull, fadeIn );
			if( float.IsInfinity( tensionFadeInSpeed ) )
				tensionAccent.color = tensionColorFull;
			yield;
		}

		// while loop for while buttonstate is true
		while( buttonStates.getButton == true )
			yield;

		currentColor = tensionAccent.color;
		// for loop for when the button state is false
		for( var fadeOut : float = 0.0; fadeOut < 1.0 && buttonStates.getButton == false; fadeOut += Time.deltaTime * tensionFadeOutSpeed )
		{
			tensionAccent.color = Color.Lerp( currentColor, tensionColorNone, fadeOut );
			if( float.IsInfinity( tensionFadeOutSpeed ) )
				tensionAccent.color = tensionColorNone;
			yield;
		}
	}

	function ButtonFade ()
	{
		var currentFade : float = canvasGroup.alpha;
		// for loop for fade in with conditional for buttonstate true
		for( var fadeIn : float = 0.0; fadeIn < 1.0 && buttonStates.getButton == true; fadeIn += Time.deltaTime * fadeInSpeed )
		{
			canvasGroup.alpha = Mathf.Lerp( currentFade, fadeTouched, fadeIn );
			if( float.IsInfinity( fadeInSpeed ) )
				canvasGroup.alpha = fadeTouched;
			yield;
		}

		// while loop for while buttonstate is true
		while( buttonStates.getButton == true )
			yield;

		currentFade = canvasGroup.alpha;
		// for loop for when the button state is false
		for( var fadeOut : float = 0.0; fadeOut < 1.0 && buttonStates.getButton == false; fadeOut += Time.deltaTime * fadeOutSpeed )
		{
			canvasGroup.alpha = Mathf.Lerp( currentFade, fadeUntouched, fadeOut );
			if( float.IsInfinity( fadeOutSpeed ) )
				canvasGroup.alpha = fadeUntouched;
			yield;
		}
	}
	/* \\\\\\ -------------------< END PRIVATE FUNCTIONS >-------------------- ////// */
	
	/* ////// ----------------------< STATIC FUNCTIONS >---------------------- \\\\\\ */
	static function GetButtonDown ( btnName : String ) : boolean
	{
		if( UltimateButtons.ContainsKey( btnName ) )
			return UltimateButtons[ btnName ].getButtonDown;

		Debug.LogError( "ArgumentException: Ultimate Button: '" + btnName.ToString() + "' is not present in the scene.\nPlease makes sure the reference is correct, and also make sure you have actually created the targeted button." );
		return false;
	}

	static function GetButtonUp ( btnName : String ) : boolean
	{
		if( UltimateButtons.ContainsKey( btnName ) )
			return UltimateButtons[ btnName ].getButtonUp;

		Debug.LogError( "ArgumentException: Ultimate Button: '" + btnName.ToString() + "' is not present in the scene.\nPlease makes sure the reference is correct, and also make sure you have actually created the targeted button." );
		return false;
	}

	static function GetButton ( btnName : String ) : boolean
	{
		if( UltimateButtons.ContainsKey( btnName ) )
			return UltimateButtons[ btnName ].getButton;

		Debug.LogError( "ArgumentException: Ultimate Button: '" + btnName.ToString() + "' is not present in the scene.\nPlease makes sure the reference is correct, and also make sure you have actually created the targeted button." );
		return false;
	}

	static function GetTapCount ( btnName : String ) : boolean
	{
		if( UltimateButtons.ContainsKey( btnName ) )
			return UltimateButtons[ btnName ].tapCount;

		Debug.LogError( "ArgumentException: Ultimate Button: '" + btnName.ToString() + "' is not present in the scene.\nPlease makes sure the reference is correct, and also make sure you have actually created the targeted button." );
		return false;
	}
	/* \\\\\\ --------------------< END STATIC FUNCTIONS >-------------------- ////// */
	
	/* ////// ----------------------< RETURN FUNCTIONS >---------------------- \\\\\\ */
	// This function is used only to find the canvas parent if its not the root object.
	private function GetParentCanvas () : Canvas
	{
		var parent : Transform = transform.parent;
		while( parent != null )
		{
			if( parent.transform.GetComponent.<Canvas>() )
				return parent.transform.GetComponent.<Canvas>();

			parent = parent.transform.parent;
		}
		return null;
	}

	private function GetCanvasGroup () : CanvasGroup
	{
		if( GetComponent.<CanvasGroup>() )
			return GetComponent.<CanvasGroup>();
		else
		{
			gameObject.AddComponent.<CanvasGroup>();
			return GetComponent.<CanvasGroup>();
		}
	}

	// This function will configure the position of an image based on the size and custom spacing selected.
	private function ConfigureImagePosition ( textureSize : Vector2, customSpacing : Vector2 ) : Vector2
	{
		// First, fix the customSpacing to be a value between 0.0f and 1.0f.
		var fixedCustomSpacing : Vector2 = customSpacing / 100;

		// Then configure position spacers according to canvasSize, the fixed spacing and texture size.
		var positionSpacerX : float = canvasSize.x * fixedCustomSpacing.x - ( textureSize.x * fixedCustomSpacing.x );
		var positionSpacerY : float = canvasSize.y * fixedCustomSpacing.y - ( textureSize.y * fixedCustomSpacing.y );

		// Create a temporary Vector2 to modify and return.
		var tempVector : Vector2;

		// If it's left, simply apply the positionxSpacerX, else calculate out from the right side and apply the positionSpaceX.
		tempVector.x = anchor == Anchor.Left ? positionSpacerX : ( canvasSize.x - textureSize.x ) - positionSpacerX;

		// Apply the positionSpacerY variable.
		tempVector.y = positionSpacerY;

		// Return the updated temporary Vector.
		return tempVector;
	}
	/* \\\\\\ --------------------< END RETURN FUNCTIONS >-------------------- ////// */
}