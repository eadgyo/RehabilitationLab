using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Seance : MonoBehaviour {

    private List<GameObject> checkpoints;

    public List<GameObject> Checkpoints
    {
        get { return checkpoints; }
        set { checkpoints = value; }
    }

}
