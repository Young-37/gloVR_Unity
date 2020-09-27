using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlLoadingScene : MonoBehaviour
{
  public Slider progressBar;
  public float max;

  private SerialPortHandler SPHandler;
  string start_string = "s1e";
  
  float timer;
  float waiting_time;


  // Start is called before the first frame update
  void Start()
  {
    SPHandler = GameObject.Find("SP").GetComponent<SerialPortHandler>();
    max = progressBar.maxValue;

    timer = 0;
    waiting_time = 2;

  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
  
    if(timer > waiting_time)
    {
      //serial port로 데이터 보내기
      SPHandler.SendString(start_string);

      //udp로 데이터 보내기


      //Serial으로 데이터 받기
      if(SPHandler.IsConnected()){
        progressBar.value += (float)0.3;
      }

      //udp로 데이터 받기
      
      

      

      
      // progressBar.value += Time.deltaTime;

      if (progressBar.value >= max)
      {
        SceneManager.LoadScene("GameScene");
      }
      
      timer = 0;
    }

  }
}
