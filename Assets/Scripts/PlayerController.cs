using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour {

	public float speedMultiplier;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

	private float nextFire;
	private bool isUpgraded;

	private GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void Update ()
	{
		isUpgraded = gameController.getUpgraded ();

		if (Input.GetButton ("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
//			GameObject clone = 

			if (isUpgraded)
			{
				Vector3 currentEuler = transform.rotation.eulerAngles;
				Vector3 eulerMod = new Vector3 (0.0f, 15.0f, 0.0f);

				Instantiate (shot, transform.position, Quaternion.Euler(currentEuler-eulerMod)); // as GameObject;
				Instantiate (shot, transform.position, Quaternion.Euler(currentEuler+eulerMod)); // as GameObject;
				Instantiate (shot, transform.position, transform.rotation); // as GameObject;
			}
			else
			{
				Instantiate (shot, transform.position, transform.rotation); // as GameObject;
			}
			
			audio.Play ();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speedMultiplier;

		rigidbody.position = new Vector3 (Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
		                                  0.0f, 
		                                  Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax));

		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
}
