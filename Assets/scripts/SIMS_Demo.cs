using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Android;

[System.Serializable]
public class Model
{
    public int idx;
    public int model_id;
    public string model_3dfile_name;
    public string model_3dfile;
    public string model_3dfile_type;
    public string model_3dfile_size;
    public string model_2dfile_name;
    public string model_2dfile;
    public string model_2dfile_type;
    public string model_2dfile_size;

}

[System.Serializable]
public class InsResponse
{
    public string api_result;
    public string err_message;
    public List<InspectionDto> data = new List<InspectionDto>();
}

[System.Serializable]
public class ModelResponse
{
    public string api_result;
    public string err_message;
    public List<InspectionDto> data = new List<InspectionDto>();
}

[System.Serializable]
public class Inspection
{
	public int idx;
    public int model_idx;
    public string inspector_name;
    public string admin_name;
	public int damage_type;
	public int damage_object;
	public float damage_loc_x;
	public float damage_loc_y;
	public float damage_loc_z;
	public string ins_date;
    public string admin_date;
    public string inspector_etc;
    public string admin_etc;
    public int state;
	public string ins_image_name;
	public string ins_image_url;
	public string ins_image_size;
	public string ins_image_type;
	public byte[] ins_bytes; 
    public string ad_image_name;
	public string ad_image_url;
	public string ad_image_size;
	public string ad_image_type;
	public byte[] ad_bytes;

    public Inspection () {} 
    public Inspection (int _idx)
    {
        idx= _idx;
    } 
}

[System.Serializable]
public class InspectionDto
{
	public int idx;
    public int model_idx;
    public string inspector_name;
    public string admin_name;
	public int damage_type;
	public int damage_object;
	public float damage_loc_x;
	public float damage_loc_y;
	public float damage_loc_z;
	public string ins_date;
    public string admin_date;
    public string inspector_etc;
    public string admin_etc;
    public int state;
	public string ins_image_name;
	public string ins_image_url;
	public string ins_image_size;
	public string ins_image_type;
    public string ad_image_name;
	public string ad_image_url;
	public string ad_image_size;
	public string ad_image_type;
    public string damage_name;
    public string object_name;
    public string state_name;
	public byte[] ins_bytes; 
	public byte[] ad_bytes;
}

public class SIMS_Demo : MonoBehaviour
{
    //private string serverPath = "http://localhost:8080";
    private string serverPath = "http://182.215.11.80:8080";

    private string serverPort = "8080";

    public int DefectIdx{get;set;}

    private Vector3 DefectPosition;

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();
    private bool ischeck =true;
    

    void Start()
    {
        SimsLog("헿");
        if (Application.platform == RuntimePlatform.Android)
        { 
            StartCoroutine("CheckPermissionAndroid");
        }
        Start_DefectInstantiate();
    }

    private void UpdateServerIpPort()
    {
        //string ip = "localhost";
        string ip = "182.215.11.80";
        string port = "8080";

        if (ip == "" || port == "")
        {
            Debug.Log("IP 및 Port 입력하세요. 서버와 통신을 할 수가 있습니다.");
        }
        else
        {
            serverPath = "http://" + ip + ":" + port;
            Debug.Log(serverPath);
        }
    }

    private void SimsLog(string text)
    {
       GameObject.Find("Text_console").GetComponent<Text>().text += text + "\n";
       // strToEdit=;
    }

    // 점검자 정보 조회 inputfeild
    private void UpdateDataFormIns(InspectionDto ins)
    {
        switch(ins.damage_type)
        {
            case 1 : GameObject.Find("txtDamageType").GetComponent<Text>().text = "균열"; break;
            case 2 : GameObject.Find("txtDamageType").GetComponent<Text>().text = "부식"; break;
            case 3 : GameObject.Find("txtDamageType").GetComponent<Text>().text = "변형"; break;
        }
        GameObject.Find("txtInsDate").GetComponent<Text>().text = "날짜 : "+ins.ins_date;
        GameObject.Find("txtInsName").GetComponent<Text>().text = "점검자 : "+ins.inspector_name;
        GameObject.Find("txtInsPosition").GetComponent<Text>().text = "하자 위치 : "+ins.damage_loc_x + " / " + ins.damage_loc_y + " / " + ins.damage_loc_z;
        GameObject.Find("txtInsEtc").GetComponent<Text>().text = "기타사항 : "+ins.inspector_etc;
        
        SimsLog(ins.ins_image_url);
        //ViewImageAndroid("imageView",ins.ins_bytes);
        //ViewImage("imageView", ins.ins_image_name);
    }

