using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {

	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("Menu-c#");
	}
}
