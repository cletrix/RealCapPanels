using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBallsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderBalls;

    [Header("GRID RAFFLED BALLS")]
    [SerializeField] private SelectBall[] ballsRaffled;
    [SerializeField] private SelectBall ballRaffledPrefab;
    [SerializeField] private GameObject panelGridBallsRaffled;
    private SelectBall lastBallRaffled;
    public List<int> indexBalls;

    [Header("GRID BALLS")]
    public List<SelectBall> balls;
    [SerializeField] private GameObject panelGridBalls;
    [SerializeField] private int indexBallSelected;

    [Header("CONFIRM BALL")]
    //[SerializeField] private Button btConfirm;
    [SerializeField] private GameObject panelConfirmBall;
    [SerializeField] private TextMeshProUGUI txtViewNumberBallConfirm;
    [Header("REVOKE BALL")]
    //[SerializeField] private Button btRevoke;
    [SerializeField] private GameObject panelRevokeBall;
    [SerializeField] private TextMeshProUGUI txtViewNumberBallRevoke;

    public void StartVariables()
    {
        balls = new List<SelectBall>();
        balls.AddRange(panelGridBalls.GetComponentsInChildren<SelectBall>());
        PopulateBalls();
    }

    private void PopulateBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            int number = i + 1;
            balls[i].InitializeVariables();
            balls[i].SetNumberInText(number.ToString());
        }
    }

    public void CheckStateBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].CheckState();
        }

        if (GameManager.instance.globeDrawData.ganhadorContemplado.Length > 0)
        {
            SetDisableAllNotRevoke();
        }
    }
    public void DisableHasRevokedAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetHasCanRevoked(false);
            balls[i].SetHasSelected(false);
            balls[i].SetHasRaffled(false);
        }
    }
    public void DisableAllConfirmed()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetHasRaffled(false);
        }
    }
    private void SetDisableAllNotRevoke()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (!balls[i].GetHasCanRevoked())
            {
                balls[i].DisableInteractable();
            }
        }
    }
    public void SetDisableAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].DisableInteractable();
        }
    }

    public void SetEnableAll()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].EnableInteractable();
        }
        CheckStateBalls();
    }
    public void OpenPanelBall(int index)
    {
        indexBallSelected = index - 1;

        if (balls[indexBallSelected].GetHasRaffled() == true)
        {
            panelRevokeBall.SetActive(true);
        }
        else
        {
            balls[indexBallSelected].SetHasSelected(true);
            if (!GameManager.instance.isWinner)
            {
                panelConfirmBall.SetActive(true);
            }
        }
        SetTxtViewBall(balls[indexBallSelected].GetNumberBall());
        CheckStateBalls();
    }
    public void ConfirmBallSelected()
    {
        GameManager.instance.SetNewBall(balls[indexBallSelected].GetNumberBall());
        GlobeManager.hasRevoked = false;

    }
    public void RevokeBallSelected()
    {
        indexBalls.Remove(indexBalls[indexBalls.Count - 1]);
        GameManager.instance.SetRemoveBall(balls[indexBallSelected].GetNumberBall());
        lastBallRaffled.SetHasSelected(true);
        lastBallRaffled.SetHasRaffled(false);
        lastBallRaffled.SetHasCanRevoked(false);

        GlobeManager.hasRevoked = true;

    }
    public void ClosePanelConfirmBall()
    {
        panelConfirmBall.SetActive(false);
        balls[indexBallSelected].SetHasSelected(false);
        CheckStateBalls();
    }
    public void ClosePanelRevokeBall()
    {
        panelRevokeBall.SetActive(false);
        balls[indexBallSelected].SetHasSelected(false);
        CheckStateBalls();
    }
    public void SetTxtViewBall(string _number)
    {
        if (panelConfirmBall.activeSelf)
        {
            txtViewNumberBallConfirm.text = _number;
        }
        if (panelRevokeBall.activeSelf)
        {
            txtViewNumberBallRevoke.text = _number;
        }
    }
    private void SpawnBallRaffled(List<string> _balls)
    {
        foreach (var item in ballsRaffled)
        {
            Destroy(item.gameObject);
        }
        ballsRaffled = new SelectBall[_balls.Count];
        for (int i = 0; i < _balls.Count; i++)
        {
            SelectBall inst = Instantiate(ballRaffledPrefab, transform.position, Quaternion.identity);
            inst.transform.SetParent(panelGridBallsRaffled.transform);
            inst.SetNumberInText(_balls[i]);
            inst.transform.localScale = new Vector3(1, 1, 1);
            if (i == _balls.Count - 1)
            {
                inst.SetCanRevokebleColor();
            }
            else
            {
                inst.SetConfirmedColor();
            }
            ballsRaffled[i] = inst;
        }
        orderBalls.text = $"Dezenas Sorteadas: {ballsRaffled.Length}";
    }
    public void ValidateBall()
    {
        if (indexBalls.Count > 0)
        {
            lastBallRaffled = balls[indexBalls[indexBalls.Count - 1]];
            lastBallRaffled.SetHasCanRevoked(true);
        }

        CheckStateBalls();

        SpawnBallRaffled(GameManager.instance.GetBallsRaffled());
    }
}

