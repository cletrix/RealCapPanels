using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RestNetworkManager : MonoBehaviour
{
    private static RestNetworkManager _instance;
    public static RestNetworkManager instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;

            }
            else if (_instance != value)
            {
                Debug.Log($"{nameof(RestNetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        SetCurrentMode(current_mode);
        DontDestroyOnLoad(gameObject);
    }
    
    public enum MODE{LOCAL, REMOTO,PRODUCAO}

    public MODE current_mode = MODE.LOCAL;
    
    [Header("BASE URL")]
    public string baseUrl;
    // path server bruno rvrgs.sytes.net:43212
   // public string baseTest = "http://192.168.0.2:43212/";
    public string payloadWrite = "writeMemory";
    public string payloadRead = "readMemory";
    public string payloadInfo = "info";
    [Header("PAYLOADS")]
    public string urlLogin;
    [Space]
    public string urlInfoLottery;
    public string urlResultLottery;
    [Space]
    public string urlGlobeInfos;
    public string urlRaffleGlobe;
    [Space]
    public string urlSpin;
    public string urlResultSpin;

    #region REQUESTS
    private void Start()
    {
        string json = JsonUtility.ToJson(GameManager.instance.globeData);

    }
    private void SetCurrentMode(MODE _current_mode)
    {
        switch (_current_mode)
        {
            case MODE.LOCAL:
                baseUrl = "http://192.168.0.4:43212/";
                break;
            case MODE.REMOTO:
                baseUrl = "http://192.168.0.4:43212/";
                break;
            case MODE.PRODUCAO:
                baseUrl = "http://192.168.20.31:43212/";
                break;
            default:
                break;
        }
    }
    private void OnEnable()
    {
        GameManager.OnPopulateRaffles += GetRaffleInfos;
    }
    private void OnDisable()
    {
        GameManager.OnPopulateRaffles -= GetRaffleInfos;
    }

    public void DisableInvokInfosServer()
    {
        CancelInvoke("CallGetInfoServer");
    }
    public void GetRaffleInfos()
    {
        CallGetInfoServer();

        StartCoroutine(GetGlobeInfos(baseUrl + urlGlobeInfos));

        StartCoroutine(GetSpinInfos(baseUrl + urlSpin));

    }
    public void CallGetInfoServer()
    {
        StartCoroutine(GetInfosServer(baseUrl + payloadInfo));
        if (GameManager.instance.isBackup == true)
            StartCoroutine(GetReadMemory(baseUrl + payloadRead));
    }
    public void CallInfosGlobe()
    {
        StartCoroutine(GetGlobeInfos(baseUrl + urlGlobeInfos));
    }
    private IEnumerator GetInfosServer(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string json = webRequest.downloadHandler.text;
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.recoveryData);
                        GameManager.instance.recoveryData.UpdateInfosGlobe();
                        if (GameManager.instance.isBackup)
                        {
                            yield return new WaitForSeconds(1f);
                            StartCoroutine(GetInfosServer(baseUrl + payloadInfo));
                        }
                        break;
                    }
            }
        }
    }
    public void CallWriteMemory()
    {
        if (!GameManager.instance.isBackup)
            StartCoroutine(PostWriteMemory(baseUrl + payloadWrite));
    }
    //public string RemoveAccents(string text)
    //{
    //    StringBuilder sbReturn = new StringBuilder();
    //    var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
    //    foreach (char letter in arrayText)
    //    {
    //        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
    //            sbReturn.Append(letter);
    //    }
    //    return sbReturn.ToString();
    //}
    private IEnumerator PostWriteMemory(string uri)
    {
        string json = JsonUtility.ToJson(GameManager.instance.operatorData);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, json))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    }
                    break;
            }
        }
    }

    public IEnumerator GetReadMemory(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        byte[] bytesResponse = webRequest.downloadHandler.data;
                        string response = Encoding.UTF8.GetString(bytesResponse);
                        JsonUtility.FromJsonOverwrite(response, GameManager.instance.operatorData);
                        GameManager.instance.operatorData.PopulateConfig();
                        if (GameManager.instance.isBackup)
                        {
                            yield return new WaitForSeconds(1f);
                            StartCoroutine(GetReadMemory(baseUrl + payloadRead));
                        }
                        break;
                    }
            }
        }
    }

    private IEnumerator GetGlobeInfos(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string jsonResponse = webRequest.downloadHandler.text;
                        JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.globeData);

                        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();
                        if (uiInfos != null)
                            uiInfos.PopulateRaffleInfos(GameManager.instance.globeData.GetOrder().ToString(),
                            GameManager.instance.globeData.GetDescription(), GameManager.instance.globeData.GetValue());
                    }
                    break;
            }
        }
    }

    public void SendBallsRaffledFromServer()
    {
        StartCoroutine(PostGlobeRaffle(baseUrl + urlRaffleGlobe));
    }
    private IEnumerator PostGlobeRaffle(string uri)
    {
        GameManager.RequestBallsRaffled ballsRaffled = new GameManager.RequestBallsRaffled();
        ballsRaffled.balls = new List<int>();
        for (int i = 0; i < GameManager.instance.globeDrawData.bolasSorteadas.Count; i++)
        {
            ballsRaffled.balls.Add(int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]));
        }
        ballsRaffled.sorteioOrdem = GameManager.instance.globeData.GetOrder();
        string json = JsonUtility.ToJson(ballsRaffled);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, json))
        {
            print(json);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    {
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    }
                case UnityWebRequest.Result.ProtocolError:
                    {
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    }
                case UnityWebRequest.Result.Success:
                    {

                        string jsonResponse = webRequest.downloadHandler.text;

                        GlobeController globeController = FindObjectOfType<GlobeController>();

                        JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.globeDrawData);
                        GameManager.instance.PopulateListOfVisibleTicket();
                        if (globeController != null)
                        {

                            globeController.SendBallsRaffledToScreen();
                        }
                        break;
                    }
            }
        }
    }

    #region SPIN REQUESTS

    private IEnumerator GetSpinInfos(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:

                    string json = webRequest.downloadHandler.text;
                    JsonUtility.FromJsonOverwrite(json, GameManager.instance.spinData);
                    //GameManager.instance.spinData.sorteioOrdem = 1;
                    break;
            }
        }
    }
    public void SetPostResultSpin(int index)
    {
        StartCoroutine(PostResultSpin(baseUrl + urlResultSpin, index));

    }
    private IEnumerator PostResultSpin(string uri, int index)
    {
        GameManager.RequestSpin requestSpin = new GameManager.RequestSpin();
        requestSpin.sorteioOrdem = index;

        string json = JsonUtility.ToJson(requestSpin);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            SpinController spinController = FindObjectOfType<SpinController>();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    spinController.btGenerateLuckyNumber.interactable = true;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string jsonResponse = webRequest.downloadHandler.text;
                    JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.spinDrawData);
                    
                    spinController.ShowNumberLuckySpin();
                    break;

            }
        }
    }
    #endregion
    #endregion

}
