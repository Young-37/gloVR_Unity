using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlLoadingScene : MonoBehaviour
{
	public Slider progressBar;
	public float max;

    // Start is called before the first frame update
    void Start()
    {
		max = progressBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
		progressBar.value += Time.deltaTime;

		if (progressBar.value >= max)
		{
			SceneManager.LoadScene("GameScene");
		}
    }
}
