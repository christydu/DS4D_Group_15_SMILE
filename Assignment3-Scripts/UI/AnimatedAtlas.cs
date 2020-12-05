using UnityEngine;
using System.Collections;

public class AnimatedAtlas : MonoBehaviour {
		
	//***************************************************************************//
	// This class will animate an atlas texture by changing it's frame and set it on the material.
	//***************************************************************************//

	public Texture2D[] availableAnimations;
	private int randomAnimIndex = 0;

	public int uvAnimationTileX; 
	public int uvAnimationTileY;
	public float framesPerSecond = 10.0f;

	private int index;
	private Vector2 size;
	private float uIndex;
	private float vIndex;
	private Vector2 offset;

	void Start() {
		randomAnimIndex = Random.Range(0, availableAnimations.Length);
		GetComponent<Renderer>().material.mainTexture = availableAnimations[randomAnimIndex];
	}

	void Update (){
	    index = (int)(Time.time * framesPerSecond);
	    index = index % (uvAnimationTileX * uvAnimationTileY);
	    size = new Vector2 (1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);
	    uIndex = index % uvAnimationTileX;
	    vIndex = index / uvAnimationTileX;
	    offset = new Vector2 (uIndex * size.x, 1.0f - size.y - vIndex * size.y);
	    GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
	    GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);
	}
}