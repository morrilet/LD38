using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
	////////// Variables //////////
	public float turnSpeed;
	public float gravRadius; //The radius of the gravitational field.

	////////// Primary Methods //////////
	void Start()
	{
	}

	////////// Custom Methods //////////
	public void Turn(int dir)
	{
		float turnAngle = dir * turnSpeed;
		transform.Rotate (new Vector3 (0.0f, 0.0f, turnAngle));
	}

	////////// Misc //////////
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, gravRadius);
	}
}
