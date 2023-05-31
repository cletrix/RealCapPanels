using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class BigBall : MonoBehaviour
{
    public TextMeshProUGUI textNumber;

    public Ease easeType;
    public Image imageBall;
    public Sprite bgBall;
    public Sprite bgBallLogo;
    public Animator animBall;
    public string numberBall;
    public GameObject trackBall;

    [SerializeField] private Color initialColor;
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;
    [SerializeField] private Image bgLineBall;

    public void Start()
    {
        SetBgBallWithLogo();
    }
    public void SetInfoInBigBall(string _numberBall, bool isAnim = true)
    {
        if (isAnim)
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.DOScale(1.5f, 0.7f).SetEase(easeType);
        }
        imageBall.sprite = bgBall;
        numberBall = _numberBall;
        textNumber.text = _numberBall;
        imageBall.enabled = true;
        ChangeColorBgLine();
  
    }

    public void SetBgBallWithLogo()
    {
        imageBall.sprite = bgBallLogo;
        textNumber.text = string.Empty;
        imageBall.enabled = true;
        bgLineBall.color = initialColor;
    }

    private void ChangeColorBgLine()
    {
        int number = int.Parse(numberBall);
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

    public void SetBallWinner()
    {
        imageBall.color = Color.yellow;
    }

}
