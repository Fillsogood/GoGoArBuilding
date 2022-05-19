using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
public class Control : MonoBehaviour
{
    
   public void On_Ar()
    {
       GameObject Map = GameObject.Find("Canvas").transform.Find("MiniMap").gameObject;
       GameObject scroll = GameObject.Find("Canvas").transform.Find("Scrollbar").gameObject;
       Transform On = GameObject.Find("Canvas").transform.Find("Switch_on_btn");
       Transform Off = GameObject.Find("Canvas").transform.Find("Switch_off_btn");
        Map.SetActive(false);
        scroll.SetActive(false);
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
    }
    public void Off_Ar()
    {
       GameObject Map = GameObject.Find("Canvas").transform.Find("MiniMap").gameObject;
       GameObject scroll = GameObject.Find("Canvas").transform.Find("Scrollbar").gameObject;
       Transform On = GameObject.Find("Canvas").transform.Find("Switch_on_btn");
       Transform Off = GameObject.Find("Canvas").transform.Find("Switch_off_btn");
        Map.SetActive(true);
        scroll.SetActive(true);
        Debug.Log(On);
        On.gameObject.SetActive(true);
        Debug.Log(Off);
        Off.gameObject.SetActive(false);
    }
    public void On_List()
    {
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(true);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(false);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(true);

    }
    public void Off_List()
    {
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(false);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(true);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(false);
    }
}
