using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns a prefab randomly throughout the volume of a Unity transform. Attach to a Unity cube to visually scale or rotate. For best results disable collider and renderer.
/// </summary>
public class FogGenerator : MonoBehaviour {

	public GameObject ObjectToSpawn;   
	public float RateOfSpawn = 1;

	private float nextSpawn = 0;
	private float counter = 0;

	// Update is called once per frame

	void Start(){
	}
	void Update () {           

		transform.rotation = Camera.main.transform.rotation;

		if(Time.time > nextSpawn)
		{
			counter++;
			nextSpawn = Time.time + RateOfSpawn;

			// Random position within this transform
			Vector3 rndPosWithin;
			rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);

			if (counter <= 200) {
				Instantiate (ObjectToSpawn, rndPosWithin, transform.rotation);    
			}

		}
	}


}