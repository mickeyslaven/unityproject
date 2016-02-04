/* Written by Kaz Crowe */
/* UltimateStatusBarController.cs ver 1.0.1a */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent( typeof( CanvasGroup ) )]
[ExecuteInEditMode]
[AddComponentMenu( "Ultimate UI/Ultimate Status Bar/Controller" )]
public class UltimateStatusBarController : MonoBehaviour
{
	// Enum for Enable/Disable options.
	public enum SwitchState{ Enabled, Disabled }

	// Transform for sizing and positioning.
	RectTransform myTransform;

	/* -----< ASSIGNED VARIABLES >----- */
	public Text statusBarText;
	public Image statusBarIcon;

	/* -----< SIZE AND PLACEMENT >----- */
	public SwitchState usePositioning = SwitchState.Enabled;
	public enum ScalingAxis{ Height, Width }
	public ScalingAxis scalingAxis = ScalingAxis.Width;
	public float statusBarSize = 1.75f;
	public float spacingX = 0.0f, spacingY = 0.0f;
	public bool preserveAspect = false;
	public Image targetImage;// This is the target image for keeping the aspect ratio of.
	Sprite s_TargetImage;// This is the base sprite of the targetImage variable.
	Vector2 aspectRatio = Vector2.zero;// The current aspect ratio to size the status bar with.
	public float xRatio = 1.0f, yRatio = 1.0f;// The Custom Ratio's to bbe setup by the user.
	Canvas parentCanvas;// The parent Canvas for configuring size and position according to selected options.
	Vector2 canvasSize = Vector2.zero;// The size of the parent canvas.

	/* -----< STYLE AND OPTIONS >----- */
	public SwitchState defaultState = SwitchState.Enabled;
	public bool enableIdleTimeout = false;
	public float idleTimeout = 2.0f;
	public enum TimeoutOption{ Snap, Fade }
	public TimeoutOption statusBarEnabled = TimeoutOption.Snap;
	public float enabledSpeed = 1.0f;
	public TimeoutOption statusBarDisabled = TimeoutOption.Snap;
	public float disabledSpeed = 1.0f;
	bool isFading = false;
	bool isCountingDown = false;
	float currentTime = 0.0f;
	CanvasGroup statusBarGroup;

	
	void Start ()
	{
		// Get the CanvasGroup component for Enable/Disable options.
		statusBarGroup = GetComponent<CanvasGroup>();

		if( statusBarGroup == null )
		{
			this.gameObject.AddComponent( typeof( CanvasGroup ) );
			statusBarGroup = GetComponent<CanvasGroup>();
		}

		// If the user wants to use the positioning of this script, then run the UpdatePositioning() function.
		if( usePositioning == SwitchState.Enabled )
			UpdatePositioning();

		// If the user wants the status bar disabled from start, do that here if the game is running.
		if( defaultState == SwitchState.Disabled && Application.isPlaying == true )
			statusBarGroup.alpha = 0.0f;
		// Else show the status bar.
		else
			RequestShowStatusBar();
	}

	/// <summary>
	/// This function updates the size and positioning of the Status Bar. 
	/// </summary>
	public void UpdatePositioning ()
	{
		// If the user doesn't want to use the positioning of this script, then return.
		if( usePositioning == SwitchState.Disabled )
			return;

		// If myTransform is not assigned...
		if( myTransform == null )
		{
			// If there is no RectTransform component on this gameObject, then return.
			if( !GetComponent<RectTransform>() )
			{
				Debug.LogError( "There is no RectTransform component attached to this gameobject. Please make sure to apply this script to a uGUI element." );
				return;
			}

			// Assign myTransform to this gameObject's RectTransform.
			myTransform = GetComponent<RectTransform>();
		}

		if( parentCanvas == null )
		{
			// If myTransform.root contains a Canvas component, then store it for refernece.
			if( myTransform.root.GetComponent<Canvas>() )
				parentCanvas = myTransform.root.GetComponent<Canvas>();
			// Else the root doesn't have a Canvas, so search through all parents to find one.
			else
				parentCanvas = GetParentCanvas();
		}

		// The canvas size is determined by the size of the parentCanvas transform * it's local scale to allow different CanvasScaler options.
		canvasSize = parentCanvas.GetComponent<RectTransform>().sizeDelta * parentCanvas.transform.localScale.x;

		// If the user is wanting to preserve the aspect ratio of the selected image...
		if( preserveAspect == true )
		{
			// If the targetImage variable has been left unassigned, then inform the user and return.
			if( targetImage == null )
				return;

			// Store the original sprite so it's size can be referenced.
			s_TargetImage = targetImage.sprite;

			// Store the raw values of the sprites ratio so that a smaller value can be configured.
			Vector2 rawRatio = new Vector2( s_TargetImage.rect.width, s_TargetImage.rect.height );

			// Temporary float to store the largest side of the sprite.
			float maxValue = rawRatio.x > rawRatio.y ? rawRatio.x : rawRatio.y;

			// Now configure the ratio based on the above information.
			aspectRatio.x = rawRatio.x / maxValue;
			aspectRatio.y = rawRatio.y / maxValue;
		}
		// Else store the sizing variables as our aspect ratio.
		else
			aspectRatio = new Vector2( xRatio, yRatio );

		// Store the calculation value of either Height or Width.
		float referenceSize = scalingAxis == ScalingAxis.Height ? canvasSize.y : canvasSize.x;

		// Configure a size for the image based on the Canvas's size and scale.
		float textureSize = referenceSize * ( statusBarSize / 10 ) / parentCanvas.transform.localScale.x;

		// Apply the configured size to the myTransform's sizeDelta.
		myTransform.sizeDelta = new Vector2( textureSize * aspectRatio.x, textureSize * aspectRatio.y );

		// CONFIGURE THE PIVOT SPACAH!!!
		Vector2 pivotSpacer = new Vector2( myTransform.sizeDelta.x * myTransform.pivot.x * parentCanvas.transform.localScale.x, myTransform.sizeDelta.y * myTransform.pivot.y * parentCanvas.transform.localScale.y );

		// Configure the position of the image according to the information that was gathered above.
		Vector2 imagePosition = ConfigureImagePosition( new Vector2( myTransform.sizeDelta.x * parentCanvas.transform.localScale.x, myTransform.sizeDelta.y * parentCanvas.transform.localScale.y ) );

		// Apply the positioning.
		myTransform.position = imagePosition + pivotSpacer;
	}

