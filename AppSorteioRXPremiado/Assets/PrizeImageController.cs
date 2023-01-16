using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrizeImageController : MonoBehaviour
{
    [SerializeField] private List<Sprite> prizeImages;
    [SerializeField] private Image prizeImage;
    void Start()
    {
        prizeImage = GetComponent<Image>();
    }
    public void SetPrizeImage(int index)
    {
        print(index);
        prizeImage.sprite = prizeImages[index-1];
    }
}
