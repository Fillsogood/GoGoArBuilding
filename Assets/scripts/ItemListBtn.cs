using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListBtn : MonoBehaviour
{
    public void OpenSelectPanel()
    {
        GameObject Defectpanel= GameObject.Find("Canvas").transform.Find("Defect_Panel").gameObject;
        Defectpanel.SetActive(false);
        GameObject Panal= GameObject.Find("Canvas").transform.Find("PanelSelect").gameObject;
        Panal.SetActive(true);
    }
}
