using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Gallery : MonoBehaviour
{
    public void TakeGallery()
    {
        NativeGallery.Permission permission;
        permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Path Single Image: " + path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(path));
             GameObject.Find("Canvas").transform.Find("Panel").transform.Find("ifPicturePath").GetComponent<InputField>().text = path;
        
        }, title: "Select single image", mime: "image/*");       
    }
    
}
