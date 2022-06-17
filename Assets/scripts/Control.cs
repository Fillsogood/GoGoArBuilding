using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.XR.ARCore;
public class Control : MonoBehaviour
{
    public GameObject MiniMap;
    public GameObject Scrollbar;
    public GameObject Switch_on_btn;
    public GameObject Switch_off_btn;
    public string model_name { get; set; }

    public void On_Ar()
    {
        Transform model = GameObject.Find(model_name + "(Clone)").transform;
        Transform[] children = new Transform[model.childCount];

        for(int i=0;i<model.childCount;i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(false);
        }

        MiniMap.SetActive(false);
        Scrollbar.SetActive(false);
        Switch_on_btn.SetActive(false);
        Switch_off_btn.SetActive(true);
    }

    public void Off_Ar()
    {
        Transform model = GameObject.Find(model_name + "(Clone)").transform;
        Transform[] children = new Transform[model.childCount];

        for(int i=0;i<model.childCount;i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(true);
        }

        MiniMap.SetActive(true);
        Scrollbar.SetActive(true);
        Switch_on_btn.SetActive(true);
        Switch_off_btn.SetActive(false);     
    }
}
