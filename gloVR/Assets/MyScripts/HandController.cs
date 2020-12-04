using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandController : MonoBehaviour
{
    //Serial Port Handler
    private SerialPortHandler SPHandler;

	//Udp Socket Handler
	private UDPHandler UHandler;

    //received data from arduino;
    int[] flexData = new int[5];
	
	float[] q = new float[4];
	Quaternion quat = new Quaternion(1, 0, 0, 0);

	//data for openCV
	float beforeXPos;
	float beforeYPos;
	float beforeZPos;

	private GameObject hand;
	private Transform handTransform;
	private float speed = 5f;

	//finger
	private GameObject finger_1;
	private GameObject finger_2;
	private GameObject finger_3;

	// thumb variable
	private GameObject thumb_0;
	private GameObject thumb_1;
	private GameObject thumb_2;

	// index finger variable
	private GameObject index_finger_1;
	private GameObject index_finger_2;
	private GameObject index_finger_3;

	// middle finger variable
	private GameObject middle_finger_1;
	private GameObject middle_finger_2;
	private GameObject middle_finger_3;

	// ring finger variable
	private GameObject ring_finger_1;
	private GameObject ring_finger_2;
	private GameObject ring_finger_3;

	// pinky variable
	private GameObject pinky_1;
	private GameObject pinky_2;
	private GameObject pinky_3;

	private Transform thumb_0Transform;
	private Transform thumb_1Transform;
	private Transform thumb_2Transform;

	private Transform index_finger_1Transform;
	private Transform index_finger_2Transform;
	private Transform index_finger_3Transform;

	private Transform middle_finger_1Transform;
	private Transform middle_finger_2Transform;
	private Transform middle_finger_3Transform;

	private Transform ring_finger_1Transform;
	private Transform ring_finger_2Transform;
	private Transform ring_finger_3Transform;

	private Transform pinky_1Transform;
	private Transform pinky_2Transform;
	private Transform pinky_3Transform;

	// rotate value
	private int finger_flex;
	private int finger_degree;

	// flex value
	public int thumb_flex;
	public int index_finger_flex;
	public int middle_finger_flex;
	public int ring_finger_flex;
	public int pinky_flex;

	// catch ball
	private GameObject catch_ball_object;
	private GameObject catch_ball_copy;
	public bool catch_ball;
	private int score;

	// catching
	public bool isCatching;

	// fail ball
	// public bool fail_ball;
	public int fail_ball_num;

	// baseball UI
	public GameObject baseball1;
	public GameObject baseball2;
	public GameObject baseball3;

	// UI
	public Text scoreText;

	// mouse click point
	private Vector3 mouseWorldPosition;


//-----------------------------------------------------------------start----------------------------------------------------------------------------------
    void Start()
	{
		//get SPHandler
		try{
			SPHandler = GameObject.Find("SP").GetComponent<SerialPortHandler>();
		}
		catch(Exception e){
			Debug.Log(e);
		}
		
		//get UHandler
		try{
			print("GET UHandler");
			UHandler = GameObject.Find("UP").GetComponent<UDPHandler>();
			// UHandler.InitUDP();
		}
		catch(Exception e){
			Debug.Log(e);
		}

		// catch ball object
		catch_ball_object = GameObject.Find("Catch_Ball");
		catch_ball_object.gameObject.SetActive(false);

		// catch ball
		catch_ball = false;
		score = 0;
		scoreText.text = string.Format("Score: {0}", score);

		// fail_ball = false;
		fail_ball_num = 0;

		// catching
		isCatching = false;

		// read all Children of current object
		//Transform child = null;
		Transform[] allChildren = GetComponentsInChildren<Transform>();

		int length = allChildren.Length;
		for (int i = 0; i < length; i++)
		{
			// find hand object
			if (allChildren[i].name == "Hand")
				hand = allChildren[i].gameObject;

			// find thumb object
			if (allChildren[i].name == "thumb_0")
				thumb_0 = allChildren[i].gameObject;
			if (allChildren[i].name == "thumb_1")
				thumb_1 = allChildren[i].gameObject;
			if (allChildren[i].name == "thumb_2")
				thumb_2 = allChildren[i].gameObject;

			// find index finger object
			if (allChildren[i].name == "index_finger_1")
				index_finger_1 = allChildren[i].gameObject;
			if (allChildren[i].name == "index_finger_2")
				index_finger_2 = allChildren[i].gameObject;
			if (allChildren[i].name == "index_finger_3")
				index_finger_3 = allChildren[i].gameObject;

			// find middle finger object
			if (allChildren[i].name == "middle_finger_1")
				middle_finger_1 = allChildren[i].gameObject;
			if (allChildren[i].name == "middle_finger_2")
				middle_finger_2 = allChildren[i].gameObject;
			if (allChildren[i].name == "middle_finger_3")
				middle_finger_3 = allChildren[i].gameObject;

			// find ring finger object
			if (allChildren[i].name == "ring_finger_1")
				ring_finger_1 = allChildren[i].gameObject;
			if (allChildren[i].name == "ring_finger_2")
				ring_finger_2 = allChildren[i].gameObject;
			if (allChildren[i].name == "ring_finger_3")
				ring_finger_3 = allChildren[i].gameObject;

			// find pinky object
			if (allChildren[i].name == "pinky_1")
				pinky_1 = allChildren[i].gameObject;
			if (allChildren[i].name == "pinky_2")
				pinky_2 = allChildren[i].gameObject;
			if (allChildren[i].name == "pinky_3")
				pinky_3 = allChildren[i].gameObject;
		}

		handTransform = hand.transform;

		thumb_0Transform = thumb_0.transform;
		thumb_1Transform = thumb_1.transform;
		thumb_2Transform = thumb_2.transform;

		index_finger_1Transform = index_finger_1.transform;
		index_finger_2Transform = index_finger_2.transform;
		index_finger_3Transform = index_finger_3.transform;

		middle_finger_1Transform = middle_finger_1.transform;
		middle_finger_2Transform = middle_finger_2.transform;
		middle_finger_3Transform = middle_finger_3.transform;

		ring_finger_1Transform = ring_finger_1.transform;
		ring_finger_2Transform = ring_finger_2.transform;
		ring_finger_3Transform = ring_finger_3.transform;

		pinky_1Transform = pinky_1.transform;
		pinky_2Transform = pinky_2.transform;
		pinky_3Transform = pinky_3.transform;

		// initialization
		mouseWorldPosition = handTransform.position;

		finger_flex = 180;
		finger_degree = 0;

		thumb_flex = 180;
		index_finger_flex = 180;
		middle_finger_flex = 180;
		ring_finger_flex = 180;
		pinky_flex = 180;

		catch_ball = false;

		// set finger as release
		thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
		thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
		thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;

		index_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		index_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		index_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		middle_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		middle_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		middle_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		ring_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		ring_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		ring_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		pinky_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		pinky_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		pinky_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
        
	}

//--------------------------------------------------------------------update--------------------------------------------------------------------------------
    // Update is called once per frame
	float minus_tmp = 0.0005f;
	int tmp_num = 0;
	float[] tmp_q = new float[2];
	float dif;

	void Update()
	{
		SPHandler.ReceiveArduinoData(ref flexData, ref q);
		if(tmp_num < 2)
		{
			tmp_q[tmp_num] = q[3];
			tmp_num++;
		}
		else
		{
			dif = tmp_q[1] - tmp_q[0];
		}
		

		// 못잡았을때.
		if(! isCatching){
			RotateFinger(flexData);
			//q[0] -> w, q[1] -> x, q[2] -> y, q[3] -> z
			// quat.Set(-q[2], q[3] + dif , q[1], -q[0]);
			quat.Set(-q[2], q[3], q[1], -q[0]);
			dif += dif;
			handTransform.rotation = quat;

			//debug.
			Debug.Log(q[0].ToString()+", "+q[1].ToString()+", " + q[2].ToString() + ", " + q[3].ToString());
			Debug.Log("Receive serial data");
		}

		
		if(isCatching){
			setFingerValue(flexData);
			quat.Set(-q[2], q[3], q[1], -q[0]);
			handTransform.rotation = quat;
		}
		
		if(UHandler.newData){
			MoveHand();
			Debug.Log("Receive UDP data");
		}

		// // when mouse button down
		// if (Input.GetMouseButtonUp(0))
		// {
		// 	// change hand's world coordinate to screen coordinate (to get z(depth) value)
		// 	Vector3 handScreenPosition = Camera.main.WorldToScreenPoint(handTransform.position);

		// 	// change mouse's screen coordinate to world coordinate
		// 	mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, handScreenPosition.z));
		// 	Debug.Log("클릭시 : ");
		// 	print(Input.mousePosition.x);
		// 	print(Input.mousePosition.y);
		// 	print(handScreenPosition.z);
		// 	//mouseWorldPosition.y = mouseWorldPosition.y - 1.15f;
		// 	//mouseWorldPosition.y = mouseWorldPosition.y;
		// }

		// // move hand
		// if (handTransform.position != mouseWorldPosition)
		// {
		// 	handTransform.position = Vector3.MoveTowards(handTransform.position, mouseWorldPosition, speed * Time.deltaTime);
		// }

		// catch ball
		if (catch_ball)
		{
			try{
				SPHandler.setServo(0);
				SPHandler.SendVibe();
			}
			catch(Exception e){
				Debug.Log(e);
			}
				
			isCatching = true;
		
			// thumb
			finger_degree = -50;
		
			thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
			thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
			thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;
		
			thumb_flex = finger_degree + 180;
		
			// index finger
			finger_degree = -70;
		
			index_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
			index_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
			index_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
		
			index_finger_flex = finger_degree + 180;
		
			// middle finger & ring finger
			finger_degree = -65;
		
			middle_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
			middle_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
			middle_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
		
			middle_finger_flex = finger_degree + 180;
		
			ring_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
			ring_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
			ring_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
		
			ring_finger_flex = finger_degree + 180;
		
			finger_degree = -55;
		
			// pinky
			pinky_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
			pinky_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
			pinky_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
		
			pinky_flex = finger_degree + 180;
		
			// copy catch ball
			catch_ball_copy = Instantiate(catch_ball_object) as GameObject;
			// create catch ball as hand's child
			catch_ball_copy.transform.parent = this.transform;
			// set position
			catch_ball_copy.transform.localPosition = new Vector3(0, -0.04f, -0.115f);
			// set scale
			catch_ball_copy.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			catch_ball_copy.gameObject.SetActive(true);
		
			// add score and update UI
			score = score + 10;
			scoreText.text = string.Format("Score: {0}", score);
		
			catch_ball = false;
		}

		// check catching
		if (isCatching
			&& thumb_flex > 140
			&& index_finger_flex > 120
			&& middle_finger_flex > 125
			&& ring_finger_flex > 125
			&& pinky_flex > 135)
		{
			isCatching = false;
			// catch_ball_copy.gameObject.SetActive(false);
			catch_ball_copy.transform.parent = null;
			catch_ball_copy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
		
		
	}

//--------------------------------------------------------------------update end--------------------------------------------------------------------------------

	public void GripHand()
	{
		// set finger as grip & set finger's flex value

		// thumb
		finger_degree = -50;

		thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
		thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
		thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;

		thumb_flex = finger_degree + 180;

		// index finger
		finger_degree = -70;

		index_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		index_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		index_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		index_finger_flex = finger_degree + 180;

		// middle finger & ring finger
		finger_degree = -65;

		middle_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		middle_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		middle_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		middle_finger_flex = finger_degree + 180;

		ring_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		ring_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		ring_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		ring_finger_flex = finger_degree + 180;

		finger_degree = -55;

		// pinky
		pinky_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		pinky_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		pinky_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		pinky_flex = finger_degree + 180;
	}

	public void ReleaseHand()
	{
		finger_degree = 0;

		// set finger as release
		thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
		thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
		thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;

		index_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		index_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		index_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		middle_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		middle_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		middle_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		ring_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		ring_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		ring_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		pinky_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		pinky_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		pinky_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		//set finger's flex value
		thumb_flex = finger_degree + 180;
		index_finger_flex = finger_degree + 180;
		middle_finger_flex = finger_degree + 180;
		ring_finger_flex = finger_degree + 180;
		pinky_flex = finger_degree + 180;
	}

	public void ResetHand()
	{
		handTransform.localEulerAngles = new Vector3(85, 0, 0);
	}

    void RotateFinger(int[] intDataArr){

		int[] rotate_degree = new int[5];

		for(int i=0;i<5;i++){
			rotate_degree[i] = intDataArr[i] - 180;
		}

        thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-rotate_degree[0] - 160) / 5), -25.86f);
		thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -rotate_degree[0]) * 0.5f;
		thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -rotate_degree[0]) * 0.3f;

		index_finger_1Transform.localEulerAngles = new Vector3(rotate_degree[1], 0, 0) * 0.5f;
		index_finger_2Transform.localEulerAngles = new Vector3(rotate_degree[1], 0, 0) * 0.8f;
		index_finger_3Transform.localEulerAngles = new Vector3(rotate_degree[1], 0, 0) * 0.3f;
		
		middle_finger_1Transform.localEulerAngles = new Vector3(rotate_degree[2], 0, 0) * 0.5f;
		middle_finger_2Transform.localEulerAngles = new Vector3(rotate_degree[2], 0, 0) * 0.8f;
		middle_finger_3Transform.localEulerAngles = new Vector3(rotate_degree[2], 0, 0) * 0.3f;
		
		ring_finger_1Transform.localEulerAngles = new Vector3(rotate_degree[3], 0, 0) * 0.5f;
		ring_finger_2Transform.localEulerAngles = new Vector3(rotate_degree[3], 0, 0) * 0.8f;
		ring_finger_3Transform.localEulerAngles = new Vector3(rotate_degree[3], 0, 0) * 0.3f;
		
		pinky_1Transform.localEulerAngles = new Vector3(rotate_degree[4], 0, 0) * 0.5f;
		pinky_2Transform.localEulerAngles = new Vector3(rotate_degree[4], 0, 0) * 0.8f;
		pinky_3Transform.localEulerAngles = new Vector3(rotate_degree[4], 0, 0) * 0.3f;

		thumb_flex = intDataArr[0];
		index_finger_flex = intDataArr[1];
		middle_finger_flex = intDataArr[2];
		ring_finger_flex = intDataArr[3];
		pinky_flex = intDataArr[4];

	}

	void setFingerValue(int[] intDataArr){
		thumb_flex = intDataArr[0];
		index_finger_flex = intDataArr[1];
		middle_finger_flex = intDataArr[2];
		ring_finger_flex = intDataArr[3];
		pinky_flex = intDataArr[4];
	}

	float changeValue = 2.5f;

	void MoveHand(){
		string text = UHandler.text;

		Vector3 handScreenPosition = Camera.main.WorldToScreenPoint(handTransform.position);

    	int index1 = text.IndexOf(',');
    	int index2 = text.IndexOf(',',index1+1);
        
    	String string_xpos = text.Substring(0,index1);
    	String string_ypos = text.Substring(index1+1,index2 - index1 - 1);
        String string_zpos = text.Substring(index2+1,text.Length - index2 - 1);

		Debug.Log(string_xpos);
		Debug.Log(string_ypos);
		Debug.Log(string_zpos);

    	float xPos = float.Parse(string_xpos);
    	float yPos = float.Parse(string_ypos);
        float zPos = float.Parse(string_zpos);

    	//filter1
    	xPos = (float)(xPos * 0.8 + beforeXPos * 0.2);
    	yPos = (float)(yPos * 0.8 + beforeYPos * 0.2);
        zPos = (float)(zPos * 0.8 + beforeZPos * 0.2);

		// xPos = 1400 - (xPos * 2.222f);
		// yPos = 700 - (yPos * 1.489f);
		// 정계산

		xPos = 1800 - (xPos * 2.857f);
		yPos = 900 - (yPos * 1.914f);
		zPos = zPos * 0.0032f + 1.5f;

		//filter2
    	if( ((beforeXPos - xPos) * (beforeXPos - xPos) > 10) || ((beforeYPos - yPos) * (beforeYPos - yPos) > 10))
    	{

      		mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, zPos));
			
			Debug.Log(xPos);
        	Debug.Log(yPos);
        	Debug.Log(zPos);

      		Debug.Log(mouseWorldPosition);
      		handTransform.position = new Vector3(mouseWorldPosition.x,mouseWorldPosition.y,mouseWorldPosition.z);

      		beforeXPos = xPos;
      		beforeYPos = yPos;
			beforeZPos = zPos;
    	}
		
		UHandler.newData = false;
	}

	public void CatchBall()
	{
		catch_ball = true;
		isCatching = true;
		setFingerValue(flexData);

		//catch_ball_copy.gameObject.SetActive(true);

		// thumb
		finger_degree = -50;
	
		thumb_0Transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
		thumb_1Transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
		thumb_2Transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;
	
		thumb_flex = finger_degree + 180;
	
		// index finger
		finger_degree = -70;
	
		index_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		index_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		index_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
	
		index_finger_flex = finger_degree + 180;
	
		// middle finger & ring finger
		finger_degree = -65;
	
		middle_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		middle_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		middle_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
	
		middle_finger_flex = finger_degree + 180;
	
		ring_finger_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		ring_finger_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		ring_finger_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
	
		ring_finger_flex = finger_degree + 180;
	
		finger_degree = -55;
	
		// pinky
		pinky_1Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		pinky_2Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		pinky_3Transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
	
		pinky_flex = finger_degree + 180;
	
		// copy catch ball
		catch_ball_copy = Instantiate(catch_ball_object) as GameObject;
		// create catch ball as hand's child
		catch_ball_copy.transform.parent = this.transform;
		// set position
		catch_ball_copy.transform.localPosition = new Vector3(0, -0.04f, -0.115f);
		// set scale
		catch_ball_copy.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		catch_ball_copy.gameObject.SetActive(true);

		// add score and update UI
		score = score + 10;
		scoreText.text = string.Format("Score: {0}", score);
	
		catch_ball = false;
	}
}