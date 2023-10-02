using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CellGridTicket : MonoBehaviour
{
    [SerializeField] private int numberBall;
    [SerializeField] private TextMeshProUGUI txtBall;
    public void SetNumberInText(string _numberBall)
    {
        txtBall.text = _numberBall;
        numberBall = System.Convert.ToInt32(_numberBall);
    }
}
