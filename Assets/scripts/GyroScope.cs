using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GyroScope : MonoBehaviour
{
    GameObject Capsule;
    Vector3 forceVec;
    Rigidbody rb;
    Text x;
    Text y;
    float speed=0.05f; 
    bool isBorder;
    bool Istri;
    Transform xText;
    Transform yText;
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
        Capsule.transform.Rotate(0,-Input.gyro.rotationRateUnbiased.y*1,0);     
        Vector3 angleAcceler = Input.acceleration;      
    }
    void FixedUpdate()
    {
        StropToWall();
        AddRigidbody();
        if(Input.gyro.userAcceleration.y>0.2&&!isBorder)
        {
            Capsule.transform.Translate(-speed,0,0);
           
        }
        else if(Capsule.activeSelf==false)
        {

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
    void StropToWall()
    {
        Debug.DrawRay(transform.position,-transform.right*0.5f,Color.green);
        isBorder = Physics.Raycast(transform.position,-transform.right,0.5f,LayerMask.GetMask("wall"));
    }
    void AddRigidbody()
    {
         Debug.DrawRay(transform.position,-transform.right*0.8f,Color.red);
        Istri = Physics.Raycast(transform.position,-transform.right,0.8f,LayerMask.GetMask("rb"));
    }
}
