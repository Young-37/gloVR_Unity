using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HandControlTester : MonoBehaviour
{
	//Udp Socket Handler
	private UDPHandler UHandler;

	//data for openCV
	float beforeXPos;
	float beforeYPos;
    float beforeZPos;

    public GameObject hand;
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

	// rotate value
	private int finger_flex;
	private int finger_degree;

	// flex value
	public int thumb_flex;
	public int index_finger_flex;
	public int middle_finger_flex;
	public int ring_finger_flex;
	public int pinky_flex;

	// mouse click point
	private Vector3 mouseWorldPosition;

//-----------------------------------------------------------------start----------------------------------------------------------------------------------
    void Start()
	{

		//get UHandler
    	try{
      		UHandler = GameObject.Find("UP").GetComponent<UDPHandler>();
            UHandler.InitUDP();
    	}
    	catch(Exception e){
      		Debug.Log(e);
    	}

		// read all Children of current object
		Transform[] allChildren = GetComponentsInChildren<Transform>();

		// iterate
		foreach (Transform child in allChildren)
		{
			// find hand object
			if (child.name == "Hand")
				hand = child.gameObject;

			// find thumb object
			if (child.name == "thumb_0")
				thumb_0 = child.gameObject;
			if (child.name == "thumb_1")
				thumb_1 = child.gameObject;
			if (child.name == "thumb_2")
				thumb_2 = child.gameObject;

			// find index finger object
			if (child.name == "index_finger_1")
				index_finger_1 = child.gameObject;
			if (child.name == "index_finger_2")
				index_finger_2 = child.gameObject;
			if (child.name == "index_finger_3")
				index_finger_3 = child.gameObject;

			// find middle finger object
			if (child.name == "middle_finger_1")
				middle_finger_1 = child.gameObject;
			if (child.name == "middle_finger_2")
				middle_finger_2 = child.gameObject;
			if (child.name == "middle_finger_3")
				middle_finger_3 = child.gameObject;

			// find ring finger object
			if (child.name == "ring_finger_1")
				ring_finger_1 = child.gameObject;
			if (child.name == "ring_finger_2")
				ring_finger_2 = child.gameObject;
			if (child.name == "ring_finger_3")
				ring_finger_3 = child.gameObject;

			// find pinky object
			if (child.name == "pinky_1")
				pinky_1 = child.gameObject;
			if (child.name == "pinky_2")
				pinky_2 = child.gameObject;
			if (child.name == "pinky_3")
				pinky_3 = child.gameObject;
		}

		// set default finger as index finger
		finger_1 = index_finger_1;
		finger_2 = index_finger_2;
		finger_3 = index_finger_3;

		// initialization
		mouseWorldPosition = hand.transform.position;

		// set finger as release
		thumb_0.transform.localEulerAngles = new Vector3(-28.32f, ((-finger_degree - 160) / 5), -25.86f);
		thumb_1.transform.localEulerAngles = new Vector3(2.37f, -0.297f, -finger_degree) * 0.5f;
		thumb_2.transform.localEulerAngles = new Vector3(1.36f, -0.126f, -finger_degree) * 0.3f;

		index_finger_1.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		index_finger_2.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		index_finger_3.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		middle_finger_1.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		middle_finger_2.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		middle_finger_3.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		ring_finger_1.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		ring_finger_2.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		ring_finger_3.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;

		pinky_1.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.5f;
		pinky_2.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.8f;
		pinky_3.transform.localEulerAngles = new Vector3(finger_degree, 0, 0) * 0.3f;
        
	}

//--------------------------------------------------------------------update--------------------------------------------------------------------------------
    // Update is called once per frame
	void Update()
	{
		if(UHandler.newData){
			MoveHand();
		}
	}

//--------------------------------------------------------------------update end--------------------------------------------------------------------------------

	void MoveHand(){
		string text = UHandler.text;

		Vector3 handScreenPosition = Camera.main.WorldToScreenPoint(hand.transform.position);

    	int index1 = text.IndexOf(',');
        Debug.Log(index1);
    	int index2 = text.IndexOf(',',index1+1);
        Debug.Log(index2);
        Debug.Log(text.Length);
        
    	String string_xpos = text.Substring(0,index1);
    	String string_ypos = text.Substring(index1+1,index2 - index1 - 1);
        String string_zpos = text.Substring(index2+1,text.Length - index2 - 1);

    	float xPos = float.Parse(string_xpos);
    	float yPos = float.Parse(string_ypos);
        float zPos = float.Parse(string_zpos);

        Debug.Log(xPos);
        Debug.Log(yPos);
        Debug.Log(zPos);
    	//filter1
    	xPos = (float)(xPos * 0.8 + beforeXPos * 0.2);
    	yPos = (float)(yPos * 0.8 + beforeYPos * 0.2);
        zPos = (float)(zPos * 0.8 + beforeZPos * 0.2);

		//filter2
    	if( ((beforeXPos - xPos) * (beforeXPos - xPos) > 10) || ((beforeYPos - yPos) * (beforeYPos - yPos) > 10))
    	{

      		mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, handScreenPosition.z));

      		Debug.Log(mouseWorldPosition);
      		hand.transform.position = new Vector3(mouseWorldPosition.x,mouseWorldPosition.y,mouseWorldPosition.z);

      		beforeXPos = xPos;
      		beforeYPos = yPos;
    	}
		
		UHandler.newData = false;
	}
}