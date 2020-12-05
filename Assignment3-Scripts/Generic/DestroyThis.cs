using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

	void Start () {
		Destroy(this.gameObject, 1.5f);
	}

}
