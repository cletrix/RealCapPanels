using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class BallSlot : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Color selectedColor;
    public Color noSelectedColor;
    public Color finishedColor;
    public Color winnerColor;
    public TextMeshProUGUI textNumber;
    public int numberBall;
    public Image imageBall;
    public GameObject ballBorder;
    public Animator animBorder;
    public bool isActive = false;
    public bool isFinishRaffle = false;
    public bool hasRaffled = false;
    public Transform ballTransform;

    public Image bgLineBall;
    public Color firstColor;
    public Color secondColor;

    void Start()
    {
        imageBall.color = noSelectedColor;
        ballTransform = GetComponent<Transform>();
    }

    public void SetNumberBall(int _number)
    {
        SetEnableBall();
        numberBall = _number;
        textNumber.text = _number.ToString();

        ballBorder.SetActive(false);
        ChangeColorBgLine();
        textNumber.color = new Color(1, 1, 1, 0.4f);
        bgLineBall.color = new Color(bgLineBall.color.r, bgLineBall.color.g, bgLineBall.color.b, 0.5f);

    }
    public void SetSelectedBall()
    {
        ballBorder.SetActive(true);
        imageBall.enabled = true;
        imageBall.color = selectedColor;
        ChangeColorBgLine();

    }
    public void ResetBall()
    {
        canvasGroup.alpha = 1;
        hasRaffled = false;
        isFinishRaffle = false;
        isActive = false;
        bgLineBall.enabled = true;
        textNumber.enabled = true;
        SetNormalColor();
        ballBorder.SetActive(isActive);
    }
    public void SetEnableBall()
    {
        imageBall.enabled = true;
        canvasGroup.alpha = 1;

        ChangeColorBgLine();
    }
    public void SetDisableBall()
    {
        imageBall.enabled = true;
        imageBall.color = new Color(1, 1, 1, 0.3f);
        canvasGroup.DOFade(0, 1f);
    }
    public void SetEnableBallBorder()
    {
        ballTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        SetEnableBall();
        isActive = true;
        hasRaffled = true;
        if (UIManager.instance.panelScriptable.winnersCount > 0)
        {
            imageBall.color = winnerColor;
        }
        else
        {
            imageBall.color = selectedColor;
        }
        Invoke("ActiveBlinkAnimBorder", 0.1f);
    }
    private void ActiveBlinkAnimBorder()
    {
        ballBorder.SetActive(isActive);
        animBorder.SetBool("IsBlink", true);
        Invoke("DesactiveBlinkAnimBorder", 3f);
    }
    private void DesactiveBlinkAnimBorder()
    {
        CancelInvoke("ActiveBlinkAnimBorder");
        animBorder.SetBool("IsBlink", false);
        CancelInvoke("DesactiveBlinkAnimBorder");
        ballTransform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }

    private void ChangeColorBgLine()
    {
        int number = numberBall;
        if (number <= 30)
        {
            bgLineBall.color = firstColor;
            textNumber.color = firstColor;
        }
        else
        {
            bgLineBall.color = secondColor;
            textNumber.color = secondColor;
        }
    }


    public void SetNormalColor()
    {
        imageBall.enabled = true;
        imageBall.color = noSelectedColor;
        textNumber.color = new Color(1, 1, 1, 0.4f);
    }
    public void SetSelectedColor()
    {
        imageBall.enabled = true;
        imageBall.color = selectedColor;
        ChangeColorBgLine();

    }

    public void SetFinishedColor()
    {
        imageBall.enabled = true;
        imageBall.color = finishedColor;
        ChangeColorBgLine();
    }



}
