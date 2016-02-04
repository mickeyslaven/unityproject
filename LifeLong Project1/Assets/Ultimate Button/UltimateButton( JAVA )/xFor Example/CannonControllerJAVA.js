#pragma strict

/* VARIABLES */
var cameraTransform : Transform;
var powerTrans : RectTransform;
var cannonBall : Rigidbody;
var shootDirection : Transform;
private var canShoot : boolean = true;
private var canBlowUp : boolean = false;
var power : int;
private var powerModifier : float;
private var shootCannon : boolean;
private var updatedPower : float;
var helpText : UnityEngine.UI.Text;
private var shootHelp : String = "Press the button to fire Evel 'Capsule' Knievel";
private var splodeHelp : String = "Press the button again to blow him up!";
private var resetHelp : String = "Now press the button to reset the scene!";


function Start ()
{
	StartCoroutine( "UpdatePowerImage" );
	helpText.text = shootHelp;
}

function UpdatePowerImage ()
{
	var t : float = 0.0f;
	var countingUp : boolean = true;
	while( canShoot == true )
	{
		// Scale our power image
		if( countingUp == true )
		{
			if( t > 0.99f )
			{
				countingUp = false;
			}
			else
			{
				t += Time.deltaTime;
			}
		}
		else
		{
			if( t < 0.025f )
			{
				countingUp = true;
			}
			else
			{
				t -= Time.deltaTime;
			}
		}
		var updatedScale : Vector3 = powerTrans.localScale;
		updatedScale.x = t;
		powerTrans.localScale = updatedScale;
		powerModifier = t;
		yield;
	}
}

function ShootProjectile ()
{
	if( canShoot == true )
	{
		updatedPower = powerModifier * power;
		canShoot = false;
		cannonBall.isKinematic = false;
		cannonBall.AddForce( shootDirection.forward * updatedPower, ForceMode.Impulse );
		canBlowUp = true;
		helpText.text = splodeHelp;
	}
	else if( canBlowUp == true )
	{
		canBlowUp = false;
		BlowUp();
		helpText.text = resetHelp;
	}
	else
		Application.LoadLevel( Application.loadedLevel );
}

function BlowUp ()
{
	// We need a few variables to determine far will reach, and how much power it will have
	var radius : float = 20.0f;
	var splodePower : float = 10.0f;

	// We need to determine where the explosion is origionating from
	var explosionPos : Vector3 = cannonBall.transform.position;

	// Then we need a list of all the colliders in the scene within our radius
	var colliders : Collider[] = Physics.OverlapSphere( explosionPos, radius );

	// Now go through every collider that we got and if it has a rigidbody component, then addforce
	for( var collision : Collider in colliders )
	{
		if( collision && collision.GetComponent.<Rigidbody>() )
			collision.GetComponent.<Rigidbody>().AddExplosionForce( splodePower, explosionPos, radius, 3.0f, ForceMode.Impulse );
	}
}

function Update ()
{
	if( cannonBall.gameObject.activeInHierarchy == true )
	{
		var updatedPosition : Vector3 = cameraTransform.position;
		updatedPosition.x = cannonBall.position.x;
		cameraTransform.position = updatedPosition;
	}
}