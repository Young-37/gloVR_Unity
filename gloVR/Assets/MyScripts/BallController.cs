using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
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

	// Start is called before the first frame update
	void Start()
    {
		// find Hand object
		hand = GameObject.Find("Hand");

		hand_Motion = hand.GetComponent<HandController>();

		// put ball at start point
		startPos = baseballPlayerBall.transform.position; ;
		targetPos = startPos;
		this.transform.position = targetPos;
		moveVector = new Vector3(0, 0, 0);

		// initialization
		speed = 0.3f;
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
			this.transform.position = baseballPlayerBall.transform.position;
			targetPos = this.transform.position;
			moveVector = new Vector3(0, 0, 0);
		}

		if (throwBall)
		{
			level = Random.Range(1, 4);

			// put ball at start point
			this.gameObject.SetActive(true);
			this.transform.position = baseballPlayerBall.transform.position;

			// set target point
			float targetX = Random.Range(35f - range, 35f + range);
			float targetY = Random.Range(2.5f - range, 2.5f + range) + 0.95f;
			if (targetY < 1.6f)
			{
				targetY = 1.6f;
			}
			float targetZ = hand.transform.position.z - 0.46f;

			Debug.Log(targetPos);
			targetPos = new Vector3(targetX, targetY, targetZ);
			moveVector = targetPos - this.transform.position;

			throwBall = false;
			isFlying = true;
		}
		
		if (isFlying && !throwBall)
		{
			//this.transform.position != targetPos
			this.transform.Translate(moveVector * Time.deltaTime * speed, Space.World);
			this.transform.Rotate(moveVector);
		}

		// catch ball (distance, z value, flex value)
		if (Vector3.Distance(this.transform.position, new Vector3(hand.transform.position.x, hand.transform.position.y + 0.95f, hand.transform.position.z)) < distance
			&& this.transform.position.z < (hand.transform.position.z - 0.46f)
			&& hand_Motion.thumb_flex < 120 
			&& hand_Motion.index_finger_flex < 110
			&& hand_Motion.middle_finger_flex < 110
			&& hand_Motion.ring_finger_flex < 110
			&& hand_Motion.pinky_flex < 110
			&& isFlying)
		{
			isFlying = false;

			// put ball at start point
			this.transform.position = startPos;
			// stop ball
			targetPos = this.transform.position;

			hand_Motion.catch_ball = true;
			hand_Motion.add_score = true;
		}

		// fail ball
		if (this.transform.position.z > (hand.transform.position.z + 2f) && isFlying)
		{
			isFlying = false;

			hand_Motion.fail_ball_num = hand_Motion.fail_ball_num + 1;
			// put ball at start point
			this.transform.position = startPos;
			// stop ball
			targetPos = this.transform.position;

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
				hand_Motion.baseball1.gameObject.SetActive(false);
			}
		}
	}
}
