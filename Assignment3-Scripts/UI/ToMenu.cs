using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour {
	
	// Use this for initialization
	public GameObject Backbutton_R;

	void Start () {
		Backbutton_R.GetComponent<Button>().onClick.AddListener(OnClick1);
    }

    void OnClick1()
    {
        SceneManager.LoadScene("Menu-c#"); //back to main menu scene
    }

}
