using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlGameOverScene : MonoBehaviour
{
	private Text text;

    // Start is called before the first frame update
    void Start()
    {
		text = GetComponent<Text>();
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
	}

    // Update is called once per frame
    void Update()
    {
		if (text.color.a < 1.0f)
		{
			text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime/3.0f));
		}

		if (text.color.a >= 1.0f)
		{
			SceneManager.LoadScene("MainMenu_0");
		}
    }
}