    private void UpdateDataFormAdmin(InspectionDto ins)
    {
        GameObject.Find("DdAdminState").GetComponent<Dropdown>().value = ins.state -1;
    }

    // 점검자 정보 조회 inputfeild Clear
    // private void ClearDataFormIns()
    // {
    //     GameObject.Find("ifInsDate").GetComponent<InputField>().text = "";
    //     GameObject.Find("ifInsName").GetComponent<InputField>().text = ""; 
    //     GameObject.Find("ifInsState").GetComponent<InputField>().text = "";
    //     GameObject.Find("ifInsEtc").GetComponent<InputField>().text = "";
    //     GameObject.Find("ifInsPosition").GetComponent<InputField>().text = "";
    // }

    // 관리자 inputfeild의 값 삭제
    public void ClearDataInspection()
    {  
        GameObject.Find("ifAdminName").GetComponent<InputField>().text = "";
        GameObject.Find("ifAdminEtc").GetComponent<InputField>().text = "";
        Debug.Log(DefectPosition.x.ToString() + " / " + DefectPosition.y.ToString() + " / "+  DefectPosition.z.ToString());
        GameObject.Find("Capsules(Clone)").transform.Find("Capsule").transform.position=DefectPosition;
        Back();
    }

    // 관리자 inputfeild 적은 값 _Ins에 넣기
    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        try
        {
            _Ins.idx = DefectIdx;
        }
        catch (FormatException)
        {
            _Ins.idx = -1;
        }

        _Ins.admin_name = GameObject.Find("ifAdminName").GetComponent<InputField>().text.ToString();
        _Ins.admin_etc = GameObject.Find("ifAdminEtc").GetComponent<InputField>().text.ToString();

        try
        {
            _Ins.state = (GameObject.Find("DdAdminState").GetComponent<Dropdown>().value)+1;//
        }
        catch (FormatException)
        {
            _Ins.state = 0;
        }
            
            //_Ins.ins_image_name = GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
            _Ins.ins_image_name = GameObject.Find("Canvas").transform.Find("Panel").transform.Find("txtPicturepath").GetComponent<Text>().text.ToString();

