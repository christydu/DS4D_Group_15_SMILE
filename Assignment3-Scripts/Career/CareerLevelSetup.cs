using UnityEngine;
using System.Collections;

public class CareerLevelSetup : MonoBehaviour {
	
	///*************************************************************************///
	/// Use this class to set different missions for each level.
	/// when you click/tap on any level button, these values automatically get saved 
	/// inside playerPrefs and then get read when the game starts.
	///*************************************************************************///

	public GameObject label;				//reference to child gameObject
	public int levelID;						//unique level identifier. Starts from 1.

	//there is no limitation on how many blender you want to use inside a level,
	//but if you want to override the default value of 1 or 2, you have to also make the required changes in 
	//"MainGameController" class.
	[Range(1, 2)]
	public int availableBlender = 1;		//total available blender machines inside the level

	public int levelPrize = 150;			//prize (money) given to player if level is finished successfully
	public int careerGoalBalance = 1500;	//mission goal
	public int careerAvailableTime = 30;	//mission time
	public bool canUseCandy = true;			//are we allowed to use candy
	public int[] availableProducts;			//array of indexes of available products. starts from 1.

	//star rating for levels
	//start rating checks player saved (passed) time with the target time, and based on a fixed difference,
	//grants 3, 2 or 1 star. Unlocked levels will always show 0-star image.
	public GameObject levelStarsGo;	//reference to child game object
	public Material[] starMats;		//avilable start materials
	private float levelSavedTime;	//time record for this level
	private float timeDifference;	//difference between player saved time & target time


	void Start (){
		
		if(CareerMapManager.userLevelAdvance >= levelID - 1) {
			//this level is open
			GetComponent<BoxCollider>().enabled = true;
			label.GetComponent<TextMesh>().text = levelID.ToString();
			GetComponent<Renderer>().material.color = new Color(1,1,1,1);

			//grant a few stars
			levelSavedTime = PlayerPrefs.GetFloat("Level-" + levelID.ToString() , careerAvailableTime);
			timeDifference = careerAvailableTime - levelSavedTime;
			if (timeDifference > 60) {
				//3-star
				levelStarsGo.GetComponent<Renderer>().material = starMats[3];
			} else if (timeDifference <= 60 && timeDifference > 30) {
				//2-star
				levelStarsGo.GetComponent<Renderer>().material = starMats[2];
			} else if (timeDifference <= 30 && timeDifference > 0) {
				//1-star
				levelStarsGo.GetComponent<Renderer>().material = starMats[1];
			} else if (timeDifference <= 0) {	//onlu occures if this is the first time we want to play this level
				//0-star
				levelStarsGo.GetComponent<Renderer>().material = starMats[0];
			}

		} else {
			//level is locked
			GetComponent<BoxCollider>().enabled = false;
			label.GetComponent<TextMesh>().text = "Locked";
			GetComponent<Renderer>().material.color = new Color(1,1,1,0.5f);

			//set 0-star image
			levelStarsGo.GetComponent<Renderer>().material = starMats[0];
			levelStarsGo.GetComponent<Renderer>().material.color = new Color(1,1,1,0.5f);
		}
	}
}