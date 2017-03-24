using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MotionCapture : MonoBehaviour
{

    [SerializeField]
    private GameObject checkpoint;

    private List<GameObject> leftPath;

    private List<GameObject> rightPath;

    private GameObject currentSphereRight;

    public GameObject CurrentSphereRight
    {
        get { return currentSphereRight; }
        set { currentSphereRight = value; }
    }

    private GameObject currentSphereLeft;

    public GameObject CurrentSphereLeft
    {
        get { return currentSphereLeft; }
        set { currentSphereLeft = value; }
    }

    [SerializeField]
    private GameObject helpText;


    private bool canStart;

    public bool CanStart
    {
        get { return canStart; }
        set { canStart = value; }
    }

    private bool hasEnded;

    public bool HasEnded
    {
        get { return hasEnded; }
        set { hasEnded = value; }
    }



    /// <summary>
    /// GameObject de la manette gauche
    /// </summary>
    [SerializeField]
    private SteamVR_TrackedObject leftObject;

    /// <summary>
    /// Instance de Controller de la manette gauche
    /// </summary>
    private SteamVR_Controller.Device leftController
    {
        get { return SteamVR_Controller.Input((int)leftObject.index); }
    }

    /// <summary>
    /// GameObject de la manette droite
    /// </summary>
    [SerializeField]
    private SteamVR_TrackedObject rightObject;

    /// <summary>
    /// Instance de Controller de la manette droite
    /// </summary>
    private SteamVR_Controller.Device rightController
    {
        get { return SteamVR_Controller.Input((int)rightObject.index); }
    }

    [SerializeField]
    private float cooldownTime;

    [SerializeField]
    private float currentCooldown;
    [SerializeField]
    private bool hasStarted;
    [SerializeField]

    private float startedTime;

    public float StartedTime
    {
        get { return startedTime; }
        set { startedTime = value; }
    }

    public bool Quittable;

    [SerializeField]

    private bool isDoctor;

    public bool IsDoctor
    {
        get { return isDoctor; }
        set { isDoctor = value; }
    }

    private SphereBehaviour lastSphere;


    // Use this for initialization
    void Start()
    {
        hasEnded = false;
        hasStarted = false;
        Quittable = false;
        currentCooldown = -1f;
        leftPath = new List<GameObject>();
        rightPath = new List<GameObject>();
    }

    void UpdateDoctor()
    {
        // Début de l'enregistrement
        if ((leftController.GetHairTrigger() || rightController.GetHairTrigger()) && !hasStarted)
        {
            hasStarted = true;
            startedTime = Time.time;
        }

        // Spawn Checkpoint Left
        if (leftController.GetHairTrigger() && currentCooldown <= 0)
        {
            GameObject check = Instantiate(checkpoint, leftObject.transform.position, Quaternion.identity, transform);
            check.GetComponent<SphereBehaviour>().TargetTime = Time.time - startedTime;
            check.GetComponent<SphereBehaviour>().IsLeft = true;
            check.GetComponent<MeshRenderer>().material.color = Color.blue;
            StartCoroutine(Utility.Vibration(leftController, 0.1f, 1000f));
            leftPath.Add(check);

            if (lastSphere != null)
            {
                lastSphere.IsLast = false;
            }

            lastSphere = check.GetComponent<SphereBehaviour>();
            lastSphere.IsLast = true;
        }

        // Spawn checkpoint right
        if (rightController.GetHairTrigger() && currentCooldown <= 0)
        {
            GameObject check = Instantiate(checkpoint, rightObject.transform.position, Quaternion.identity, transform);
            check.GetComponent<SphereBehaviour>().TargetTime = Time.time - startedTime;
            check.GetComponent<SphereBehaviour>().IsLeft = false;
            check.GetComponent<MeshRenderer>().material.color = Color.green;
            StartCoroutine(Utility.Vibration(rightController, 0.1f, 1000f));
            rightPath.Add(check);

            if (lastSphere != null)
            {
                lastSphere.IsLast = false;
            }

            lastSphere = check.GetComponent<SphereBehaviour>();
            lastSphere.IsLast = true;
        }

        // Reposition right
        if (currentSphereRight != null && rightController.GetPress(SteamVR_Controller.ButtonMask.Grip) && !currentSphereRight.GetComponent<SphereBehaviour>().IsLeft)
        {
            currentSphereRight.transform.position = rightObject.transform.position;
        }

        // Reposition Left
        if (currentSphereLeft != null && leftController.GetPress(SteamVR_Controller.ButtonMask.Grip) && currentSphereLeft.GetComponent<SphereBehaviour>().IsLeft)
        {
            currentSphereLeft.transform.position = leftObject.transform.position;
        }

        // Remove Right
        if (currentSphereRight != null && rightController.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !currentSphereRight.GetComponent<SphereBehaviour>().IsLeft)
        {
            rightPath.Remove(currentSphereRight);
            Destroy(currentSphereRight);
            currentSphereRight = null;
        }

        // Remove Left
        if (currentSphereLeft != null && leftController.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && currentSphereLeft.GetComponent<SphereBehaviour>().IsLeft)
        {
            rightPath.Remove(currentSphereRight);
            Destroy(currentSphereRight);
            currentSphereRight = null;
        }

        // Reset cooldown
        if ((leftController.GetHairTrigger() || rightController.GetHairTrigger()) && currentCooldown <= 0)
        {
            currentCooldown = cooldownTime;
        }

        // Cooldown
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        // Next Phase
        if (leftController.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            foreach (GameObject item in leftPath)
            {
                item.SetActive(false);
            }
            foreach (GameObject item in rightPath)
            {
                item.SetActive(false);
            }
            hasEnded = true;
        }
    }

    [SerializeField]
    private int seeAhead;

    public int SeeAhead
    {
        get { return seeAhead; }
        set { seeAhead = value; }
    }


    void UpdatePatient()
    {
    }

    public void launchPath()
    {
        for (int i = 0; i < leftPath.Count; i++)
        {
            if (i == 0)
            {
                leftPath[i].SetActive(true);
            }
            SphereBehaviour yolo = leftPath[i].GetComponent<SphereBehaviour>();
            if (i + 1 < leftPath.Count)
            {
                yolo.NextSphere = leftPath[i + 1].GetComponent<SphereBehaviour>();
            }

        }

        for (int i = 0; i < rightPath.Count; i++)
        {
            if (i == 0)
            {
                rightPath[i].SetActive(true);
            }
            SphereBehaviour yolo = rightPath[i].GetComponent<SphereBehaviour>();
            if (i + 1 < rightPath.Count)
            {
                yolo.NextSphere = rightPath[i + 1].GetComponent<SphereBehaviour>();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Quittable && rightController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            print(Quittable);
            SceneManager.LoadScene(0);
        }
        else if (rightController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) || leftController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            helpText.SetActive(!helpText.activeSelf);
        }
        if (rightController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            helpText.SetActive(false);
        }
        if (canStart)
        {
            if (isDoctor)
            {
                UpdateDoctor();
            }
            else
            {
                UpdatePatient();
            }
        }

    }
}
