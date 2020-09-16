using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ball_control : MonoBehaviour
{
	// get hand_motion script (for catch_ball)
	private hand_motion hand_Motion;

	// hand
	private GameObject hand;

	// target vector
	private Vector3 targetPos;
	private Vector3 moveVector;

	// about difficulty & game
	public float speed;
	public float distance;
	public int range;
	private int score;

	// UI
	public Text scoreText;

	// Start is called before the first frame update
	void Start()
    {
		// find Hand object
		hand = GameObject.Find("Hand");

		hand_Motion = hand.GetComponent<hand_motion>();

		// put ball at start point
		targetPos = new Vector3(0, 0.5f, -11f);
		this.transform.position = targetPos;
		moveVector = new Vector3(0, 0, 0);

		// initialization
		speed = 0.5f;
		distance = 1f;
		range = 3;
		score = 0;
		scoreText.text = string.Format("Score: {0}", score);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.S))
		{
			// put ball at start point
			this.transform.position = new Vector3(0, 0.5f, -11f);

			// set target point
			float targetX = Random.Range(hand.transform.position.x - range, hand.transform.position.x + range);
			float targetY = Random.Range(hand.transform.position.y - range, hand.transform.position.y + range) + 0.95f;
			if (targetY < 0.5f)
			{
				targetY = 0.5f;
			}
			float targetZ = hand.transform.position.z - 0.46f;

			targetPos = new Vector3(targetX, targetY, targetZ);
			moveVector = targetPos - this.transform.position;

			Debug.Log(targetPos);
		}
		
		if (this.transform.position != targetPos)
		{
			this.transform.Translate(moveVector * Time.deltaTime * speed, Space.World);
			this.transform.Rotate(moveVector);

			//if (Vector3.Distance(this.transform.position, hand.transform.position) > 10f)
			//{
			//	// put ball at start point
			//	this.transform.position = new Vector3(0, 0.5f, -11f);
			//}
		}

		// catch ball
		if (Vector3.Distance(this.transform.position, new Vector3(hand.transform.position.x, hand.transform.position.y + 0.95f, hand.transform.position.z)) < distance)
		{
			// put ball at start point
			this.transform.position = new Vector3(0, 0.5f, -11f);
			// stop ball
			targetPos = this.transform.position;

			hand_Motion.catch_ball = true;

			score = score + 10;
			scoreText.text = string.Format("Score: {0}", score);
		}

		// fail ball
		if (this.transform.position.z > 10f)
		{
			// put ball at start point
			this.transform.position = new Vector3(0, 0.5f, -11f);
			// stop ball
			targetPos = this.transform.position;
		}
	}
}
