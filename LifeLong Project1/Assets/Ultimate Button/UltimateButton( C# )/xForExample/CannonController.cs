using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
	/* VARIABLES */
	public Transform cameraTransform;
	public RectTransform powerTrans;
	public Rigidbody cannonBall;
	public Transform shootDirection;
	bool canShoot = true;
	bool canBlowUp = false;
	public int power;
	float powerModifier;
	bool shootCannon;
	float updatedPower;
	public Text helpText;
	string shootHelp = "Press the button to fire Evel 'Capsule' Knievel";
	string splodeHelp = "Press the button again to blow him up!";
	string resetHelp = "Now press the button to reset the scene!";


	void Start ()
	{
		StartCoroutine( "UpdatePowerImage" );
		helpText.text = shootHelp;
	}

	IEnumerator UpdatePowerImage ()
	{
		float t = 0.0f;
		bool countingUp = true;
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
			Vector3 updatedScale = powerTrans.localScale;
			updatedScale.x = t;
			powerTrans.localScale = updatedScale;
			powerModifier = t;
			yield return null;
		}
	}

	// This function is public so our Ultimate Button can find it
	public void ShootProjectile ()
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

	public void BlowUp ()
	{
		// We need a few variables to determine far will reach, and how much power it will have
		float radius = 20.0f;
		float splodePower = 10.0f;

		// We need to determine where the explosion is origionating from
		Vector3 explosionPos = cannonBall.transform.position;

		// Then we need a list of all the colliders in the scene within our radius
		Collider[] colliders = Physics.OverlapSphere( explosionPos, radius );

		// Now go through every collider that we got and if it has a rigidbody component, then addforce
		foreach( Collider collision in colliders )
		{
			if( collision && collision.GetComponent<Rigidbody>() )
				collision.GetComponent<Rigidbody>().AddExplosionForce( splodePower, explosionPos, radius, 3.0f, ForceMode.Impulse );
		}
	}

	void Update ()
	{
		if( cannonBall.gameObject.activeInHierarchy == true )
		{
			Vector3 updatedPosition = cameraTransform.position;
			updatedPosition.x = cannonBall.position.x;
			cameraTransform.position = updatedPosition;
		}
	}
}