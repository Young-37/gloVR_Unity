using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballPlayerController : MonoBehaviour
{
	private Animator animator;

	private GameObject ball;
	private BallController ballController;

	// hand
	private GameObject hand;

	// get HandController script (for catch_ball)
	private HandController hand_Motion;

	// Start is called before the first frame update
	void Start()
    {
		animator = this.GetComponent<Animator>();
		ball = GameObject.Find("Ball");
		ballController = ball.GetComponent<BallController>();

		// find Hand object
		hand = GameObject.Find("Hand");
		hand_Motion = hand.GetComponent<HandController>();

		//animator.SetBool("isThrowing", false);
	}

    // Update is called once per frame
    void Update()
    {
		// set value for animator
		if (Input.GetKeyUp(KeyCode.S) && !hand_Motion.isCatching)
		{
			animator.SetBool("isThrowing", true);
		}
		else
		{
			animator.SetBool("isThrowing", false);
		}
	}

	void throwBall()
	{
		if (!hand_Motion.isCatching)
		{
			//ballController.throwBall = true;
			ballController.ThrowBall();
		}
	}
}
