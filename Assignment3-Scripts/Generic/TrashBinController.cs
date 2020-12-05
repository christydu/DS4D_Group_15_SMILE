using UnityEngine;
using System.Collections;

public class TrashBinController : MonoBehaviour {

	//***************************************************************************//
	// This class manages all thing related to TrashBin.
	// 
	//***************************************************************************//

	//AudioClip
	public AudioClip deleteSfx;

	//Flags
	internal bool canDelete = true;

	private GameObject[] blenders;		//all available blenders inside the game
	private float[] distanceToBlenders; //distance to all available blender machines
	private GameObject target;

	//Textures for open/closed states
	public Texture2D[] state;

	//Flag used to let managers know that player is intended to send the order to trashbin.
	public bool isCloseEnoughToTrashbin;//Do not modify this.


	//***************************************************************************//
	// Simple Init
	//***************************************************************************//
	void Awake (){

		target = null;

		blenders = GameObject.FindGameObjectsWithTag("blender");
		distanceToBlenders = new float[blenders.Length];

		isCloseEnoughToTrashbin = false;
		GetComponent<Renderer>().material.mainTexture = state[0];
	}


	//***************************************************************************//
	// FSM
	//***************************************************************************//
	void Update (){	

		for(int i = 0; i < blenders.Length; i++) {
			distanceToBlenders[i] = Vector3.Distance(blenders[i].transform.position, gameObject.transform.position);
			//find the correct (nearest blender) target
			target = blenders[(int)findMinInArray(distanceToBlenders).y];
		}

		//check if player wants to move the order to trash bin
		checkDistanceToDelivery();
	}


	///**********************************************************
	/// Takes an array and return the lowest number in it, 
	/// with the optional index of this lowest number (position of it in the array)
	///**********************************************************
	Vector2 findMinInArray(float[] _array) {
		int lowestIndex = -1;
		float minval = 1000000.0f;
		for (int i = 0; i < _array.Length; i++) {
			if (_array[i] < minval)  {
				minval = _array[i];
				lowestIndex = i;
			}
		}
		//return the Vector2(minimum population, index of this minVal in the argument Array)
		return(new Vector2(minval, lowestIndex));
	}


	//***************************************************************************//
	// If player is dragging the blender, check if maybe he wants to trash it.
	// we do this by calculation the distance of blender and trashBin.
	//***************************************************************************//
	private float myDistance;
	void checkDistanceToDelivery (){
		myDistance = Vector3.Distance(transform.position, target.transform.position);
		//print("distance to trashBin is: " + myDistance + ".");
		
		//2.0f is a hardcoded value. specify yours with caution.
		if(myDistance < 2.0f) {
			isCloseEnoughToTrashbin = true;
			//change texture
			GetComponent<Renderer>().material.mainTexture = state[1];
		} else {
			isCloseEnoughToTrashbin = false;
			//change texture
			GetComponent<Renderer>().material.mainTexture = state[0];
		}
	}


	/// <summary>
	/// Allow other controllers to update the animation state of this trashbin object
	/// by controlling it's door state.
	/// </summary>
	public void updateDoorState(int _state) {
		if(_state == 1)
			GetComponent<Renderer>().material.mainTexture = state[1];
		else
			GetComponent<Renderer>().material.mainTexture = state[0];
	}


	//***************************************************************************//
	// Activate using trashbin again, after a few seconds.
	//***************************************************************************//
	IEnumerator reactivate (){
		yield return new WaitForSeconds(0.25f);
		canDelete = true;
	}


	//***************************************************************************//
	// Play audioclips.
	//***************************************************************************//
	public void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}

}