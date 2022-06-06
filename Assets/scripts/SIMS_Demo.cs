using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Android;
using Dummiesman;
using Jacovone.AssetBundleMagic;


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
public class ModelDto
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
    public byte[] model_3dbytes;
    public byte[] model_2dbytes;
}

[System.Serializable]
public class ModelResponse
{
    public string api_result;
    public string err_message;
    public List<ModelDto> data = new List<ModelDto>();
}

[System.Serializable]
public class InsResponse
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
    public int damage_space;
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
    public string space_name_en;
    public string damage_name_en;
    public string object_name_en;
    public string state_name;
	public byte[] ins_bytes; 
	public byte[] ad_bytes;
}

public class SIMS_Demo : MonoBehaviour
{
    private string serverPath = "http://14.7.197.215:8080";
    //private string serverPath = "http://localhost:8080";
    private string serverPort = "8080";

    public int DefectIdx{get;set;}
    private Vector3 DefectPosition;

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();
    private bool ischeck =true;
    private int listCount = 0;
    
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        { 
            StartCoroutine("CheckPermissionAndroid");
        }
        Start_DefectInstantiate();
        OnClick_ModelInstantiate();
    }
    
    private void UpdateServerIpPort()
    {
        string ip = "14.7.197.215";
        //string ip = "localhost";
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
    }

    // 점검자 정보 조회 inputfeild
    private void UpdateDataFormIns(InspectionDto ins)
    {
        GameObject.Find("txtDamageType").GetComponent<Text>().text = ins.space_name_en+"_"+ins.object_name_en+"_"+ins.damage_name_en;
        GameObject.Find("txtInsDate").GetComponent<Text>().text = "Date : "+ins.ins_date;
        GameObject.Find("txtInsName").GetComponent<Text>().text = "Inspector : "+ins.inspector_name;
        GameObject.Find("txtInsPosition").GetComponent<Text>().text = "Location Defect : "+ins.damage_loc_x + " / " + ins.damage_loc_y + " / " + ins.damage_loc_z;
        GameObject.Find("txtInsEtc").GetComponent<Text>().text = "Etc : "+ins.inspector_etc; 
    }

    private void UpdateDataFormAdmin(InspectionDto ins)
    {
        GameObject.Find("DdAdminState").GetComponent<Dropdown>().value = ins.state -1;
    }

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
    
    public void OnClick_InsGetImage()
    {
        UpdateServerIpPort();
        GetImage();
        //StartCoroutine(this.GetInsAll("inspection/selectall")); 
    }

    public void OnClick_InsSelectList()
    {
        UpdateServerIpPort();
        On_List();
        // var json = JsonConvert.SerializeObject(new Inspection(SingletonModelIdx.instance.ModelIdx));
        InsModelIdx("inspection/select_modelidx"); 
    }

    public void OnClick_AdminSelect()
    {
        UpdateServerIpPort();
        var json = JsonConvert.SerializeObject(new Inspection(DefectIdx));
        ischeck=false;
        StartCoroutine(this.InsPostIdx("inspection/select_idx",json));    
    }

    public void OnClick_ModelInstantiate()
    {
        UpdateServerIpPort();
        ModelInstantiate("model/select_idx");
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
        switch(_Ins.state)
        {
            case 1 : GameObject.Find(DefectIdx.ToString()).transform.Find("defect").GetComponent<MeshRenderer>().materials[0].color = Color.red; break;
            case 2 : GameObject.Find(DefectIdx.ToString()).transform.Find("defect").GetComponent<MeshRenderer>().materials[0].color = Color.yellow; break;
            case 3 : GameObject.Find(DefectIdx.ToString()).transform.Find("defect").GetComponent<MeshRenderer>().materials[0].color = Color.green; break;
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
            var message = Encoding.UTF8.GetString(results);
            Debug.Log(message);     //응답했다.
            InsResponse ins = (InsResponse)JsonUtility.FromJson<InsResponse>(message);
            List<InspectionDto> list1 = new List<InspectionDto>(ins.data);
            int count =0;
            foreach (InspectionDto c in list1)
            {
                count++;
                if(ischeck==true)
                {   
                    UpdateDataFormIns(c);   
                }
                else
                {
                    UpdateDataFormAdmin(c);
                }
                DefectPosition = new Vector3(c.damage_loc_x, c.damage_loc_y, c.damage_loc_z);
            }
        }
    }

     private void InsModelIdx(string uri)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);
        string responseText = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 30 * 10000; // 30초
        request.ContentType = "application/json; charset=utf-8";

        string postData ="{\"idx\" : "+SingletonModelIdx.instance.ModelIdx+"}";
        byte[] byteArray =Encoding.UTF8.GetBytes(postData);

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
        {
            HttpStatusCode status = resp.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                Debug.Log("모델 ID " + SingletonModelIdx.instance.ModelIdx + "이 조회에 실패했습니다. " + status);
           
            }
            Stream respStream = resp.GetResponseStream();
            using (StreamReader sr = new StreamReader(respStream))
            {
                responseText = sr.ReadToEnd();
            }
        }

        var jObject = JObject.Parse(responseText);
        string data =jObject.GetValue("data").ToString();
               
        InsResponse jObjText = (InsResponse) JsonConvert.DeserializeObject<InsResponse>(jObject.ToString());
        List<InspectionDto> list = new List<InspectionDto>(jObjText.data);

        GameObject Item = Resources.Load<GameObject>("Item_Panel");
        //GameObject Item = Resources.Load<GameObject>("Item_Panel_1");

        listCount = list.Count;
        int yValue = 0;
        int Count = 0;
        //defectIdxList.Clear();
    
        foreach(InspectionDto j in list)
        {
            var index = Instantiate(Item, new Vector3(0, yValue, 0), Quaternion.identity);
            index.name = "item"+Count;
            index.transform.SetParent(GameObject.Find("Content").transform);
            yValue -= 200;

            //defectIdxList.Add(j.idx);

            //이미지 넣기
            byte[] newBytes22 = j.ins_bytes;

            MemoryStream ms = new MemoryStream(newBytes22);
            newBytes22 = ms.ToArray();

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(newBytes22);
            
            GameObject imageObj = GameObject.Find("item"+Count).transform.Find("Item_Image").gameObject;
            Image image = imageObj.GetComponent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);
            image.sprite = sprite; 

            //하자 리스트 정보값 출력
            GameObject.Find("item"+Count).transform.Find("ItemInsType_Text").GetComponent<Text>().text = j.space_name_en+"_"+j.object_name_en+"_"+j.damage_name_en;

            GameObject.Find("item"+Count).transform.Find("ItemInsDate_Text").GetComponent<Text>().text = "날짜 : "+j.ins_date; 

            GameObject.Find("item"+Count).transform.Find("ItemInsInspector_Text").GetComponent<Text>().text ="점검자 : "+j.inspector_name; 

            GameObject.Find("item"+Count).transform.Find("ItemInsLoc_Text").GetComponent<Text>().text ="하자 위치 : "+j.damage_loc_x+" / "+j.damage_loc_y+" / "+j.damage_loc_z; 

            GameObject.Find("item"+Count).transform.Find("ItemInsETC_Text").GetComponent<Text>().text ="기타사항 : "+j.inspector_etc;

            Count++;
        }
    }

    private void GetImage()
    {
        string url = serverPath+"/inspection/select_idx"; 
        string responseText = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 30 * 10000; // 30초
        request.ContentType = "application/json; charset=utf-8";

        string postData ="{\"idx\" : "+DefectIdx+"}";
        byte[] byteArray =Encoding.UTF8.GetBytes(postData);

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
        {
            HttpStatusCode status = resp.StatusCode;
            Debug.Log(status);
            Stream respStream = resp.GetResponseStream();
            using (StreamReader sr = new StreamReader(respStream))
            {
                responseText = sr.ReadToEnd();
            }
        }

        var jObject = JObject.Parse(responseText);
        string data = jObject.GetValue("data")[0].ToString();
        var jObject2 = JObject.Parse(data);

        Inspection m = JsonConvert.DeserializeObject<Inspection>(data); 
        byte[] newBytes22 = m.ins_bytes;

        MemoryStream ms = new MemoryStream(newBytes22);
        newBytes22 = ms.ToArray();

        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(newBytes22);

        GameObject imageObj = GameObject.Find("Canvas").transform.Find("PanelSelect").transform.Find("imageView").gameObject;
        Image image = imageObj.GetComponent<Image>();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);
        image.sprite = sprite;   
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

    private void ModelInstantiate(string uri)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);
        string responseText = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 30 * 10000; // 30초
        request.ContentType = "application/json; charset=utf-8";

        string postData ="{\"idx\" : "+SingletonModelIdx.instance.ModelIdx+"}"; //  마커에서 모델 idx 받아오면 나중에 바꿔줘야함
        
        byte[] byteArray =Encoding.UTF8.GetBytes(postData);

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
        {
            HttpStatusCode status = resp.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                Debug.Log("모델 ID " + SingletonModelIdx.instance.ModelIdx + "이 조회에 실패했습니다. " + status);
           
            }
            Stream respStream = resp.GetResponseStream();
            using (StreamReader sr = new StreamReader(respStream))
            {
                responseText = sr.ReadToEnd();
            }
        }

        var jObject = JObject.Parse(responseText);

        ModelResponse jObjText = (ModelResponse) JsonConvert.DeserializeObject<ModelResponse>(jObject.ToString());
        List<ModelDto> list = new List<ModelDto>(jObjText.data);

        Control con = GameObject.Find("Manager").GetComponent<Control>();
        con.model_name = list[0].model_3dfile_name;
        
        byte[] msgByte = list[0].model_3dbytes;

        string write_path = Application.persistentDataPath +"/"+ list[0].model_3dfile_name; 
        
        // SimsLog(write_path);
        File.WriteAllBytes(write_path, msgByte);

        Debug.Log(list[0].model_3dfile_name);
        StartCoroutine(LoadFromMemoryAsync(write_path, list[0].model_3dfile_name));
    }

    IEnumerator LoadFromMemoryAsync(string path, string object_name)
    {
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(path); //Asset bundle load
        yield return createRequest;

        AssetBundle bundle = createRequest.assetBundle;

        var prefab = bundle.LoadAsset(object_name); //asset bundle에서 사용하고 싶은 object find
        Transform points = GameObject.Find("StartModelPoint").GetComponent<Transform>();
        Instantiate(prefab, points.position, points.rotation); //prefab instance화
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

    private void On_List()
    {
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(true);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(false);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(true);

    }

    public void Off_List()
    {
        int count = 0;
        for(int i=0; i<listCount; i++)
        {
            Destroy(GameObject.Find("item"+count));
            //Destroy(GameObject.Find("item@"+defectIdxList[i]));
            count++;
        }
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(false);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(true);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(false);
    }

}




