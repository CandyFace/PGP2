using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour
{
	public float waveAmount;
	public float waveAmount2;
	public float offsetX;
	public float offsetY;
	public float offsetX2;
	public float offsetY2;
	public float scaleY;
	public float scaleY2;
	public float scaleX;
	public float scaleX2;

	public bool randomOffset;
	public bool UseAnimation;
	public bool enableWaves;

	private Renderer renderer;
	private float animateCutoff1;
	private float animateCutoff2;

	void Start()
	{
		//get the renderr component
		renderer = GetComponent<Renderer> ();
	}
	void Update()
	{
		//Make a material so we dan get the Texture.
		Material mat = GetComponent<Renderer> ().material;


		//Enable if you want fancy waves, making the texture move up and down.
		if (enableWaves) {
			scaleX2 = Mathf.Cos (Time.time) * 0.2f + 0.7f;
			scaleX = Mathf.Cos (Time.time) * 0.2f + 0.5f;
			mat.mainTextureScale = new Vector2 (scaleX, scaleY);
			renderer.material.SetTextureScale ("_Tex2", new Vector2 (scaleX2, scaleY2));
		} else {
//			//Debug.Log ("second");
//			mat.mainTextureScale = new Vector2 (scaleX, scaleY);
//			renderer.material.SetTextureScale ("_Tex2", new Vector2 (scaleX2, scaleY2));
		}
			
		//If enabled, cutoff alpha animation will start.
		if (UseAnimation == true) {
			animateCutoff1 = Mathf.PingPong (Time.time * 0.4f, 0.7f) + 0.7f;
			animateCutoff2 = Mathf.PingPong (Time.time * 0.4f, 0.9f) + 0.7f;
			renderer.material.SetFloat ("_Cutoff", animateCutoff1);
			renderer.material.SetFloat ("_CutoffTwo", animateCutoff2);
		}

		//We don't always want a random offset value.
		if (randomOffset == true && animateCutoff1 >= 1) {
			mat.mainTextureOffset = new Vector2 (Random.value * Time.time, Random.value * Time.time);
			renderer.material.SetTextureOffset ("_Tex2", new Vector2 (Random.value * Time.time, Random.value * Time.time));
		} else if (randomOffset == false) {
			mat.mainTextureOffset = new Vector2(offsetX * Time.time, offsetY * Time.time);
			renderer.material.SetTextureOffset ("_Tex2", new Vector2(offsetX2 * Time.time, offsetY2 * Time.time));
		}
	}

}