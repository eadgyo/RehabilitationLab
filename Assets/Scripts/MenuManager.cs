using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public List<Button> buttons;
    private int cur_index;

    public SteamVR_TrackedObject leftObject;
    public SteamVR_TrackedObject rightObject;

    public SteamVR_Controller.Device leftController {
        get { return SteamVR_Controller.Input((int)leftObject.index); }
    }

    public SteamVR_Controller.Device rightController {
        get { return SteamVR_Controller.Input((int)rightObject.index); }
    }

    // Use this for initialization
    void Start () {
        cur_index = 0;
        if (buttons.Count > 0) {
            buttons[0].Select();
        }
    }

    bool keyUp() {
        Vector2 axis = leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        return Input.GetKeyDown(KeyCode.UpArrow) || axis.y > 0;
    }

    bool keyDown() {
        Vector2 axis = leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        return Input.GetKeyDown(KeyCode.DownArrow) || axis.y < 0 ;
    }

    bool keyValid() {
        return leftController.GetHairTriggerDown() || Input.GetKeyDown(KeyCode.Return);
    }

    // Update is called once per frame
    void Update() {
        if(keyDown() && cur_index < buttons.Count - 1) {
            cur_index++;
            buttons[cur_index].Select();
            print(cur_index);
        }
        if(keyUp() && cur_index > 0) {
            cur_index--;
            buttons[cur_index].Select();
            print(cur_index);
        }
        if (keyValid()) {
            print(cur_index + " : " + buttons[cur_index]);
            if (buttons[cur_index].name == "quit-application")
            {
                GetComponent<QuitApplication>().Quit();
            }
            if (buttons[cur_index].name == "creer-exercice")
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
