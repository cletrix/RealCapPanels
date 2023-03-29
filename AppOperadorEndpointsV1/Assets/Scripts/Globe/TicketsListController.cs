using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketsListController : MonoBehaviour
{
    public List<BtTicketList> btTickets;
    [SerializeField] private BtTicketList btTicket;
    [Space]
    [SerializeField] private GameObject content;

    [SerializeField] private List<string> savedInfos;
    public void PopulateListTickets(List<string> _infos, bool _hasWinner)
    {

        for (int i = 0; i < _infos.Count; i++)
        {
            BtTicketList inst = Instantiate(btTicket, transform.position, Quaternion.identity);
            inst.transform.SetParent(content.transform);
            inst.transform.localScale = Vector3.one;
            inst.PopulateInfos(_infos[i]);
            inst.SetIndex(i);

            if (_hasWinner)
            {
                inst.SetInteractableButton(true);
            }
            else
            {
                inst.SetInteractableButton(false);
            }

            btTickets.Add(inst);
            //if (!savedInfos.Contains(_infos[i]))
            //    savedInfos.Add(_infos[i]);
        }


    }

    public void CheckWinnerButtonState(List<bool> _ticketsShown, int index)
    {
        for (int i = 0; i < _ticketsShown.Count; i++)
        {
            btTickets[i].SetIsFinished(_ticketsShown[i]);
        }
        btTickets[index].SelectWinner();
    }
    public void SetInteractableBtTicketsList(bool _isActive)
    {
        for (int i = 0; i < btTickets.Count; i++)
        {
            if (GameManager.instance.isBackup)
            {
                btTickets[i].SetInteractableButton(false);
            }
            else
            {
                btTickets[i].SetInteractableButton(_isActive);
                btTickets[i].SetNormalColor();
            }
        }
    }
    public void ResetGrid()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject, 0.01f);
        }
        btTickets.Clear();
    }
}
