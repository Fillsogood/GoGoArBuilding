using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script to make secondary camera follow user.
public class CameraCtr : MonoBehaviour
{  
    GameObject Target; // Capsule 게임오브젝트 가져오기
    public Transform TargetCapsule;
    public Transform Navigation;
    public Scrollbar scrollbar;
    
    void Start()
    {
        scrollbar.onValueChanged.AddListener((float val)=>ScrollbarCallback(val));

    }
    void ScrollbarCallback(float value)
    {
        Camera navigation = GameObject.Find("NavigationCamera").GetComponent<Camera>();
        if(value>=0&&value<=0.02)
        {
           navigation.orthographicSize=3.318265f;
        }
        else
        {
            navigation.orthographicSize=value*150f;
        }
    }
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

    private void LateUpdate()
    {
		Navigation.position =new Vector3(TargetCapsule.position.x,TargetCapsule.position.y+20f,TargetCapsule.position.z);
		Navigation.LookAt(TargetCapsule);
    }

}
