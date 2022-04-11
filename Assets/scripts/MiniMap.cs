using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    Transform Defect_information;
    Transform Map;
    private static GameObject CapBuildingStructuresul;
    private static GameObject[] objects;

    void Start()
    {
          
        CapBuildingStructuresul = GameObject.Find("BuildingStructure(Clone)");
        //Capsul = GameObject.Find("Capsules(Clone)").transform.Find("Capsule"); 
        objects = GameObject.FindGameObjectsWithTag("Love");
               
    }
    public void Buttorn()
    {
        Defect_information = GameObject.Find("Canvas").transform.Find("Defect_information");
        Map = GameObject.Find("Canvas").transform.Find("MiniMap");
        Map.gameObject.SetActive(true);
        Defect_information.gameObject.SetActive(false);
        GameObject DefectImg = GameObject.Find("GameObject").transform.Find("DefectCube").gameObject;
        DefectImg.SetActive(false);

        CapBuildingStructuresul.SetActive(true); 
        Transform Capsul = GameObject.Find("Capsules(Clone)").transform.Find("Capsule");
        Capsul.gameObject.SetActive(true);
        for(int i=0;i<2;i++)
        {
            objects[i].SetActive(true);      
        }
        
               
    }

}
