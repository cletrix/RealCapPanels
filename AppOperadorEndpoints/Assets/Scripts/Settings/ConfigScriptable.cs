using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Settings/Config")]

public class ConfigScriptable : ScriptableObject
{
    public int currentSceneID;
    public List<string> globeRaffle;
    public int globeOrder;
    public int spinOrder;
    public bool hasRaffleVisibility;
    public List<string> spinsNumberTickets;

    public void UpdateConfig(int sceneId, List<string> _globeRaffle, int globe, bool raffleVisibility)
    {
        if (!GameManager.instance.isbackup)
        {
            currentSceneID = sceneId;
            globeRaffle = _globeRaffle;
            globeOrder = globe;
            hasRaffleVisibility = !raffleVisibility;

            RestNetworkManager.instance.CallWriteMemory();
        }
    }
    public void AddSpinNumber(string _numberticket, int _spinOrder)
    {
        if (!spinsNumberTickets.Contains(_numberticket))
        {
            spinsNumberTickets.Add(_numberticket);
            spinOrder = _spinOrder;
            RestNetworkManager.instance.CallWriteMemory();
        }
    }
    public void PopulateConfig()
    {
        GameManager.instance.isbackup = false;

        GameManager.instance.sceneId = currentSceneID;
        GameManager.instance.globeRaffleScriptable.bolasSorteadas = globeRaffle;
        GameManager.instance.globeScriptable.sorteioOrdem = globeOrder;
        SpinController spin = FindObjectOfType<SpinController>();
        if (spin != null)
        {
            spin.SetIndexSpin(spinOrder);
            spin.PopulateSpinsFields(spinsNumberTickets);
        }
        // RestNetworkManager.instance.CallReadMemory();

    }

    //public void AddSaveSpins(string _numberSpin, string _numberticket, string _numberPrize)
    //{
    //    SaveSpinsRaffle save = new SaveSpinsRaffle();
    //    save.numberSpin = _numberSpin;
    //    save.numberTicket = _numberticket;
    //    save.numberPrize = _numberPrize;

    //    //saveSpinsRaffles.Add(save);
    //}
    //[System.Serializable]
    //public class SaveSpinsRaffle
    //{
    //    public string numberSpin;
    //    public string numberTicket;
    //    public string numberPrize;
    //}
}
