using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	////////// Variables //////////
	Planet currentPlanet; //The planet we're currently on.
	Planet prevPlanet; //The last planet we were on. Ignore it's gravity in jumps.
	bool moving = false; //Whether or not we're current moving on the surface of a planet.
	[HideInInspector]
	public bool canJump;
	[HideInInspector]
	public bool KeepFeetOnPlanet;
	public float moveSpeed;
	public float jumpForce;
	public float maxGravDistance; //The maximum distance at which a planet can affect us with it's gravity.
	public float maxGrav; //The maximum gravity that can be exerted on us by one planet.
	private Collider2D coll;
	private Rigidbody2D rb;
	private Planet[] planets;
	private bool touchingHeadRight; //Whether or not we're touching the head of the planet we're on.
	private bool touchingHeadLeft; //Lefft/right is based on the side of the player that the head is on.

	////////// Accessors //////////
	public Planet CurrentPlanet { get { return this.currentPlanet; } }
	public bool Moving { get { return this.moving; } }

	////////// Primary Methods //////////
	void Start()
	{
		coll = this.GetComponent<Collider2D> ();
		rb = this.GetComponent<Rigidbody2D> ();

		currentPlanet = null;
		prevPlanet = null;

		GameObject[] tmp = GameObject.FindGameObjectsWithTag ("Planet");
		planets = new Planet[tmp.Length];
		for (int i = 0; i < tmp.Length; i++) 
		{
			planets [i] = tmp [i].GetComponent<Planet> ();
		}

		canJump = true;
		KeepFeetOnPlanet = true;
		Debug.Log (canJump);
	}

	void Update()
	{
		if(currentPlanet != null)
			HandleMovement ();
	}

	void FixedUpdate()
	{
		if (currentPlanet != null) //If we're on a planet, only use its gravity.
		{ 
			//HandleMovement ();
			ApplyGravity (new Planet[] { currentPlanet });
			if(KeepFeetOnPlanet)
				OrientTowardsPlanet ();
		}
		else
		{
			ApplyGravity (planets);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.tag == "Planet")
		{
			if (currentPlanet == null) 
			{
				rb.velocity = Vector2.zero;
				currentPlanet = other.gameObject.GetComponent<Planet> ();
			}
		}

		if(other.transform.parent != null)
		{
			if (other.transform.parent == currentPlanet.transform) 
			{
				if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacles")) 
				{
					//Raycast left and right to see which direction the head is...
					RaycastHit2D left, right;
					left = Physics2D.Raycast (transform.position, -transform.right, 
						5.0f, 1 << LayerMask.NameToLayer("Obstacles"));
					right = Physics2D.Raycast (transform.position, transform.right, 
						5.0f, 1 << LayerMask.NameToLayer("Obstacles"));
					if (left.collider == other.collider) 
					{
						touchingHeadLeft = true;
					}
					if (right.collider == other.collider)
					{
						touchingHeadRight = true;
					}
				}
			}
		}
	}
	/*
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.transform.parent != null) 
		{
			if (other.transform.parent == currentPlanet.transform) 
			{
				if (other.gameObject.layer == LayerMask.NameToLayer ("Obstacles")) 
				{
					//Raycast left and right to see which direction the head is...
					RaycastHit2D left, right;
					left = Physics2D.Raycast (transform.position, -transform.right, 
						5.0f, 1 << LayerMask.NameToLayer("Obstacles"));
					right = Physics2D.Raycast (transform.position, transform.right, 
						5.0f, 1 << LayerMask.NameToLayer("Obstacles"));
					if (left.collider == other.collider) 
					{
						touchingHeadLeft = false;
					}
					if (right.collider == other.collider) 
					{
						touchingHeadRight = false;
					}
				}
			}
		}
	}*/

	////////// Custom Methods //////////
	private void HandleMovement()
	{
		moving = false;
		if (Input.GetKey (KeyCode.A)) 
		{
			touchingHeadRight = false;
			Move (-1);
			FlipImage (-1);
			if (!touchingHeadLeft) 
			{
				moving = true;
				currentPlanet.Turn (-1);
			}
		} 
		if (Input.GetKey (KeyCode.D)) 
		{
			touchingHeadLeft = false;
			Move (1);
			FlipImage (1);
			if (!touchingHeadRight) 
			{
				moving = true;
				currentPlanet.Turn (1);
			}
		}
		if (Input.GetKeyDown (KeyCode.Space) && canJump) 
		{
			Jump ();
			touchingHeadLeft = false;
			touchingHeadRight = false;
			prevPlanet = currentPlanet;
			currentPlanet = null;

			AudioManager.instance.PlaySoundEffect ("Jump");
		}
	}

	private void Jump()
	{
		rb.velocity = Vector2.zero;
		Vector2 jumpVector = transform.up * jumpForce;
		rb.AddForce(jumpVector, ForceMode2D.Impulse);
	}

	private void Move(int inputDir) //Input must be -1 or 1.
	{ 
		Vector2 moveVector = transform.right * inputDir * moveSpeed;
		rb.position += moveVector;
	}

	public void Die()
	{
		StartCoroutine (Die_Coroutine (1.5f));
	}

	private IEnumerator Die_Coroutine (float duration)
	{
		SpinAndShrink (duration, 50.0f);
		yield return new WaitForSeconds (duration);
		GameManager.instance.RestartLevel ();
	}

	public void SpinAndShrink (float duration, float spinSpeed)
	{
		StartCoroutine (SpinAndShrink_Coroutine (duration, spinSpeed));
	}

	private IEnumerator SpinAndShrink_Coroutine (float duration, float spinSpeed)
	{
		//Spin...
		Vector3 newRot = transform.rotation.eulerAngles;
		newRot.z += spinSpeed;
		transform.rotation = Quaternion.Euler(newRot);

		//Shrink...
		for (float i = 0; i <= duration; i += Time.deltaTime) 
		{
			Vector3 newScale = transform.localScale;
			newScale *= 1.0f - (i / duration);
			transform.localScale = newScale;

			yield return null;
		}
	}

	private void FlipImage(int dir)
	{
		Vector3 newScale = this.transform.localScale;
		if (dir < 0 && newScale.x > 0) //Left
		{
			newScale.x = -newScale.x;
		}
		else if (dir > 0 && newScale.x < 0) //Right
		{
			newScale.x = -newScale.x;
		}
		this.transform.localScale = newScale;
	}

	private void OrientTowardsPlanet() //Points our feet towards the gravity of a planet.
	{
		Vector2 dir = Vector2.zero;
		float newRot = 0.0f;
		if (currentPlanet != null) 
		{
			dir = (Vector2)currentPlanet.transform.position - (Vector2)transform.position;
		} 
		else 
		{
			dir = (Vector2)GetNearestPlanet ().transform.position - (Vector2)transform.position;
		}
		newRot = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		rb.MoveRotation (newRot + 90.0f);
	}

	private Planet GetNearestPlanet()
	{
		Planet nearestPlanet = null;
		float shortestDistance = float.PositiveInfinity;
		for (int i = 0; i < planets.Length; i++) 
		{
			float dist = (transform.position - planets [i].transform.position).magnitude;
			if (dist < shortestDistance) 
			{
				nearestPlanet = planets [i];
			}
		}
		return nearestPlanet;
	}

	private void ApplyGravity(Planet[] planets)
	{
		for (int i = 0; i < planets.Length; i++) 
		{
			if (planets [i] != prevPlanet) 
			{
				rb.AddForce (GetGravityToPlanet (planets [i]));
			}
		}
	}

	private Vector2 GetGravityToPlanet(Planet planet) //Returns the gravity that a planet is exerting on us.
	{
		Vector2 grav = planet.transform.position - transform.position;
		float distance = grav.magnitude;
		if (distance <= planet.gravRadius) 
		{
			grav.Normalize ();
			grav *= (1.0f - distance / planet.gravRadius) * maxGrav;
			return grav;
		} 
		else 
		{
			return Vector2.zero;
		}
	}
}
