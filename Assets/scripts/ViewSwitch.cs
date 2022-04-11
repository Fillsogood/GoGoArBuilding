using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSwitch : MonoBehaviour
{
    public GameObject currentbuilding;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void EnableExternalModel()
    {
        // FeedbackText.text = "Enable ";
        if (currentbuilding != null)
        {
            foreach (Transform child in currentbuilding.transform)
            {
                if (child.gameObject.name == "rebar")
                {
                    // FeedbackText.text += "rebar ";
                    child.gameObject.SetActive(false);
                }

                if (child.gameObject.name == "external")
                {
                    //  FeedbackText.text += "external ";
                    child.gameObject.SetActive(true);
                }

            }
        }
    }


    public void DisableExternalModel()
    {
        // FeedbackText.text = "Disable ";
        if (currentbuilding != null)
        {
            foreach (Transform child in currentbuilding.transform)
            {

                if (child.gameObject.name == "external")
                {
                    //  FeedbackText.text += "external ";
                    child.gameObject.SetActive(false);
                }

                if (child.gameObject.name == "rebar")
                {
                    //  FeedbackText.text += "rebar ";
                    child.gameObject.SetActive(true);
                }



            }
        }
    }
}