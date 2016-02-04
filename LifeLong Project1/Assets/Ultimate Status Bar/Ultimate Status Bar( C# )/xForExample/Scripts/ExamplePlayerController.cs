/* Written by Kaz Crowe */
/* ExamplePlayerController.cs ver 1.0.1 */
using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class ExamplePlayerController : MonoBehaviour
{
	/* Variables */
	Rigidbody myRigidbody;
	Transform myTransform;
	public Transform cameraTransform;
	public Vector3 followDistance = new Vector3( 0, 2, -10 );

	Vector3 movement;
	[Range( 0.01f, 1.0f )]
	public float moveSpeed = 0.1f;

	[HideInInspector]
	public bool canControl = true;

	public Animator myAnimator;

	bool isGrounded = false;
	bool isJumping = false;

	public Transform playerVisuals;
	
	
	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = GetComponent<Transform>();
	}
	
	void Update ()
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

	void FixedUpdate ()
	{
		if( canControl == false )
			return;

		myTransform.Translate( movement * moveSpeed );
	}

	IEnumerator PlayerJump ()
	{
		if( isGrounded == true )
		{
			isJumping = true;
			myAnimator.SetTrigger( "isJump" );
			yield return new WaitForSeconds( 0.15f );
			myRigidbody.velocity = Vector3.up * 5.0f;
			yield return new WaitForSeconds( 0.1f );
			isJumping = false;
		}
	}

	void CheckForGround ()
	{
		RaycastHit hitInfo;
		if( Physics.Raycast( myTransform.position, Vector3.down, out hitInfo, 1.25f ) )
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

	void CameraFollow ()
	{
		cameraTransform.position = myTransform.position + followDistance;
	}
}