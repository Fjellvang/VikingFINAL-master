﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lady : MonoBehaviour {

    public bool isTouching;
    public GameObject lady;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("we touching");
            isTouching = true;
        }
    }

    public void DestroyThis()
    {
        gameObject.SetActive(false);
    }
    public void SpawnNew(Transform pos)
    {
        gameObject.SetActive(true);
        Vector3 yo = pos.position;
        yo.y += 1;
        gameObject.transform.position = yo;
    }
}
