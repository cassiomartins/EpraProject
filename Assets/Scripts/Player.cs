using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float speed;
	public float laneSpeed;
	public float jumpLength;
	public float jumpHeight;
	public float slideLength;
	public int maxLife = 3;
	public float minSpeed = 10f;
	public float maxSpeed = 30f;
	public float invincibleTime;
	public GameObject model;


	private Animator anim;
	private Rigidbody rb;
	private BoxCollider boxCollider;
	private int currentLane = 1;
	private Vector3 verticalTargetPosition;
	private bool jumping = false;
	private float jumpStart;
	private bool sliding = false;
	private float slideStart;
	private Vector3 boxColliderSize;
	private int currentLife;
	private bool invincible = false;
	static int blinkingValue;
	private UIManager uiManager;
	private float timecounter;

	[HideInInspector]
	public int recyclables;
	[HideInInspector]
	public float score;

	private bool canMove;

	// Use this for initialization
	void Start () {

		canMove = false;
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>();
		boxCollider = GetComponent<BoxCollider>();
		boxColliderSize = boxCollider.size;
		
		currentLife = maxLife;
		
		blinkingValue = Shader.PropertyToID("_BlinkingValue");
		uiManager = FindObjectOfType<UIManager>();
		uiManager.UpdateBucket();

		Invoke("StartRun", 3f); //Use invoke for wait a little bit for call the function StartRun
	}
	
	void Update () {

		if (!canMove)
			return;

		score += Time.deltaTime * speed;
		uiManager.UpdateScore((int)score);

		//Inputs for control the player
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			ChangeLane(-1); //ChangeLane with value -1 for move left
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			ChangeLane(1);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Jump(); //That function allows player jump an obstacle
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Slide(); //That function allows player slide under an obstacle
		}		

		if (jumping)
		{
			float ratio = (transform.position.z - jumpStart) / jumpLength; //Variable to control the proportion of the jump
			if (ratio >= 1f) //When the jump is bigger than 1 the jump is over
			{
				jumping = false;
				anim.SetBool("Jumping", false);
			}
			else
			{
				verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight; //This calculation was taken from the Unity example project
			}
		}
		else
		{
			verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime); //Makes the player move in direction to targetPosition
		}

		if (sliding)
		{
			float ratio = (transform.position.z - slideStart) / slideLength; //Variable to control the proportion of the slide
			if(ratio >= 1f) //When the slide is bigger than 1 the jump is over
			{
				sliding = false;
				anim.SetBool("Sliding", false);
				boxCollider.size = boxColliderSize;
			}
		}

		timecounter += Time.deltaTime;

		if (timecounter >= 10f)
		{
			uiManager.UpdateBucket();
			timecounter = 0f;
		}

		Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);

	}

	private void FixedUpdate()
	{ 
		rb.velocity = Vector3.forward * speed; //Allows player to run 
	}

	void StartRun()
	{
		anim.Play("runStart");
		speed = minSpeed;
		canMove = true;
	}

	void ChangeLane(int direction)
	{
		int targetLane = currentLane + direction;
		if (targetLane < 0 || targetLane > 2) //Verify if the player is moving just in the 3 lanes
			return;
		currentLane = targetLane;
		verticalTargetPosition = new Vector3((currentLane - 1), 0, 0); //set the targetPosition for move the Player
	}

	void Jump()
	{
		if (!jumping) //Verify if the Player is already jumping
		{
			jumpStart = transform.position.z;
			anim.SetFloat("JumpSpeed", speed / jumpLength); //Variable is a velocity multiplier of the animation 
			anim.SetBool("Jumping", true);
			jumping = true;
		}
	}

	void Slide()
	{
		if(!jumping && !sliding) //Verify if the Player is jumping or sliding
		{
			slideStart = transform.position.z;
			anim.SetFloat("JumpSpeed", speed / slideLength); //Variable is a velocity multiplier of the animation 
			anim.SetBool("Sliding", true);
			Vector3 newSize = boxCollider.size; //Change the size of the boxcollider
			newSize.y = newSize.y / 2;
			boxCollider.size = newSize;
			sliding = true;

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//Check the collision with the recyclables materials
		if (other.CompareTag("Paper") && uiManager.RecyclableType == "Paper")
		{
			recyclables++;
			uiManager.UpdateRecyclables(recyclables);
			other.transform.parent.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Plastic") && uiManager.RecyclableType == "Plastic")
		{
			recyclables++;
			uiManager.UpdateRecyclables(recyclables);
			other.transform.parent.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Metal") && uiManager.RecyclableType == "Metal")
		{
			recyclables++;
			uiManager.UpdateRecyclables(recyclables);
			other.transform.parent.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Organic") && uiManager.RecyclableType == "Organic")
		{ 
			recyclables++;
			uiManager.UpdateRecyclables(recyclables);
			other.transform.parent.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Glass") && uiManager.RecyclableType == "Glass")
		{
			recyclables++;
			uiManager.UpdateRecyclables(recyclables);
			other.transform.parent.gameObject.SetActive(false);
		}

		if (invincible)  //if the player is blinking he cannot be hitted
			return;

		if (other.CompareTag("Obstacle"))
		{
			canMove = false;
			currentLife--;
			uiManager.UpdateLives(currentLife);
			anim.SetTrigger("Hit");  //Trigger for start the damage animation
			speed = 0;
			if(currentLife <= 0) //GameOver condition
			{
				speed = 0;
				anim.SetBool("Dead", true);
				uiManager.gameOverPanel.SetActive(true);

				Invoke("CallMenu", 2f);
			}
			else
			{
				Invoke("CanMove", 0.75f);
				StartCoroutine(Blinking(invincibleTime));
			}
		}
	}

	void CanMove()
	{
		canMove = true;
	}

	IEnumerator Blinking(float time) //Corroutine for make the player blinks and be invencible
	{
		invincible = true;
		float timer = 0;
		float currentBlink = 1f; 
		float lastBlink = 0;
		float blinkPeriod = 0.1f; //blinks every tenth of a second
		bool enabled = false;
		yield return new WaitForSeconds(1f); //for wai
		speed = minSpeed;
		while(timer < time && invincible)
		{
			model.SetActive(enabled); //activate and deactivate the model for make ir blink
			yield return null; //for wait one frame
			timer += Time.deltaTime;
			lastBlink += Time.deltaTime;
			if(blinkPeriod < lastBlink)
			{
				lastBlink = 0;
				currentBlink = 1f - currentBlink;
				enabled = !enabled;
			}
		}
		model.SetActive(true);
		invincible = false;
	}

	void CallMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void IncreaseSpeed()
	{
		speed *= 1.15f;
		if (speed >= maxSpeed)
			speed = maxSpeed;
	}
}
