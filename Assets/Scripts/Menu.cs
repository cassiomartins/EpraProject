using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public Text[] missionDescription, missionReward, missionProgress;
	public GameObject[] rewardButton;
	public Text coinsText;
	public Text costText;
	public GameObject[] characters;

	private int characterIndex = 0;

	// Use this for initialization
	void Start () {

		SetMission();
		UpdateCoins(0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCoins(int coins)
	{
		coinsText.text = coins.ToString();
	}

	public void StartRun()
	{
		SceneManager.LoadScene(1);
	}

	public void SetMission()
	{
		for (int i = 0; i < 2; i++)
		{
			/*MissionBase mission = GameManager.gm.GetMission(i);
			missionDescription[i].text = mission.GetMissionDescription();
			missionReward[i].text = "Recompensa: "  + mission.reward;
			missionProgress[i].text = mission.progress + mission.currentProgress + " / " + mission.max;
			if (mission.GetMissionComplete())
			{
				rewardButton[i].SetActive(true);
			}*/
		}

		//GameManager.gm.Save();
	}

	public void GetReward(int missionIndex)
	{
		//GameManager.gm.recyclables += GameManager.gm.GetMission(missionIndex).reward;
		//UpdateCoins(GameManager.gm.recyclables);
		rewardButton[missionIndex].SetActive(false);
	}

	public void ChangeCharacter(int index)
	{
		characterIndex += index;
		if(characterIndex >= characters.Length)
		{
			characterIndex = 0;
		}
		else if(characterIndex < 0)
		{
			characterIndex = characters.Length - 1;
		}

		for (int i = 0; i < characters.Length; i++)
		{
			if (i == characterIndex)
				characters[i].SetActive(true);
			else
				characters[i].SetActive(false);
		}

		string cost = "";

		costText.text = cost;
	}

}