        //Debug.Log("Inspection DB : " + _Ins.idx.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector_name + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.ins_image_name);       
    }



    // 관리자 insert
    public void OnClick_InsInsert()
    {
        UpdateDataInspection();
        StartCoroutine(this.PostFormDataImage("inspection", "adminupdate", _Ins.ins_image_name));
    }

    //점검자 정보 조회 
    public void OnClick_InsSelect()
    {
        UpdateServerIpPort();
        var json = JsonConvert.SerializeObject(new Inspection(DefectIdx));
        ischeck=true;
        StartCoroutine(this.InsPostIdx("inspection/select_idx",json)); 
    }
    public void OnClick_InsGetAll()
    {
        UpdateServerIpPort();
        StartCoroutine(this.GetInsAll("inspection/selectall")); 
    }
    public void OnClick_AdminSelect()
    {
        UpdateServerIpPort();
        var json = JsonConvert.SerializeObject(new Inspection(DefectIdx));
        ischeck=false;
        StartCoroutine(this.InsPostIdx("inspection/select_idx",json));    
    }
    //Start시 Defect DB에 저장된 하자 갯수만큼 소환
    public void Start_DefectInstantiate()
    {
        UpdateServerIpPort();
        var json = JsonConvert.SerializeObject(new Inspection(SingletonModelIdx.instance.ModelIdx));
        StartCoroutine(this.PostDefectIni("inspection/select_modelidx",json)); 
    }

    //안드로이드 저장소 권한 관련
    IEnumerator CheckPermissionAndroid()
    {
        //SimsLog("CheckPermissionAndroid");

        yield return new WaitForEndOfFrame();

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                //다이얼로그를 위해 별도의 플러그인을 사용했었다. 이 코드는 주석 처리함.
                //AGAlertDialog.ShowMessageDialog("권한 필요", "스크린샷을 저장하기 위해 저장소 권한이 필요합니다.",
                //"Ok", () => OpenAppSetting(),
                //"No!", () => AGUIMisc.ShowToast("저장소 요청 거절됨"));

                // 별도로 확인 팝업을 띄우지 않을꺼면 OpenAppSetting()을 바로 호출함.
                //OpenAppSetting();
                Debug.Log("저장소 권한이 필요함.");
                yield break;
            }
        }

        //string fileLocation = "/storage/emulated/0" + "/DCIM/Screenshots/"; // "mnt/sdcard/DCIM/Screenshots/";
    }

    //이미지 띄우기?
    private void ViewImage(string component_name, string PATH)
    {
        SimsLog(PATH);
        byte[] byteTexture = System.IO.File.ReadAllBytes(PATH);

        Texture2D texture = new Texture2D(0, 0);

        texture.LoadImage(byteTexture);
        GameObject imageObj = GameObject.Find("Canvas").transform.Find("PanelSelect").transform.Find(component_name).gameObject;

        Image image = imageObj.GetComponent<Image>();
        
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);

        image.sprite = sprite;
    }
        private void ViewImageAndroid(string component_name, byte[] image_bytes)
    {
        GameObject imageObj = GameObject.Find(component_name);

        Image image = imageObj.GetComponent<Image>();
        image.type = Image.Type.Simple;
        image.preserveAspect = true;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Trilinear;
        tex.LoadImage(image_bytes); 
        tex.filterMode = FilterMode.Trilinear;
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
        Debug.Log(tex.width + ", " + tex.height);
        

        //sprite.
        image.sprite = sprite;
    }

    //이미지와 Inspection 삽입 및 업데이트
    private IEnumerator PostFormDataImage(string uri, string id, string path_image)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.idx > -1)
        {
            formData.Add(new MultipartFormDataSection("idx", _Ins.idx.ToString()));
        }
        else
        {
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!!");

            yield break;;
        }

        formData.Add(new MultipartFormDataSection("admin_name", _Ins.admin_name != "" ? _Ins.admin_name : "-1"));
        formData.Add(new MultipartFormDataSection("admin_etc", _Ins.admin_etc != "" ? _Ins.admin_etc : "-1"));
        formData.Add(new MultipartFormDataSection("state", _Ins.state > -1 ? _Ins.state.ToString() : "-1"));

        byte[] img = null;
        string strImgformat = "";
        if (Path.GetExtension(path_image) == ".jpg")
        {
            img = File.ReadAllBytes(path_image);
            strImgformat = "image/jpeg";
        }
        else if (Path.GetExtension(path_image) == ".png")
        {
            img = File.ReadAllBytes(path_image);
            strImgformat = "image/png";
        }
        else
        {
            Debug.Log("jpg, png 파일만 전송이 가능합니다. 다시 확인바랍니다.!!");
            yield break;
        }

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 전송이 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            if (id == "")
            {
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + www.responseCode);
            }
            else
            {
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            }

            Debug.Log("Request Response: " + www.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다.
            ClearDataInspection();
        }
    }

    private IEnumerator InsPostModelIdx(string uri,string data)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);

         var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //Debug.Log(bodyRaw.Length);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

          //응답을 기다립니다.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 조회에 실패했습니다. " + request.responseCode);
           
        }
        else
        {
            byte[] results = request.downloadHandler.data;
 
            //Debug.Log(results.Length);
 
            var message = Encoding.UTF8.GetString(results);
 
            //Debug.Log(message);     //응답했다.!

            InsResponse ins = (InsResponse)JsonUtility.FromJson<InsResponse>(message);
            List<InspectionDto> list1 = new List<InspectionDto>(ins.data);

            int count =0;
            foreach (InspectionDto c in list1)
            {
                count++;
                Debug.Log(count.ToString() + " : " + c.admin_name+ "/" + c.admin_etc + "/" + c.state);
            }
        }

    }

    private IEnumerator InsPostIdx(string uri,string data)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);

         var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
       
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        request.SetRequestHeader("Content-Type", "application/json");

          //응답을 기다립니다.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 조회에 실패했습니다. " + request.responseCode);
           
        }
        else
        {
            byte[] results = request.downloadHandler.data;
 
            //Debug.Log(results.Length);  //14 
 
            var message = Encoding.UTF8.GetString(results);
 
            Debug.Log(message);     //응답했다.!

            InsResponse ins = (InsResponse)JsonUtility.FromJson<InsResponse>(message);
            List<InspectionDto> list1 = new List<InspectionDto>(ins.data);
            
            int count =0;
            foreach (InspectionDto c in list1)
            {
                count++;
                if(ischeck==true)
                {
                    //SimsLog(c.ins_image_url);
                    //UpdateDataFormIns(c); 
                    //UnityWebRequestTexture.GetTexture("https://www.my-server.com/" + c.ins_image_name);
                }
                else
                {
                    UpdateDataFormAdmin(c);
                }
                
                DefectPosition = new Vector3(c.damage_loc_x, c.damage_loc_y, c.damage_loc_z);
                Debug.Log(DefectPosition.x.ToString() + " / " + DefectPosition.y.ToString() + " / "+  DefectPosition.z.ToString());
                
                Debug.Log(count.ToString() + " : " + c.idx+ "/" + c.inspector_name + "/" + c.state);
            }
        }
    }

    private IEnumerator GetInsAll(string uri = "")
    {
        //http서버에 요청 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            //http서버로 부터 응답을 대기 
            yield return www.SendWebRequest();

            //http서버로부터 응답을 받았다. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //결과를 문자열로 출력 
                //Debug.Log(www.downloadHandler.text);
                //바이너리 데이터를 복구 
                byte[] results = www.downloadHandler.data;

                // Debug.Log(results.Length);  //14 
                var message = Encoding.UTF8.GetString(results);
                // Debug.Log(message);     //응답했다.!
                InsResponse list = (InsResponse)JsonUtility.FromJson<InsResponse>(message);
                List<InspectionDto> list1 = new List<InspectionDto>(list.data);
                int count = 0;
                foreach (InspectionDto c in list1)
                {
                    count++;
                    Debug.Log("DefectIDX " + DefectIdx);
                    Debug.Log("c.IDX " + c.idx);
                    if(DefectIdx == c.idx)
                    {
                        ViewImageAndroid("imageView",c.ins_bytes);
                        
                    }
                        
                    Debug.Log(count.ToString() + " : " + c.admin_name+ "/" + c.admin_etc + "/" + c.state);
                }
                
            }
        }
    }

    private void Set_Image(byte[] recevByteArr)
    {
            Texture2D bmp;
            bmp = new Texture2D(8, 8);
            //bmp.LoadRawTextureData(recevBuffer);
            bmp.LoadImage(recevByteArr, false);

            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Rect tRect = new Rect(0, 0, bmp.width, bmp.height);
            Sprite newSprite = Sprite.Create(bmp, tRect, pivot);

            GameObject Image = GameObject.Find("Canvas").transform.Find("PanelSelect").transform.Find("imageView").gameObject;
            Image.GetComponent<Image>().overrideSprite = newSprite;
    }

    private IEnumerator PostDefectIni(string uri, string data )
    {
        var url = string.Format("{0}/{1}", serverPath, uri);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
       
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //응답을 기다립니다.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 조회에 실패했습니다. " + request.responseCode);
        }
        else
        {
            byte[] results = request.downloadHandler.data;
 
            //Debug.Log(results.Length);  //14 
 
            var message = Encoding.UTF8.GetString(results);
 
            //Debug.Log(message);     //응답했다.!

            InsResponse ins = (InsResponse)JsonUtility.FromJson<InsResponse>(message);
            List<InspectionDto> list1 = new List<InspectionDto>(ins.data);

            foreach (InspectionDto c in list1)
            {
                DefectPosition = new Vector3(c.damage_loc_x,c.damage_loc_y,c.damage_loc_z);
                Quaternion a = new Quaternion(0,0,0,0);
                GameObject Defectpoint= Resources.Load<GameObject>("DefectPrefab/Defect");  
                GameObject Instance = (GameObject) Instantiate(Defectpoint, DefectPosition,a);
                Instance.name = c.idx.ToString();
            }
        }
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Back()
    {
       Transform back = GameObject.Find("Canvas").transform.Find("Panel");
        back.gameObject.SetActive(false);
    }

}

