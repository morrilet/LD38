using UnityEngine;
using System.Collections;

/// <summary>
/// Responsible for controlling the camera. The camera should follow the player and
/// grow larger as the players speed increases. There should also be a way to zoom
/// the camera out to its max size with a button press.
/// </summary>
public class CameraFollowPlayer : MonoBehaviour 
{
	////////// Variables //////////
	public GameObject target; //The target to follow.
	public float followSpeed; //How quickly we can follow the target.
	public float minOrthoSize, maxOrthoSize; //The min/max size we can shrink/grow to.
	private float prevTargetVel, targetVel; //The current and previous velocity of the target.
	private Camera cam; //The camera attached to this game object.
	private float startZPos; //The starting z position of this game object.

	////////// Primary Methods //////////
	void Start()
	{
		cam = this.GetComponent<Camera> ();

		startZPos = transform.position.z;
		transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, startZPos);
		cam.orthographicSize = 5.0f;

		targetVel = 0;
		prevTargetVel = 0;
	}

	void Update()
	{
		FollowTarget ();

		if (Input.GetKey (KeyCode.LeftShift)) 
		{
			ZoomOut ();
		}
		else
		{
			ZoomToTarget ();
		}
	}

	////////// Custom Methods //////////
	void FollowTarget()
	{
		Vector3 newPos = transform.position;
		newPos = Vector3.Lerp (newPos, target.transform.position, followSpeed * Time.deltaTime);
		newPos.z = startZPos;
		transform.position = newPos;
	}

	void ZoomOut()
	{
		cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, maxOrthoSize, 
			Time.deltaTime * 5.0f);
	}

	void ZoomToTarget()
	{
		if (target.GetComponent<Player> () != null) 
		{
			Rigidbody2D targetRB = target.GetComponent<Rigidbody2D> ();
			Player targetPlayer = target.GetComponent<Player> ();
			targetVel = targetRB.velocity.magnitude;

			if (targetPlayer.CurrentPlanet == null)
			{
				cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, maxOrthoSize, 
					Time.deltaTime * 10.0f);
			}
			else if (!targetPlayer.Moving)
			{
				cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, minOrthoSize, 
					Time.deltaTime * 5.0f);
			}
			else
			{
				cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, minOrthoSize + (0.25f * (maxOrthoSize - minOrthoSize)), 
					Time.deltaTime * 1.0f);
			}

			prevTargetVel = targetVel;
		}
	}
}
