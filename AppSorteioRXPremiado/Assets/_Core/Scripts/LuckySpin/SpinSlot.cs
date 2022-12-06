using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class SpinSlot : MonoBehaviour
{
    public List<float> positions;
    public List<NumberSlot> numberSlots;
    public List<NumberSlot> numberSlotsFinal;
    public int indexNumber = 0;
    public const float time = 0.01f;
    public bool isStop = false;
    public bool stopped = false;
    public Vector3 posCentral = new Vector3(-85.5f, -18.8f, 90f);

    [SerializeField] private Image bgTamp;
    [SerializeField] private float tampDelay = 2f;
    [SerializeField] private float spinSpeed = 40f;

    private void Start()
    {
        StartPositions();
    }
    public void ShowNumber()
    {
        bgTamp.transform.DOLocalMoveY(1000, 0.5f).SetDelay(tampDelay);
    }
    public void HideNumber()
    {
        bgTamp.transform.DOLocalMoveY(0, 0.2f);
    }
    private void StartPositions()
    {
        positions.Clear();
        foreach (var item in numberSlots)
        {
            positions.Add(item.transform.localPosition.y);
        }
    }
    public void SetSpeedNumber(float speed)
    {
        foreach (var item in numberSlots)
        {
            item.speed = speed;
        }
    }
    public void ReloadPositions()
    {
        for (int i = 0; i < numberSlots.Count; i++)
        {
            numberSlots[i].transform.localPosition = new Vector3(numberSlots[i].transform.localPosition.x, positions[i], numberSlots[i].transform.localPosition.z);
        }
        HideNumber();
    }
    public int GetIndexNumber()
    {
        return indexNumber;
    }
    public void SetIndexNumber()
    {
        indexNumber++;
        if (indexNumber > 10)
            indexNumber = 0;
    }
    public void MovementSpin()
    {
       
        for (int i = 0; i < numberSlots.Count; i++)
        {
            numberSlots[i].speed = spinSpeed;
            numberSlots[i].isActive = true;
        }
    }

    public void ShowNumberNow()
    {
        MovementSpin();
        ShowNumber();
        ActiveStopSlot();
    }
    public void ActiveStopSlot()
    {
        int newIndex = indexNumber;
        for (int i = 0; i < numberSlots.Count; i++)
        {
            if (newIndex > 8)
            {
                numberSlotsFinal.Add(numberSlots[newIndex]);
                newIndex = 0;
            }
            else
            {
                numberSlotsFinal.Add(numberSlots[newIndex]);
                newIndex++;
            }
        }

        for (int i = 0; i < numberSlotsFinal.Count; i++)
        {
            numberSlotsFinal[i].isActive = false;
            numberSlotsFinal[i].NewPosition(positions[i]);
        }
        stopped = true;
    }

}
