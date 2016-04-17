using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour
{
	public float waveFreq;
	public float waveFreq2;
	public float offsetX;
	public float offsetY;
	public float offsetX2;
	public float offsetY2;
	public float scaleX;
	public float scaleX2;
	public float scaleY;
	public float scaleY2;

	public bool randomOffset;
	public bool UseAnimation;
	public bool enableWaves;

	private Renderer rend;
	private float animateCutoff1;
	private float animateCutoff2;

	private Material mat;

	void Start()
	{
		//get the renderr component
		rend = GetComponent<Renderer> ();

		//Make a material so we dan get the Texture.
		mat = rend.material;

	}
	void Update()
	{

		//Enable if you want fancy waves, making the texture move up and down.
		if (enableWaves) {

			if (waveFreq == 0 && waveFreq2 == 0) {

				//safety measure, to ensure a value is set, if inspector has 0.
				waveFreq = 0.4f;
				waveFreq2 = 0.5f;
			}
			scaleX2 = Mathf.Cos (Time.time) * 0.2f + waveFreq;
			scaleX = Mathf.Cos (Time.time) * 0.2f + waveFreq2;
			mat.mainTextureScale = new Vector2 (scaleX, scaleY);

			if (rend.material.HasProperty ("_Tex2")) {
				rend.material.SetTextureScale ("_Tex2", new Vector2 (scaleX2, scaleY2));
			}
			if (rend.material.HasProperty ("_SecondTex")) {
				rend.material.SetTextureScale ("_SecondTex", new Vector2 (scaleX2, scaleY2));
			}
		} 	
			
		//If enabled, cutoff alpha animation will start.
		if (UseAnimation == true) {
			animateCutoff1 = Mathf.PingPong (Time.time * 0.4f, 0.7f) + 0.7f;
			animateCutoff2 = Mathf.PingPong (Time.time * 0.4f, 0.9f) + 0.7f;
			rend.material.SetFloat ("_Cutoff", animateCutoff1);
			rend.material.SetFloat ("_CutoffTwo", animateCutoff2);
		}

		//We don't always want a random offset value.
		if (randomOffset == true && animateCutoff1 >= 1) {
			mat.mainTextureOffset = new Vector2 (Random.value * Time.time, Random.value * Time.time);

			if(rend.material.HasProperty("_Tex2")){
				rend.material.SetTextureOffset ("_Tex2", new Vector2 (Random.value * Time.time, Random.value * Time.time));
			}
			if(rend.material.HasProperty("_SecondTex") && animateCutoff2 >= 1)
			{
				rend.material.SetTextureOffset ("_SecondTex", new Vector2 (Random.value * Time.time, Random.value * Time.time));
			}

		} else if (randomOffset == false) {
			mat.mainTextureOffset = new Vector2(offsetX * Time.time, offsetY * Time.time);

			if (rend.material.HasProperty ("_Tex2")) {
				rend.material.SetTextureOffset ("_Tex2", new Vector2 (offsetX2 * Time.time, offsetY2 * Time.time));
			}
		}
	}
}