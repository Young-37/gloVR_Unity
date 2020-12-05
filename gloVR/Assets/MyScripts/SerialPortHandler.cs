using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialPortHandler : MonoBehaviour
{
    SerialPort sp = new SerialPort("/dev/tty.MAIN-Port",9600);
    SerialPort sp2 = new SerialPort("/dev/tty.AWARD-Port",9600);

    public string servoControl;

    void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // port open
        sp.ReadTimeout = 10;
        sp.Open();
        print("Serial ports open");

        servoControl = "s00000e";

        sp2.ReadTimeout = 5;
        sp2.Open();
        print("Serial port2 open");
    }

    public bool SendString(string string_data){

        if(sp.IsOpen){
            sp.Write(string_data);
        }
        else{
            // throw Exception;
            return false;
        }

        if(sp2.IsOpen){
            sp2.Write(string_data);
            return true;
        }
        else{
            return false;
        }
    }

    public bool IsConnected(){
        int b = 0;
        
        if(sp.IsOpen){
            try{                
                b = sp.ReadByte();
            }
            catch(System.Exception e){
                Debug.Log(e);
            }
            if(b != 0){
                return true;
            }
        }

        return false;
    }

    char[] teapotPacket = new char[14];  // InvenSense Teapot packet
    int serialCount = 0;                 // current packet byte position
    int synced = 0;

    public void ReceiveArduinoData(ref int[] flex_data,ref float[] q) {

        int start_byte = 0;

        try{
            start_byte = sp.ReadByte();
        }
        catch(System.Exception e){
            Debug.Log(e);
        }

        if(start_byte != 200) return;

        for(int i=0;i<5;i++){
            flex_data[i] = sp.ReadByte();
        }

        while (sp.BytesToRead > 0) {

            int ch = sp.ReadByte();

            if (synced == 0 && ch != '$') return;   // initial synchronization - also used to resync/realign if needed
            synced = 1;
            // print((char)ch);

            if ((serialCount == 1 && ch != 2)
                || (serialCount == 12 && ch != '\r')
                || (serialCount == 13 && ch != '\n'))  {
                serialCount = 0;
                synced = 0;
                return;
            }

            if (serialCount > 0 || ch == '$') {
                teapotPacket[serialCount++] = (char)ch;
                if (serialCount == 14) {
                    serialCount = 0; // restart packet byte position
                    
                    // get quaternion from data packet
                    q[0] = ((teapotPacket[2] << 8) | teapotPacket[3]) / 16384.0f;
                    q[1] = ((teapotPacket[4] << 8) | teapotPacket[5]) / 16384.0f;
                    q[2] = ((teapotPacket[6] << 8) | teapotPacket[7]) / 16384.0f;
                    q[3] = ((teapotPacket[8] << 8) | teapotPacket[9]) / 16384.0f;
                    for (int i = 0; i < 4; i++) if (q[i] >= 2) q[i] = -4 + q[i];
                    
                    // set our toxilibs quaternion to new data
                    // quat.set(q[0], q[1], q[2], q[3]);

                }
            }
        }
    }

    public void setServo(int level){
        int i=0;
        string temp = servoControl;

        for(i=1;i<6;i++){
            char a = servoControl[i];
            a = (char)((int)a + level);

            if((int)a > 51){
                // print((int)a);
                a = '3';
            }

            temp = temp.Insert(i,a.ToString());
            temp = temp.Remove(i+1,1);
        }
        
        // print(temp);

        for(i=0;i<5;i++){
            SendString(temp);
        }
    }

    public void SendVibe(){
        int i=0;
        for(i=0;i<5;i++){
            SendString("s2e");
        }
    }

    public void DiscardBuffer(){
        sp.DiscardInBuffer();
    }

}