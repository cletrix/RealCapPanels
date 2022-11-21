using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
public class UiManagerLoginScene : MonoBehaviour
{
    [Header("GERAL REFERENCES")]
    [SerializeField] private FadeController fadeController;

    [Header("LOGIN COMPONENTS")]
    [SerializeField] private GameObject panelLogin;

    [Header("COMPONENTS")]
    [SerializeField] private GameObject panelSelectEdition;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputPassword;
    public Button btSelectBackup;
    public bool hasbackup = false;
    void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        fadeController = FindObjectOfType<FadeController>();
        panelSelectEdition.SetActive(false);
        panelLogin.SetActive(true);
        btSelectBackup.onClick.AddListener(SelectBackup);
    }
    public void SelectBackup()
    {
        if (GameManager.instance.isbackup)
        {
            GameManager.instance.isbackup = false;
            btSelectBackup.image.color = Color.white;
            //btSelectBackup.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        else
        {
            GameManager.instance.isbackup = true;
            btSelectBackup.image.color = Color.green;
           // btSelectBackup.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }
    public void Login()
    {
        string url = RestNetworkManager.instance.baseUrl1 + RestNetworkManager.instance.urlLogin;
        StartCoroutine(GetLoginInfos(url));
    }
    private IEnumerator GetLoginInfos(string uri)
    {
        print("urlLogin   " + uri);
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
                        JsonUtility.FromJsonOverwrite(json, GameManager.instance.globalScriptable);
                        GameManager.instance.CallEventLogin();
                        panelSelectEdition.SetActive(true);
                        panelLogin.SetActive(false);
                        PopulateDropdownEditions();
                    }
                    break;
            }
        }
    }
  
    public void PopulateDropdownEditions()
    {
        dropdown.ClearOptions();
        List<string> newOptions = new List<string>();

        for (int i = 0; i < GameManager.instance.globalScriptable.edicaoInfos.Count; i++)
        {
            string option = $"{GameManager.instance.globalScriptable.edicaoInfos[i].numero} - {GameManager.instance.globalScriptable.edicaoInfos[i].nome} - {GameManager.instance.globalScriptable.edicaoInfos[i].dataRealizacao}";
            newOptions.Add(option);
        }
        dropdown.AddOptions(newOptions);
    }
    public void ConfirmSelection()
    {
        StartCoroutine(SelectEdition());
    }
    private IEnumerator SelectEdition()
    {
        GameManager.instance.SetEditionIndex(dropdown.value);
        fadeController.SetStateFadeOUT();
        yield return new WaitForSeconds(1f);
        GameManager.instance.LoadSceneGame("MainScene");
    }

    public class RequestLogin
    {
        public string usuario;
        public string senha;
    }
}
