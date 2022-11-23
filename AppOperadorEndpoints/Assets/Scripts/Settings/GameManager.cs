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

    public RecoveryScriptable recoveryScriptable;
    [Space]
    public EditionInfosScriptable editionScriptable;
    public technicalScriptable technicalScriptable;
    [Space]
    public LotteryScriptable lotteryScriptable;
    public LotteryResultScriptable lotteryResultScriptable;
    [Space]
    public GlobeScriptable globeScriptable;
    public GlobeRaffleScriptable globeRaffleScriptable;
    [Space]
    public SpinScriptable spinScriptable;
    public SpinResultScriptable spinResultScriptable;

    [Header("Settings")]
    public bool isConnected = false;
    public bool isVisibleRaffle = false;
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
        //globeRaffleScriptable.bolasSorteadas.Clear();
        //foreach (var item in recoveryScriptable.globe_balls)
        //{
        //    globeRaffleScriptable.bolasSorteadas.Add(item.ToString());
        //}

        if (isbackup)
        {
            GlobeController globeController = FindObjectOfType<GlobeController>();
            if (globeController != null)
                globeController.UpdateScreen();
        }
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

    #region GLOBE FUNCTIONS
    public void SetNewBall(string newBall)
    {
        if (!globeRaffleScriptable.bolasSorteadas.Contains(newBall))
        {
            globeRaffleScriptable.SetNewBall(newBall);
            RestNetworkManager.instance.SendBallsRaffledFromServer(globeScriptable.sorteioOrdem);
            WriteInfosGlobe();
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
            WriteInfosGlobe();
        }
    }
    public List<string> GetForOneBalls()
    {
        List<string> forOneBalls = new List<string>();

        for (int i = 0; i < globeRaffleScriptable.porUmaBolas.Count; i++)
        {
            forOneBalls.Add($"{globeRaffleScriptable.porUmaBolas[i].numeroBola} - {globeRaffleScriptable.porUmaBolas[i].numeroChance} - {globeRaffleScriptable.porUmaBolas[i].numeroTitulo}");
        }
        WriteInfosGlobe();
        return forOneBalls;
    }

    public List<string> GetWinners()
    {
        List<string> winners = new List<string>();

        for (int i = 0; i < globeRaffleScriptable.ganhadorContemplado.Length; i++)
        {
            winners.Add($"{globeRaffleScriptable.ganhadorContemplado[i].numeroTitulo} - {globeRaffleScriptable.ganhadorContemplado[i].chance} ");
        }
        WriteInfosGlobe();
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
        globeRaffleScriptable.porDuasBolas = 0;
        globeRaffleScriptable.ganhadorContemplado = new TicketInfos[0];
        WriteInfosGlobe();
    }
    #endregion

    public void WriteInfosGlobe()
    {
        technicalScriptable.UpdateConfig(sceneId, isVisibleRaffle, globeRaffleScriptable.porDuasBolas, globeRaffleScriptable.porUmaBolas);
    }
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

