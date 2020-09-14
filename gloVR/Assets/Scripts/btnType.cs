using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class btnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    private void Start(){
        defaultScale = buttonScale.localScale;
    }

    public void OnBtnClick(){

        switch(currentType){
            case BTNType.Start:
            SceneManager.LoadScene("HandMotion");
            break;

            case BTNType.Setting:
            Debug.Log("Setting");
            break;

            case BTNType.Exit:
            Application.Quit();
            Debug.Log("Exit!");
            break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData){
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData){
        buttonScale.localScale = defaultScale;
    }
}