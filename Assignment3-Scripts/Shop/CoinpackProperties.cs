using UnityEngine;
using System.Collections;

public class CoinpackProperties : MonoBehaviour {
	
	///*************************************************************************///
	/// A very simple value holder for different coinpack items.
	// You can easily add/edit properties via this controller.
	///*************************************************************************///

	public int itemIndex;
	public float itemPrice;
	public int itemValue;

	//GameObjects
	public GameObject nameGo;
	public GameObject priceGo;
	public GameObject currentInventory; //optional

	void Start () {

		if(itemIndex == 4) {
			//this is an IAP for Candy Items
			nameGo.GetComponent<TextMesh>().text = itemValue + " Candy";
			if(currentInventory)
				currentInventory.GetComponent<TextMesh>().text = "You already have " + PlayerPrefs.GetInt("availableCandy");
		} else {
			//this is an IAP for coin pack
			nameGo.GetComponent<TextMesh>().text = itemValue + " Coins";
		}


		priceGo.GetComponent<TextMesh>().text = "Buy for $" + itemPrice;
	}

}