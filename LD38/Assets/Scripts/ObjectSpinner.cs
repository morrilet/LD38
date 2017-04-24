using UnityEngine;
using System.Collections;

public class ObjectSpinner : MonoBehaviour 
{
	////////// Variables //////////
	public float spinSpeed;

	////////// Primary Methods //////////
	void Update()
	{
		Spin (spinSpeed);
	}

	////////// Custom Methods //////////
	private void Spin(float speed)
	{
		Vector3 newRot = transform.rotation.eulerAngles;
		newRot.z += Time.deltaTime * speed;
		transform.rotation = Quaternion.Euler (newRot);
	}
}
