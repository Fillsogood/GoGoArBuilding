using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Back_btn_ctr : MonoBehaviour
{
   public void Back_QRcode()
    {
        SceneManager.LoadScene("QRScanScene");
        
    }
}
