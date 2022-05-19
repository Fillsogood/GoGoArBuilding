using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemInstantiate : MonoBehaviour
{
    GameObject Item;

    public void Init()
    {
        Item = Resources.Load<GameObject>("Item_Panel");
        int yValue = 0;
        for(int i=0; i<10; i++)
        {
            var index = Instantiate(Item, new Vector3(0, yValue, 0), Quaternion.identity);
            index.name = "item"+i;
            index.transform.SetParent(GameObject.Find("Content").transform);
            yValue -= 200;
        }
    }
}
