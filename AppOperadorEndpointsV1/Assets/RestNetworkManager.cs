using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
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
    public Comunication comunication = new Comunication();
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

        SetCurrentMode(currentMode);
        DontDestroyOnLoad(gameObject);
    }
    
    public enum MODE{LOCAL, REMOTO,PRODUCAO}

    public MODE currentMode = MODE.LOCAL;
    
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
    public string payloadInfosGlobe = "infosGlobe";
    public string payloadDrawGlobe = "raffleGlobe";
    [Space]
    public string payloadInfosSpin = "infosSpin";
    public string payloadDrawSpin = "resultSpin";

    private void SetCurrentMode(MODE _current_mode)
    {
        switch (_current_mode)
        {
            case MODE.LOCAL:
                baseUrl = "http://192.168.0.4:43212/";
                break;
            case MODE.REMOTO:
                baseUrl = "http://realcap.servegame.com:43212/";
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
    public void GetRaffleInfos()
    {
        GetRecoveryInfosDrawn();

        GetGlobeInfosDrawn();
        GetSpinInfos();
        if(GameManager.instance.isBackup)
        {
            GetReadMemory();
        }

    }
    public async void GetRecoveryInfosDrawn()
    {
        string response = await comunication.Get(baseUrl + payloadInfo);
        JsonUtility.FromJsonOverwrite(response, GameManager.instance.recoveryData);
        GameManager.instance.recoveryData.UpdateInfosGlobe();
        if (GameManager.instance.isBackup)
        {
            await Task.Delay(1000);
            GetRecoveryInfosDrawn();
        }
    }
    public async void GetGlobeInfosDrawn()
    {
        string response = await comunication.Get(baseUrl + payloadInfosGlobe);
        JsonUtility.FromJsonOverwrite(response, GameManager.instance.globeData);

        UiInfosRaffle uiInfos = FindObjectOfType<UiInfosRaffle>();
        if (uiInfos != null)
            uiInfos.PopulateRaffleInfos(GameManager.instance.globeData.GetOrder().ToString(),
            GameManager.instance.globeData.GetDescription(), GameManager.instance.globeData.GetValue());
    }
    public async void PostBallsRaffled()
    {
        GameManager.RequestBallsRaffled ballsRaffled = new GameManager.RequestBallsRaffled();
        ballsRaffled.balls = new List<int>();
        for (int i = 0; i < GameManager.instance.globeDrawData.bolasSorteadas.Count; i++)
        {
            ballsRaffled.balls.Add(int.Parse(GameManager.instance.globeDrawData.bolasSorteadas[i]));
        }
        ballsRaffled.sorteioOrdem = GameManager.instance.globeData.GetOrder();
        string jsonToSend = JsonUtility.ToJson(ballsRaffled);

        string response = await comunication.Post(baseUrl + payloadDrawGlobe, jsonToSend);
        
        GlobeManager globeController = FindObjectOfType<GlobeManager>();

        JsonUtility.FromJsonOverwrite(response, GameManager.instance.globeDrawData);
        GameManager.instance.PopulateListOfVisibleTicket();
        if (globeController != null)
        {

            globeController.SendBallsRaffledToScreen();
        }
    }
    public async void PostWriteMemory()
    {
        if (!GameManager.instance.isBackup)
        {
            string jsonToSend = JsonUtility.ToJson(GameManager.instance.operatorData);

            string response = await comunication.Post(baseUrl + payloadWrite, jsonToSend);

        }
    }
    string readMemoryDataSaved = string.Empty;
    public async void GetReadMemory()
    {
        string response = await comunication.Get(baseUrl + payloadRead);
        Debug.Log("response:  " + response);
        if(readMemoryDataSaved!= response)
        {
            JsonUtility.FromJsonOverwrite(response, GameManager.instance.operatorData);
            readMemoryDataSaved = response;
            GameManager.instance.operatorData.PopulateConfig();
        }
        if (GameManager.instance.isBackup)
        {
            await Task.Delay(1000);
            GetReadMemory();
        } 
    }
   
    private async void GetSpinInfos()
    {
        string response = await comunication.Get(baseUrl + payloadInfosSpin);
        JsonUtility.FromJsonOverwrite(response, GameManager.instance.spinData);
    }
    public async void PostResultSpin(int index)
    {
        GameManager.RequestSpin requestSpin = new GameManager.RequestSpin();
        requestSpin.sorteioOrdem = index;

        string jsonToSend = JsonUtility.ToJson(requestSpin);

        string response = await comunication.Post(baseUrl + payloadDrawSpin, jsonToSend);
        SpinController spinController = FindObjectOfType<SpinController>();
        if (response!= string.Empty)
        {
            JsonUtility.FromJsonOverwrite(response, GameManager.instance.spinDrawData);

            spinController.ShowNumberLuckySpin();
        }
        else
        {
            spinController.btGenerateLuckyNumber.interactable = true;
        }
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
}
