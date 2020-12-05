using UnityEngine;
using System.Collections;

public class ProductManager : MonoBehaviour {

	//***************************************************************************//
	// This class is a placeholder for products in the game.
	// You can define and add/remove/edit product attributes with the provided 
	// variables. all modifications should be done to prefabs, via inspector.
	// nothing should be changed in this class by hardcoding.
	//***************************************************************************//

	//product's price
	//Amount of money that customer pays after receiving his/her order
	public int price = 100;

	//Number of ingredients this product consists.
	//must be between 2 to 4 item.
	public int totalIngredients = 1;
	//to adjust the ingredient number to simulate alarm situation.

	//In the inspector, use the number above "totalIngredients" as the lentgh of this array.
	//then assign a proper ID of desired ingredients to array's childs.
	
	public int[] ingredientsIDs;
    //for example
    // a custom product definition is like this:
    /*
	totalIngredients = 4;
	ingredientsIDs[0] = 1;
	ingredientsIDs[1] = 4;
	ingredientsIDs[2] = 7;
	ingredientsIDs[3] = 12;
	*/

    //Another example
    /*
	totalIngredients = 3;
	ingredientsIDs[0] = 1;
	ingredientsIDs[1] = 2;
	ingredientsIDs[2] = 8;
	*/

}
