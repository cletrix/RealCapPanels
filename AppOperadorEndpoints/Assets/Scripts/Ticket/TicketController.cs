using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using RiptideNetworking;
using System;
using System.Globalization;
using System.Linq;

public class TicketController : MonoBehaviour
{
    [Header("TICKET IMAGE")]
    [SerializeField] private Sprite bgticketGlobe;
    [SerializeField] private Sprite bgticketSpin;
    [SerializeField] private Image imgTicket;

    [Header("WINNER INFOS")]
    [SerializeField] private TextMeshProUGUI titleDescription;
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
    [SerializeField] private TextMeshProUGUI cep;
    [SerializeField] private TextMeshProUGUI state;
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

    [Header("Settings")]
    [SerializeField] private GameObject groupCard;
    [SerializeField] private Button btBackTicket;
    [SerializeField] private GameObject bgTicket;

    [SerializeField] public bool canShowPrize = false;

    private void Start()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        bgTicket = transform.GetChild(0).gameObject;
        btBackTicket = bgTicket.transform.GetComponentInChildren<Button>();

        bgTicket.SetActive(false);
    }
    public void SetButtonEvent(UnityAction action)
    {
        btBackTicket.onClick.AddListener(action);
    }
    
    public void SetTicketVisibility()
    {
        if (GameManager.instance.isTicketVisible)
        {
            GameManager.instance.isTicketVisible = false;
        }
        else
        {
            GameManager.instance.isTicketVisible = true;
        }
        GameManager.instance.WriteInfosGlobe();

        CheckStateVisibility();
    }
    public void CheckStateVisibility()
    {
        if (GameManager.instance.isTicketVisible)
        {
            bgTicket.SetActive(true);
        }
        else
        {
            bgTicket.SetActive(false);
            SpinController spin = FindObjectOfType<SpinController>();
            if (spin != null)
            {
                spin.ActiveButtonNewRaffleSpin();
            }
            SendMessageToClientHideTicket(GameManager.instance.isTicketVisible);
        }
    }
    
    private string RevertDate(string date)
    {
        DateTime dateTime = System.DateTime.Parse(date);
        return dateTime.ToString("dd/MM/yyyy");
    }
    private string DateRaffle(string date)
    {
        DateTime dateTime = System.DateTime.Parse(date);
        return dateTime.ToString("dd/MM/yyyy - HH:mm");
    }
    private string HidePartCPF(string cpf)
    {
        cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        string cpfFormated = cpf.Substring(0, 8);
        cpfFormated += "XXX-XX";
        return cpfFormated;
    }
    private string HidePartCEP(string cep)
    {
        string Newcep = cep;
        Newcep = "XXXXX-XXX";
        return Newcep;
    }
    private string HidePartBirthDate(string date)
    {
        DateTime dateTime = System.DateTime.Parse(date);

        string dateFormated = dateTime.ToString("dd/MM/yyyy").Substring(0, 5);
        dateFormated += "/XXXX";
        return dateFormated;
    }
    private string HidePartPhone(string info)
    {
        string newInfo = info.Substring(0, 9);
        newInfo += "-XXXX";
        return newInfo;
    }
    private string HidePartEmail(string info)
    {
        string[] newInfos = info.Split('@');
        string newInfo = newInfos[0];
        newInfo += "@xxxxxxxxx";
        return newInfo;
    }
    private string FormatMoneyInfo(int value)
    {
        string prizeFormated = string.Format(CultureInfo.CurrentCulture, value.ToString("C2"));
        return prizeFormated;
    }

    private string CheckIsNullInfo(string info)
    {
        if (info != "nan")
            return info;
        else
            return "XXXXX";
    }

    private string CheckPDV(string _pdv)
    {
        if (_pdv != "nan")
            return _pdv;
        else
            return "Compra Online";
    }
    private string CheckDistrictPDV(string _districtPDV)
    {
        if (_districtPDV != "nan")
            return _districtPDV;
        else
            return "SITE";
    }
    public void PopulateTicketInfos(string _nameWinner, string _cpf, string _birthDate, string _phone,
        string _email, string _address, string _numberAddress, string _complement, string _district,
        string _county, string _cep, string _state, string _dateRaffle, string _editionName, int _value, string _PDV,
        string _districtPDV, string _dateBuy, string _hourBuy, string _ticketNumber, string _chance,
        List<int> _numbersCard, string _luckyNumber, bool _isCard = false, int _typeRaffle = 1)
    {
        nameWinner.text = $"{CheckIsNullInfo(_nameWinner)}";
        cpf.text = $"{HidePartCPF(_cpf)}";
        birthDate.text = $"{HidePartBirthDate(_birthDate)}";
        phone.text = $"{HidePartPhone(_phone)}";
        email.text = $"{HidePartEmail(_email)}";
        address.text = $"{CheckIsNullInfo(_address)}";
        numberAddress.text = $"{CheckIsNullInfo(_numberAddress)}";
        complement.text = $"{CheckIsNullInfo(_complement)}";
        district.text = $"{CheckIsNullInfo(_district)}";
        county.text = $"{CheckIsNullInfo(_county)}";
        cep.text = $"{HidePartCEP(_cep)}";
        state.text = $"{CheckIsNullInfo(_state)}";
        dateRaffle.text = $"{DateRaffle(_dateRaffle)}";
        editionName.text = $"{CheckIsNullInfo(_editionName)}";
        value.text = $"{FormatMoneyInfo(_value)}";
        PDV.text = $"{CheckPDV(_PDV)} \n{CheckDistrictPDV(_districtPDV)}";
        dateAndHourBuy.text = $"\n{RevertDate(_dateBuy)} - {_hourBuy}";
        numberTicket.text = CheckIsNullInfo(_ticketNumber);
        Chance.text = $"{CheckIsNullInfo(_chance)}";

        if (_isCard)
        {
            groupCard.SetActive(true);
            luckyNumber.gameObject.SetActive(false);
            for (int i = 0; i < _numbersCard.Count; i++)
            {
                numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().text = _numbersCard[i].ToString();
                numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                if (_numbersCard[i].ToString() == GameManager.instance.globeRaffleScriptable.bolasSorteadas[GameManager.instance.globeRaffleScriptable.bolasSorteadas.Count - 1])
                {
                    numbersCard[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                    // numbersCard[i].GetComponent<Image>().color = Color.red;
                }
            }
            imgTicket.sprite = bgticketGlobe;
        }
        else
        {
            groupCard.SetActive(false);
            luckyNumber.gameObject.SetActive(true);
            luckyNumber.text = CheckIsNullInfo(_luckyNumber);
            imgTicket.sprite = bgticketSpin;

        }
        string[] _ticketInfos = new string[22];
        _ticketInfos[0] = CheckIsNullInfo(_nameWinner);
        _ticketInfos[1] = HidePartCPF(_cpf);
        _ticketInfos[2] = HidePartBirthDate(_birthDate);
        _ticketInfos[3] = HidePartPhone(_phone);
        _ticketInfos[4] = HidePartEmail(_email);
        _ticketInfos[5] = CheckIsNullInfo(_address);
        _ticketInfos[6] = CheckIsNullInfo(_numberAddress);
        _ticketInfos[7] = CheckIsNullInfo(_complement);
        _ticketInfos[8] = CheckIsNullInfo(_district);
        _ticketInfos[9] = CheckIsNullInfo(_county);
        _ticketInfos[10] = HidePartCEP(_cep);
        _ticketInfos[11] = CheckIsNullInfo(_state);
        _ticketInfos[12] = RevertDate(_dateRaffle);
        _ticketInfos[13] = CheckIsNullInfo(_editionName);
        _ticketInfos[14] = FormatMoneyInfo(_value);
        _ticketInfos[15] = CheckPDV(_PDV);
        _ticketInfos[16] = CheckDistrictPDV(_districtPDV);
        _ticketInfos[17] = RevertDate(_dateBuy);
        _ticketInfos[18] = _hourBuy;
        _ticketInfos[19] = CheckIsNullInfo(_ticketNumber);
        _ticketInfos[20] = CheckIsNullInfo(_chance);
        _ticketInfos[21] = CheckIsNullInfo(_luckyNumber);

        if (!GameManager.instance.isBackup)
        {
           
            SendMessageToClientShowTicket(GameManager.instance.isTicketVisible, _ticketInfos, _numbersCard.ToArray(), _isCard, _typeRaffle);
        }
    }
    #region Messages
    public void SendMessageToClientShowTicket(bool _canShowTicket, string[] _ticketInfos, int[] _numbersCard, bool _isCard, int _typeRaffle)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageShowTicket(Message.Create(MessageSendMode.reliable, ServerToClientId.messageShowTicket), _canShowTicket, _ticketInfos, _numbersCard, _isCard, _typeRaffle));
    }
    private Message GetMessageShowTicket(Message message, bool _canShowTicket, string[] _ticketInfos, int[] _numbersCard, bool _isCard, int _typeRaffle)
    {
        message.AddBool(_canShowTicket);
        message.AddStrings(_ticketInfos);
        message.AddInts(_numbersCard);
        message.AddBool(_isCard);
        message.AddInt(_typeRaffle);
        return message;
    }
    public void SendMessageToClientHideTicket(bool _canShowTicket)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageHideTicket(Message.Create(MessageSendMode.reliable, ServerToClientId.messageHideTicket), _canShowTicket));
    }
    private Message GetMessageHideTicket(Message message, bool _canShowTicket)
    {
        message.AddBool(_canShowTicket);
        return message;
    }

    public void SendMessageToClientVisibilityPrize(bool _canShowPrize)
    {
        TcpNetworkManager.instance.Server.SendToAll(GetMessageHideTicket(Message.Create(MessageSendMode.reliable, ServerToClientId.messageVisibilityPrize), _canShowPrize));
    }
    #endregion
}
