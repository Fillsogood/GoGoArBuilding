using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefectIni : MonoBehaviour
{     
    List<float> x = new List<float>();
    List<float> y = new List<float>();
    List<float> z = new List<float>();
    Vector3 DefectPosition;
    void Start()
    {
        x.Add(19.5f);
        x.Add(20.822f);
        y.Add(-1f);
        y.Add(-1f);
        z.Add(9.2f);
        z.Add(11.74f);

        for(int i =0;i<x.Count;i++)
        {
            DefectPosition = new Vector3(x[i],y[i],z[i]);
            Quaternion a = new Quaternion(0,0,0,0);
            GameObject Defectpoint= Resources.Load<GameObject>("DefectPrefab/Defect");  
            GameObject Instance = (GameObject) Instantiate(Defectpoint, DefectPosition,a);
            
            
        }
        
        
    }

    void Update()
    {
        
    }
}
