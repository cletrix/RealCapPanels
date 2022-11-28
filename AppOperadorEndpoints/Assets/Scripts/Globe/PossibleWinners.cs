using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PossibleWinners : MonoBehaviour
{
    [SerializeField] private string infos;
    private TextMeshProUGUI textInfo;
    [SerializeField]private Button button;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private int index;
    void Start()
    {
        InitializeVariables();
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    void InitializeVariables()
    {
        textInfo = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectWinner);
    }
    public void SelectWinner()
    {
        GlobeController globe = FindObjectOfType<GlobeController>();
        globe.ResetPossiblesWinners();
        globe.UpdateStateVisibilityButtonsTicket(true);
        button.image.color = selectedColor;
        isSelected = true;
    }
    public void SetInteractableButton(bool _isActive)
    {
        button.interactable = _isActive;
        button.image.color = normalColor;
    }
    public void SetIndex(int _index)
    {
        index = _index;
    }
    public int GetIndex()
    {
        return index;
    }
    public bool GetIsSelected()
    {
        return isSelected;
    }
    public void DesactiveIsSelect()
    {
        isSelected = false;
        button.image.color = normalColor;
    }
    public void PopulateInfos(string _infos)
    {
        textInfo = GetComponentInChildren<TextMeshProUGUI>();
        infos = _infos;
        textInfo.text = infos;
    }
}
