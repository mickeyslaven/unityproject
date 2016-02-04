/* Written by Kaz Crowe */
/* UltimateStatusBarJAVA.js ver 1.0.1b */
#pragma strict
import System.Collections.Generic;
import UnityEngine.UI;

public class UltimateStatusBarJAVA extends MonoBehaviour
{
	/* --------------------< IDLE TIMEOUT >-------------------- */
	private var myController : UltimateStatusBarControllerJAVA;

	/* ------------------< SCRIPT REFERENCE >------------------ */
	static private var StatusBarLogics : Dictionary.<String, UltimateStatusBarJAVA> = new Dictionary.<String, UltimateStatusBarJAVA>();
	var statusBarName : String = String.Empty;

	/* --------------------< CUSTOMIZATION >------------------- */
	var statusBar : Image;
	var statusBarColor : Color = Color.white;
	var showText : boolean = false;
	var statusBarText : Text;
	var statusBarTextColor : Color = Color.white;
	var usePercentage : boolean = false;
	var additionalText : String = String.Empty;
	var alternateState : boolean = false;
	enum StateTrigger{ Manual, Percentage, ColorBlended }
	var stateTrigger : StateTrigger = StateTrigger.Manual;
	var triggerValue : float = 0.25f;
	var alternateStateColor : Color = Color.white;
	var alternateStateColorMid : Color = Color.white;
	var flashing : boolean = false;
	var flashingSpeed : float = 1.0;
	private var currentState : boolean = false;


	function Start ()
	{
		// If the status bar or the text are assigned, then apply the color.
		if( statusBar != null )
			UpdateStatusBarColor( statusBarColor );
		if( statusBarText != null && showText == true )
			UpdateStatusBarTextColor( statusBarTextColor );
			
		// Submit this status bar with it's name for reference.
		SetStatus( statusBarName, this.gameObject.GetComponent( UltimateStatusBarJAVA ) );
		
		// Store the parent controller for fading, disabling and enabling the status bar.
		myController = GetComponentInParent( UltimateStatusBarControllerJAVA );

		// If the parent does not have the controller, then search through all parents and find it.
		if( myController == null )
			myController = GetParentController();
	}

	/// <summary>
	/// This function will update the selected Status Bar using the current and max values of the status.
	/// </summary>
	/// <param name="currentValue">The Current Value of the status.</param>
	/// <param name="maxValue">The Max Value of the status.</param>
	function UpdateStatusBar ( currentValue : float, maxValue : float )
	{
		// If the status bar is left unassigned, then return.
		if( statusBar == null )
			return;

		// Fix the value to be a percentage.
		var fillAmount : float = currentValue / maxValue;

		// If the value is greater than 1 or less than 0, then fix the values to being min/max.
		if( fillAmount < 0 || fillAmount > 1 )
		{
			fillAmount = fillAmount <= 0 ? 0 : 1;
		}

		// Apply the fill amount to the image.
		statusBar.fillAmount = fillAmount;

		// If the user is wanting to show the value as text and the text variable is assigned...
		if( showText == true && statusBarText != null )
		{
			// If the user does not want to show percentage, then show the current value next to the max value.
			if( usePercentage == false )
				statusBarText.text = additionalText + currentValue.ToString() + " / " + maxValue.ToString();
			// Else transfer the values into a percentage and display it.
			else
			{
				var valuePercentage : float = ( fillAmount ) * 100;
				statusBarText.text = additionalText + valuePercentage.ToString( "F0" ) + "%";
			}
		}

		// If this script has a controller parent, then request to show the status bar.
		if( myController != null )
			myController.RequestShowStatusBar();
			
		// If the user is wanting to display an alternate state according to percentage...
		if( alternateState == true )
		{
			if( stateTrigger != StateTrigger.ColorBlended && flashing == true && Application.isPlaying == true )
			{
				if( fillAmount <= triggerValue && currentState == false )
				{
					currentState = true;
					StartCoroutine( "AlternateStateFlashing" );
				}
				else if( fillAmount >= triggerValue && currentState == true )
				{
					currentState = false;
					StopCoroutine( "AlternateStateFlashing" );
				}
			}
			if( stateTrigger == StateTrigger.Percentage )
			{
				// Then configure what state it is in, and call the AlternateState() function with the correct parameter.
				if( fillAmount <= triggerValue )
					AlternateState( true );
				else
					AlternateState( false );
			}
			else if( stateTrigger == StateTrigger.ColorBlended )
			{
				AlternateStateColorBlend( fillAmount );
			}
		}
	}

