using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {


    public Canvas tutoriel;
    public Canvas menu;
    public SteamVR_TrackedObject leftObject;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (SteamVR_Controller.Input((int)leftObject.index).GetPress(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
  //          tutoriel.enabled = false;
  //          menu.enabled = !menu.enabled;
  //      }
	}
}
