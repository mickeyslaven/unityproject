/* Written by Kaz Crowe */
/* PlayerHealth.cs ver 1.0 */
using UnityEngine;
using System.Collections;

public class ExamplePlayerHealth : MonoBehaviour
{
	/* Variables */
	ExamplePlayerController myController;

	int maxHealth = 100;
	int health;

	[HideInInspector]
	public bool isDead = false;

	public Animator myAnimator;

	public GameObject deadScreen;
	

	void Start ()
	{
		// store our var
		myController = GetComponent<ExamplePlayerController>();

		health = maxHealth;

		deadScreen.SetActive( false );
	}

	public void ReduceHealth ( int damage )
	{
		if( isDead == true )
			return;

		health -= damage;

		if( health <= 0 )
			StartCoroutine( "PlayerDeath" );

		UltimateStatusBar.UpdateStatus( "Health", health, maxHealth );
		
		myAnimator.SetTrigger( "isHurt" );
	}
	
	IEnumerator PlayerDeath ()
	{
		isDead = true;
		myController.canControl = false;
		myAnimator.SetBool( "isDead", true );

		yield return new WaitForSeconds( 2.5f );
		CanvasGroup myGroup = deadScreen.GetComponent<CanvasGroup>();
		myGroup.alpha = 0.0f;
		deadScreen.SetActive( true );
		for( float t = 0.0f; t < 1.0; t += Time.deltaTime * 0.5f )
		{
			myGroup.alpha = t;
			yield return null;
		}
		myGroup.alpha = 1.0f;
	}

	public void RestartLevel ()
	{
		Application.LoadLevel( Application.loadedLevel );
	}
}