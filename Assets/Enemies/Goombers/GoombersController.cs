﻿using UnityEngine;
using System.Collections;

public class GoombersController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "Player") {
			Destroy(gameObject.transform.parent.gameObject);
		}

	}
	
}
