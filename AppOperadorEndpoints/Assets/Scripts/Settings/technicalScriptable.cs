using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technical", menuName = "Settings/Technical")]

public class technicalScriptable : ScriptableObject
{
    public int currentSceneID;
    public bool isVisibleRaffle;
    public int forTwoBalls;
    public List<GlobeRaffleScriptable.porUmaBola> forOneBalls;

    public void UpdateConfig(int sceneId, bool raffleVisibility, int _forTwoBalls, List<GlobeRaffleScriptable.porUmaBola> _forOneBall)
    {
        if (!GameManager.instance.isbackup)
        {
            currentSceneID = sceneId;
            isVisibleRaffle = raffleVisibility;
            forTwoBalls = _forTwoBalls;
            forOneBalls = _forOneBall;

            RestNetworkManager.instance.CallWriteMemory();
        }

    }
    public void PopulateConfig()
    {
        GameManager.instance.sceneId = currentSceneID;
        GameManager.instance.isVisibleRaffle = isVisibleRaffle;
    }

}

