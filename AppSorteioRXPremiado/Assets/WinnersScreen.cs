using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Globalization;

public class WinnersScreen : MonoBehaviour
{
    public static WinnersScreen instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtCount;
    [SerializeField] private TextMeshProUGUI txtRaffleRound;
    [SerializeField] private GameObject winnersPanel;
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


    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        canvasGroup.alpha = 0;
        winnersPanel.transform.DOScale(new Vector3(0, 0, 0), 0.1f);
    }
    public void SetWinnersScreenVisibility(bool isActive, float timeAnim=1f)
    {
        print("PRIZE VISIBLE? ==>" + isActive);

        if (isActive)
        {
            canvasGroup.DOFade(1, timeAnim).OnComplete(() =>
            {
                winnersPanel.transform.DOScale(1, 1f);
                UiGlobeManager uiGlobeManager = FindObjectOfType<UiGlobeManager>();
                uiGlobeManager.ActiveConfets();
            });
        }
        else
        {
            canvasGroup.DOFade(0, timeAnim).OnComplete(() =>
            {
                winnersPanel.transform.DOScale(0,0.1f);
            });
        }
    }

    public void SetInfosWinnerScreen(int _count, int _prize)
    {
        string prizeFormated = string.Format(CultureInfo.CurrentCulture, _prize.ToString("C2"));
        if(_count>1)
        {
            txtTitle.text = $"TEMOS {_count}\nGANHADORES";
            txtCount.text = $"{prizeFormated} para cada!";
        }
        else
        {
            txtTitle.text = $"TEMOS {_count}\nGANHADOR";
            txtCount.text = $"valor líquido de {prizeFormated}";
        }
        txtRaffleRound.text = $"{GameManager.instance.globeScriptable.order}º Sorteio";

    }
}
