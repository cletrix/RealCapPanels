using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GlobeInfos : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textInfo;
    [SerializeField] TextMeshProUGUI textCountInfo;

    public void SetInfoLastBall(string _count, string _info)
    {
        textCountInfo.text = $"{_count}° Sorteio";
        textInfo.text = _info;
    }
}
