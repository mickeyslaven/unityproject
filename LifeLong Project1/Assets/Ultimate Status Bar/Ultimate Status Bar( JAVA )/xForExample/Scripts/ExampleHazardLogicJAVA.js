/* Written by Kaz Crowe */
/* ExampleHazardLogicJAVA.js ver 1.0.1 */
#pragma strict

/* Variables */
private var myTransform : Transform;

function Start ()
{
	myTransform = GetComponent( Transform );
}

function OnTriggerEnter ( collider : Collider )
{
	if( collider.name == "Player" )
	{
		var playerHealth : ExamplePlayerHealthJAVA = collider.GetComponent( ExamplePlayerHealthJAVA );

		if( playerHealth.isDead == true )
			return;

		playerHealth.ReduceHealth( Random.Range( 10, 30 ) );

		StartCoroutine( "MovePlayer", collider.attachedRigidbody );
	}
}

function MovePlayer ( playerRig : Rigidbody )
{
	for( var t : float = 0.0; t < 1.0; t += Time.deltaTime * 3.5 )
	{
		playerRig.velocity = myTransform.up * 7.5;

		yield;
	}
	playerRig.velocity = Vector3.zero;
	yield WaitForSeconds( 1.0f );
}