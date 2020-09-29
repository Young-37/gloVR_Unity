using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UDP;

public class UDPHandler : MonoBehaviour
{
    UDPSocket sk = new UDPSocket();

    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    public void SendString(string sData)
    {
    sk.Client("127.0.0.1", 8000);
    sk.Send(sData);

    Console.ReadKey();
    }

}
