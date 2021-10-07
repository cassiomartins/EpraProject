using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	public GameObject[] players;

	void Awake () {

		Instantiate(players[0], transform.position, Quaternion.identity);

	}
	
}
