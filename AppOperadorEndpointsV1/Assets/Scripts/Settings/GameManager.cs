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
    public Resoucers.EditionData editionData;
    public Resoucers.OperatorData operatorData;
    [Space]
    public Resoucers.GlobeData globeData;
    public Resoucers.GlobeDrawData globeDrawData;
    [Space]
    public Resoucers.SpinData spinData;
    public Resoucers.SpinDrawData spinDrawData;

    [Header("Settings")]
    [SerializeField] private int countBallsCard = 0;
    [SerializeField] private int countBallsGrid = 0;
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
        globeDrawData.ResetInfos();
        operatorData.ResetInfos();
        recoveryData.ResetInfos();
    }
    public void RecoveryGlobeScreen()
    {
        GlobeManager globeController = FindObjectOfType<GlobeManager>();
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
            uIChangeRaffleType.SelectPanelForActivate(operatorData.panelActive);
        }
    }
    public void RecoverySpinScreen()
    {
        SpinController spinController = FindObjectOfType<SpinController>();
        TicketController ticket = FindObjectOfType<TicketController>();
        if (spinController != null && operatorData.spinNumbers.Count > 0)
        {
            spinController.ShowSpinOrder(operatorData.spinIndex);
            spinController.PopulateSpinsFields(operatorData.spinNumbers);
            spinController.PopulateTicketSpin();
            ticket.CheckStateVisibility();
        }

        UIChangeRaffleType uIChangeRaffleType = FindObjectOfType<UIChangeRaffleType>();
        if (uIChangeRaffleType != null)
        {
            uIChangeRaffleType.CheckStateVisibilityRaffle();
            uIChangeRaffleType.SelectPanelForActivate(operatorData.panelActive);
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
        SetMatrizInfos();
    }

    #region GLOBE FUNCTIONS

    public int GetCountBallsGrid()
    {
        return countBallsGrid;

    }
    public int GetCountBallsCard()
    {
        return countBallsCard;

    }
    public void SetMatrizInfos()
    {
        countBallsCard = int.Parse(editionData.edicaoInfos[0].matriz.Substring(0, 2));
        int startIndex = editionData.edicaoInfos[0].matriz.Length - 2;
        countBallsGrid = int.Parse(editionData.edicaoInfos[0].matriz.Substring(startIndex));
    }
    public void SetNewBall(string newBall)
    {
        if (!globeDrawData.bolasSorteadas.Contains(newBall))
        {
            globeDrawData.SetNewBall(newBall);
            RestNetworkManager.instance.PostBallsRaffled();
        }
    }
    public List<String> GetBallsRaffled()
    {
        return globeDrawData.bolasSorteadas;
    }
    public void SetRemoveBall(string newBall)
    {
        if (globeDrawData.bolasSorteadas.Contains(newBall))
        {
            globeDrawData.RevokeBall(newBall);
            RestNetworkManager.instance.PostBallsRaffled();
        }
    }
    public void PopulateListOfVisibleTicket()
    {
        globeDrawData.ticketListVisible = new bool[globeDrawData.ganhadorContemplado.Length];
    }
    public void SetIsVisibleTicketList(int index)
    {
        globeDrawData.ticketListVisible[index] = true;
    }
    public bool GetAllTicketsVisible()
    {
        int index = 0;
        for (int i = 0; i < globeDrawData.ticketListVisible.Length; i++)
        {
            if (globeDrawData.ticketListVisible[i] == true)
            {
                index++;
            }
        }
        if (index >= globeDrawData.ticketListVisible.Length)
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

        for (int i = 0; i < globeDrawData.porUmaBolas.Count; i++)
        {
            forOneBalls.Add($"{globeDrawData.porUmaBolas[i].numeroBola} - {globeDrawData.porUmaBolas[i].numeroChance} - {globeDrawData.porUmaBolas[i].numeroTitulo}");
        }
        return forOneBalls;
    }

    public List<string> GetWinners()
    {
        List<string> winners = new List<string>();

        for (int i = 0; i < globeDrawData.ganhadorContemplado.Length; i++)
        {
            winners.Add($"{globeDrawData.ganhadorContemplado[i].numeroTitulo} - {globeDrawData.ganhadorContemplado[i].chance} ");
        }
        return winners;
    }
    public string GetForTwoBalls()
    {
        string result = string.Empty;
        result = globeDrawData.porDuasBolas.ToString();
        return result;
    }
    public string GetWinnersCount()
    {
        string result = string.Empty;
        result = globeDrawData.ganhadorContemplado.ToString();
        return result;
    }

    public void ResetScreenGlobe()
    {
        globeDrawData.bolasSorteadas.Clear();
        globeDrawData.porUmaBolas.Clear();
        globeDrawData.porDuasBolas = 0;
        globeDrawData.ganhadorContemplado = new TicketInfos[0];

        WriteInfosGlobe();
    }
    #endregion

    public void WriteInfosGlobe()
    {
        if (!instance.isBackup)
        {
            operatorData.UpdateConfig(sceneId, globeData.GetOrder(), isVisibleRaffle, globeDrawData.porDuasBolas, globeDrawData.porUmaBolas
                , globeDrawData.ganhadorContemplado.ToList(),
                globeDrawData.ticketListVisible.ToList(),
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

        if (globeDrawData.ganhadorContemplado.Length > 0)
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

