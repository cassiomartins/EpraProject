using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	public GameObject[] players;

	// Use this for initialization
	void Awake () {

		Instantiate(players[0], transform.position, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
