using UnityEngine;
using System.Collections;

public class JuiceAnimation : MonoBehaviour {

	private bool animFlag = false;
	private Vector3 initialScale;

	public static bool juiceAnimationIsRunning;

	void OnEnable() {
		juiceAnimationIsRunning = true;
	}

	void OnDestroy() {
		juiceAnimationIsRunning = false;
	}

	void Start () {
		initialScale = transform.localScale;
		Destroy(this.gameObject, 1.6f);
	}
	

	void Update () {
		if(animFlag) {
			animFlag = false;
			StartCoroutine(runAnimation());
		}
	}

	IEnumerator runAnimation() {
		float t = 0;
		while(t < 1) {
			t += Time.deltaTime * 0.60f;
			transform.localScale = new Vector3(Mathf.SmoothStep(initialScale.x, 1.1f, t),
			                                   Mathf.SmoothStep(initialScale.y, 1.45f, t),
			                                   1);
			yield return 0;
		}
	}
}
