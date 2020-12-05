using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductMover : MonoBehaviour {

	//***************************************************************************//
	// This class manages user inputs (drags and touches) on ingredients panel,
	// and update main queue array accordingly.
	//***************************************************************************//

	//public variable.
	//do not edit these vars.
	//we get their values from other classes.
	public int factoryID;				//actual ingredient ID to be served to customer

	public GameObject juiceAnim;		//used to simulate juice pouring when we add a juice to the blender

	public AudioClip sliceSfx;			//used for normal fruits.
	public AudioClip pouringSfx;		//used just for liquied juices (ings 10, 11, 12)

	//sliced texture for this ingredient when we move it to blender
	public Texture2D[] slicedImage;

	//Private flags.
	private GameObject target;			//target object for this ingredient 
										//(blender, a processor machine, etc...)
	private bool canGetDragged;			//are we allowed to drag this blender to customer?

	private GameObject[] blenders;		//all available blenders inside the game
	private float[] distanceToBlenders; //distance to all available blender machines

	private bool isFinished;			//are we done with positioning and processing the ingredient on the blender?
	private float minDeliveryDistance;	//Minimum distance to blender required to land this ingredint on blender.
	private Vector3 tmpPos;				//temp variable for storing player input position on screen

	//player input variables
	private RaycastHit hitInfo;
	private Ray ray;

	//***************************************************************************//
	// Simple Init
	//***************************************************************************//
	void Start (){
		canGetDragged = true;
		minDeliveryDistance = 1.5f;
		isFinished = false;			//!Important : we use this flag to prevent ingredients to be draggable after placed on the blender.

		//find possible targets
		blenders = GameObject.FindGameObjectsWithTag("blender");

		distanceToBlenders = new float[blenders.Length];
		target = blenders[0];

		//print (gameObject.name + " - " + target.name);
		//print ("blenders.length: " + blenders.Length);
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
	// FSM
	//***************************************************************************//
	void Update (){

		for(int i = 0; i < blenders.Length; i++) {
			distanceToBlenders[i] = Vector3.Distance(blenders[i].transform.position, gameObject.transform.position);
			//find the correct (nearest blender) target
			target = blenders[(int)findMinInArray(distanceToBlenders).y];
			//debug
			//print ("Distance to blender-" + i.ToString() + " : " + distanceToBlenders[i]);
			//print ("Target is: " + target.name);
		}


		//if dragged
		if(Input.GetMouseButton(0) && canGetDragged) {
			followInputPosition();
		}
		
		//if released 
		if( (!Input.GetMouseButton(0) && Input.touches.Length < 1) && !isFinished) {
			canGetDragged = false;
			checkCorrectPlacement();
		}

		//Optional - change target's color when this ingredient is near enough
		if(target.tag == "blender")
			changeTargetsColor(target);

	}


	//***************************************************************************//
	// change blender's color when dragged ingredients are near enough
	//***************************************************************************//
	private float myDistance;
	void changeTargetsColor(GameObject _target) {
		if(!IngredientsController.itemIsInHand)	//if nothing is being dragged
			return;
		else if(_target.tag == "blender" && _target.GetComponent<BlenderController>().deliveryQueueIsFull)
			return;
		else {
			myDistance = Vector3.Distance(_target.transform.position, gameObject.transform.position);
			//print("myDistance: " + myDistance);
			if(myDistance < minDeliveryDistance) {
				//change target's color to let the player know this is the correct place to release the items.
				_target.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
			} else {
				//change target's color back to normal
				_target.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
			}
		}
	}


	//***************************************************************************//
	// Check if the ingredients are dragged into the blender. Otherwise delete them.
	//***************************************************************************//
	void checkCorrectPlacement() {

		//if this ingredient is close enough to serving blender, we can add it to main queue. otherwise drop and delete it.
		float distanceToBlender = Vector3.Distance(target.transform.position, gameObject.transform.position);
		//print("distanceToBlender: " + distanceToBlender);
		
		if(distanceToBlender <= minDeliveryDistance && !target.GetComponent<BlenderController>().deliveryQueueIsFull) {

			//add it to the blender queue
			target.GetComponent<BlenderController>().deliveryQueueItems++;
			target.GetComponent<BlenderController>().deliveryQueueItemsContent.Add(factoryID);

			//close enough to land on blender
			transform.parent = target.transform;

			//play slice or pouring sfx
			if(factoryID >= 1 && factoryID <= 9)
				playSfx(sliceSfx);
			else if(factoryID >= 10 && factoryID <= 12)
				playSfx(pouringSfx);

			//set the correct position in blender
			if(factoryID >= 10 && factoryID <= 12) {
				StartCoroutine(animateJuice());
				transform.position = new Vector3(target.transform.position.x + 0.15f,
				                                 target.transform.position.y + 0.45f,
				                                 target.transform.position.z + 0.2f - (0.02f * target.GetComponent<BlenderController>().deliveryQueueItems + 0.1f));
			} else {
				StartCoroutine(animateFruit());
				transform.position = new Vector3(target.transform.position.x + 0.15f,
				                                 target.transform.position.y - 0.15f + (0.25f * target.GetComponent<BlenderController>().deliveryQueueItems),
				                                 target.transform.position.z + 0.25f - (0.02f * target.GetComponent<BlenderController>().deliveryQueueItems + 0.1f));
			}

            //change texture to sliced
            GetComponent<Renderer>().enabled = false;

			//correct the scale
			if(factoryID >= 10 && factoryID <= 12) {
				transform.localScale = new Vector3((transform.localScale.x * 0.7f),
													0.001f,
													transform.localScale.z * 1.0f);
			} else {
				transform.localScale = new Vector3(transform.localScale.x * 0.55f + (target.GetComponent<BlenderController>().deliveryQueueItems * 0.025f),
				                                   0.001f,
				                                   transform.localScale.z * 0.5f);
			}


			//scale correction
			//ftransform.localScale *= 0.5f;

			//we no longer need this ingredient's script (ProductMover class)
			GetComponent<ProductMover>().enabled = false;
		} else {
			Destroy(gameObject);
		}

		//change blender's color back to normal
		target.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
		
		//Not draggable anymore.
		isFinished = true;
	}


	IEnumerator animateFruit() {
		transform.position = new Vector3(target.transform.position.x,
		                                 target.transform.position.y,
		                                 target.transform.position.z);

		yield return new WaitForSeconds(3.0f);
	}


	IEnumerator animateJuice() {
		//hide the juice for now
		GetComponent<Renderer>().enabled = false;
		
		//create a juice animation
		//GameObject juice = Instantiate (juiceAnim, 
		//                                target.transform.position + new Vector3(0.15f, -0.25f, 0.05f), 
		//                                Quaternion.Euler(0, 0, 0)) as GameObject;
		//juice.name = "Juice";

		yield return new WaitForSeconds(1.5f);
		
		//destroy the juice animation prefab and unhide the static juice 
		GetComponent<Renderer>().enabled = true;
	}


	//***************************************************************************//
	// Follow players mouse or finger position on screen.
	//***************************************************************************//
	private Vector3 _Pos;
	void followInputPosition (){
		_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Custom offset. these objects should be in front of every other GUI instances.
		_Pos = new Vector3(_Pos.x, _Pos.y, -0.5f);
		//follow player's finger
		transform.position = _Pos + new Vector3(0, 0, 0);
	}


	//***************************************************************************//
	// Follow players mouse or finger position on screen.
	// This is an IEnumerator and run independent of game main cycle
	//***************************************************************************//
	IEnumerator followInputTimeIndependent() {
		while(IngredientsController.itemIsInHand) {

			tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tmpPos = new Vector3(tmpPos.x, tmpPos.y, -0.5f);
			transform.position = tmpPos + new Vector3(0, 0, 0);

			//if user release the input, check if we delivered the processed ingredient to the blender or we just release it nowhere!
			if(Input.touches.Length < 1 && !Input.GetMouseButton(0)) {
				//if we delivered it to the blender
				if(Vector3.Distance(target.transform.position, gameObject.transform.position) <= minDeliveryDistance) {
					print ("Landed on blender in: " + Time.time);

					gameObject.tag = "deliveryQueueItem";
					gameObject.GetComponent<MeshCollider>().enabled = false;
					transform.position = new Vector3(target.transform.position.x,
					                                 target.transform.position.y + (0.35f * target.GetComponent<BlenderController>().deliveryQueueItems),
					                                 target.transform.position.z - (0.2f * target.GetComponent<BlenderController>().deliveryQueueItems + 0.1f));
					
					transform.parent = target.transform;
					target.GetComponent<BlenderController>().deliveryQueueItems++;
					target.GetComponent<BlenderController>().deliveryQueueItemsContent.Add(factoryID);
					//change blender's color back to normal
					target.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
					//we no longer need this ingredient's script (ProductMover class)
					GetComponent<ProductMover>().enabled = false;
					yield break;

				} 
			}

			yield return 0;
		}
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