using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technical", menuName = "Settings/Technical")]

public class technicalScriptable : ScriptableObject
{
    public int currentSceneID;
    public List<string> globeRaffle;
    public int globeOrder;
    public int spinOrder;
    public bool canHideRaffle;
    public List<string> spinsNumberTickets;

    public void UpdateConfig(int sceneId, List<string> _globeRaffle, int globe, bool raffleVisibility)
    {
        if (!GameManager.instance.isbackup)
        {
            currentSceneID = sceneId;
            globeRaffle = _globeRaffle;
            globeOrder = globe;
            canHideRaffle = !raffleVisibility;

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


        GameManager.instance.sceneId = currentSceneID;
        GameManager.instance.globeRaffleScriptable.bolasSorteadas = globeRaffle;
        GameManager.instance.globeScriptable.sorteioOrdem = globeOrder;
        SpinController spin = FindObjectOfType<SpinController>();
        if (spin != null)
        {
            spin.SetIndexSpin(spinOrder);
            spin.PopulateSpinsFields(spinsNumberTickets);
        }

    }
}
