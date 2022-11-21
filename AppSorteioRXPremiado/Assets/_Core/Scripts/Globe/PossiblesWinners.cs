using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class PossiblesWinners : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInfo;

    public AnimationHeart animHeart;
    public GameObject ticketIDInfo;

    public Image imgHeart;

    public bool newCallHeart = true;


    public void Start()
    {
        transform.DOLocalMoveY(-1000, 0.1f);
        ActiveMovement();
    }
    public void SetTicketForOneBallInfo(string _info)
    {
        textInfo.text = _info;
        
    }

    public void ActiveMovement()
    {
        transform.DOLocalMoveY(-540, 1f).SetDelay(2f);
      
        PlayAnimationHeart(false);

    }
    public void PlayAnimationHeart(bool isRed)
    {
        StopAllCoroutines();
        if (isRed)
        {
            StartCoroutine(PlayAnimation(animHeart.heartRed));
        }
        else
        {
            StartCoroutine(PlayAnimation(animHeart.heartBlue));
        }
    }
    private IEnumerator PlayAnimation(List<Sprite> currentAnim)
    {
        while (animHeart.indexAnim <= currentAnim.Count - 1)
        {
            imgHeart.sprite = currentAnim[animHeart.indexAnim];
            animHeart.indexAnim++;
            yield return new WaitForSeconds(animHeart.interval);
        }
        animHeart.indexAnim = 0;
        StartCoroutine(PlayAnimation(currentAnim));

    }
    [System.Serializable]
    public class AnimationHeart
    {
        public float interval = 0.05f;
        public int indexAnim = 0;
        public List<Sprite> heartBlue;
        public List<Sprite> heartRed;
    }
}
