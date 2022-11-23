using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using static GameManager;

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
        DontDestroyOnLoad(gameObject);
    }
    [Header("BASE URL")]

    public string baseUrl1 = "45.235.54.188:43212";
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
        //StartCoroutine(PostConfig(baseUrl1 + urlWriteMemory));
    }
    private void OnEnable()
    {
        GameManager.OnPopulateRaffles += GetRaffleInfos;

    }
    private void OnDisable()
    {
        GameManager.OnPopulateRaffles -= GetRaffleInfos;
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
        if (GameManager.instance.isbackup)
        {
            StartCoroutine(GetInfosServer(baseUrl1 + payloadInfo));
        }
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
                        GameManager.instance.RecoveryScreen();
                    }
                    break;
            }
            Invoke("CallGetInfoServer", 2f);
        }
    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    public void CallWriteMemory()
    {
        if (!GameManager.instance.isbackup)
            StartCoroutine(PostConfig(baseUrl1 + payloadWrite));
    }

    public void CallReadMemory()
    {
        StartCoroutine(GetConfig(baseUrl1 + payloadRead));
    }
    private IEnumerator PostConfig(string uri)
    {
        technicalScriptable config = GameManager.instance.technicalScriptable;
        string json = JsonUtility.ToJson(config);
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
                        string json = webRequest.downloadHandler.text;
                        string newj = json.Trim(charsToTrim);
                        print("newjson-------------------" + newj);
                        JsonUtility.FromJsonOverwrite(newj, GameManager.instance.technicalScriptable);
                    }
                    break;
            }
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
                    }
                    break;
            }
        }
    }

    public void SendBallsRaffledFromServer(int orderIndex, bool hasUpdateScreen = false)
    {
        StartCoroutine(PostGlobeRaffle(baseUrl1 + urlRaffleGlobe, orderIndex, hasUpdateScreen));
    }
    private IEnumerator PostGlobeRaffle(string uri, int index, bool hasUpdateScreen = false)
    {
        GameManager.RequestBallsRaffled ballsRaffled = new GameManager.RequestBallsRaffled();
        ballsRaffled.balls = new List<int>();
        for (int i = 0; i < GameManager.instance.globeRaffleScriptable.bolasSorteadas.Count; i++)
        {
            ballsRaffled.balls.Add(int.Parse(GameManager.instance.globeRaffleScriptable.bolasSorteadas[i]));
        }
        ballsRaffled.sorteioOrdem = index;
        string json = JsonUtility.ToJson(ballsRaffled);

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

                            if (globeController != null)
                            {
                                globeController.PopulatePossibleWinners();
                                globeController.SendBallsRaffledToScreen();

                            }
                        }
                        else
                        {
                            globeController.SetInteractableBtNextRaffle(true);
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
        print(index);
        RequestSpin requestSpin = new RequestSpin();
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
                        GameManager.instance.technicalScriptable.AddSpinNumber(GameManager.instance.spinResultScriptable.numeroSorteado, GameManager.instance.spinResultScriptable.sorteioOrdem);
                    }
                    break;
            }
        }
    }
    #endregion
    #endregion

}
