﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballPlayerController : MonoBehaviour
{
	private Animator animator;

	private GameObject ball;
	private BallController ballController;

	// Start is called before the first frame update
	void Start()
    {
		animator = this.GetComponent<Animator>();
		ball = GameObject.Find("Ball");
		ballController = ball.GetComponent<BallController>();
	}

    // Update is called once per frame
    void Update()
    {
		// set value for animator
		if (Input.GetKeyUp(KeyCode.S))
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
		ballController.throwBall = true;
	}
}
