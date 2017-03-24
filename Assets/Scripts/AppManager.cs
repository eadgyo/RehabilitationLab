using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    /// <summary>
    /// Définit l'état courant de l'application. 0 : Configuration Docteur / 1 : Docteur / 2 : Configuration Patient / 3 : Patient / 4 : Fin
    /// </summary>
    [SerializeField]
    private int state;

    private MotionCapture motionCapture;

    private ConfigureScaling configScale;

    private Seance seance;

    [SerializeField]
    private Text rightHairText;
    [SerializeField]
    private Text rightMenuText;
    [SerializeField]
    private Text leftHairText;
    [SerializeField]
    private Text leftMenuText;
    [SerializeField]
    private Text helpText;

    // Use this for initialization
    void Start()
    {
        state = 0;

        motionCapture = GameObject.Find("Motion").GetComponent<MotionCapture>();

        configScale = GetComponent<ConfigureScaling>();

        SaveLoadSequences.SeanceParent = motionCapture.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            rightHairText.text = "|\nCalibrer";
            rightMenuText.text = "Aide\n|";
            leftHairText.text = "";
            leftMenuText.text = "Aide\n|";
            helpText.text = "Bienvenue dans le module création d'exercices de rééducation !\nPour passer a la suite, veuillez calibrer l'application.\nMettez vos bras le long du corps et appuyez sur la gachette droite.";
            motionCapture.IsDoctor = false;
            motionCapture.CanStart = false;
            if (configScale.CurrentState == ConfigureScaling.State.Doctor_done)
            {
                state = 1;
            }

        }
        else if (state == 1)
        {
            rightHairText.text = "|\nTracer mouvement";
            rightMenuText.text = "";
            leftHairText.text = "|\nTracer mouvement";
            leftMenuText.text = "Passer en mode Patient\n|";
            helpText.text = "";
            motionCapture.IsDoctor = true;
            motionCapture.CanStart = true;

            if (motionCapture.HasEnded)
            {
                state = 2;
                motionCapture.CanStart = false;
                motionCapture.HasEnded = false;
                motionCapture.StartedTime = Time.time;
                configScale.SetPatientConfig();
            }
        }
        else if (state == 2)
        {
            rightHairText.text = "|\nCalibrer";
            rightMenuText.text = "Aide\n|";
            leftHairText.text = "";
            leftMenuText.text = "Aide\n|";
            helpText.text = "Bienvenue dans le mode patient !\nReproduisez les mouvements fait par votre medecin.\nPour passer a la suite, veuillez calibrer l'application.\nMettez vos bras le long du corps et appuyez sur la gachette droite.";
            if (configScale.CurrentState == ConfigureScaling.State.Patient_done)
            {
                state = 3;
            }
        }
        else if (state == 3)
        {
            motionCapture.Quittable = true;

            rightHairText.text = "";
            rightMenuText.text = "";
            leftHairText.text = "";
            leftMenuText.text = "Terminer\n|";
            helpText.text = "";
            motionCapture.launchPath();

            motionCapture.IsDoctor = false;
            motionCapture.CanStart = true;

            if (motionCapture.HasEnded)
            {
                state = 4;
                motionCapture.CanStart = false;
                motionCapture.HasEnded = false;
            }
        }
        else if (state == 4)
        {
            Debug.Log("Fini !");
        }
    }
}
