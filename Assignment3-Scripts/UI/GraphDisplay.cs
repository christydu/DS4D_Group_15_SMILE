using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphDisplay: MonoBehaviour {

 // Use this for initialization
    public GameObject buttonShow;
    public GameObject buttonClose;
    public GameObject Scene;
    public GameObject Sceneclose;
    public GameObject Scenecontinue;
    private void Start()
     {
         buttonShow.GetComponent<Button>().onClick.AddListener(Show);
         buttonClose.GetComponent<Button>().onClick.AddListener(Close);
         Sceneclose.GetComponent<Button>().onClick.AddListener(Scenebuttonclose);
         Scenecontinue.GetComponent<Button>().onClick.AddListener(Show);

     }
     void Show()
     {
        Scene.GetComponent<Image>().enabled = true;
        Sceneclose.GetComponent<Image>().enabled = true;
        Scenecontinue.GetComponent<Image>().enabled = true;
    }
    void Close()
     {
        Scene.GetComponent<Image>().enabled = false;
    }
    void Scenebuttonclose()
    {
        Sceneclose.GetComponent<Image>().enabled = false;
        Scenecontinue.GetComponent<Image>().enabled = false;
    }
 }