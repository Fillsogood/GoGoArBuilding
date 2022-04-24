using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GyroScope : MonoBehaviour
{
    GameObject Capsule;
    Rigidbody rb;
    Text x;
    Text y;
    float speed=0.05f; 
    bool isBorder;
    bool Istri;
    Transform xText;
    Transform yText;
    Vector3 m_PlayerRot;

    void Start()
    {
        Capsule =GameObject.Find("Capsules(Clone)").transform.Find("Capsule").gameObject;
        Capsule.transform.position =this.transform.position;
        this.transform.parent = Capsule.transform;       
        Input.gyro.enabled=true;
        xText = GameObject.Find("Canvas").transform.Find("XText");
        x=xText.GetComponent<Text>();
        yText = GameObject.Find("Canvas").transform.Find("YText");
        y=yText.GetComponent<Text>();
    }

    protected void Update()
    {   
        Vector3 pos;
        pos = this.gameObject.transform.position;
        x.text=" X:"+pos.x;
        y.text="Y:"+pos.z;
        gyroupdate();    
    }

    void FixedUpdate()
    {
        AddRigidbody();

        if(average()>=0.0612304173409939 && average()<=0.09&&!isBorder)
        {
            //이동
            Capsule.transform.Translate(-speed,0,0); 
        }
        else if(average()>=-0.00394487800076604||average()<=0.00479192985221744 &&!isBorder)
        {
           //멈추고 돌때...
        }      
         
        if(Istri&&Capsule.activeSelf==true)
        {           
            if(Capsule.GetComponent<Rigidbody>()==null)
            {
                Capsule.AddComponent<Rigidbody>();   
            }
            else
            {
                rb = Capsule.GetComponent<Rigidbody>();       
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        else
        {
            Destroy(rb);
        }
    }

    void gyroupdate()
	{
        m_PlayerRot.y -= Input.gyro.rotationRate.z*1.0f;
		Capsule.transform.eulerAngles = m_PlayerRot;
	}

    double average()
    {
        double[] arr = new double[98]; //가속도 센서 1초 평균 98개의 데이터 추출
        double result = 0;

        for(int i=0;i<arr.Length;i++)
        {
            arr[i] = Input.gyro.userAcceleration.y; 
        }

        for(int i=0;i<arr.Length;i++)
        {
            result += arr[i];
        }

        return result /= arr.Length;
    }
    
    void AddRigidbody()
    {
         Debug.DrawRay(transform.position,-transform.right*0.8f,Color.red);
        Istri = Physics.Raycast(transform.position,-transform.right,0.8f,LayerMask.GetMask("rb"));
    }
    
}
