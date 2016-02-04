/* Written by Kaz Crowe */
/* UltimateStatusBar.cs ver 1.0.1 */
// 1.0.1 - Created seperate funciton for showing text so that it can be called from multiple functions.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UltimateStatusBar : MonoBehaviour
{
	/* --------------------< IDLE TIMEOUT >-------------------- */
	UltimateStatusBarController myController;

	/* ------------------< SCRIPT REFERENCE >------------------ */
	static Dictionary<string, UltimateStatusBar> StatusBarLogics = new Dictionary<string, UltimateStatusBar>();
	public string statusBarName = string.Empty;

	/* --------------------< CUSTOMIZATION >------------------- */
	public Image statusBar;
	public Color statusBarColor = Color.white;
	public bool showText = false;
	public Text statusBarText;
	public Color statusBarTextColor = Color.white;
	public bool usePercentage = false;
	public string additionalText = string.Empty;
	public bool alternateState = false;
	public enum StateTrigger{ Manual, Percentage, ColorBlended }
	public StateTrigger stateTrigger = StateTrigger.Manual;
	public float triggerValue = 0.25f;
	public Color alternateStateColorMid = Color.white;
	public Color alternateStateColor = Color.white;
	public bool flashing = false;
	public float flashingSpeed = 1.0f;
	bool currentState = false;
	

	void Start ()
	{
		// If the status bar or the text are assigned, then apply the color.
		if( statusBar != null )
			UpdateStatusBarColor( statusBarColor );
		if( statusBarText != null && showText == true )
			UpdateStatusBarTextColor( statusBarTextColor );

		// Submit this status bar with it's name for reference.
		SetStatus( statusBarName, this.gameObject.GetComponent<UltimateStatusBar>() );

		// Store the parent controller for fading, disabling and enabling the status bar.
		myController = GetComponentInParent<UltimateStatusBarController>();

		// If the parent does not have the controller, then search through all parents and find it.
		if( myController == null )
			myController = GetParentController();
	}
	
	/// <summary>
	/// This function will update the selected Status Bar using the current and max values of the status.
	/// </summary>
	/// <param name="currentValue">The Current Value of the status.</param>
	/// <param name="maxValue">The Max Value of the status.</param>
	public void UpdateStatusBar ( float currentValue, float maxValue )
	{
		// If the status bar is left unassigned, then return.
		if( statusBar == null )
			return;

		// Fix the value to be a percentage.
		float fillAmount = currentValue / maxValue;

		// If the value is greater than 1 or less than 0, then fix the values to being min/max.
		if( fillAmount < 0 || fillAmount > 1 )
			fillAmount = fillAmount < 0 ? 0 : 1;

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
				float valuePercentage = ( fillAmount ) * 100;
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

	void ShowText ( float currentValue, float maxValue )
	{
		float fillAmount = currentValue / maxValue;

		// If the user does not want to show percentage, then show the current value next to the max value.
		if( usePercentage == false )
			statusBarText.text = additionalText + currentValue.ToString() + " / " + maxValue.ToString();
		// Else transfer the values into a percentage and display it.
		else
		{
			float valuePercentage = ( fillAmount ) * 100;
			statusBarText.text = additionalText + valuePercentage.ToString( "F0" ) + "%";
		}
	}

	void AlternateState ( bool state )
	{
		if( alternateState == false )
			return;

		if( state == true )
			UpdateStatusBarColor( alternateStateColor );
		else
			UpdateStatusBarColor( statusBarColor );
	}

	void AlternateStateColorBlend ( float currentValue )
	{
		Color updatedColor = Color.white;
		if( currentValue > 0.5 )
		{
			currentValue = Mathf.Lerp( -1.0f, 1.0f, currentValue );
			updatedColor = Color.Lerp( alternateStateColorMid, statusBarColor, currentValue );
		}
		else
		{
			currentValue *= 2;
			updatedColor = Color.Lerp( alternateStateColor, alternateStateColorMid, currentValue );
		}
		UpdateStatusBarColor( updatedColor );
	}

	IEnumerator AlternateStateFlashing ()
	{
		bool isIncreasing = false;
		float time = 0;
		while( true )
		{
			if( isIncreasing == true )
			{
				if( time < 1.0f )
					time += Time.deltaTime * flashingSpeed;
				else
					isIncreasing = false;
			}
			else
			{
				if( time > 0.0f )
					time -= Time.deltaTime * flashingSpeed;
				else
					isIncreasing = true;
			}
			statusBar.color = Color.Lerp( alternateStateColor, alternateStateColorMid, time );
			yield return null;
		}
	}
	
	/// <summary>
	/// Updates the color of the status bar with a targeted color.
	/// </summary>
	/// <param name="targetColor">The color that the status bar should change to.</param>
	public void UpdateStatusBarColor ( Color targetColor )
	{
		statusBar.color = targetColor;
	}

	/// <summary>
	/// Updates the color of the status bar text.
	/// </summary>
	/// <param name="targetColor">The color that the text of the status bar should be changed to.</param>
	public void UpdateStatusBarTextColor ( Color targetColor )
	{
		statusBarText.color = targetColor;
	}

	// This function is used only to find the UltimateStatusBarController parent if the parent doesn't have it
	UltimateStatusBarController GetParentController ()
	{
		Transform parent = transform.parent;
		while( parent != null )
		{ 
			if( parent.transform.GetComponent<UltimateStatusBarController>() )
				return parent.transform.GetComponent<UltimateStatusBarController>();
			
			parent = parent.transform.parent;
		}
		return null;
	}

	// This function is called by all UltimateStatusBar scripts to set up their status bars.
	void SetStatus ( string statusName, UltimateStatusBar statusBar )
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
	static public void UpdateStatus ( string statusName, float currentValue, float maxValue )
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
	static public void AlternateState ( string statusName, bool state )
	{
		if( !StatusBarLogics.ContainsKey( statusName ) )
			return;

		StatusBarLogics[ statusName ].AlternateState( state );
	}
	/* ----------------< END STATIC FUNCTIONS >---------------- */
}