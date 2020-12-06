using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphDisplay: MonoBehaviour {

 // Use this for initialization
    public GameObject buttonShow;
    public GameObject buttonClose;
    public GameObject Scene; // for stats graphs
    public GameObject Sceneclose; // for stats graphs' close button
    public GameObject Scenecontinue; // for stats graphs' continue button
    private void Start()
     {
         buttonShow.GetComponent<Button>().onClick.AddListener(Show);
         buttonClose.GetComponent<Button>().onClick.AddListener(Close);
         Sceneclose.GetComponent<Button>().onClick.AddListener(Scenebuttonclose);
         Scenecontinue.GetComponent<Button>().onClick.AddListener(Show);

     }
 
    // click region to show stats graphs, close buttons & continue buttons
     void Show()
     {
        Scene.GetComponent<Image>().enabled = true;
        Sceneclose.GetComponent<Image>().enabled = true;
        Scenecontinue.GetComponent<Image>().enabled = true;
    }
 
    // click "close button" to close stats graphs
    void Close()
     {
        Scene.GetComponent<Image>().enabled = false;
    }
    
    // click "close button" to close "close button" and "continue button"
    void Scenebuttonclose()
    {
        Sceneclose.GetComponent<Image>().enabled = false;
        Scenecontinue.GetComponent<Image>().enabled = false;
    }
 }
