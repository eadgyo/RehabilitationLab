using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour
{

    private bool touched;

    public bool Touched
    {
        get { return touched; }
        set { touched = value; }
    }

    private SphereBehaviour nextSphere;

    public SphereBehaviour NextSphere
    {
        get { return nextSphere; }
        set { nextSphere = value; }
    }


    public Material material;

    [SerializeField]
    private MotionCapture motionCapture;

    [SerializeField]
    private bool isLeft;

    public bool IsLeft
    {
        get { return isLeft; }
        set { isLeft = value; }
    }

    private bool isLast;

    public bool IsLast
    {
        get { return isLast; }
        set { isLast = value; }
    }

    private float targetTime;

    public float TargetTime
    {
        get { return targetTime; }
        set { targetTime = value; }
    }

    private float executedTime;

    public float ExecutedTime
    {
        get { return executedTime; }
        set { executedTime = value; }
    }

    public CardinalSpline cardinalLeft;
    public CardinalSpline cardinalRight;

    

    // Use this for initialization
    void Start()
    {
        touched = false;
        motionCapture = GameObject.Find("Motion").GetComponent<MotionCapture>();
        cardinalLeft = GameObject.Find("CardinalLeft").GetComponent<CardinalSpline>();
        cardinalRight = GameObject.Find("CardinalRight").GetComponent<CardinalSpline>();
    }

    void OnTriggerExit(Collider other)
    {
        if (motionCapture.IsDoctor)
        {
            if (other.CompareTag("LeftControl"))
            {
                GetComponent<MeshRenderer>().material.color = isLeft ? Color.blue : Color.green;
                motionCapture.CurrentSphereLeft = null;
            }

            if (other.CompareTag("RightControl"))
            {
                GetComponent<MeshRenderer>().material.color = isLeft ? Color.blue : Color.green;
                motionCapture.CurrentSphereRight = null;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (motionCapture.IsDoctor)
        {
            if (other.CompareTag("LeftControl") && isLeft)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                motionCapture.CurrentSphereLeft = gameObject;
            }

            if (other.CompareTag("RightControl") && !isLeft)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                motionCapture.CurrentSphereRight = gameObject;
            }
                
        }
        else
        {
            if ((other.CompareTag("LeftControl") && isLeft) || (other.CompareTag("RightControl") && !isLeft))
            {
                touched = true;
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

                SphereBehaviour sp = nextSphere;
                for (int i = 0; i < motionCapture.SeeAhead; i++)
                {
                    if (sp != null)
                    {
                        if (isLeft)
                        {
                            cardinalLeft.AddPoint(sp.gameObject, sp.nextSphere.targetTime - targetTime);
                        }
                        else
                        {
                            cardinalRight.AddPoint(sp.gameObject, sp.nextSphere.targetTime - targetTime);
                        }
                        if (i == 0)
                        {
                            sp.gameObject.SetActive(true);
                            sp.GetComponent<MeshRenderer>().material.color = new Color(
                                sp.GetComponent<MeshRenderer>().material.color.r,
                                sp.GetComponent<MeshRenderer>().material.color.g,
                                sp.GetComponent<MeshRenderer>().material.color.b,
                                1f
                                );
                        }
                        else
                        {
                            sp.gameObject.SetActive(true);
                            sp.GetComponent<MeshRenderer>().material.color = new Color(
                                sp.GetComponent<MeshRenderer>().material.color.r,
                                sp.GetComponent<MeshRenderer>().material.color.g,
                                sp.GetComponent<MeshRenderer>().material.color.b,
                                0f
                                );
                        }
                    }
                    sp = sp.nextSphere;
                }


                executedTime = Time.time - motionCapture.StartedTime;
                Debug.Log("Temps cible :" + targetTime + ", temps fait : " + executedTime + ", Différence : " + (targetTime - executedTime));
            }
        }

    }
}
