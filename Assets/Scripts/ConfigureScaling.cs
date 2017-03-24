using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureScaling : MonoBehaviour
{
    public enum State
    {
        Doctor,
        Doctor_done,
        Patient,
        Patient_done
    }

    private State m_state = State.Doctor;

    public State CurrentState
    {
        get { return m_state; }
        set { m_state = value; }
    }


    [SerializeField]
    private GameObject m_gameObject;

    [SerializeField]
    private SteamVR_TrackedObject m_head;
    [SerializeField]
    private SteamVR_TrackedObject m_controller;

    private SteamVR_Controller.Device Head
    {
        get
        {
            return SteamVR_Controller.Input((int) m_head.index);
        }
    }

    private SteamVR_Controller.Device Controller
    {
        get
        {
            return SteamVR_Controller.Input((int) m_controller.index);
        }
    }

    private Vector3 m_head_position;
    private Quaternion m_head_rotation;
    private Vector3 m_controller_position;
    private Quaternion m_controller_rotation;

    void Update()
    {
        switch (m_state)
        {
            case State.Doctor:
                if (Controller.GetHairTriggerDown())
                {
                    m_head_position = m_head.transform.position;
                    m_head_rotation = m_head.transform.rotation;
                    m_controller_position = m_controller.transform.position;
                    m_controller_rotation = m_controller.transform.rotation;

                }
                if (Controller.GetHairTriggerUp())
                {
                    m_state = State.Doctor_done;
                }
                break;
            case State.Patient:
                if (Controller.GetHairTriggerDown())
                {
                    Vector3 head_position = m_head.transform.position;
                    Quaternion head_rotation = m_head.transform.rotation;
                    Vector3 controller_position = m_controller.transform.position;
                    Quaternion controller_rotation = m_controller.transform.rotation;

                    Vector3 translate = head_position - m_head_position;
                    float scale = (head_position.y - controller_position.y) / (m_head_position.y - m_controller_position.y);
                    //float scale = (head_position - left_position).magnitude / (m_head_position - m_left_position).magnitude;

                    Vector3 a = head_rotation.eulerAngles - m_head_rotation.eulerAngles;

                    //m_gameObject.transform.localRotation = Quaternion.Euler(0, a.y, 0);
                    m_gameObject.transform.rotation = Quaternion.Euler(0, a.y, 0);
                    m_gameObject.transform.localPosition = translate;
                    m_gameObject.transform.localScale = new Vector3(scale, scale, scale);

                    m_state = State.Patient_done;
                }
                break;
            case State.Doctor_done:
            case State.Patient_done:
                break;
        }
    }

    public void SetPatientConfig()
    {
        m_state = State.Patient;
    }
}
