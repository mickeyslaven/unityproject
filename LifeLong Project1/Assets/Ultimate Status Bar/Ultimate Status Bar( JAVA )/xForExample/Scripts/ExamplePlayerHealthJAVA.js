/* Written by Kaz Crowe */
/* ExamplePlayerHealthJAVA.js ver 1.0 */
#pragma strict

private var myController : ExamplePlayerControllerJAVA;

var maxHealth : int = 100;
private var health : int;

@HideInInspector
var isDead : boolean = false;

var myAnimator : Animator;

var deadScreen : GameObject;


function Start ()
{
	// store our var
	myController = GetComponent( ExamplePlayerControllerJAVA );

	health = maxHealth;

	deadScreen.SetActive( false );
}

function ReduceHealth ( damage : int )
{
	if( isDead == true )
		return;

	health -= damage;

	if( health <= 0 )
		StartCoroutine( "PlayerDeath" );

	UltimateStatusBarJAVA.UpdateStatus( "Health", health, maxHealth );

	myAnimator.SetTrigger( "isHurt" );
}

function PlayerDeath ()
{
	isDead = true;
	myController.canControl = false;
	myAnimator.SetBool( "isDead", true );

	yield WaitForSeconds( 2.5 );
	var myGroup : CanvasGroup = deadScreen.GetComponent( CanvasGroup );
	myGroup.alpha = 0.0;
	deadScreen.SetActive( true );
	for( var t : float = 0.0; t < 1.0; t += Time.deltaTime * 0.5 )
	{
		myGroup.alpha = t;
		yield;
	}
	myGroup.alpha = 1.0;
}

function RestartLevel ()
{
	Application.LoadLevel( Application.loadedLevel );
}