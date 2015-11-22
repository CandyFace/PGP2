﻿using UnityEngine;
using System.Collections;

public class SequencePuzzle : MonoBehaviour
{

    // This is the controller script for the Sequence puzzle.
    // It should be put onto an empty game object. Preferably you should make the interactable buttons, which the other script is attached to, children of this empty object.

    private bool isCorrect;

	private bool gongHasPlayed;
	private bool bellHasPlayed;
	private bool chimesHasPlayed;
    private bool cog1correcet;
    private bool cog2correcet;
    private bool cog3correcet;

    public bool cogSoundPlayed;
    public bool magicSoundStarted;

    private RayCast rc;
    private GameObject button;

	private int instrumentsPlayed = 0;
    public int count = 0;
	private float delayCount;
    public int countValue
    {
        get { return count; }
        set { count = value; }
    }

    private GameObject gear1, gear2, gear3, gear4, gear5;
    private GameObject top1, mid1, bot1;
    private GameObject top2, mid2, bot2;
    private GameObject top3, mid3, bot3;
	private GameObject top4, mid4, bot4;
	private GameObject top5, mid5, bot5;

    public GameObject spawnObject;

    void Awake()
    {
        spawnObject.SetActive(false);
        rc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RayCast>();
        //rc.SqnPzl = GameObject.Find("Gear_Cliff").GetComponent<SequencePuzzle>();

        gear1 = GameObject.Find("Gear01");
		gear2 = GameObject.Find("Gear02");
		gear3 = GameObject.Find("Gear03");
		gear4 = GameObject.Find("Gear04");
		gear5 = GameObject.Find("Gear05");

        // 3 points for 1st lane
        top1 = GameObject.Find("Trigger 1 - Top");
		mid1 = GameObject.Find("Trigger 1 - Middle");
        bot1 = GameObject.Find("Trigger 1 - Bottom");
        // 3 points for 2nd lane
        top2 = GameObject.Find("Trigger 2 - Top");
		mid2 = GameObject.Find("Trigger 2 - Middle");
        bot2 = GameObject.Find("Trigger 2 - Bottom");
        // 3 points for 3rd lane
        top3 = GameObject.Find("Trigger 3 - Top");
		mid3 = GameObject.Find("Trigger 3 - Middle");
        bot3 = GameObject.Find("Trigger 3 - Bottom");
		// 3 points for 4th lane
		top4 = GameObject.Find("Trigger 4 - Top");
		mid4 = GameObject.Find("Trigger 4 - Middle");
		bot4 = GameObject.Find("Trigger 4 - Bottom");
		// 3 points for 5th lane
		top5 = GameObject.Find("Trigger 5 - Top");
		mid5 = GameObject.Find("Trigger 5 - Middle");
		bot5 = GameObject.Find("Trigger 5 - Bottom");

		gear1.transform.position = bot1.transform.position;
		gear2.transform.position = bot2.transform.position;
		gear3.transform.position = mid3.transform.position;
		gear4.transform.position = mid4.transform.position;
		gear5.transform.position = bot5.transform.position;


    }

    void Update()
    {
        //Debug.Log(count);
        if (!isCorrect)
        {
            if (rc.sequenceButton == 1)
            {

                // Moves gear1 up, and gear2 up
                gear1.transform.position = Vector3.Lerp(gear1.transform.position, mid1.transform.position, Time.deltaTime * 5);
                gear2.transform.position = Vector3.Lerp(gear2.transform.position, mid2.transform.position, Time.deltaTime * 5);
				gear3.transform.position = Vector3.Lerp(gear3.transform.position, top3.transform.position, Time.deltaTime * 5);
				gear4.transform.position = Vector3.Lerp(gear4.transform.position, top4.transform.position, Time.deltaTime * 5);
				gear5.transform.position = Vector3.Lerp(gear5.transform.position, mid5.transform.position, Time.deltaTime * 5);
                cog1correcet = false;
                cog2correcet = false;
                if (!cogSoundPlayed)
                {
                    //GameObject.Find("Cog1UpSound").GetComponent<SECTR_PointSource>().Play();
                    //GameObject.Find("Cog2UpSound").GetComponent<SECTR_PointSource>().Play();
                    cogSoundPlayed = true;
                }

            }
            else if (rc.sequenceButton == 2)
            {
                // Moves gear1 down, and gear3 up
				gear1.transform.position = Vector3.Lerp(gear1.transform.position, top1.transform.position, Time.deltaTime * 5);
				gear2.transform.position = Vector3.Lerp(gear2.transform.position, top2.transform.position, Time.deltaTime * 5);
				gear5.transform.position = Vector3.Lerp(gear5.transform.position, top5.transform.position, Time.deltaTime * 5);
                cog1correcet = true;
                cog3correcet = false;
                if (!cogSoundPlayed)
                {
                    //GameObject.Find("Cog1DownSound").GetComponent<SECTR_PointSource>().Play();
                    //GameObject.Find("Cog3UpSound").GetComponent<SECTR_PointSource>().Play();
                    cogSoundPlayed = true;
                }
            }
            else if (rc.sequenceButton == 3)
            {
                // Moves gear2 down, and gear3 down
				gear1.transform.position = Vector3.Lerp(gear1.transform.position, bot1.transform.position, Time.deltaTime * 5);
				gear3.transform.position = Vector3.Lerp(gear3.transform.position, bot3.transform.position, Time.deltaTime * 5);
				gear5.transform.position = Vector3.Lerp(gear5.transform.position, bot5.transform.position, Time.deltaTime * 5);
                cog2correcet = true;
                cog3correcet = true;
                if (!cogSoundPlayed)
                {
                    //GameObject.Find("Cog2DownSound").GetComponent<SECTR_PointSource>().Play();
                    //GameObject.Find("Cog3DownSound").GetComponent<SECTR_PointSource>().Play();
                    cogSoundPlayed = true;
                }
            }
            if (cog1correcet && cog2correcet && cog3correcet)
            {
				delayCount += Time.deltaTime;
				if(delayCount >= 3f){
					isCorrect = true;
				}
            }
        }

        if (isCorrect)
        {
            //Debug.Log("Done!");
            spawnObject.SetActive(true);
            if (!magicSoundStarted)
            {
                //spawnObject.GetComponent<SECTR_PropagationSource>().Play();
                //GameObject.Find("Dragonstatue").GetComponent<SECTR_PointSource>().Play();
                magicSoundStarted = true;
            }
        }
    }
}