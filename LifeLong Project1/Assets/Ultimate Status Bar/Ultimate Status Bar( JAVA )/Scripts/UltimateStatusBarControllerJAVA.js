/* Written by Kaz Crowe */
/* UltimateStatusBarControllerJAVA.js ver 1.0 */
#pragma strict
import UnityEngine.UI;

@script ExecuteInEditMode()
public class UltimateStatusBarControllerJAVA extends MonoBehaviour
{
	// Enum for Enable/Disable options.
	public enum SwitchState{ Enabled, Disabled }

	// Transform for sizing and positioning.
	private var myTransform : RectTransform;

	/* -----< ASSIGNED VARIABLES >----- */
	var statusBarText : Text;
	var statusBarIcon : Image;

	/* -----< SIZE AND PLACEMENT >----- */
	var usePositioning : SwitchState = SwitchState.Enabled;
	enum ScalingAxis{ Height, Width }
	var scalingAxis : ScalingAxis = ScalingAxis.Width;
	var statusBarSize : float = 1.75;
	var spacingX : float = 0.0;
	var spacingY : float = 0.0;
	var preserveAspect : boolean = false;
	var targetImage : Image;
	private var s_TargetImage : Sprite;
	private var aspectRatio : Vector2 = Vector2.zero;
	var xRatio : float = 1.0;
	var yRatio : float = 1.0;
	private var parentCanvas : Canvas;
	private var canvasSize : Vector2 = Vector2.zero;

	/* -----< STYLE AND OPTIONS >----- */
	var defaultState : SwitchState = SwitchState.Enabled;
	var enableIdleTimeout : boolean = false;
	var idleTimeout : float = 2.0;
	enum TimeoutOption{ Snap, Fade }
	var statusBarEnabled : TimeoutOption = TimeoutOption.Snap;
	var enabledSpeed : float = 1.0;
	var statusBarDisabled : TimeoutOption = TimeoutOption.Snap;
	var disabledSpeed : float = 1.0;
	private var isFading : boolean = false;
	private var isCountingDown : boolean = false;
	private var currentTime : float = 0.0;
	private var statusBarGroup : CanvasGroup;
	
	
	function Start ()
	{
		// Get the CanvasGroup component for Enable/Disable options.
		statusBarGroup = GetComponent( CanvasGroup );

		if( statusBarGroup == null )
		{
			this.gameObject.AddComponent( typeof( CanvasGroup ) );
			statusBarGroup = GetComponent( CanvasGroup );
		}
		
		// If the user wants to use the positioning of this script, then run the UpdatePositioning() function.
		if( usePositioning == SwitchState.Enabled )
			UpdatePositioning();

		// If the user wants the status bar disabled from start, do that here if the game is running.
		if( defaultState == SwitchState.Disabled && Application.isPlaying == true )
			statusBarGroup.alpha = 0.0;
		// Else show the status bar.
		else
			RequestShowStatusBar();
	}

