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
        if(isTest==true)
        {
            baseUrl1 = baseTest;
        }
        DontDestroyOnLoad(gameObject);
    }
    [Header("BASE URL")]
    public bool isTest = false;
    public string baseUrl1 = "http://192.168.20.31:43212/";
    public string baseTest = "http://192.168.0.2:43212/";
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
        string json = JsonUtility.ToJson(GameManager.instance.globeScriptable);

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

        StartCoroutine(GetLotteryInfos(baseUrl1 + urlInfoLottery));

        StartCoroutine(GetGlobeInfos(baseUrl1 + urlGlobeInfos));

        StartCoroutine(GetSpinInfos(baseUrl1 + urlSpin));

    }
    public void CallGetInfoServer()
    {
        StartCoroutine(GetInfosServer(baseUrl1 + payloadInfo));
    }
    public void CallInfosGlobe()
    {
        StartCoroutine(GetGlobeInfos(baseUrl1 + urlGlobeInfos));

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
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.recoveryScriptable);
                        GameManager.instance.recoveryScriptable.UpdateInfos();
                        if (GameManager.instance.isBackup)
                        {
                            CallReadMemory();
                        }
                    }
                    break;
            }
            if (GameManager.instance.isBackup)
            {
                Invoke("CallGetInfoServer", 1f);
            }
        }
    }

    public void CallWriteMemory()
    {
        if (!GameManager.instance.isBackup)
            StartCoroutine(PostConfig(baseUrl1 + payloadWrite));
    }

    public void CallReadMemory()
    {
        StartCoroutine(GetConfig(baseUrl1 + payloadRead));
    }
    public string RemoveAccents(string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }
    private IEnumerator PostConfig(string uri)
    {
        TechnicalScriptable config = GameManager.instance.technicalScriptable;

        for (int i = 0; i < config.forOneBalls.Count; i++)
        {
            config.forOneBalls[i].numeroChance = config.forOneBalls[i].numeroChance.Replace("°", "");
        }
        string json = RemoveAccents(JsonUtility.ToJson(config));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, json))
        {
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

    public IEnumerator GetConfig(string uri)
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
                        char[] charsToTrim = { 'b', '\'' };
                        string json = Encoding.GetEncoding("UTF-8").GetString(webRequest.downloadHandler.data);
                        string newj = json.Trim(charsToTrim);

                        JsonUtility.FromJsonOverwrite(newj, GameManager.instance.technicalScriptable);
                        break;
                    }
            }

            GameManager.instance.technicalScriptable.PopulateConfig();
        }
    }

    private IEnumerator GetLotteryInfos(string uri)
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
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.lotteryScriptable);
                    }
                    break;
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
                        JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.globeScriptable);

                        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();
                        if (uiInfos != null)
                            uiInfos.PopulateRaffleInfos(GameManager.instance.globeScriptable.GetGlobeOrder().ToString(),
                            GameManager.instance.globeScriptable.GetGlobeDescription(), GameManager.instance.globeScriptable.GetGlobeValue());
                    }
                    break;
            }
        }
    }

    public void SendBallsRaffledFromServer()
    {
        StartCoroutine(PostGlobeRaffle(baseUrl1 + urlRaffleGlobe));
    }
    private IEnumerator PostGlobeRaffle(string uri)
    {
        GameManager.RequestBallsRaffled ballsRaffled = new GameManager.RequestBallsRaffled();
        ballsRaffled.balls = new List<int>();
        for (int i = 0; i < GameManager.instance.globeRaffleScriptable.bolasSorteadas.Count; i++)
        {
            ballsRaffled.balls.Add(int.Parse(GameManager.instance.globeRaffleScriptable.bolasSorteadas[i]));
        }
        ballsRaffled.sorteioOrdem = GameManager.instance.globeScriptable.GetGlobeOrder();
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
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string jsonResponse = webRequest.downloadHandler.text;

                        GlobeController globeController = FindObjectOfType<GlobeController>();
                        if (jsonResponse != "EOF Globe")
                        {
                            JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.globeRaffleScriptable);
                            GameManager.instance.PopulateListOfVisibleTicket();
                            if (globeController != null)
                            {
                                globeController.CheckWinners();
                                globeController.SendBallsRaffledToScreen();
                            }
                        }
                    }
                    break;
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
                    {
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        string json = webRequest.downloadHandler.text;
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.spinScriptable);
                        GameManager.instance.spinScriptable.sorteioOrdem = 1;
                    }
                    break;
            }
        }
    }
    public void SetPostResultSpin(int index)
    {
        StartCoroutine(PostResultSpin(baseUrl1 + urlResultSpin, index));

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
                        JsonUtility.FromJsonOverwrite(jsonResponse, GameManager.instance.spinResultScriptable);
                        SpinController spinController = FindObjectOfType<SpinController>();
                        spinController.ShowNumberLuckySpin();
                    }
                    break;
            }
        }
    }
    #endregion
    #endregion

}
