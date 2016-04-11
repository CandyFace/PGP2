using UnityEngine;
using System.Collections;

public class DepthActivate : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Camera>();
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

//	public Material mat;
//	void OnRenderImage(RenderTexture src, RenderTexture dest) {
//		Graphics.Blit(src, dest, mat);
//	}
}