using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Defect : MonoBehaviour
{   

    Transform Btn_BackAr;
    Transform Defect_information;
    Transform Map;

    void Start()
    {
        Defect_information = GameObject.Find("Canvas").transform.Find("Defect_information");
        GameObject DefectImg = GameObject.Find("GameObject").transform.Find("DefectCube").gameObject;
        DefectImg.SetActive(false);
    }

   public void OnTriggerEnter (Collider other)
    {    
        Map = GameObject.Find("Canvas").transform.Find("MiniMap");   
        if(other.tag=="defect"&&Map.gameObject.activeSelf == true)
        {       
            Defect_information.gameObject.SetActive(true);
            //하자 이미지 불러오기
            GameObject DefectImg = GameObject.Find("GameObject").transform.Find("DefectCube").gameObject;
            DefectImg.SetActive(true);
            Renderer a = DefectImg.GetComponent<Renderer>();
            a.material.SetTexture("_MainTex",Resources.Load("Texture/a") as Texture);//a에다 DB 이미지파일 넣기       
        }   
    }
   public void DefectBtn()
   {
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
   }
   public void CloseBtn()
   {
        Defect_information = GameObject.Find("Canvas").transform.Find("Defect_information");          
        Defect_information.gameObject.SetActive(false);
        //하자 이미지 끄기
        GameObject DefectImg = GameObject.Find("GameObject").transform.Find("DefectCube").gameObject;
        DefectImg.SetActive(false);
        
        
   }
}
