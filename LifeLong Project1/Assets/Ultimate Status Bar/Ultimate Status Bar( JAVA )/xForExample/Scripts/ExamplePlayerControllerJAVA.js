/* Written by Kaz Crowe */
/* ExamplePlayerControllerJAVA.js ver 1.0 */
#pragma strict

private var myRigidbody : Rigidbody;
private var myTransform : Transform;
var cameraTransform : Transform;
var followDistance : Vector3 = new Vector3( 0, 2, -10 );

private var movement : Vector3;
var moveSpeed : float;

@HideInInspector
var canControl : boolean;
var myAnimator : Animator;

private var isGrounded = false;
private var isJumping = false;

var playerVisuals : Transform;


function Start ()
{
	myRigidbody = GetComponent( Rigidbody );
	myTransform = GetComponent( Transform );
}

function Update ()
{
	CameraFollow();

	movement = new Vector3( Input.GetAxis( "Horizontal" ), 0, 0 );

	if( canControl == false )
		return;

	myAnimator.SetFloat( "isMoving", Mathf.Abs ( movement.x ) );

	if( movement.x > 0 )
		playerVisuals.LookAt( myTransform.position + new Vector3( 0, 0, 2 ) );
	else if( movement.x < 0 )
		playerVisuals.LookAt( myTransform.position + new Vector3( 0, 0, -2 ) );

	CheckForGround();

	if( Input.GetKeyDown( KeyCode.Space ) )
		StartCoroutine( "PlayerJump" );
}

function FixedUpdate ()
{
	if( canControl == false )
		return;

	myTransform.Translate( movement * moveSpeed );
}

function PlayerJump ()
{
	if( isGrounded == true )
	{
		isJumping = true;
		myAnimator.SetTrigger( "isJump" );
		yield WaitForSeconds( 0.15f );
		myRigidbody.velocity = Vector3.up * 5.0f;
		yield WaitForSeconds( 0.1f );
		isJumping = false;
	}
}

function CheckForGround ()
{
	var hitInfo : RaycastHit;
	if( Physics.Raycast( myTransform.position, Vector3.down, hitInfo, 1.25 ) )
	{
		if( hitInfo.transform.name == "Ground" && isJumping == false )
			isGrounded = true;
		else
			isGrounded = false;
	}
	else
		isGrounded = false;

	myAnimator.SetBool( "isGrounded", isGrounded );
}

function CameraFollow ()
{
	cameraTransform.position = myTransform.position + followDistance;
}