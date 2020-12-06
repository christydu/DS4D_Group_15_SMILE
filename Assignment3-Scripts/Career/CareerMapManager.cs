using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CareerMapManager : MonoBehaviour {

	///*************************************************************************///
	/// CareerMapManager will load the game scene with parameters set by you
	/// for the selected level. It will saves those values inside playerPrefs and
	/// they will be fetched and applied in the game scene.
	///*************************************************************************///

	static public int userLevelAdvance;
	private int totalLevels;
	private GameObject[] levels;

	public AudioClip menuTap;
	private bool canTap;
	private float buttonAnimationSpeed = 9;

	void Awake (){
		canTap = true; //player can tap on buttons
		
		if(PlayerPrefs.HasKey("userLevelAdvance"))
			userLevelAdvance = PlayerPrefs.GetInt("userLevelAdvance");
		else
			userLevelAdvance = 0; //default. only level 1 in open.

		//cheat
		userLevelAdvance = 5;
	}

	void Start (){
		//prevent screenDim in handheld devices
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	void Update (){
		if(canTap)	
			StartCoroutine(tapManager());
	}


	///***********************************************************************
	/// Process user inputs
	///***********************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator tapManager (){

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
			
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			print(objectHit.name);
			if(objectHit.tag == "levelSelectionItem") {
				canTap = false;
				playSfx(menuTap);
				StartCoroutine(animateButton(objectHit));
				
				//save the game mode
				PlayerPrefs.SetString("gameMode", "CAREER");
				PlayerPrefs.SetInt("careerLevelID", objectHit.GetComponent<CareerLevelSetup>().levelID);

				//save number of available blenders
				PlayerPrefs.SetInt("careerAvailableBlender", objectHit.GetComponent<CareerLevelSetup>().availableBlender);

				
				//save level prize
				PlayerPrefs.SetInt("careerPrize", objectHit.GetComponent<CareerLevelSetup>().levelPrize);
				
				//save mission variables
				PlayerPrefs.SetInt("careerGoalBalance", objectHit.GetComponent<CareerLevelSetup>().careerGoalBalance);
				PlayerPrefs.SetInt("careerAvailableTime", objectHit.GetComponent<CareerLevelSetup>().careerAvailableTime);
				
				int availableProducts = objectHit.GetComponent<CareerLevelSetup>().availableProducts.Length;
				PlayerPrefs.SetInt("availableProducts", 1); //save the length of availableProducts
				for(int j = 0; j < availableProducts; j++) {
					PlayerPrefs.SetInt(	"careerProduct_" + j.ToString(), 
										objectHit.GetComponent<CareerLevelSetup>().availableProducts[j]);
				}
				
				PlayerPrefs.SetInt( "canUseCandy", 
									Convert.ToInt32(objectHit.GetComponent<CareerLevelSetup>().canUseCandy) );
				
				
				yield return new WaitForSeconds(0.25f);
				SceneManager.LoadScene("Game-c#");
			}

			if(objectHit.name == "BackButton") {
				playSfx(menuTap);
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene("RegionSelection"); // switch to "RegionSelection" Scene
				yield break;
			}
		}
	}


	///***********************************************************************
	/// Animate button by modifying it's scale
	///***********************************************************************
	IEnumerator animateButton ( GameObject _btn  ){
		Vector3 startingScale = _btn.transform.localScale;
		Vector3 destinationScale = startingScale * 1.1f;
		//yield return new WaitForSeconds(0.1f);
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
			                                        Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
			                                        _btn.transform.localScale.z);
			yield return 0;
		}
		
		float r = 0.0f; 
		if(_btn.transform.localScale.x >= destinationScale.x) {
			while (r <= 1.0f) {
				r += Time.deltaTime * buttonAnimationSpeed;
				_btn.transform.localScale = new Vector3( Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
				                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
				                                        _btn.transform.localScale.z);
				yield return 0;
			}
		}
		
		if(r >= 1)
			canTap = true;
	}


	///***********************************************************************
	/// play audio clip
	///***********************************************************************
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}


}
