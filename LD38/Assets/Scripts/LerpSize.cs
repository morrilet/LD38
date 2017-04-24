using UnityEngine;
using System.Collections;

public class LerpSize : MonoBehaviour 
{
	////////// Variables //////////
	public float maxSize, minSize;
	public float speed;

	////////// Primary Methods //////////
	void Update()
	{
		CycleSize ();
	}

	////////// Custom Methods //////////
	private void CycleSize() //Cycle the size between max and min w/ a sine wave.
	{
		Vector3 scale = transform.localScale;

		//float angle = (timer / period) * 360.0f;

		scale.x = Mathf.PingPong(Time.time * speed, maxSize - minSize) + minSize;
		scale.y = Mathf.PingPong (Time.time * speed, maxSize - minSize) + minSize;

		transform.localScale = scale;
	}
}
