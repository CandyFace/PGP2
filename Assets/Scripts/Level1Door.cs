﻿using UnityEngine;
using System.Collections;
using System;

// -----------------------
// Add to the level 1 exit door.
// -----------------------

public class Level1Door : MonoBehaviour {

    public GameObject level1Object;
    public GameObject exitObject;

    private Vector3 slideVector;
    private Vector3 startVector;
	private bool nowOpen = false;
	private bool soundHasPlayed = false;
	private bool doorHasClosedAgain = false;
    private bool hasNotDeleted = true;
    public bool puzzleSolved;

    public AudioClip doorSlide;

	void Start () {

        if (level1Object.GetComponent<Pickable>() == null)
        {
            Debug.Log("Pickable.cs is missing in the GameObject!");
        }

        slideVector = new Vector3(this.transform.position.x + 5f, this.transform.position.y, this.transform.position.z);
        startVector = this.transform.position;
	}
	

	void FixedUpdate () {

        if (hasNotDeleted)
        {
            if (!level1Object.GetComponent<Pickable>().CanPickUp && !exitObject.GetComponent<ExitPoint>().HasEntered)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, slideVector, 0.52f * Time.deltaTime);
                nowOpen = true;
                puzzleSolved = true;
				if(!soundHasPlayed){
					this.GetComponent<SECTR_PointSource>().Play();
					soundHasPlayed = true;
				}
            }
            else if (exitObject.GetComponent<ExitPoint>().HasEntered)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, startVector, 0.52f * Time.deltaTime);
                nowOpen = false;
                if (!doorHasClosedAgain)
                {
                    //Sectr exterminatus
                    this.GetComponent<SECTR_PointSource>().Play();
                    //this.GetComponent<AudioSource>().PlayOneShot(doorSlide, 1.0f);
                    doorHasClosedAgain = true;
                }
            }
            try
            {
                if (GameObject.Find("Level1Deleter").GetComponent<ExitPoint>().HasEntered)
                {
                    Destroy(GameObject.Find("EntireLevel1"));
                    hasNotDeleted = false;
                }
            }
            catch(NullReferenceException)
            {
                return;
            }
            

            if (nowOpen && !soundHasPlayed)
            {
                //this.GetComponent<SECTR_PropagationSource>().Play();
                //this.GetComponent<AudioSource>().PlayOneShot(doorSlide, 1.0f);
                soundHasPlayed = true;
            }
        }
	}
}
