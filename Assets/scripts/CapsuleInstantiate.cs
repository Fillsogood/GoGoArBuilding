using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleInstantiate : MonoBehaviour
{
  void Start()
    {
        Transform points = GameObject.Find("StartCapule").GetComponent<Transform>();
        GameObject Capsule= Resources.Load<GameObject>("DefectPrefab/Capsules"); 
        GameObject Instance2 = (GameObject) Instantiate(Capsule, points.position, points.rotation );
    }
}
