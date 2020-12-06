using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToMonth : MonoBehaviour {

	// Use this for initialization
	public GameObject WestContinue;
    public GameObject EastContinue;
	public GameObject NorthContinue;

	void Start () {
		WestContinue.GetComponent<Button>().onClick.AddListener(OnClick1);
        EastContinue.GetComponent<Button>().onClick.AddListener(OnClick2);
        NorthContinue.GetComponent<Button>().onClick.AddListener(OnClick3);		
    }

    void OnClick1()
    {
        SceneManager.LoadScene("LevelSelection-c#"); //Switch to West region's month selection scene
    }

    void OnClick2()
    {
        SceneManager.LoadScene("LevelSelection2-c#"); //Switch to East region's month selection scene
    }

    void OnClick3()
    {
        SceneManager.LoadScene("LevelSelection3-c#"); //Switch to North region's month selectoin scene
    }
    // Update is called once per frame
    void Update () {

    }
}
