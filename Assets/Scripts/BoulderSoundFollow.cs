﻿using UnityEngine;
using System.Collections;

public class BoulderSoundFollow : MonoBehaviour {

	private GameObject boulder;

	// Use this for initialization
	void Start () {
		boulder = GameObject.Find ("lvl3_ Large_Boulder (1)");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = boulder.transform.position;
	}
}