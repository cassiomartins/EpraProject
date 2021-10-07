using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public GameObject[] obstacles;
	public Vector2 numberOfObstacles;
	public GameObject[] recyclable;
	public Vector2 numberOfRecyclable;

	public List<GameObject> newObstacles;
	public List<GameObject> newRecyclables;

	void Start () {

		int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y); //Quantity of obstacles in the track
		int newNumberOfRecyclables = (int)Random.Range(numberOfRecyclable.x, numberOfRecyclable.y); //Quantity of Recyclables in the track

		for (int i = 0; i < newNumberOfObstacles; i++)  //Instatiate the obstacles
		{
			newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
			newObstacles[i].SetActive(false);
		}

		for (int i = 0; i < newNumberOfRecyclables; i++) //Instatiate the recyclables
		{
			newRecyclables.Add(Instantiate(recyclable[Random.Range(0,recyclable.Length)], transform));
			newRecyclables[i].SetActive(false);
		}

		PositionateObstacles();  //Gives a new position of every obstacles
		PositionateRecyclabes(); //Gives a new position of every recyclables

	}

	void PositionateObstacles()
	{
		for (int i = 0; i < newObstacles.Count; i++)
		{
			float posZMin = (297f / newObstacles.Count) + (297f / newObstacles.Count) * i;  //297 is the size of the entire track
			float posZMax = (297f / newObstacles.Count) + (297f / newObstacles.Count) * i + 1;
			newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
			newObstacles[i].SetActive(true);
			if (newObstacles[i].GetComponent<ChangeLane>() != null)
				newObstacles[i].GetComponent<ChangeLane>().PositionLane();
		}
	}

	void PositionateRecyclabes()
	{
		float minZPos = 10f;
		for (int i = 0; i < newRecyclables.Count; i++)
		{
			float maxZPos = minZPos + 5f;
			float randomZPos = Random.Range(minZPos, maxZPos);
			newRecyclables[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
			newRecyclables[i].SetActive(true);
			newRecyclables[i].GetComponent<ChangeLane>().PositionLane();
			minZPos = randomZPos + 1;
		}
	}

	private void OnTriggerEnter(Collider other)  //Verify colision with the player 
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Player>().IncreaseSpeed();  //Increase speed of the player to the initial speed
			transform.position = new Vector3(0, 0, transform.position.z + 297 * 2);
			PositionateObstacles();
			PositionateRecyclabes();
		}
	}


}
