using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Camera : MonoBehaviour
{
    
    public void TakePictureButton()
    {
        TakePicture(512);
    }

    private void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission =NativeCamera.TakePicture((path)=>
        {
            Debug.Log("Image path:"+path);
            if(path != null)
            {
                Texture2D texture= NativeCamera.LoadImageAtPath(path,maxSize);
                if(texture == null)
                {
                    Debug.Log("Couldn't load texture from"+path);
                    return;
                }
               GameObject.Find("Canvas").transform.Find("panel_Inspection").transform.Find("ifPicturePath").GetComponent<InputField>().text = path;
            }
        }, maxSize);
        Debug.Log("Permission result:"+permission);
    }
   
}

    

