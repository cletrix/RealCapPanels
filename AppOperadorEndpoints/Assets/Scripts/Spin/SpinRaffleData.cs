using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpinRaffleData : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private bool hasFinishedRaffle = false;
    public string ticketNumber;

    [Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI txtNumberSpin;
    [SerializeField] private TextMeshProUGUI txtNumberTicket;
    [SerializeField] private TextMeshProUGUI txtNumberPrize;

    [Header("BUTTONS")]
    [SerializeField] private Button btShowTicket;

    [Header("BACKGROUND IMAGES")]
    [SerializeField] private List<Image> bgImages = new List<Image>();

    [Header("COLORS")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color finishedColor;

    [Header("BACKGROUND COLORS")]
    [SerializeField] private Color BgNormalColor;
    [SerializeField] private Color BgSelectedColor;
    [SerializeField] private Color BgFinishedColor;

    private GameObject thisObject;
    void Start()
    {
        thisObject = this.gameObject;
    }

    public void InitializeVariables()
    {
        SetStateInteractableButton(false);
    }
    public void SetNumberSpin(int _number)
    {
        txtNumberSpin.text = _number.ToString();
    }

    public void SetSpinPrize(string _value)
    {
        txtNumberPrize.text = $"R$ {_value}";
    }
    public void SetNumberTicket(string _number)
    {
        txtNumberTicket.text = _number;
        ticketNumber = _number;
    }
    public void SetStateInteractableButton(bool _isActive)
    {
        if (GameManager.instance.isBackup)
        {
            btShowTicket.interactable = false;
        }
        else
        {
            btShowTicket.interactable = _isActive;
        }
    }

    public void SetEventButton(UnityAction action)
    {
        btShowTicket.onClick.AddListener(action);
    }
    public void SetStateFinishedRaffle(bool _isfinished)
    {
        hasFinishedRaffle = _isfinished;
    }

    public bool GetIsFinishedRaffle()
    {
        return hasFinishedRaffle;
    }
    #region SetColors
    public void SetBtNormalColor()
    {
        btShowTicket.image.color = normalColor;
    }
    public void SetBtSelectedColor()
    {
        btShowTicket.image.color = selectedColor;
    }
    public void SetBtFinishedColor()
    {
        btShowTicket.image.color = finishedColor;
        btShowTicket.interactable = false;
    }
    public void SetBgNormalColor()
    {
        foreach (var item in bgImages)
        {

            item.color = BgNormalColor;
        }
    }
    public void SetBgSelectedColor()
    {
        foreach (var item in bgImages)
        {
            item.color = BgSelectedColor;
        }
    }
    public void SetBgFinishedColor()
    {
        foreach (var item in bgImages)
        {
            item.color = BgFinishedColor;
        }
    }
    #endregion

}
