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
      UDPSocket c = new UDPSocket();


      public void SendInGame()
      {
        c.Client("127.0.0.1", 8000);
        /*UDPSocket s = new UDPSocket();
        s.Server("127.0.0.1",27000);
        s.Send("Hello World!");*/
        c.Send("s");

        Console.ReadKey();
      }

      public void SendOutGame()
      {
        c.Client("127.0.0.1", 8000);
        c.Send("1");

        Console.ReadKey();
      }

      public void SendEnd()
      {
        c.Client("127.0.0.1", 8000);
        /*UDPSocket s = new UDPSocket();
        s.Server("127.0.0.1",27000);
        s.Send("Hello World!");*/
        c.Send("e");

        Console.ReadKey();
      }

      // Update is called once per frame
      void Update()
      {

      }
  }
}