	/// <summary>
	/// This function updates the size and positioning of the Status Bar. 
	/// </summary>
	function UpdatePositioning ()
	{
		// If the user doesn't want to use the positioning of this script, then return.
		if( usePositioning == SwitchState.Disabled )
			return;

		// If myTransform is not assigned...
		if( myTransform == null )
		{
			// If there is no RectTransform component on this gameObject, then return.
			if( !GetComponent( RectTransform ) )
			{
				Debug.LogError( "There is no RectTransform component attached to this gameobject. Please make sure to apply this script to a uGUI element." );
				return;
			}

			// Assign myTransform to this gameObject's RectTransform.
			myTransform = GetComponent( RectTransform );
		}

	    // If myTransform.root contains a Canvas component, then store it for refernece.
		if( myTransform.root.GetComponent( Canvas ) )
		    parentCanvas = myTransform.root.GetComponent( Canvas );
	    // Else the root doesn't have a Canvas, so search through all parents to find one.
		else
		    parentCanvas = GetParentCanvas();

	    // The canvas size is determined by the size of the parentCanvas transform * it's local scale to allow different Canvas options.
		canvasSize = parentCanvas.GetComponent( RectTransform ).sizeDelta * parentCanvas.transform.localScale.x;

		// If the user is wanting to preserve the aspect ratio of the selected image...
		if( preserveAspect == true )
		{
			// If the targetImage variable has been left unassigned, then inform the user and return.
			if( targetImage == null )
				return;

			// Store the original sprite so it's size can be referenced.
			s_TargetImage = targetImage.sprite;

			// Store the raw values of the sprites ratio so that a smaller value can be configured.
			var rawRatio : Vector2 = new Vector2( s_TargetImage.rect.width, s_TargetImage.rect.height );

			// Temporary float to store the largest side of the sprite.
			var maxValue : float = rawRatio.y;
			if( rawRatio.x > rawRatio.y )
				maxValue = rawRatio.x;

			// Now configure the ratio based on the above information.
			aspectRatio.x = rawRatio.x / maxValue;
			aspectRatio.y = rawRatio.y / maxValue;
		}
		// Else store the sizing variables as our aspect ratio.
		else
			aspectRatio = new Vector2( xRatio, yRatio );

		// Store the calculation value of either Height or Width.
		var referenceSize : float = canvasSize.x;
		if( scalingAxis == ScalingAxis.Height )
			referenceSize = canvasSize.y;

	    // Configure a size for the image based on the Canvas's size and scale.
		var textureSize : float = referenceSize * ( statusBarSize / 10 ) / parentCanvas.transform.localScale.x;

		// Apply the configured size to the myTransform's sizeDelta.
		myTransform.sizeDelta = new Vector2( textureSize * aspectRatio.x, textureSize * aspectRatio.y );

		// CONFIGURE THE PIVOT SPACAH!!!
		var pivotSpacer : Vector2 = new Vector2( myTransform.sizeDelta.x * myTransform.pivot.x * parentCanvas.transform.localScale.x, myTransform.sizeDelta.y * myTransform.pivot.y * parentCanvas.transform.localScale.y );

		// Configure the position of the image according to the information that was gathered above.
		var imagePosition : Vector2 = ConfigureImagePosition( new Vector2( myTransform.sizeDelta.x * parentCanvas.transform.localScale.x, myTransform.sizeDelta.y * parentCanvas.transform.localScale.y ) );

		// Apply the positioning.
		myTransform.position = imagePosition + pivotSpacer;
	}

	// This function will configure a Vector2 for the position of the image.
	function ConfigureImagePosition ( baseSize : Vector2 ) : Vector2
	{
		// Create a temporary Vector2 to modify and return.
		var tempPosVector : Vector2 = Vector2.zero;
		
		// Fix the custom spacing variables to something that is easy to work with.
		var fixedCSX : float = spacingX / 100;
		var fixedCSY : float = spacingY / 100;
		
		// Create two floats for applying our spacers according to our canvas size.
		var positionSpacerX : float = canvasSize.x * fixedCSX - ( baseSize.x * fixedCSX );
		var positionSpacerY : float = canvasSize.y * fixedCSY - ( baseSize.y * fixedCSY );
		
		// Apple the position spacers to the temporary Vector2.
		tempPosVector.x = positionSpacerX;
		tempPosVector.y = positionSpacerY;
		
		// Return the updated Vector2.
		return tempPosVector;
	}

	// This function is used only to find the canvas parent if it is not located on the root object.
	function GetParentCanvas () : Canvas
	{
		// Store the current parent.
		var parent : Transform = myTransform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{ 
		    // If there is a Canvas component, return the component.
		    if( parent.transform.GetComponent( Canvas ) )
		        return parent.transform.GetComponent( Canvas );

			// Else, shift to the next parent.
			parent = parent.transform.parent;
		}

	    // If no Canvas was found on any parents, inform the user and return nothing.
		Debug.LogError( "No Canvas component is attached to the parent gameObects. Please make sure there is a Canvas component on the root canvas." );
		return null;
	}

	/// <summary>
	/// Updates the name of the status bar.
	/// </summary>
	/// <param name="newName">The new name to apply to the Ultimate Status Bar.</param>
	function UpdateStatusBarName ( newName : String )
	{
		// If the statusBarText component is left unassigned, inform the user and return.
		if( statusBarText == null )
		{
			Debug.Log( "The Text for Status Bar Text must be assigned in order to update the name of the status bar. Please exit play mode " +
			          "and assign the Status Bar Text variable in the inspector." );
			return;
		}

		// Set the text to being the newName that the user has passed.
		statusBarText.text = newName;
	}
	
