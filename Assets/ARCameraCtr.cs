using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraCtr : MonoBehaviour
{
    GameObject Target; // Capsule 게임오브젝트 가져오기
    public Transform TargetCapsule;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetCapsule==null)
        {
			Target = GameObject.Find("Capsule");

			TargetCapsule = Target.transform;
        }
        else
        {
        }
    }
}
