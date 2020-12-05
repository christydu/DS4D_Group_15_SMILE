using UnityEngine;
using System.Collections;

public class JukeboxController : MonoBehaviour {

	public AudioClip[] jukeboxSongs;
	private int currentIndexPlaying;
	private bool canChangeSong = true;

	// Use this for initialization
	void Awake () {
		currentIndexPlaying = 0;
		playSfx(currentIndexPlaying);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(tapManager());
	}


	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator tapManager (){

		if(!canChangeSong)
			yield break;

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
		
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			switch(objectHit.name) {
			case "Jukebox":
				GetComponent<AudioSource>().Stop();
				currentIndexPlaying = Random.Range(0, jukeboxSongs.Length);
				playSfx(currentIndexPlaying);
				print ("Now playing song #" + currentIndexPlaying);
				canChangeSong = false;
				yield return new WaitForSeconds(0.5f);
				canChangeSong = true;
				break;
			}
		}
	}

	void playSfx ( int index  ){
		GetComponent<AudioSource>().clip = jukeboxSongs[index];
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
			GetComponent<AudioSource>().loop = true;
		}
	}
}