	/// <summary>
	/// Updates the icon shown on the status bar.
	/// </summary>
	/// <param name="newIcon">The new targeted icon to apply to the Ultimate Status Bar.</param>
	function UpdateStatusBarIcon ( newIcon : Sprite )
	{
		// If the statusBarIcon is left unassigned, then inform the user and return.
		if( statusBarIcon == null )
		{
			Debug.Log( "The Image for Status Bar Icon must be assigned in order to update the icon of the status bar. Please exit play mode " +
			          "and assign the Status Bar Icon variable in the inspector." );
			return;
		}

		// Apply the newIcon to the statusBarIcon.
		statusBarIcon.sprite = newIcon;
	}

	/* -------------------------------------------------< IDLE TIMEOUT FUNCTIONS >------------------------------------------------- */
	/// <summary>
	/// Shows the status bar.
	/// </summary>
	function ShowStatusBar ()
	{
		// If there is no CanvasGroup, then return.
		if( statusBarGroup == null )
			return;

		// If the status bar is currently fading, then stop the FadeStatusBar coroutine.
		if( isFading == true )
		{
			StopCoroutine( "FadeStatusBar" );
			isFading = false;
		}

		// If the user is wanting the status bar to fade in, then start the FadeStatusBar coroutine.
		if( statusBarEnabled == TimeoutOption.Fade )
			StartCoroutine( FadeStatusBar( enabledSpeed, 1.0 ) );
		// Else, set the CanvasGroup's alpha to visible.
		else
			statusBarGroup.alpha = 1.0;
	}

	/// <summary>
	/// Hides the status bar.
	/// </summary>
	function HideStatusBar ()
	{
		// If the statusBarGroup isn't assigned, return.
		if( statusBarGroup == null )
			return;

		// If the status bar is currently fading, then stop the coroutine.
		if( isFading == true )
		{
			StopCoroutine( "FadeStatusBar" );
			isFading = false;
		}

		// If the user wants to fade out, then start the FadeStatusBar coroutine.
		if( statusBarDisabled == TimeoutOption.Fade )
			StartCoroutine( FadeStatusBar( disabledSpeed, 0.0 ) );
		// Else set the CanvasGroup to invisible.
		else
			statusBarGroup.alpha = 0.0;
	}

	function FadeStatusBar ( speed : float, finalValue : float )
	{
		isFading = true;
		var currentAlpha : float = statusBarGroup.alpha;
		for( var t : float = 0.0; t < 1.0 && isFading == true; t += Time.deltaTime * speed )
		{
			statusBarGroup.alpha = Mathf.Lerp( currentAlpha, finalValue, t );
			yield;
		}
		if( isFading == true )
			statusBarGroup.alpha = finalValue;

		isFading = false;
	}

	/// <summary>
	/// Requests to show the status bar. This function should be used only if the user has the timeout option enabled. 
	/// If the timeout option is not enabled, please use the ShowStatusBar function instead.
	/// </summary>
	function RequestShowStatusBar ()
	{
		// If the user doesn't have the timeout option enabled, return.
		if( enableIdleTimeout == false )
			return;

		// If the timeout is currently not counting down, then start the countdown timer.
		if( isCountingDown == false )
		{
			currentTime = idleTimeout;
			StartCoroutine( "ShowStatusBarCountdown" );
		}
		// Else reset the currentTime to the max time.
		else
			currentTime = idleTimeout;

		// Show the status bar.
		ShowStatusBar();
	}

	function ShowStatusBarCountdown ()
	{
		// Set isCountingDown to true for checks.
		isCountingDown = true;

		// While the currentTime is greater than zero, continue counting down.
		while( currentTime > 0 )
		{
			currentTime -= Time.deltaTime;
			yield;
		}

		// Once the countdown is complete, set isCountingDown to false and hide the status bar.
		isCountingDown = false;
		HideStatusBar();
	}
	/* -----------------------------------------------< END IDLE TIMEOUT FUNCTIONS >----------------------------------------------- */
	
	#if UNITY_EDITOR
	function Update ()
	{
		if( !Application.isPlaying )
			UpdatePositioning();
	}
	#endif
}