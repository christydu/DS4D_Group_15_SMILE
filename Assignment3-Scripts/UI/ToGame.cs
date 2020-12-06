using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour {
	// 12 level buttons to choose under each region
	// West region
	public GameObject level1_W;
    public GameObject level2_W;
	public GameObject level3_W;
	public GameObject level4_W;
    public GameObject level5_W;
	public GameObject level6_W;	
	public GameObject level7_W;
    public GameObject level8_W;
	public GameObject level9_W;
	public GameObject level10_W;
    public GameObject level11_W;
	public GameObject level12_W;

	// East region
	public GameObject level1_E;
    public GameObject level2_E;
	public GameObject level3_E;
	public GameObject level4_E;
    public GameObject level5_E;
	public GameObject level6_E;	
	public GameObject level7_E;
    public GameObject level8_E;
	public GameObject level9_E;
	public GameObject level10_E;
    public GameObject level11_E;
	public GameObject level12_E;

	// North region
	public GameObject level1_N;
    public GameObject level2_N;
	public GameObject level3_N;
	public GameObject level4_N;
    public GameObject level5_N;
	public GameObject level6_N;	
	public GameObject level7_N;
    public GameObject level8_N;
	public GameObject level9_N;
	public GameObject level10_N;
    public GameObject level11_N;
	public GameObject level12_N;

	// Use this for initialization
	void Start () {
		level1_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level2_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level3_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level4_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level5_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level6_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level7_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level8_W.GetComponent<Button>().onClick.AddListener(OnClick1);	
		level9_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level10_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level11_W.GetComponent<Button>().onClick.AddListener(OnClick1);
		level12_W.GetComponent<Button>().onClick.AddListener(OnClick1);	

		//East
		level1_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level2_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level3_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level4_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level5_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level6_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level7_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level8_E.GetComponent<Button>().onClick.AddListener(OnClick1);	
		level9_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level10_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level11_E.GetComponent<Button>().onClick.AddListener(OnClick1);
		level12_E.GetComponent<Button>().onClick.AddListener(OnClick1);	
		
		//North
		level1_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level2_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level3_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level4_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level5_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level6_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level7_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level8_N.GetComponent<Button>().onClick.AddListener(OnClick1);	
		level9_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level10_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level11_N.GetComponent<Button>().onClick.AddListener(OnClick1);
		level12_N.GetComponent<Button>().onClick.AddListener(OnClick1);	

	}
	
	// Update is called once per frame
    void OnClick1()
    {
        SceneManager.LoadScene("Game-c#");
    }
}
