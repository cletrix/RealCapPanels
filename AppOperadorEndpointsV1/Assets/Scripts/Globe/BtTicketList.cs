using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BtTicketList : MonoBehaviour
{
    public static event Action<TicketInfos> OnShowticket;
    private TextMeshProUGUI textInfo;
    private Button button;

    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isFinished = false;
    [SerializeField] private int index;
    [SerializeField] private string infos;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color finishedColor;

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
        OnShowticket?.Invoke(GameManager.instance.globeDraw.ganhadorContemplado[index]);
        button.image.color = selectedColor;
        isSelected = true;

    }
    public void SetInteractableButton(bool _isActive)
    {
        button.interactable = _isActive;
    }
    public void SetNormalColor()
    {
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
    public void SetIsFinished(bool _isActive)
    {
        isFinished = _isActive;
        DesactiveIsSelect();

    }
    public void DesactiveIsSelect()
    {
        if (isFinished)
        {
            button.image.color = finishedColor;
        }
        else
        {
            button.image.color = normalColor;
        }
        isSelected = false;
    }
    public void PopulateInfos(string _infos)
    {
        textInfo = GetComponentInChildren<TextMeshProUGUI>();
        infos = _infos;
        textInfo.text = infos;
    }
}
