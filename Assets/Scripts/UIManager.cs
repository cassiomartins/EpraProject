using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image[] lifeHearts;
	public Sprite[] buckets;
	public Text recyclableText;
	public GameObject gameOverPanel;
	public Image bucketObject;
	public Text scoreText;
	public string RecyclableType;
	private int BucketIndex;

	void Start()
    {
		bucketObject.GetComponent<Image>();
	}

	public void UpdateLives(int lives)
	{
		for (int i = 0; i < lifeHearts.Length; i++)
		{
			if(lives > i)
			{
				lifeHearts[i].color = Color.white;
			}
			else
			{
				lifeHearts[i].color = Color.black;
			}
		}
	}

	public void UpdateRecyclables(int recyclable)
	{
		recyclableText.text = recyclable.ToString();
	}

	public void UpdateScore(int score)
	{
		scoreText.text = "Score: " + score + "m";
	}

	public void UpdateBucket()
    {
		BucketIndex = Random.Range(0, buckets.Length);
		bucketObject.sprite = buckets[BucketIndex];

        switch (BucketIndex)
        {
			case 0:
				RecyclableType = "Paper";
				break;
			case 1:
				RecyclableType = "Organic";
				break;
			case 2:
				RecyclableType = "Glass";
				break;
			case 3:
				RecyclableType = "Metal";
				break;
			case 4:
				RecyclableType = "Plastic";
				break; 
			default:
                break;
        }

    }

}
