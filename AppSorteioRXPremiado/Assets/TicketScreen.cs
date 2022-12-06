using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;

public class TicketScreen : MonoBehaviour
{
    [Header("INFOS SUPERIOR")]
    public GameObject globeSuperior;
    public GameObject spinSuperior;
    public List<SpinSlot> spins;
    [Header("TICKET IMAGE")]
    [SerializeField] private Sprite bgticketGlobe;
    [SerializeField] private Sprite bgticketSpin;
    [SerializeField] private Image imgTicket;

    [Header("WINNER INFOS")]
    [SerializeField] private TextMeshProUGUI prizeNameGlobe;
    [SerializeField] private TextMeshProUGUI prizeNameSpin;
    [SerializeField] private TextMeshProUGUI txtGlobeRound;
    [SerializeField] private TextMeshProUGUI txtSpinRound;
    [Space]
    [SerializeField] private TextMeshProUGUI nameWinner;
    [SerializeField] private TextMeshProUGUI cpf;
    [SerializeField] private TextMeshProUGUI birthDate;
    [SerializeField] private TextMeshProUGUI phone;
    [SerializeField] private TextMeshProUGUI email;
    [SerializeField] private TextMeshProUGUI address;
    [SerializeField] private TextMeshProUGUI numberAddress;
    [SerializeField] private TextMeshProUGUI complement;
    [SerializeField] private TextMeshProUGUI district;
    [SerializeField] private TextMeshProUGUI county;
    [SerializeField] private TextMeshProUGUI state;
    [SerializeField] private TextMeshProUGUI cep;
    [SerializeField] private TextMeshProUGUI dateRaffle;
    [SerializeField] private TextMeshProUGUI editionName;
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private TextMeshProUGUI PDV;
    [SerializeField] private TextMeshProUGUI dateAndHourBuy;
    [Header("RAFFLE INFOS")]
    [SerializeField] private TextMeshProUGUI numberTicket;
    [SerializeField] private TextMeshProUGUI Chance;
    [SerializeField] private List<GameObject> numbersCard;
    [SerializeField] private TextMeshProUGUI luckyNumber;
    [Space]
    [Header("Settings")]
    [SerializeField] private GameObject groupCard;
    [Space]
    [SerializeField] private GameObject bgTicket;
    [SerializeField] private GameObject bgSuperior;
    [Header("REFERENCES")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string lastBallRaffled;

    public static TicketScreen instance { get; private set; }

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


    private void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        canvasGroup.alpha = 0;
    }
    public void SetBgBlackVisibility(bool isActive)
    {
        // txtTitle.transform.DOScale(1.2f, 0.7f).SetLoops(-1, LoopType.Yoyo);

        if (isActive)
        {
            canvasGroup.DOFade(1, 1f);
        }
        else
        {
            canvasGroup.DOFade(0, 1f);
        }
    }
    public void SetResult(string _raffleLuckyNumber)
    {
        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].numberSlotsFinal.Clear();
            spins[i].indexNumber = int.Parse(_raffleLuckyNumber[i].ToString());
            spins[i].ShowNumberNow();
        }
    }
    public void SetTicketVisibility(bool isActive, float timeAnim = 1f)
    {
        if (isActive)
        {  
            SetBgBlackVisibility(true);
           
            WinnersScreen.instance.SetWinnersScreenVisibility(false);
            bgTicket.transform.DOLocalMoveY(-250, timeAnim);
            bgSuperior.transform.DOLocalMoveY(250, timeAnim);
        }
        else
        {
            SetBgBlackVisibility(false);
            bgTicket.transform.DOLocalMoveY(-1000, timeAnim);
            bgSuperior.transform.DOLocalMoveY(1000, timeAnim);
        }
    }

    public void SetTicketInfos(string[] infosWinner, int[] _numbersCard, bool isCard, int typeRaffle)
    {

        if (isCard)
        {
            imgTicket.sprite = bgticketGlobe;
            globeSuperior.SetActive(true);
            spinSuperior.SetActive(false);
            txtGlobeRound.text = $"{GameManager.instance.globeScriptable.order}º Sorteio";
            if (GameManager.instance.globeScriptable.Winners > 1)
            {
                string prizeFormated = string.Format(CultureInfo.CurrentCulture, GameManager.instance.globeScriptable.prizeValue.ToString("C2"));
                prizeNameGlobe.text = prizeFormated;
            }
            else
            {
                prizeNameGlobe.text = GameManager.instance.globeScriptable.description;
            }

        }
        else
        {
            imgTicket.sprite = bgticketSpin;
            spinSuperior.SetActive(true);
            globeSuperior.SetActive(false);
            SetResult(infosWinner[21]);
            txtSpinRound.text = $"{GameManager.instance.luckySpinScriptable.currentSpinID}º Sorteio";
            prizeNameSpin.text = GameManager.instance.luckySpinScriptable.prizeDescription;

        }
        PopulateTicketInfos(
           infosWinner[0],
           infosWinner[1],
           infosWinner[2],
           infosWinner[3],
           infosWinner[4],
           infosWinner[5],
           infosWinner[6],
           infosWinner[7],
           infosWinner[8],
           infosWinner[9],
           infosWinner[10],
           infosWinner[11],
           infosWinner[12],
           infosWinner[13],
           infosWinner[14],
           infosWinner[15],
           infosWinner[16],
           infosWinner[17],
           infosWinner[18],
           infosWinner[19],
           infosWinner[20],
           _numbersCard,
           infosWinner[21],
           isCard,
           typeRaffle);

    }

    public void SetLastBallGlobeRaffle(string _lastBall)
    {
        lastBallRaffled = _lastBall;
    }
    private void PopulateTicketInfos(string _nameWinner, string _cpf, string _birthDate, string _phone,
         string _email, string _address, string _numberAddress, string _complement, string _district,
         string _county, string _cep, string _state, string _dateRaffle, string _editionName, string _value, string _PDV,
         string _districtPDV, string _dateBuy, string _hourBuy, string _ticketNumber, string _chance,
         int[] _numbersCard, string _luckyNumber, bool isCard = false, int typeRaffle = 1)
    {
        nameWinner.text = $"{_nameWinner}";
        cpf.text = $"{_cpf}";
        birthDate.text = $"{_birthDate}";
        phone.text = $"{_phone}";
        email.text = $"{_email}";
        address.text = $"{_address}";
        numberAddress.text = $"{_numberAddress}";
        complement.text = $"{_complement}";
        district.text = $"{_district}";
        county.text = $"{_county}";
        cep.text = $"{_cep}";
        state.text = $"{_state}";
        dateRaffle.text = $"{_dateRaffle}";
        editionName.text = $"{_editionName}";
        value.text = $"{_value}";
        PDV.text = $"{_PDV} \n{_districtPDV}";
        dateAndHourBuy.text = $"\n {_dateBuy} - {_hourBuy}";
        numberTicket.text = _ticketNumber;
        Chance.text = $"{_chance}";

        for (int i = 0; i < _numbersCard.Length; i++)
        {
            numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().text = _numbersCard[i].ToString();
            numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
            if (_numbersCard[i].ToString() == lastBallRaffled)
            {
                numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
        }
        luckyNumber.text = _luckyNumber;

        if (isCard)
        {   
            groupCard.SetActive(true);
            luckyNumber.gameObject.SetActive(false);
            UiGlobeManager uiGlobeManager = FindObjectOfType<UiGlobeManager>();
            uiGlobeManager.ActiveConfets();
        }
        else
        {
            groupCard.SetActive(false);
            luckyNumber.gameObject.SetActive(true);
            LuckySpinController luckySpin = FindObjectOfType<LuckySpinController>();
            luckySpin.confets.SetActive(true);
        }
    }
}