	private function AlternateState ( state : boolean )
	{
		if( alternateState == false )
			return;

		if( state == true )
			UpdateStatusBarColor( alternateStateColor );
		else
			UpdateStatusBarColor( statusBarColor );
	}
	
	private function AlternateStateColorBlend ( currentValue : float )
	{
		var updatedColor : Color = Color.white;
		if( currentValue > 0.5 )
		{
			currentValue = Mathf.Lerp( -1.0, 1.0, currentValue );
			updatedColor = Color.Lerp( alternateStateColorMid, statusBarColor, currentValue );
		}
		else
		{
			currentValue *= 2;
			updatedColor = Color.Lerp( alternateStateColor, alternateStateColorMid, currentValue );
		}
		UpdateStatusBarColor( updatedColor );
	}

	function AlternateStateFlashing ()
	{
		var isIncreasing : boolean = false;
		var time : float = 0;
		while( true )
		{
			if( isIncreasing == true )
			{
				if( time < 1.0 )
					time += Time.deltaTime * flashingSpeed;
				else
					isIncreasing = false;
			}
			else
			{
				if( time > 0.0 )
					time -= Time.deltaTime * flashingSpeed;
				else
					isIncreasing = true;
			}
			statusBar.color = Color.Lerp( alternateStateColor, alternateStateColorMid, time );
			yield;
		}
	}

	/// <summary>
	/// Updates the color of the status bar with a targeted color.
	/// </summary>
	/// <param name="targetColor">The color that the status bar should change to.</param>
	function UpdateStatusBarColor ( targetColor : Color )
	{
		statusBar.color = targetColor;
	}

	/// <summary>
	/// Updates the color of the status bar text.
	/// </summary>
	/// <param name="targetColor">The color that the text of the status bar should be changed to.</param>
	function UpdateStatusBarTextColor ( targetColor : Color )
	{
		statusBarText.color = targetColor;
	}

	// This function is used only to find the UltimateStatusBarControllerJAVA parent if the parent doesn't have it
	function GetParentController () : UltimateStatusBarControllerJAVA
	{
		var parent : Transform = transform.parent;
		while( parent != null )
		{ 
			if( parent.transform.GetComponent( UltimateStatusBarControllerJAVA ) )
				return parent.transform.GetComponent( UltimateStatusBarControllerJAVA );
			
			parent = parent.transform.parent;
		}
		return null;
	}

	// This function is called by all UltimateStatusBar scripts to set up their status bars.
	private function SetStatus ( statusName : String, statusBar : UltimateStatusBarJAVA )
	{
		if( !StatusBarLogics.ContainsKey( statusName ) )
			StatusBarLogics.Add( statusName, null );
		
		StatusBarLogics[ statusName ] = statusBar;
	}

	/* ------------------< STATIC FUNCTIONS >------------------ */
	/// <summary>
	/// Updates the targeted status bar to the current status value.
	/// </summary>
	/// <param name="statusName">Targeted Status Bar's name.</param>
	/// <param name="currentValue">The current value of the status.</param>
	/// <param name="maxValue">The max value of the status.</param>
	static public function UpdateStatus ( statusName : String, currentValue : float, maxValue : float )
	{
		if( !StatusBarLogics.ContainsKey( statusName ) )
		{
			Debug.LogError( "Status bar name: \"" + statusName + "\" does not exist." );
			return;
		}
		
		StatusBarLogics[ statusName ].UpdateStatusBar( currentValue, maxValue );
	}
	
	/// <summary>
	/// Alternates the state.
	/// </summary>
	/// <param name="statusName">Targeted Status Bar's name.</param>
	/// <param name="state">If set to <c>true</c>, the targeted status bar will trigger to it's alternate state.</param>
	static public function AlternateState ( statusName : String, state : boolean )
	{
		if( !StatusBarLogics.ContainsKey( statusName ) )
			return;

		StatusBarLogics[ statusName ].AlternateState( state );
	}
	/* ----------------< END STATIC FUNCTIONS >---------------- */
}