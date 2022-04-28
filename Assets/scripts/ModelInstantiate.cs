using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInstantiate : MonoBehaviour
{
    void Start()
    {
        Transform points = GameObject.Find("StartPoint").GetComponent<Transform>();
        GameObject Building= Resources.Load<GameObject>("BuildingPrefab/"+SingletonModelIdx.instance.ModelIdx);
        GameObject Instance = (GameObject) Instantiate(Building, points.position, points.rotation );
    }
}
