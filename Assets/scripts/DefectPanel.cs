using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DefectPanel : MonoBehaviour
{
    Transform Map;
    SIMS_Demo _Sims;
  
    void Start()
    {
        _Sims = GameObject.Find("TargetManager").GetComponent<SIMS_Demo>();
    }

   public void OnTriggerEnter (Collider other)
    {    
       GameObject Defectpanel= GameObject.Find("Canvas").transform.Find("Defect_Panel").gameObject;
        Map = GameObject.Find("Canvas").transform.Find("MiniMap");   
        if(other.tag=="defect"&&Map.gameObject.activeSelf == true)
        {       
            Defectpanel.gameObject.SetActive(true);
            _Sims.DefectIdx= int.Parse(this.name);
            Debug.Log( _Sims.DefectIdx+"DefectPanel");
        }   
    }

    public void OpenSelectPanel()
    {
        GameObject Defectpanel= GameObject.Find("Canvas").transform.Find("Defect_Panel").gameObject;
        Defectpanel.SetActive(false);
        GameObject Panal= GameObject.Find("Canvas").transform.Find("PanelSelect").gameObject;
        Panal.SetActive(true);
       /*
        Defect_information = GameObject.Find("Canvas").transform.Find("Defect_information");
        Map = GameObject.Find("Canvas").transform.Find("MiniMap");
        Btn_BackAr = GameObject.Find("Canvas").transform.Find("Btn_BackAr");
        Defect_information.gameObject.SetActive(false); //ar넘어가면 사라지는 패널
        Map.gameObject.SetActive(false);
        Btn_BackAr.gameObject.SetActive(true); 


        GameObject Capsul =GameObject.Find("Capsules(Clone)").transform.Find("Capsule").gameObject;    
        GameObject CapBuildingStructuresul = GameObject.Find("BuildingStructure(Clone)");


        Capsul.SetActive(false);
       
       
        for(int i=0;i<2;i++) //i 길이는 DB에서 Length로
        {
            GameObject Defect = GameObject.Find("Defect(Clone)");
            Defect.SetActive(false);
        }
        
        CapBuildingStructuresul.SetActive(false);
        */
    }
   public void CloseBtn()
   {
       GameObject Defectpanel= GameObject.Find("Canvas").transform.Find("Defect_Panel").gameObject;
        Defectpanel.SetActive(false);
        /*
        Defect_information = GameObject.Find("Canvas").transform.Find("Defect_information");          
        Defect_information.gameObject.SetActive(false);
        //하자 이미지 끄기
        GameObject DefectImg = GameObject.Find("GameObject").transform.Find("DefectCube").gameObject;
        DefectImg.SetActive(false);
        */      
   }
   public void OpenPanel()
   {
        GameObject Panal= GameObject.Find("Canvas").transform.Find("PanelSelect").gameObject;
        Panal.SetActive(false);
        GameObject selectpanel= GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        selectpanel.SetActive(true);

        GameObject.Find("Canvas").transform.Find("Panel").transform.Find("txtGyrovalue").GetComponent<Text>().text = GyroScopeCtr.GetGyroData();
   }
}
