using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
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
    public static event Action OnPopulateRaffles;

    public RecoveryScriptable recoveryScriptable;
    [Space]
    public EditionInfosScriptable editionScriptable;
    public TechnicalScriptable technicalScriptable;
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
    public bool isBackup = false;
    public bool isConnected = false;
    public bool isVisibleRaffle = false;
    public bool isWinner = false;
    public bool isTicketVisible = false;
    public int ticketWinnerIndex = 0;
    public int EditionIndex { get; private set; }

    public void SetEditionIndex(int value)
    {
        EditionIndex = value;
    }
    public int sceneId;
    void Start()
    {
        globeRaffleScriptable.ResetInfos();
        technicalScriptable.ResetInfos();
        recoveryScriptable.ResetInfos();
    }
    public void RecoveryScreen()
    {
        if (isBackup)
        {
            TicketController ticket = FindObjectOfType<TicketController>();
            GlobeController globeController = FindObjectOfType<GlobeController>();
            if (globeController != null && isWinner == false)
            {
                globeController.UpdateScreen();
            }
            else if (ticket != null && isWinner == true)
            {

                if (globeController != null)
                {
                    globeController.PopulateTicketGlobe();
                }

                ticket.CheckStateVisibility();
            }
        }
        UIChangeRaffleType uIChangeRaffleType = FindObjectOfType<UIChangeRaffleType>();
        if (uIChangeRaffleType != null)
        {
            uIChangeRaffleType.CheckStateVisibilityRaffle();
            uIChangeRaffleType.SelectPanelForActivate(technicalScriptable.panelActive);
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
    public string FormatMoneyInfo(int value, int decimalHouse = 2)
    {
        string prizeFormated = string.Format(CultureInfo.CurrentCulture, value.ToString($"C{decimalHouse}"));
        return prizeFormated;
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
    public void PopulateListOfVisibleTicket()
    {
        globeRaffleScriptable.ticketListVisible = new bool[globeRaffleScriptable.ganhadorContemplado.Length];
    }
    public void SetIsVisibleTicketList(int index)
    {
        globeRaffleScriptable.ticketListVisible[index] = true;
    }
    public bool GetAllTicketsVisible()
    {
        int index = 0;
        for (int i = 0; i < globeRaffleScriptable.ticketListVisible.Length; i++)
        {
            if (globeRaffleScriptable.ticketListVisible[i] == true)
            {
                index++;
            }
        }
        if (index >= globeRaffleScriptable.ticketListVisible.Length)
        {
            return true;
        }
        else
        {
            return false;
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
        globeRaffleScriptable.porDuasBolas = 0;
        globeRaffleScriptable.ganhadorContemplado = new TicketInfos[0];

        WriteInfosGlobe();
    }
    #endregion

    public void WriteInfosGlobe()
    {
        technicalScriptable.UpdateConfig(sceneId, globeScriptable.sorteioOrdem, isVisibleRaffle, globeRaffleScriptable.porDuasBolas, globeRaffleScriptable.porUmaBolas);
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Application.Quit();
            }
        }

        if (globeRaffleScriptable.ganhadorContemplado.Length > 0)
        {
            isWinner = true;
        }
        else
        {
            isWinner = false;
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

