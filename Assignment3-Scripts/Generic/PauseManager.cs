using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using admob;

public class PauseManager : MonoBehaviour {

	public static double savehulu;
	//***************************************************************************//
	// This class manages pause and unpause states.
	//***************************************************************************//
	public static bool  soundEnabled;
	public static bool  isPaused;
	private float savedTimeScale;
	public GameObject pausePlane;

	private GameObject AdManagerObject;

	enum Page {
		PLAY, PAUSE
	}
	private Page currentPage = Page.PLAY;


	void Awake (){		
		soundEnabled = true;
		isPaused = false;
		
		Time.timeScale = 1.0f;

		//AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
		
		if(pausePlane)
	    	pausePlane.SetActive(false); 
	}


	void Update (){
		//touch control
		touchManager();
		
		//optional pause
		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) {
			//PAUSE THE GAME
			switch (currentPage) {
	            case Page.PLAY: 
					PauseGame(); 
					break;
	            case Page.PAUSE: 
					UnPauseGame(); 
					break;
	            default: 
					currentPage = Page.PLAY;
					break;
	        }
		}

		//debug restart
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}


	void touchManager (){
		if(Input.GetMouseButtonUp(0)) {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo)) {
				string objectHitName = hitInfo.transform.gameObject.name;
				switch(objectHitName) {
					case "PauseBtn":
					
						//pause is not allowed when game is finished
						if(MainGameController.gameIsFinished)
							return;
							
						switch (currentPage) {
				            case Page.PLAY: 
								PauseGame(); 
								break;
				            case Page.PAUSE: 
								UnPauseGame(); 
								break;
				            default: 
								currentPage = Page.PLAY;
								break;
				        }
						break;
					
					case "Btn-Resume":
						switch (currentPage) {
				            case Page.PLAY: 
								PauseGame(); 
								break;
				            case Page.PAUSE: 
								UnPauseGame(); 
								break;
				            default: 
								currentPage = Page.PLAY;
								break;
				        }
						break;
						
					case "Btn-Menu":
						UnPauseGame();
						SceneManager.LoadScene("Menu-c#");
						break;
						
					case "Btn-Restart":
						UnPauseGame();
                        //Time.timeScale = 0;
                        MainGameController.countMinutes = 0;
                        MainGameController.hours= 7;
                        //print("虫灾");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        //Time.timeScale = 0;
                        print(Time.timeSinceLevelLoad);
						break;
						
					case "End-Menu":
						SceneManager.LoadScene("Menu-c#");
						break;
						
					case "End-Restart":
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
						break;
				}
			}
		}
	}


	void PauseGame (){
		print("Game in Paused...");

		//show an Interstitial Ad when the game is paused
		//if(AdManagerObject)
		//	AdManagerObject.GetComponent<AdManager>().showInterstitial();

		isPaused = true;
		savehulu=MainGameController.timeload;
       //print("timeload-" + savehulu);
        MainGameController.timeload = 0;
        //print("timeload-" + MainGameController.timeload);
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
	    AudioListener.volume = 0;
	    if(pausePlane)
	    	pausePlane.SetActive(true);
	    currentPage = Page.PAUSE;
	}


	void UnPauseGame (){
		//print("Unpause");
	    isPaused = false;
	    MainGameController.timeload=savehulu;
	    Time.timeScale = savedTimeScale;
	    AudioListener.volume = 1.0f;
		if(pausePlane)
	    	pausePlane.SetActive(false);   
	    currentPage = Page.PLAY;
	}

}