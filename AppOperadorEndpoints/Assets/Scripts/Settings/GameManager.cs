using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
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
    public bool isbackup;


    public static event Action OnPopulateRaffles;

    [Space]
    [SerializeField] public GlobalScriptable globalScriptable;
    [SerializeField] public ConfigScriptable configScriptable;
    [Space]
    [SerializeField] public LotteryScriptable lotteryScriptable;
    [SerializeField] public LotteryResultScriptable lotteryResultScriptable;
    [Space]
    [SerializeField] public GlobeScriptable globeScriptable;
    [SerializeField] public GlobeRaffleScriptable globeRaffleScriptable;
    [Space]
    [SerializeField] public SpinScriptable spinScriptable;
    [SerializeField] public SpinResultScriptable spinResultScriptable;

    [Header("Settings")]
    public bool isConnected = false;
    public bool hasVisibleRaffle = false;
    public int EditionIndex { get; private set; }

    public void SetEditionIndex(int value)
    {
        EditionIndex = value;
    }
    public int sceneId;
    void Start()
    {
        globeRaffleScriptable.ResetInfos();
    }
    public void RecoveryScreen()
    {
        configScriptable.PopulateConfig();
    
        UIChangeRaffleType uIChangeRaffle = FindObjectOfType<UIChangeRaffleType>();
        uIChangeRaffle.SelectPanelForActivate(sceneId);

        GlobeController globeController = FindObjectOfType<GlobeController>();
        globeController.UpdateScreen();
        globeController.UpdateStateVisibilityButtonsTicket(false);

        RestNetworkManager.instance.SendBallsRaffledFromServer(globeScriptable.sorteioOrdem, true);
        uIChangeRaffle.DefineModalyties(GameManager.instance.globalScriptable.edicaoInfos[GameManager.instance.EditionIndex].modalidades);
    }
 
    public void LoadSceneGame(string map)
    {
        StartCoroutine(ChangeScene(map));
    }
    private IEnumerator ChangeScene(string sceneName)
    {
        yield return new WaitForSeconds(0.8f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void CallEventLogin()
    {
        isConnected = true;
        OnPopulateRaffles?.Invoke();
    }
    //private void LoadLotteryInfos()
    //{
    //    //StartCoroutine(GetLotteryInfos(baseUrl1 + RestNetworkManager.instance.urlInfoLottery));
    //    //StartCoroutine(GetLotteryInfos(baseUrl2 + RestNetworkManager.instance.urlInfoLottery));

    //}
    //private IEnumerator GetLotteryResult(string uri)
    //{

    //    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    //    {
    //        yield return webRequest.SendWebRequest();

    //        string[] pages = uri.Split('/');
    //        int page = pages.Length - 1;

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                {
    //                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
    //                    string json = webRequest.downloadHandler.text;
    //                    JsonUtility.FromJsonOverwrite(json, lotteryResultScriptable);
    //                }
    //                break;
    //        }
    //    }
    //}
    //private void LoadLotteryResult()
    //{
    //    //// StartCoroutine(GetLotteryResult(baseUrl1 + RestNetworkManager.instance.urlResultLottery));
    //    //StartCoroutine(GetLotteryResult(baseUrl2 + RestNetworkManager.instance.urlResultLottery));

    //}

    #region GLOBE FUNCTIONS
    public void SetNewBall(string newBall)
    {
        if (!globeRaffleScriptable.bolasSorteadas.Contains(newBall))
        {
            globeRaffleScriptable.SetNewBall(newBall);
            RestNetworkManager.instance.SendBallsRaffledFromServer(globeScriptable.sorteioOrdem);
        }
    }
    public List<String> GetBallsRaffled()
    {
        return globeRaffleScriptable.bolasSorteadas;
    }
    public void SetRemoveBall(string newBall)
    {
        if (globeRaffleScriptable.bolasSorteadas.Contains(newBall))
        {
            globeRaffleScriptable.RevokeBall(newBall);
            RestNetworkManager.instance.SendBallsRaffledFromServer(globeScriptable.sorteioOrdem);
        }
    }
    public List<string> GetForOneBalls()
    {
        List<string> forOneBalls = new List<string>();

        for (int i = 0; i < globeRaffleScriptable.porUmaBolas.Count; i++)
        {
            forOneBalls.Add($"{globeRaffleScriptable.porUmaBolas[i].numeroBola} - {globeRaffleScriptable.porUmaBolas[i].numeroChance} - {globeRaffleScriptable.porUmaBolas[i].numeroTitulo}");
        }
        return forOneBalls;
    }

    public List<string> GetWinners()
    {
        List<string> winners = new List<string>();

        for (int i = 0; i < globeRaffleScriptable.ganhadorContemplado.Length; i++)
        {
            winners.Add($"{globeRaffleScriptable.ganhadorContemplado[i].numeroTitulo} - {globeRaffleScriptable.ganhadorContemplado[i].chance} ");
        }
        return winners;
    }
    public string GetForTwoBalls()
    {
        string result = string.Empty;
        result = globeRaffleScriptable.porDuasBolas.ToString();
        return result;
    }
    public string GetWinnersCount()
    {
        string result = string.Empty;
        result = globeRaffleScriptable.ganhadorContemplado.ToString();
        return result;
    }

    public void ResetScreenGlobe()
    {
        globeRaffleScriptable.bolasSorteadas.Clear();
        globeRaffleScriptable.porUmaBolas.Clear();
        globeRaffleScriptable.porDuasBolas=0;
        globeRaffleScriptable.ganhadorContemplado = new TicketInfos[0];
    }
    #endregion

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Application.Quit();
            }
        }
    }

    [Serializable]
    public class RequestBallsRaffled
    {
        public List<int> balls;
        public int sorteioOrdem;
    }


    [Serializable]
    public class RequestSpin
    {
        public int sorteioOrdem;
    }
}

