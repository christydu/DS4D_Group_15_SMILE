using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour {
		
	/// <summary>
	/// This class simulates a heart-beat animation (by modifying the scales)
	/// when being attached to any 3D object.
	/// </summary>

	public bool runOnce	= false;	//run only once
	public float intensity = 1.2f;	//size increse
	public float animSpeed = 1.0f;	//animation speed

	private bool  animationFlag;
	private float startScaleX;
	private float startScaleY;
	private float endScaleX;
	private float endScaleY;
	private bool stopAnimation;


	void Start (){
		animationFlag = true;
		stopAnimation = false;
		startScaleX = transform.localScale.x;
		startScaleY = transform.localScale.y;
		endScaleX = startScaleX * intensity;
		endScaleY = startScaleY * intensity;
	}


	void Update (){
		if(animationFlag && !stopAnimation) {
			animationFlag = false;
			StartCoroutine(animatePulse(this.gameObject));
		}
	}


	IEnumerator animatePulse ( GameObject go ){
		yield return new WaitForSeconds(0.1f);
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * 2.5f * animSpeed;
			go.transform.localScale = new Vector3(Mathf.SmoothStep(startScaleX, endScaleX, t),
			                                        Mathf.SmoothStep(startScaleY, endScaleY, t),
													go.transform.localScale.z);
			yield return 0;
		}
		
		float r = 0.0f; 
		if(go.transform.localScale.x >= endScaleX) {
			while (r <= 1.0f) {
				r += Time.deltaTime * 2 * animSpeed;
				go.transform.localScale = new Vector3(Mathf.SmoothStep(endScaleX, startScaleX, r),
				                                        Mathf.SmoothStep(endScaleY, startScaleY, r),
														go.transform.localScale.z);
				yield return 0;
			}
		}
		
		if(go.transform.localScale.x <= startScaleX) {
			yield return new WaitForSeconds(0.01f);
			animationFlag = true;

			if (runOnce)
				stopAnimation = true;
		}
	}

}