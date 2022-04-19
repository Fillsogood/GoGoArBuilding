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
    public int model_id;
    public string model_3dfile;
    public string model_2dfile;
}

[System.Serializable]
public class Inspection
{
	public int ins_id;
	public string ins_date;
	public string inspector;
	public int damage_type;
	public string damage_object;
	public int damage_loc_x;
	public int damage_loc_y;
	public int damage_loc_z;

	public string image_name;
	public string image_size;
	public string image_type;
	public byte[] image; 

    public string admin_name;
    public string admin_etc;
    public int state;

}

public class SIMS_Demo : MonoBehaviour
{
    private string serverPath = "http://localhost:8080";
    //private string serverPath = "http://192.168.219.104:8080";

    private string serverPort = "8080";


    public static string strToEdit = "";

    private bool isConsoleShowing = true;

    private float ta_left;
    private float ta_top;
    private float ta_width;
    private float ta_height;

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {  

            StartCoroutine("CheckPermissionAndroid");
        }
        //SimsLog(Application.persistentDataPath);
    }

    private void UpdateServerIpPort()
    {
        string ip = "localhost";
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

    private void UpdateDataModel()
    {
        UpdateServerIpPort();

        try
        {
            _model.model_id = Convert.ToInt32(GameObject.Find("ifModelID").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _model.model_id = -1;
        }
        _model.model_3dfile = GameObject.Find("if3Dfile").GetComponent<InputField>().text.ToString();
        _model.model_2dfile = GameObject.Find("if2Dfile").GetComponent<InputField>().text.ToString();

        Debug.Log("Model DB : " + _model.model_id.ToString() + "/" + _model.model_3dfile + "/" + _model.model_2dfile);
   

    }

    private void UpdateDataFormModel()
    {
        GameObject.Find("ifModelID").GetComponent<InputField>().text = _model.model_id.ToString();
        GameObject.Find("if3Dfile").GetComponent<InputField>().text = _model.model_3dfile;
        GameObject.Find("if2Dfile").GetComponent<InputField>().text = _model.model_2dfile;
    }

    public void ClearDataModel()
    {
        GameObject.Find("ifModelID").GetComponent<InputField>().text = "";
        GameObject.Find("if3Dfile").GetComponent<InputField>().text = "";
        GameObject.Find("if2Dfile").GetComponent<InputField>().text = "";
    }

    public void ClearConsole()
    {
        strToEdit = "";
    }

    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        try
        {
            _Ins.ins_id = Convert.ToInt32(GameObject.Find("ifInsadminID").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.ins_id = -1;
        }
        _Ins.admin_name = GameObject.Find("ifInsadminname").GetComponent<InputField>().text.ToString();
        _Ins.admin_etc = GameObject.Find("ifInsadminetc").GetComponent<InputField>().text.ToString();           
        try
        {
            _Ins.state = (GameObject.Find("DdInsState").GetComponent<Dropdown>().value)+1;//
        }
        catch (FormatException)
        {
            _Ins.state = -1;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            _Ins.image_name = "/storage/emulated/0/DCIM/" + GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }
        else
        {
            _Ins.image_name = GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }
        Debug.Log("Inspection DB : " + _Ins.ins_id.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.image_name);       
    }

    private void UpdateDataForm()
    {
        GameObject.Find("ifInsadminID").GetComponent<InputField>().text = _Ins.ins_id.ToString();
        GameObject.Find("ifInsadminname").GetComponent<InputField>().text = _Ins.admin_name;
        GameObject.Find("ifInsadminetc").GetComponent<InputField>().text = _Ins.admin_etc;
        GameObject.Find("DdInsState").GetComponent<Dropdown>().value = _Ins.state-1;
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = _Ins.image_name;
    }

    public void ClearDataInspection()
    {
        GameObject.Find("ifInsadminID").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsadminname").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsadminetc").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsState").GetComponent<InputField>().text = "";
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";
    }

  

    public void OnClick_InsInsert()
    {

        UpdateDataInspection();

        Debug.Log("Inspection Insert : " + _Ins.image_name);
        StartCoroutine(PostFormDataImage("inspection", "adminupdate", _Ins.image_name));
    }

    public void OnClick_InsUpdate()
    {
        UpdateDataInspection();

        StartCoroutine(PostFormDataImage("inspection", _Ins.ins_id.ToString(), _Ins.image_name));

    }
    public void OnClick_InsUpdateData()
    {
        UpdateDataInspection();

        StartCoroutine(PostFormData("inspection/data", _Ins.ins_id.ToString()));
    }

    public void OnClick_InsUpdateImage()
    {
        UpdateDataInspection();
        StartCoroutine(PostFormImage("inspection/image", _Ins.ins_id.ToString(), _Ins.image_name));
    }

    public void OnClick_InsSelect()
    {
        UpdateDataInspection();

        string uri = "inspection/" + _Ins.ins_id.ToString();
        //form-data(image, key/value) 동시 가져오기
        StartCoroutine(this.GetMultipartformImage(uri));
    }

    public void OnClick_InsDelete()
    {
        UpdateDataInspection();

        string uri = "inspection/" + _Ins.ins_id.ToString();
        //form-data(image, key/value) 삭제
        StartCoroutine(this.Delete(uri)); 
    }

    public void OnClick_ModelInsert()
    {
        UpdateDataModel();

        var json = JsonConvert.SerializeObject(_model);
        StartCoroutine(this.Post("model", "", json)); //추가
    }

    public void OnClick_ModelSelect()
    {
        UpdateDataModel();

        string uri = "model/" + _model.model_id.ToString();
        StartCoroutine(this.Get(uri));
    }

    public void OnClick_ModelUpdate()
    {
        UpdateDataModel();

        var json = JsonConvert.SerializeObject(_model);
        StartCoroutine(this.Post("model", _model.model_id.ToString(), json)); //추가
    }

    public void OnClick_ModelDelete()
    {
        UpdateDataModel();

        string uri = "model/" + _model.model_id.ToString();
        StartCoroutine(this.Delete(uri, "model")); //가져오기 1개만
    }

    public void OnClick_ModelGetAll()
    {
        UpdateDataModel();
        StartCoroutine(this.GetAll("model")); 
    }

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
        private void ViewImage(string component_name, byte[] image_bytes)
    {
        //GameObject imageObj = GameObject.Find("demoImage");
        GameObject imageObj = GameObject.Find(component_name);

        Image image = imageObj.GetComponent<Image>();
        image.type = Image.Type.Simple;
        image.preserveAspect = true;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
        tex.filterMode = FilterMode.Trilinear;
        tex.LoadImage(image_bytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
        Debug.Log(tex.width + ", " + tex.height);

        //sprite.
        image.sprite = sprite;
    }

    //이미지만 업데이트 함.
    private IEnumerator PostFormImage(string uri, string id, string path_image)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("ins_id", _Ins.ins_id.ToString()));
        }
        else
        {
           
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!!");

            yield break; ;
        }

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

        Debug.Log("파일 경로 : " + path_image);

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "이 업데이트 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("점검 ID " + _Ins.ins_id.ToString() + " 이미지가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            Debug.Log("Request Response: " + www.downloadHandler.text);
        }
    }
    //Inspection 데이터만 업데이트
    private IEnumerator PostFormData(string uri, string id)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("ins_id", _Ins.ins_id.ToString()));
        }
        else
        {
            
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!!");

            yield break;;
        }

        formData.Add(new MultipartFormDataSection("ins_date", _Ins.ins_date != "" ? _Ins.ins_date:"-1"));
        formData.Add(new MultipartFormDataSection("inspector", _Ins.inspector != "" ? _Ins.inspector : "-1"));
        formData.Add(new MultipartFormDataSection("damage_type", _Ins.damage_type > -1 ? _Ins.damage_type.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_object", _Ins.damage_object != "" ? _Ins.damage_object : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_x", _Ins.damage_loc_x > -1 ? _Ins.damage_loc_x.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_y", _Ins.damage_loc_y > -1 ? _Ins.damage_loc_y.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_z", _Ins.damage_loc_z > -1 ? _Ins.damage_loc_z.ToString() : "-1"));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "이 업데이트 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            Debug.Log("Request Response: " + www.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다.
            //ClearDataInsepction();
        }
    }

    //이미지와 Inspection 삽입 및 업데이트
    private IEnumerator PostFormDataImage(string uri, string id, string path_image)
    {
        //SimsLog("PostFormDataImage");

        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("idx", _Ins.ins_id.ToString()));
        }
        else
        {
           
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!!");

            yield break;;
        }

        // formData.Add(new MultipartFormDataSection("ins_date", _Ins.ins_date != "" ? _Ins.ins_date:"-1"));
        formData.Add(new MultipartFormDataSection("admin_name", _Ins.admin_name != "" ? _Ins.admin_name : "-1"));
        formData.Add(new MultipartFormDataSection("admin_etc", _Ins.admin_etc != "" ? _Ins.admin_etc : "-1"));
        formData.Add(new MultipartFormDataSection("state", _Ins.state > -1 ? _Ins.state.ToString() : "-1"));
        
  
        //SimsLog("PostFormDataImage:MultipartFormDataSection");

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

        //SimsLog("PostFormDataImage:MultipartFormFileSection");
        //SimsLog("파일 경로 : " + path_image);
        //SimsLog("파일  : " + img.ToString());

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        //SimsLog("MultipartFormFileSection");

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "이 전송이 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            if (id == "")
            {
                //Debug.Log("Form upload complete!");
                Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + www.responseCode);
            }
            else
            {
                Debug.Log("점검 ID " + _Ins.ins_id.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            }

            Debug.Log("Request Response: " + www.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다.
            ClearDataInspection();
        }
    }

    #region POST
    //bodyraw post 전송하는 방법임.
    private IEnumerator Post(string uri, string id, string data)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);
        Debug.Log(data);
        //POST방식으로 http서버에 요청을 보내겠습니다.
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
            Debug.Log("모델 ID " + _model.model_id.ToString() + "이 전송에 실패했습니다. " + request.responseCode);
            Debug.Log(request.error);
        }
        else
        {

            //응답을 받았습니다.
            //Debug.Log(request.downloadHandler.data);
            Debug.Log(request.downloadHandler.text);
            _model = (Model)JsonConvert.DeserializeObject<Model>(request.downloadHandler.text);
            Debug.Log(_model.model_id + "/" + _model.model_3dfile + "/" + _model.model_3dfile);

            if (id == "")
            {
                //Debug.Log("Form upload complete!");
                Debug.Log("모델 ID " + _model.model_id.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + request.responseCode);
            }
            else
            {
                Debug.Log("모델 ID " + _model.model_id.ToString() + "가 성공적으로 업데이트가 되었습니다. " + request.responseCode);
            }

            Debug.Log("Request Response: " + request.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다.
            ClearDataModel();
        }
 
    }
    #endregion


    #region IMAGE_GET
    private IEnumerator GetImage(string uri) // 이미지 하나만 받을 경우
    {
        string token = "";

        //http서버에 요청 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            //request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Content-Type", "multipart/form-data");
            //request.SetRequestHeader("Authorization", "Bearer " + token);

            string path = Path.Combine(Application.persistentDataPath, "test1.jpg");
            //request.downloadHandler = new DownloadHandlerFile(path);
            Debug.Log(path);

            yield return request.SendWebRequest();

            //http서버로부터 응답을 받았다. 
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                //결과를 문자열로 출력 
                //Debug.Log(request.downloadHandler.text);

                /* 서버의 이미지를 파일로 저장. 정상적으로 잘 받아지는지 확인을 위한 임시 코드임.
                try
                {
                    byte[] results = request.downloadHandler.data;
                    File.WriteAllBytes(path, results);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error : " + e.Message);
                }
                */
                GameObject imageObj = GameObject.Find("demoImage");
                Image image = imageObj.GetComponent<Image>();
                image.type = Image.Type.Simple;
                image.preserveAspect = true;

                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
                tex.filterMode = FilterMode.Trilinear;
                tex.LoadImage(request.downloadHandler.data);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
                Debug.Log(tex.width + ", " + tex.height);

                //sprite.
                image.sprite = sprite;
            }
        }
    }
    #endregion

    #region GETMULTIPARTFORM_IMAGE
    private IEnumerator GetMultipartformImage(string uri)
    {
        //http서버에 요청 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            //http서버로 부터 응답을 대기 
            yield return www.SendWebRequest();

            //http서버로부터 응답을 받았다. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log("점검 ID가 없거나 존재하지 않은 데이터입니다. 확인하시기 바랍니다.!!");
            }
            else
            {
                //결과를 문자열로 출력 
                //Debug.Log(www.downloadHandler.text);

                //바이너리 데이터를 복구 
                byte[] results = www.downloadHandler.data;

                Debug.Log(results.Length);  //14 

                var message = Encoding.UTF8.GetString(results);

                Debug.Log(message);     //응답했다.!

                Inspection ins = (Inspection)JsonConvert.DeserializeObject<Inspection>(message);
                if (ins != null)
                {
                    _Ins = ins;
                    Debug.Log("점검 ID : " + _Ins.ins_id.ToString() + " 조회 되었습니다.");
                    UpdateDataForm();

                    //File.WriteAllBytes("d:/sims.jpg", _Ins.image);
                    ViewImage("imgView", _Ins.image);
                }
                else
                {
                    Debug.Log("데이터를 가져오지 못했습니다. ID를 확인하세요.");
                }
            }
        }
    }
    #endregion

    #region GET
    private IEnumerator Get(string uri = "")
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
                Debug.Log("모델 ID : " + _model.model_id.ToString() + " 조회 실패했습니다.");
            }
            else
            {
                //결과를 문자열로 출력 
                Debug.Log(www.downloadHandler.text);
 
                //바이너리 데이터를 복구 
                byte[] results = www.downloadHandler.data;
 
                Debug.Log(results.Length);  //14 
 
                var message = Encoding.UTF8.GetString(results);
 
                Debug.Log(message);     //응답했다.!

                //var result = JsonConvert.DeserializeObject<Model>(message);
                //Debug.LogFormat("{0}, {1}, {2}", result.CompanyId, result.name, result.address);

                Model model = (Model)JsonConvert.DeserializeObject<Model>(message);
                if (model != null)
                {
                    _model = model;
                    UpdateDataFormModel();
                    Debug.Log("모델 ID : " + _model.model_id.ToString() + " 조회 되었습니다.");
                }
                else
                {
                    Debug.Log("데이터를 가져오지 못했습니다. ID를 확인하세요.");
                }
            }
        }
    }
    #endregion

    #region GETALL
    private IEnumerator GetAll(string uri = "")
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
                Debug.Log(www.downloadHandler.text);

                //바이너리 데이터를 복구 
                byte[] results = www.downloadHandler.data;

                Debug.Log(results.Length);  //14 

                var message = Encoding.UTF8.GetString(results);

                Debug.Log(message);     //응답했다.!

                /*var result //Inspection 전체 리스트를 알고 싶으면 아래를 풀고 사용하면 됨.
                List<Inspection> list = JsonConvert.DeserializeObject<List<Inspection>>(message);
                foreach (Inspection c in list)
                {
                    Debug.LogFormat("{0}, {1}, {2}", c.ins_id, c.ins_date, c.inspector);
                } */
                int count = 0;
                List<Model> list = JsonConvert.DeserializeObject<List<Model>>(message);
                foreach (Model c in list)
                {
                    count++;
                    Debug.Log(count.ToString() + " : " + c.model_id.ToString() + "/" + c.model_3dfile + "/" + c.model_2dfile);
                }
            }
        }
    }
    #endregion

    #region DELETE
    private IEnumerator Delete(string uri, string type = "ins")
    {
        //http서버에 요청 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            //http서버로 부터 응답을 대기 
            yield return www.SendWebRequest();
            //http서버로부터 응답을 받았다. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (type == "ins")
                    Debug.Log("점검 ID : " + _Ins.ins_id.ToString() + " 삭제가 실패했습니다.");
                else
                    Debug.Log("모델 ID : " + _model.model_id.ToString() + " 삭제가 실패했습니다.");
            }
            else
            { //성공
                //결과를 문자열로 출력 
                //Debug.Log("deleted !!");
                if (type == "ins")
                {
                    Debug.Log("점검 ID : " + _Ins.ins_id.ToString() + " 삭제 되었습니다.");
                    ClearDataInspection();
                }
                else
                {
                    Debug.Log("모델 ID : " + _model.model_id.ToString() + " 삭제 되었습니다.");
                    ClearDataModel();
                }
            }
        } //end using
    }
    #endregion

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
       Transform back = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        back.gameObject.SetActive(false);
    }
    public void UpdateBack()
    {
        Transform Updateback = GameObject.Find("Canvas").transform.Find("panel_InspectionUpdate");
        
        Updateback.gameObject.SetActive(false);
        
    }

}

