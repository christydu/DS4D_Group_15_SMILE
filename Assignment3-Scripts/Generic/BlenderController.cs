using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlenderController : MonoBehaviour {

	//***************************************************************************//
	// This calss will manages all things related to the blender, including
	// handing it over to the customers, trashing it, drag and release events, etc...
	//***************************************************************************//
	
	public GameObject[] availableProducts;		//reference to all available products (prefabs)
												//useful when we want to use an image for the product we made
	public Texture2D rubbishProductImage;		//image used for wrongly made products.
	public Texture2D blenderTexture;			//main texture of the blender machine

	public GameObject smokeFx;		//smoke used to show after blender transforms into a final product

	//Static variable to let customers know they can receive their orders
	public bool canDeliverOrder;
	public bool isReadyToServe; 	//if we have done with blending and have picked up the final product to deliver it to customers

	//Private flags
	private Vector3 initialPosition;
	private Vector3 initialScale;
	private GameObject trashbin;

	//blend status
	public AudioClip blenderSfx;
	public AudioClip productReadySfx;
	internal bool isBlending;
	internal bool blendIsFinished;
	public GameObject blenderStartButton;
	//public GameObject blenderAnimation;
	public int blendTimer = 6; //default blending time (seconds)

	//AudioClip
	public AudioClip trashSfx;

	//Queue variables for each blender , so we can have more than 1 blender!
	internal bool deliveryQueueIsFull;								//delivery queue can accept 4 ingredients. more is not acceptable.
	internal int deliveryQueueItems;								//number of items in delivery queue
	internal List<int> deliveryQueueItemsContent = new List<int>();	//contents of delivery queue
	static public int maxSlotState = 4;								//maximum available slots in delivery queue


	//***************************************************************************//
	// Simple Init
	//***************************************************************************//
	void Awake (){
		canDeliverOrder = true;
		blendIsFinished = false;
		isBlending = false;
		isReadyToServe = false;

		initialPosition = transform.position;
        initialScale = transform.localScale;
        trashbin = GameObject.FindGameObjectWithTag("trashbin");

		deliveryQueueIsFull = false;
		deliveryQueueItems = 0;
		deliveryQueueItemsContent.Clear();

		//check for blender upgrade
		if(PlayerPrefs.HasKey("UpgradedBlenderTime")) {
			if(PlayerPrefs.GetInt("UpgradedBlenderTime") > 0)
				blendTimer = PlayerPrefs.GetInt("UpgradedBlenderTime");
		}
	}


	//***************************************************************************//
	// FSM
	//***************************************************************************//
	void Update() {

		if(deliveryQueueItems >= maxSlotState || blendIsFinished)
			deliveryQueueIsFull = true;
		else	
			deliveryQueueIsFull = false;

		//check player interactions (click/touch) on blender buttons
		//checkInput();

		//if(blendIsFinished)
			manageDeliveryDrag(); 	//without the if, it allow us to be able to recycle the wrongly made product

		//play leds animation
		//if(led.Length > 0 && isBlending)
		//	StartCoroutine(playLedAnimation());


		//Monitor blender start button status
		if(deliveryQueueItems == 1 && !isBlending && !blendIsFinished) {
			blenderStartButton.GetComponent<Renderer>().enabled = true;
			blenderStartButton.GetComponent<BoxCollider>().enabled = true;
			//blenderAnimation.GetComponent<Renderer>().enabled = false;
		} else {
			blenderStartButton.GetComponent<Renderer>().enabled = false;
			blenderStartButton.GetComponent<BoxCollider>().enabled = false;
		}
	}


//	private bool ledAnimFlag = true;
//	private float ledSpeed = 0.05f;
	/**IEnumerator playLedAnimation() {

		if(ledAnimFlag && isBlending) {
			ledAnimFlag = false;
			int i;
			for(i = 0; i < led.Length; i++) {
				yield return new WaitForSeconds(ledSpeed);
				led[i].GetComponent<Renderer>().enabled = true;
			}

			if(i == led.Length) {
				for(int j = 0; j < led.Length; j++) {
					yield return new WaitForSeconds(ledSpeed);
					led[j].GetComponent<Renderer>().enabled = false;
				}
			}

			//yield return new WaitForSeconds(0.1f);
			ledAnimFlag = true;
		}
	}

    **/

	//***************************************************************************//
	// If we are starting our drag on blender, move the blender with our touch/mouse...
	//***************************************************************************//
	private RaycastHit hitInfo;
	private Ray ray;
	void manageDeliveryDrag (){
		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Moved)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonDown(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			return;
			
		if(Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			if(objectHit.tag == "blender" && 
			   objectHit.name == gameObject.name && 
			   !IngredientsController.itemIsInHand && 
			   !isBlending &&
			   !JuiceAnimation.juiceAnimationIsRunning) {
				StartCoroutine(createDeliveryPackage());
			}

			if(objectHit.name == "BlenderStartButton" && objectHit.transform.parent.gameObject == gameObject) {
				StartCoroutine(blendIngredients());
			}
		}
	}


	IEnumerator blendIngredients() {

		//destroy sliced ingredients
		GameObject[] DeliveryQueueItems = GameObject.FindGameObjectsWithTag("deliveryQueueItem");
		foreach(GameObject item in DeliveryQueueItems) {
			if(item.transform.parent.gameObject == gameObject)
				Destroy(item);
		}

		blenderStartButton.GetComponent<Renderer>().enabled = false;
		blenderStartButton.GetComponent<BoxCollider>().enabled = false;
		playSfx(blenderSfx);
		//isBlending = true;
		//blenderAnimation.GetComponent<Renderer>().enabled = true;

		//yield return new WaitForSeconds(blendTimer);

		blendIsFinished = true;
		isBlending = false;
		//blenderAnimation.GetComponent<Renderer>().enabled = false;
		playSfx(productReadySfx);

		//hide the product to show the smoke
		GetComponent<Renderer>().enabled = false;

        //create smoke fx
        //print("烟雾来啦");
		GameObject smoke = Instantiate(smokeFx,
		                               transform.position + new Vector3(0, 0.25f, 0), 
		                               Quaternion.Euler(0, 180, 0)) as GameObject;
		smoke.name = "Smoke";

		//transform the blender into a product based on customers request
		//first check which product prefab matches the ingredients requested by the customer?
		int customerProductIndex = 0;
		int[] mainProductIngredientsIDs;
		bool detectInequality = false;
		//sort our mix ingredient to ease the comparison in the next step
		deliveryQueueItemsContent.Sort();
		//loop through all available products (prefabs) to check our current mix with their formula
		int i = 0;
		for(i = 0; i < availableProducts.Length; i++) {
			//if these two mix have the same size, move on to check deeper 
			if(deliveryQueueItemsContent.Count == availableProducts[i].GetComponent<ProductManager>().totalIngredients) {
				print ("Array length seems equal. move on to check more details...");
				//sort the product (prefab) ingredients by int
				mainProductIngredientsIDs = availableProducts[i].GetComponent<ProductManager>().ingredientsIDs;
				Array.Sort(mainProductIngredientsIDs);
				for(int j = 0; j < deliveryQueueItemsContent.Count; j++) {
					detectInequality = false;
					if(deliveryQueueItemsContent[j] != mainProductIngredientsIDs[j]) {
						//print ("Product " + i + " is not a match!  Move on to the next product.");
						detectInequality = true;
						break;
					} else {
						if(!detectInequality && j == deliveryQueueItemsContent.Count - 1) {
							//print ("Product " + i + " is a match!!!!");
							customerProductIndex = i;
							detectInequality = false;
							i = availableProducts.Length;	//we use this trick to break out of the outer (main) for loop
							break;
						}
					}
				}
			}
		}

		print ("i is: " + customerProductIndex);

		if(!detectInequality)
			GetComponent<Renderer>().material.mainTexture = availableProducts[customerProductIndex].GetComponent<Renderer>().sharedMaterial.mainTexture;
		else
			GetComponent<Renderer>().material.mainTexture = rubbishProductImage;

		transform.localScale *= 0.6f;	//change the scale of the final product

		//unhide the product
		yield return new WaitForSeconds(0.5f);
		GetComponent<Renderer>().enabled = true;
	}


	//***************************************************************************//
	// Move the blender
	//***************************************************************************//
	private Vector3 _Pos;
	IEnumerator createDeliveryPackage (){
		while(canDeliverOrder && deliveryQueueItems > 0) {
			//follow mouse or touch
			_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_Pos = new Vector3(_Pos.x, _Pos.y, -0.5f);
			//follow player's finger
			transform.position = _Pos + new Vector3(0, 0, 0);

			//while we are dragging the product, this var will be true. but as soon as we released the drag, it will be false.
			isReadyToServe = true;

			//Optional - can transparent, when dragged
			/*GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
			                                    GetComponent<Renderer>().material.color.g,
			                                    GetComponent<Renderer>().material.color.b,
			                                    0.5f);*/
			
			//deliver (dragging the blender) is not possible when user is not touching screen
			//so we must decide what we are going to do after dragging and releasing the blender
			if(Input.touches.Length < 1 && !Input.GetMouseButton(0)) {

				//we no longer have control over the product
				isReadyToServe = false;

				//if we are giving the order to a customer (blender is close enough)
				GameObject[] availableCustomers = null;
				availableCustomers = GameObject.FindGameObjectsWithTag("customer");
				
				//if there is no customer in shop, or we have not blended the product ingredients, 
				//take the blender back, because it's not deliverable
				if(availableCustomers.Length < 1 || !blendIsFinished) {
					//take the blender back to it's initial position
					resetPosition();
					yield break;
				}
				
				bool delivered = false;
				GameObject theCustomer = null;
				for(int cnt = 0; cnt < availableCustomers.Length; cnt++) {
					if(availableCustomers[cnt].GetComponent<CustomerController>().isCloseEnoughToDelivery) {
						//we know that just 1 customer is always nearest to the delivery. so "theCustomer" is unique.
						theCustomer = availableCustomers[cnt];
						delivered = true;
					}
				}
				
				//if customer got the delivery
				if(delivered) {
					//deliver the order
					//deliveredProduct= new Array();
					List<int> deliveredProduct = new List<int>();
					
					//contents of the delivery which customer got from us.
					deliveredProduct = deliveryQueueItemsContent;

					//debug delivery
					for(int i = 0; i < deliveryQueueItemsContent.Count; i++) {
						//print("Delivery Items ID " + i.ToString() + " = " + deliveryQueueItemsContent[i]);
					}
					
					//let the customers know what he got.
					theCustomer.GetComponent<CustomerController>().receiveOrder(deliveredProduct);
					
					//reset main queue
					deliveryQueueItems = 0;
					deliveryQueueIsFull = false;
					deliveryQueueItemsContent.Clear();

					//destroy the contents of the serving blender.
					GameObject[] DeliveryQueueItems = GameObject.FindGameObjectsWithTag("deliveryQueueItem");
					foreach(GameObject item in DeliveryQueueItems) {
						if(item.transform.parent.gameObject == gameObject)
							Destroy(item);
					}

					//prepare for a new blend
					blendIsFinished = false;

					//reset the texture and scale of the blender
					GetComponent<Renderer>().material.mainTexture = blenderTexture;
					transform.localScale = initialScale;
						
					//take the blender back to it's initial position
					resetPosition();
					
				} else {
					resetPosition();
				}
			} 

			yield return 0;
		}
	}


	//***************************************************************************//
	// Move the blender to it's initial position.
	// we also check if user wants to trash his delivery, before any other process.
	// this way we can be sure that nothing will interfere with deleting the delivery. (prevents many bugs)
	//***************************************************************************//
	void resetPosition (){
		//just incase user wants to move this to trashbin, check it here first
		if(trashbin.GetComponent<TrashBinController>().isCloseEnoughToTrashbin) {
			//empty blender contents
			playSfx(trashSfx);
			deliveryQueueItems = 0;
			deliveryQueueIsFull = false;
			deliveryQueueItemsContent.Clear();

			blendIsFinished = false;
			isBlending = false;

			//reset the texture and scale of the blender
			GetComponent<Renderer>().material.mainTexture = blenderTexture;
			transform.localScale = initialScale;

			GameObject[] DeliveryQueueItems = GameObject.FindGameObjectsWithTag("deliveryQueueItem");
			foreach(GameObject item in DeliveryQueueItems) {
				if(item.transform.parent.gameObject == gameObject)
					Destroy(item);
			}
		}

		//take the blender back to it's initial position
		//print("Back to where we belong");
		transform.position = initialPosition;
		GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
						                                    GetComponent<Renderer>().material.color.g,
						                                    GetComponent<Renderer>().material.color.b,
						                                    1);

		canDeliverOrder = false;
		StartCoroutine(reactivate());
	}


	//***************************************************************************//
	// make the delivery blender draggable again.
	//***************************************************************************//
	IEnumerator reactivate (){
		yield return new WaitForSeconds(0.25f);
		canDeliverOrder = true;
	}


}
