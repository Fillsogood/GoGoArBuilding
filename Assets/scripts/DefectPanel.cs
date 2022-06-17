using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DefectPanel : MonoBehaviour
{
    public GameObject PanelSelect;
    public GameObject Panel;
    public Text txtGyrovalue;

    private SIMS_Demo _Sims;
    
    void Start()
    {
        _Sims = GameObject.Find("Manager").GetComponent<SIMS_Demo>();
    }

    public void OnTriggerEnter (Collider other)
    {
        if(other.tag == "defect")
        {    
            _Sims.DefectIdx= int.Parse(this.name);

            _Sims.OnClick_RowKeySelect();

            GameObject.Find("Canvas").transform.Find("MiniMap").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("LocateFlowButton").GetComponent<Button>().interactable = true;
            GameObject.Find("Canvas").transform.Find("AdminDefectBtn").GetComponent<Button>().interactable = true;
            GameObject.Find("Canvas").transform.Find("Switch_on_btn").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Switch_off_btn").gameObject.SetActive(true);
        }
        else if (other.tag != "defect")
        {
            GameObject.Find("Canvas").transform.Find("LocateFlowButton").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas").transform.Find("AdminDefectBtn").GetComponent<Button>().interactable = false;
            GameObject.Find("Canvas").transform.Find("Switch_on_btn").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Switch_off_btn").gameObject.SetActive(false);
        }
    }

    public void OpenPanel()
    {
        PanelSelect.SetActive(false);
        Panel.SetActive(true);

        txtGyrovalue.text = GyroScopeCtr.GetGyroData();
    }

    public void OpenSelectPanel()
    {
        PanelSelect.SetActive(true);
    }
}