	// This function will configure a Vector2 for the position of the image.
	Vector2 ConfigureImagePosition ( Vector2 baseSize )
	{
		// Create a temporary Vector2 to modify and return.
		Vector2 tempPosVector;
		
		// Fix the custom spacing variables to something that is easy to work with.
		float fixedCSX = spacingX / 100;
		float fixedCSY = spacingY / 100;
		
		// Create two floats for applying our spacers according to our canvas size.
		float positionSpacerX = canvasSize.x * fixedCSX - ( baseSize.x * fixedCSX );
		float positionSpacerY = canvasSize.y * fixedCSY - ( baseSize.y * fixedCSY );
		
		// Apple the position spacers to the temporary Vector2.
		tempPosVector.x = positionSpacerX;
		tempPosVector.y = positionSpacerY;
		
		// Return the updated Vector2.
		return tempPosVector;
	}

	// This function is used only to find the canvas parent if it is not located on the root object.
	Canvas GetParentCanvas ()
	{
		// Store the current parent.
		Transform parent = myTransform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{ 
			// If there is a Canvas component, return the component.
			if( parent.transform.GetComponent<Canvas>() )
				return parent.transform.GetComponent<Canvas>();

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
	public void UpdateStatusBarName ( string newName )
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
	public void UpdateStatusBarIcon ( Sprite newIcon )
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
	public void ShowStatusBar ()
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
			StartCoroutine( FadeStatusBar( enabledSpeed, 1.0f ) );
		// Else, set the CanvasGroup's alpha to visible.
		else
			statusBarGroup.alpha = 1.0f;
	}

	/// <summary>
	/// Hides the status bar.
	/// </summary>
	public void HideStatusBar ()
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
			StartCoroutine( FadeStatusBar( disabledSpeed, 0.0f ) );
		// Else set the CanvasGroup to invisible.
		else
			statusBarGroup.alpha = 0.0f;
	}

	IEnumerator FadeStatusBar ( float speed, float finalValue )
	{
		isFading = true;
		float currentAlpha = statusBarGroup.alpha;
		for( float t = 0.0f; t < 1.0f && isFading == true; t += Time.deltaTime * speed )
		{
			statusBarGroup.alpha = Mathf.Lerp( currentAlpha, finalValue, t );
			yield return null;
		}
		if( isFading == true )
			statusBarGroup.alpha = finalValue;

		isFading = false;
	}

	/// <summary>
	/// Requests to show the status bar. This function should be used only if the user has the timeout option enabled. 
	/// If the timeout option is not enabled, please use the ShowStatusBar function instead.
	/// </summary>
	public void RequestShowStatusBar ()
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

	IEnumerator ShowStatusBarCountdown ()
	{
		// Set isCountingDown to true for checks.
		isCountingDown = true;

		// While the currentTime is greater than zero, continue counting down.
		while( currentTime > 0 )
		{
			currentTime -= Time.deltaTime;
			yield return null;
		}

		// Once the countdown is complete, set isCountingDown to false and hide the status bar.
		isCountingDown = false;
		HideStatusBar();
	}
	/* -----------------------------------------------< END IDLE TIMEOUT FUNCTIONS >----------------------------------------------- */

	#if UNITY_EDITOR
	void Update ()
	{
		if( !Application.isPlaying )
			UpdatePositioning();
	}
	#endif
}