using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class HandMovingController : MonoBehaviour
{
    // 1. Declare VariablesThread receiveThread; //1
  Thread receiveThread;
  UdpClient client;
  int port;

  public GameObject hand;
  bool jump;
  String text;

  private Vector3 mouseWorldPosition;

  float beforeXPos = 0;
  float beforeYPos = 0;

	// 2. Initialize variables
  void Start ()
  {

    print(Camera.main.aspect);

    port = 5065;
    jump = false;

    InitUDP();

    Transform[] allChildren = GetComponentsInChildren<Transform>();

    foreach(Transform child in allChildren){
      if (child.name == "Hand")
        hand = child.gameObject;
    }
    mouseWorldPosition = hand.transform.position;

  }

	// 3. InitUDP
  private void InitUDP()
  {

    print ("UDP Initialized");

    receiveThread = new Thread (new ThreadStart(ReceiveData));
    receiveThread.IsBackground = true;
    receiveThread.Start();

  }

	// 4. Receive Data
  private void ReceiveData()
  {

    client = new UdpClient (port);

    while (true)
    {

      try{

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
        byte[] data = client.Receive(ref anyIP);

        text = Encoding.UTF8.GetString(data);
        print(">> " + text);

        jump = true;

      }
      catch(Exception e)
      {
        print (e.ToString());
      }

    }

  }
	// 5. Make the Player move
  public void MoveHand()
  {

		Vector3 handScreenPosition = Camera.main.WorldToScreenPoint(hand.transform.position);

    int index1 = text.IndexOf(',');
    int index2 = text.Length - index1 - 1;
    String string_xpos = text.Substring(0,index1);
    String string_ypos = text.Substring(index1+1,index2);

    float xPos = float.Parse(string_xpos);
    float yPos = float.Parse(string_ypos);

    //print(xPos);
    //print(yPos);

    //TODO
    xPos = (float)(xPos * 0.8 + beforeXPos * 0.2);
    yPos = (float)(yPos * 0.8 + beforeYPos * 0.2);




    if( ((beforeXPos - xPos) * (beforeXPos - xPos) > 10) || ((beforeYPos - yPos) * (beforeYPos - yPos) > 10) )
    {

      mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, handScreenPosition.z));

      Debug.Log(mouseWorldPosition);
      hand.transform.position = new Vector3(mouseWorldPosition.x,mouseWorldPosition.y,mouseWorldPosition.z);

      beforeXPos = xPos;
      beforeYPos = yPos;
    }

  }


	// 6. Check for variable value, and make the Player Jump!
  void Update ()
  {
    if(jump == true)
    {
      MoveHand();
      jump = false;
    }
  }
}
