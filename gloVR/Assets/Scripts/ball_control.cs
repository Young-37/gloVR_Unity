using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_control : MonoBehaviour
{
	// get hand_motion script (for catch_ball)
	private hand_motion hand_Motion;

	// hand
	private GameObject hand;

	// target vector
	private Vector3 startPos;
	private Vector3 targetPos;
	private Vector3 moveVector;

	// about difficulty & game
	private float speed;
	private float distance;
	private int range;
	public int level;

	// Start is called before the first frame update
	void Start()
    {
		// find Hand object
		hand = GameObject.Find("Hand");

		hand_Motion = hand.GetComponent<hand_motion>();

		// put ball at start point
		startPos = this.transform.position;
		targetPos = startPos;
		this.transform.position = targetPos;
		moveVector = new Vector3(0, 0, 0);

		// initialization
		speed = 0.5f;
		distance = 1f;
		range = 2;
		level = 0;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.S))
		{
			level = Random.Range(1, 4);

			// put ball at start point
			this.transform.position = startPos;

			// set target point
			float targetX = Random.Range(35 - range, 35 + range);
			float targetY = Random.Range(2 - range, 2 + range) + 0.95f;
			if (targetY < 0.7f)
			{
				targetY = 0.7f;
			}
			float targetZ = hand.transform.position.z - 0.46f;

			Debug.Log(targetPos);
			targetPos = new Vector3(targetX, targetY, targetZ);
			moveVector = targetPos - this.transform.position;
		}
		
		if (this.transform.position != targetPos)
		{
			this.transform.Translate(moveVector * Time.deltaTime * speed, Space.World);
			this.transform.Rotate(moveVector);
		}

		// catch ball
		if (Vector3.Distance(this.transform.position, new Vector3(hand.transform.position.x, hand.transform.position.y + 0.95f, hand.transform.position.z)) < distance)
		{
			// put ball at start point
			this.transform.position = startPos;
			// stop ball
			targetPos = this.transform.position;

			hand_Motion.catch_ball = true;
			hand_Motion.add_score = true;
		}

		// fail ball
		if (this.transform.position.z > (hand.transform.position.z + 5f))
		{
			hand_Motion.fail_ball_num = hand_Motion.fail_ball_num + 1;
			// put ball at start point
			this.transform.position = startPos;
			// stop ball
			targetPos = this.transform.position;
		}
	}
}
