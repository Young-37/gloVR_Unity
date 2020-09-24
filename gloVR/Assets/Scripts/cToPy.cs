using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP
{
  public class cToPy : MonoBehaviour
  {
      // Start is called before the first frame update
      public void SendHello()
      {
        /*UDPSocket s = new UDPSocket();
        s.Server("127.0.0.1",27000);
        s.Send("Hello World!");*/
        UDPSocket c = new UDPSocket();
        c.Client("127.0.0.1", 8000);
        c.Send("a");

        Console.ReadKey();
      }

      // Update is called once per frame
      void Update()
      {

      }
  }
}
