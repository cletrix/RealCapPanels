using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckySpinController : MonoBehaviour
{

    public GameObject confets;
    public UiLuckySpinController uiLuckySpinController;
    public int index = 0;
    public List<SpinSlot> spins;
    public float speed = 10f;
    public bool stopSlots = false;
    public bool hasStop = true;
    private bool startSpin = false;
    [SerializeField] private bool awaitNextRaffle = false;
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        hasStop = true;
        uiLuckySpinController = FindObjectOfType<UiLuckySpinController>();
        GameManager.instance.luckySpinScriptable.Reset();
        GameManager.instance.globeScriptable.ResetRaffle();
    }
 
    public void SetResult(string _raffleLuckyNumber)
    {
        GameManager.instance.luckySpinScriptable.currentResult = _raffleLuckyNumber;
        GameManager.instance.luckySpinScriptable.allSpinsResult.Add(GameManager.instance.luckySpinScriptable.currentResult);

        // string result = string.Empty;
        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].numberSlotsFinal.Clear();
            spins[i].indexNumber = System.Convert.ToInt16(GameManager.instance.luckySpinScriptable.currentResult[i].ToString());
            spins[i].ReloadPositions();
        }
        ActiveMovementRaffle();
    }
    public void ResetResult()
    {
        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].ReloadPositions();
        }
    }
    public void StartSpin()
    {
        speed = 80;
        StopAllCoroutines();
        stopSlots = false;
        index = 0;
        StartCoroutine(StartMovement(0.2f));
    }
    private IEnumerator StartMovement(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].stopped = false;
            yield return new WaitForSecondsRealtime(0.4f);
            spins[i].MovementSpin();
        }
        yield return new WaitForSecondsRealtime(1);
        StopMovement();
    }
   
    public IEnumerator StopRoll()
    {
        foreach (var item in spins)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            item.ActiveStopSlot();
        }
        yield return new WaitForSecondsRealtime(0.5f);
        ActiveWinners();
    }
    private void ActiveMovementRaffle()
    {
        awaitNextRaffle = true;
        StartSpin();
        startSpin = true;
        hasStop = false;
    }
    public void SetPopulateSpinInfos(string _value, string _edition)
    {
        uiLuckySpinController.PopulateSpinInfos(_value, _edition);
    }
    public void SetRoundIDSpin(int _roundNumber)
    {
        uiLuckySpinController.SetRoundIDLuckySpin(_roundNumber);
        GameManager.instance.luckySpinScriptable.currentSpinID = _roundNumber;
    }
    //public void ShowTicketSpin(bool canShowTicket)
    //{
    //    if (canShowTicket)
    //    {
    //        Invoke("CallTicketLuckySpin", 0.1f);
    //    }
    //    else
    //    {
    //        awaitNextRaffle = false;

    //        ResetResult();
    //    }
    //}
    private void StopMovement()
    {
        hasStop = true;
        StartCoroutine(StopRoll());
    }
    private void ActiveWinners()
    {
        if (!stopSlots)
        {
            foreach (var item in spins)
            {
                if (item.stopped)
                    index++;
                if (index >= spins.Count)
                {
                    confets.SetActive(true);
                    stopSlots = true;
                }
            }
        }
    }
}
