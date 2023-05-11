using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RiptideNetworking;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Globalization;

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager _instance;

    public static GameManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
    #endregion

    public GlobeScriptable globeScriptable;
    public LuckySpinScriptable luckySpinScriptable;


    // [SerializeField] private FadeController fade;
    [SerializeField] public bool isConnected = false;
    [SerializeField] public bool isLotteryOpenedScreen = false;
    [SerializeField] private string currentSceneName;
    [SerializeField] private int sceneIndex;

    public Camera cameraActive;
    private void Start()
    {
        InitializeVariables();
    }
    public string FormatMoneyInfo(float value, int decimalHouse = 2)
    {
        string prizeFormated = string.Format(CultureInfo.CurrentCulture, value.ToString($"C{decimalHouse}"));
        return prizeFormated;
    }
    private void InitializeVariables()
    {
        Invoke("ConnectServer", 1f);
        Application.targetFrameRate = 60;
        globeScriptable.ResetRaffle();
        WinnersScreen.instance.SetWinnersScreenVisibility(false, 0.1f);
        TicketScreen.instance.SetTicketVisibility(false, 0.1f);
    }
    public void ConnectServer()
    {
        NetworkManager.instance.Connect();
    }
    public void SetCamActiveInCanvas(Camera main)
    {
        WinnersScreen winners = FindObjectOfType<WinnersScreen>();
        winners.GetComponent<Canvas>().worldCamera = main;

        TicketScreen ticket = FindObjectOfType<TicketScreen>();
        ticket.GetComponent<Canvas>().worldCamera = main;
    }
    public void ResetScene()
    {
        globeScriptable.ResetRaffle();
        SceneManager.LoadScene(currentSceneName);

    }
    public void CallChangeSceneRaffle(string sceneName)
    {
        StartCoroutine(ChangeSceneRaffle(sceneName));
    }
    private IEnumerator ChangeSceneRaffle(string sceneName)
    {
        if (currentSceneName != sceneName)
        {
            currentSceneName = sceneName;
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
            yield return new WaitForSeconds(0.5f);
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 3 && WinnersScreen.instance.GetAlphaValue() >= 0.9f)
            {
                WinnersScreen.instance.SetWinnersScreenVisibility(false);
            }


            SendMessageToServerConfirmChange(true, sceneIndex);
        }
    }
    #region SendMessages
    public void SendMessageToServerVisibilityScene(bool _state)
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.messageCheckVisibilityScreen);
        message.AddBool(_state);

        NetworkManager.Client.Send(message);
        NetworkManager.Client2.Send(message);
    }
    public void SendMessageToServerGetActiveScene(int index)
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.messageGetActiveScene);
        message.AddInt(index);
        NetworkManager.Client.Send(message);
        NetworkManager.Client2.Send(message);
    }
    public void SendMessageToServerConfirmChange(bool _state, int _scene)
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.messageChangeScene);
        message.AddBool(_state);
        message.AddInt(_scene);
        NetworkManager.Client.Send(message);
        NetworkManager.Client2.Send(message);
    }

    public int GetSceneIndex()
    {
        return sceneIndex;
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
}


