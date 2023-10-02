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
    private bool enableConfet = true;
    [SerializeField] private bool awaitNextRaffle = false;
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        hasStop = true;
        uiLuckySpinController = FindObjectOfType<UiLuckySpinController>();
        GameManager.instance.spinData.Reset();
        GameManager.instance.globeData.ResetRaffle();
        GameManager.instance.SetCamActiveInCanvas(Camera.main);
    }

    public void ActiveConfets()
    {
        if(enableConfet)
        {
            confets.SetActive(true);
            enableConfet = false;
        }
    }
    public void SetResult(string _raffleLuckyNumber)
    {
        enableConfet = true;
        GameManager.instance.spinData.currentResult = _raffleLuckyNumber;
        GameManager.instance.spinData.allSpinsResult.Add(GameManager.instance.spinData.currentResult);

        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].numberSlotsFinal.Clear();
            spins[i].indexNumber = System.Convert.ToInt16(GameManager.instance.spinData.currentResult[i].ToString());
            spins[i].ReloadPositions();
        }
        ActiveMovementRaffle();

    }

    public void CloseDoor()
    {
        foreach (var item in spins)
        {
            item.HideNumber(0.1f);
        }
    }
    public void ResetResult()
    {
        for (int i = 0; i < spins.Count; i++)
        {
            spins[i].ReloadPositions();
        }
    }

    private void StopMovement()
    {
        hasStop = true;
        StartCoroutine(StopRoll());
    }
    private void ActiveMovementRaffle()
    {
        awaitNextRaffle = true;
        StartSpin();
        startSpin = true;
        hasStop = false;
    }
    public void StartSpin()
    {
        StartCoroutine(StartMovement());
        speed = 80;
        stopSlots = false;
        index = 0;
    }
    private IEnumerator StartMovement()
    {
        for (int i = 0; i < spins.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            spins[i].ShowNumber();
            spins[i].MovementSpin();
            spins[i].stopped = false;
            if (i == 2)
            {
                AudioManager.instance.PlaySFX("Spin");
            }
        }
        yield return new WaitForSeconds(1f);
        StopMovement();
    }
    public IEnumerator StopRoll()
    {
        foreach (var item in spins)
        {
            yield return new WaitForSeconds(0.5f);
            item.ActiveStopSlot();
        }
        yield return new WaitForSeconds(1f);
        ActiveWinners();
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlaySFX("Clap");
        //AudioManager.instance.PlaySFX("Winner");
    }

    public void SetPopulateSpinInfos(float _value, string _edition, string _description)
    {
        GameManager.instance.spinData.prizeDescription = _description;
        GameManager.instance.spinData.prizeValue = _value;
        GameManager.instance.spinData.editionID = _edition;
        uiLuckySpinController.PopulateSpinInfos(_value, _description, _edition);
    }
    public void SetRoundIDSpin(int _roundNumber)
    {
        uiLuckySpinController.SetRoundIDLuckySpin(_roundNumber);
        GameManager.instance.spinData.currentSpinID = _roundNumber;
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
