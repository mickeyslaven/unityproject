/* Written by Kaz Crowe */
/* ExampleHazardLogic.cs ver 1.0.1 */
using UnityEngine;
using System.Collections;

public class ExampleHazardLogic : MonoBehaviour
{
	/* Variables */
	Transform myTransform;
	
	void Start ()
	{
		myTransform = GetComponent<Transform>();
	}
	
	void OnTriggerEnter ( Collider collider )
	{
		if( collider.name == "Player" )
		{
			ExamplePlayerHealth playerHealth = collider.GetComponent<ExamplePlayerHealth>();

			if( playerHealth.isDead == true )
				return;

			playerHealth.ReduceHealth( Random.Range( 10, 30 ) );

			StartCoroutine( "MovePlayer", collider.attachedRigidbody );
		}
	}

	IEnumerator MovePlayer ( Rigidbody playerRig )
	{
		for( float t = 0.0f; t < 1.0f; t += Time.deltaTime * 3.5f )
		{
			playerRig.velocity = myTransform.up * 7.5f;

			yield return null;
		}
		playerRig.velocity = Vector3.zero;
		yield return new WaitForSeconds( 1.0f );
	}
}