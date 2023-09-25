using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    public Resoucers.RecoveryData recoveryData;
    [Space]
    public Resoucers.EditionData editonData;
    public Resoucers.OperatorData OperatorData;
    [Space]
    public LotteryScriptable lotteryScriptable;
    public LotteryResultScriptable lotteryResultScriptable;
    [Space]
    public GlobeScriptable globeScriptable;
    public GlobeRaffleScriptable globeDraw;
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
        globeDraw.ResetInfos();
        OperatorData.ResetInfos();
        recoveryData.ResetInfos();
    }
    public void RecoveryGlobeScreen()
    {
        GlobeController globeController = FindObjectOfType<GlobeController>();
        if (globeController != null)
        {
            if (isBackup)
            {
                globeController.UpdateScreen();
                TicketController ticket = FindObjectOfType<TicketController>();
                if (isWinner == true)
                {
                    globeController.PopulateTicketGlobe();
                    ticket.CheckStateVisibility();
                    globeController.UpdateStateVisibilityButtonsTicket(false);
                }

            }
        }

        UIChangeRaffleType uIChangeRaffleType = FindObjectOfType<UIChangeRaffleType>();
        if (uIChangeRaffleType != null)
        {
            uIChangeRaffleType.CheckStateVisibilityRaffle();
            uIChangeRaffleType.SelectPanelForActivate(OperatorData.panelActive);
        }
    }
    public void RecoverySpinScreen()
    {
        SpinController spinController = FindObjectOfType<SpinController>();
        TicketController ticket = FindObjectOfType<TicketController>();
        if (spinController != null && OperatorData.spinNumbers.Count > 0)
        {
            spinController.ShowSpinOrder(OperatorData.spinIndex);
            spinController.PopulateSpinsFields(OperatorData.spinNumbers);
            spinController.PopulateTicketSpin();
            ticket.CheckStateVisibility();
        }

        UIChangeRaffleType uIChangeRaffleType = FindObjectOfType<UIChangeRaffleType>();
        if (uIChangeRaffleType != null)
        {
            uIChangeRaffleType.CheckStateVisibilityRaffle();
            uIChangeRaffleType.SelectPanelForActivate(OperatorData.panelActive);
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
    public string FormatMoneyInfo(float value, int decimalHouse = 2)
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
        if (!globeDraw.bolasSorteadas.Contains(newBall))
        {
            globeDraw.SetNewBall(newBall);
            RestNetworkManager.instance.SendBallsRaffledFromServer();
        }
    }
    public List<String> GetBallsRaffled()
    {
        return globeDraw.bolasSorteadas;
    }
    public void SetRemoveBall(string newBall)
    {
        if (globeDraw.bolasSorteadas.Contains(newBall))
        {
            globeDraw.RevokeBall(newBall);
            RestNetworkManager.instance.SendBallsRaffledFromServer();
        }
    }
    public void PopulateListOfVisibleTicket()
    {
        globeDraw.ticketListVisible = new bool[globeDraw.ganhadorContemplado.Length];
    }
    public void SetIsVisibleTicketList(int index)
    {
        globeDraw.ticketListVisible[index] = true;
    }
    public bool GetAllTicketsVisible()
    {
        int index = 0;
        for (int i = 0; i < globeDraw.ticketListVisible.Length; i++)
        {
            if (globeDraw.ticketListVisible[i] == true)
            {
                index++;
            }
        }
        if (index >= globeDraw.ticketListVisible.Length)
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

        for (int i = 0; i < globeDraw.porUmaBolas.Count; i++)
        {
            forOneBalls.Add($"{globeDraw.porUmaBolas[i].numeroBola} - {globeDraw.porUmaBolas[i].numeroChance} - {globeDraw.porUmaBolas[i].numeroTitulo}");
        }
        return forOneBalls;
    }

    public List<string> GetWinners()
    {
        List<string> winners = new List<string>();

        for (int i = 0; i < globeDraw.ganhadorContemplado.Length; i++)
        {
            winners.Add($"{globeDraw.ganhadorContemplado[i].numeroTitulo} - {globeDraw.ganhadorContemplado[i].chance} ");
        }
        return winners;
    }
    public string GetForTwoBalls()
    {
        string result = string.Empty;
        result = globeDraw.porDuasBolas.ToString();
        return result;
    }
    public string GetWinnersCount()
    {
        string result = string.Empty;
        result = globeDraw.ganhadorContemplado.ToString();
        return result;
    }

    public void ResetScreenGlobe()
    {
        globeDraw.bolasSorteadas.Clear();
        globeDraw.porUmaBolas.Clear();
        globeDraw.porDuasBolas = 0;
        globeDraw.ganhadorContemplado = new TicketInfos[0];

        WriteInfosGlobe();
    }
    #endregion

    public void WriteInfosGlobe()
    {
        if (!instance.isBackup)
        {
            OperatorData.UpdateConfig(sceneId, globeScriptable.GetGlobeOrder(), isVisibleRaffle, globeDraw.porDuasBolas, globeDraw.porUmaBolas
                , globeDraw.ganhadorContemplado.ToList(),
                globeDraw.ticketListVisible.ToList(),
               ticketWinnerIndex, instance.isTicketVisible);
        }
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

        if (globeDraw.ganhadorContemplado.Length > 0)
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

