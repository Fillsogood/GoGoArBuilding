using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GyroScopeCtr : MonoBehaviour
{
    private GameObject Capsule;
    private Transform xText;
    private Transform yText;
    
    private Text x;
    private Text y;
    private Text ARText;

    private Rigidbody rb;

    private Vector3 m_PlayerRot;

    private float speed=0.15f;
    private static double GyroRotY;

    private bool isBorder;
    private bool Istri;

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

        ARText = GameObject.Find("Canvas").transform.Find("CreateFlowText").GetComponent<Text>();
    }

    protected void Update()
    {   
        if(SceneManager.GetActiveScene().name == "Test")
        {
            Vector3 pos;
            pos = this.gameObject.transform.position;
            x.text=" X:"+pos.x;
            y.text="Y:"+pos.z;
            gyroupdate(); 
        }
    }

    void FixedUpdate()
    {
        //AR 앵커 저장 전, 저장 후에만 움직임
        if(ARText.text == "Load Defect" || ARText.text == "Next: Please Touch the Load Defect Button" || ARText.text == "Next: Touch the Load Defect button to continue loading.")
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
        else
        {
            //AR 애져앵커 저장 중에는 캡슐이 움직이지 않음
        }
    }

    void gyroupdate()
	{
        m_PlayerRot.y -= Input.gyro.rotationRate.z*1.3f;
		Capsule.transform.eulerAngles = m_PlayerRot;
        GyroRotY = m_PlayerRot.y;
	}

    public static string GetGyroData()
    {
        return GyroRotY.ToString();
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
