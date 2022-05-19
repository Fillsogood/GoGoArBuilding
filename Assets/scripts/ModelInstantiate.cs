using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ModelInstantiate : MonoBehaviour
{
    void Start()
    {
        //  int countLoaded = SceneManager.sceneCount;
        // Scene[] loadedScenes = new Scene[countLoaded];

        // for(int i =0; i< countLoaded; i++)
        // {
        //     loadedScenes[i] = SceneManager.GetSceneAt(i);
        // }
        // SceneManager.SetActiveScene(loadedScenes[1]);
        
        Transform points = GameObject.Find("StartPoint").GetComponent<Transform>();
        GameObject Building= Resources.Load<GameObject>("BuildingPrefab/"+SingletonModelIdx.instance.ModelIdx);
        GameObject Instance = (GameObject) Instantiate(Building, points.position, points.rotation );
    }
 
}
