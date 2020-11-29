using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
	private SerialPortHandler SPHandler;
	
	// hand
	private GameObject hand;

	// get HandController script (for catch_ball)
	private HandController hand_Motion;

	// target vector
	private Vector3 startPos;
	private Vector3 targetPos;
	private Vector3 moveVector;

	// about difficulty & game
	private float speed;
	private float distance;
	private float range;
	public int level;

	public GameObject baseballPlayerBall;
	public bool throwBall;
	private bool isFlying;

	private Transform myTransform;
	private Transform handTransform;
	private Transform baseballPlayerBallTransform;

	// Start is called before the first frame update
	void Start()
    {

		// try{
		// 	SPHandler = GameObject.Find("SP").GetComponent<SerialPortHandler>();
		// }
		// catch(System.Exception e){
		// 	Debug.Log(e);
		// }
		// find Hand object
		myTransform = this.transform;

		hand = GameObject.Find("Hand");
		handTransform = hand.transform;

		baseballPlayerBallTransform = baseballPlayerBall.transform;
		hand_Motion = hand.GetComponent<HandController>();

		// put ball at start point
		startPos = baseballPlayerBallTransform.position;
		targetPos = startPos;
		myTransform.position = targetPos;
		moveVector = new Vector3(0, 0, 0);

		// initialization
		speed = 0.4f;
		distance = 1f;
		range = 1f;
		level = 0;

		isFlying = false;
		throwBall = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (!isFlying)
		{
			myTransform.position = baseballPlayerBallTransform.position;
			targetPos = myTransform.position;
			moveVector = new Vector3(0, 0, 0);
		}

		//if (throwBall && !hand_Motion.isCatching)
		//{
		//	level = Random.Range(0, 4);
		//	Debug.Log(level);
		//	// try{
		//	// 	SPHandler.setServo(level);
		//	// }
		//	// catch(System.Exception e){
		//	// 	Debug.Log(e);
		//	// }
		//
		//	// put ball at start point
		//	this.gameObject.SetActive(true);
		//	myTransform.position = baseballPlayerBallTransform.position;
		//
		//	// set target point
		//	float targetX = Random.Range(35f - range, 35f + range);
		//	float targetY = Random.Range(2.5f - range, 2.5f + range) + 0.95f;
		//	if (targetY < 1.6f)
		//	{
		//		targetY = 1.6f;
		//	}
		//	float targetZ = handTransform.position.z - 0.46f;
		//
		//	Debug.Log(targetPos);
		//	targetPos = new Vector3(targetX, targetY, targetZ);
		//	moveVector = targetPos - myTransform.position;
		//
		//	throwBall = false;
		//	isFlying = true;
		//}
		
		if (isFlying && !throwBall)
		{
			//myTransform.position != targetPos
			myTransform.Translate(moveVector * Time.deltaTime * speed, Space.World);
			myTransform.Rotate(moveVector);
		}

		// catch ball (distance, z value, flex value)
		if (Vector3.Distance(myTransform.position, new Vector3(handTransform.position.x, handTransform.position.y + 0.95f, handTransform.position.z)) < distance
			&& myTransform.position.z < (handTransform.position.z - 0.46f)
			// && hand_Motion.thumb_flex < 140 
			// && hand_Motion.index_finger_flex < 120
			// && hand_Motion.middle_finger_flex < 125
			// && hand_Motion.ring_finger_flex < 125
			// && hand_Motion.pinky_flex < 135
			// && hand_Motion.thumb_flex > 120
			// && hand_Motion.index_finger_flex > 100
			// && hand_Motion.middle_finger_flex > 105
			// && hand_Motion.ring_finger_flex > 105
			// && hand_Motion.pinky_flex > 115
			&& isFlying)
		{
			isFlying = false;

			// put ball at start point
			myTransform.position = startPos;
			// stop ball
			targetPos = myTransform.position;

			//hand_Motion.catch_ball = true;
			hand_Motion.CatchBall();
			// hand_Motion.add_score = true;
		}

		// fail ball
		if (myTransform.position.z > (handTransform.position.z + 2f) && isFlying)
		{
			isFlying = false;

			// hand_Motion.fail_ball = true;
			// hand_Motion.play_anim = true;

			// set hand motion's value
			hand_Motion.fail_ball_num = hand_Motion.fail_ball_num + 1;

			// put ball at start point
			myTransform.position = startPos;
			// stop ball
			targetPos = myTransform.position;

			if (hand_Motion.fail_ball_num == 1)
			{
				hand_Motion.baseball3.gameObject.SetActive(false);
			}

			if (hand_Motion.fail_ball_num == 2)
			{
				hand_Motion.baseball2.gameObject.SetActive(false);
			}

			if (hand_Motion.fail_ball_num == 3)
			{
				SceneManager.LoadScene("GameOverScene");
				hand_Motion.baseball1.gameObject.SetActive(false);
			}
		}
	}

	public void ThrowBall()
	{
		level = Random.Range(0, 4);
		Debug.Log(level);
		// try{
		// 	SPHandler.setServo(level);
		// }
		// catch(System.Exception e){
		// 	Debug.Log(e);
		// }

		// put ball at start point
		this.gameObject.SetActive(true);
		myTransform.position = baseballPlayerBallTransform.position;

		// set target point
		float targetX = Random.Range(35f - range, 35f + range);
		float targetY = Random.Range(2.5f - range, 2.5f + range) + 0.95f;
		if (targetY < 1.6f)
		{
			targetY = 1.6f;
		}
		float targetZ = handTransform.position.z - 0.46f;

		Debug.Log(targetPos);
		targetPos = new Vector3(targetX, targetY, targetZ);
		moveVector = targetPos - myTransform.position;

		throwBall = false;
		isFlying = true;
	}
}
