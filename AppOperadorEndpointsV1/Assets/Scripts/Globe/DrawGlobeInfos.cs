using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawGlobeInfos : MonoBehaviour
{
    [Header("STATUS INFOS")]
    [SerializeField] private TextMeshProUGUI txtInfosTitle;
    [SerializeField] private TextMeshProUGUI txtForOneBall;
    [SerializeField] private TextMeshProUGUI txtWinners;
    [SerializeField] private TextMeshProUGUI txtForTwoBalls;
    [SerializeField] public TicketsListController ticketsList;
    public void CheckWinners()
    {
        ticketsList.ResetGrid();

        if (GameManager.instance.globeDrawData.ganhadorContemplado.Length > 0)
        {
            txtInfosTitle.text = "GANHADORES";
            GameManager.instance.isWinner = true;
            ticketsList.PopulateListTickets(GameManager.instance.GetWinners(), GameManager.instance.isWinner);
            ticketsList.btTickets[0].SelectWinner();
        }
        else
        {
            if (GameManager.instance.GetForOneBalls().Count > 0)
            {
                txtInfosTitle.text = "POR UMA BOLA";
            }
            else
            {
                txtInfosTitle.text = "INFORMAÇÕES";
            }
            GameManager.instance.isWinner = false;
            ticketsList.PopulateListTickets(GameManager.instance.GetForOneBalls(), GameManager.instance.isWinner);
        }
        txtForOneBall.text = GameManager.instance.GetForOneBalls().Count.ToString();
        txtForTwoBalls.text = GameManager.instance.GetForTwoBalls();
        txtWinners.text = GameManager.instance.GetWinners().Count.ToString();
        ticketsList.SetInteractableBtTicketsList(GameManager.instance.isWinner);

        GameManager.instance.WriteInfosGlobe();
    }
}
