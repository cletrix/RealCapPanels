using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    #region Singleton
    private static SpriteManager _instance;

    public static SpriteManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
    #endregion
    public List<Sprite> ballsNormal;
    public List<Sprite> balls50;

    public Sprite GetBallNormal(int index)
    {
        return ballsNormal[index - 1];
    }
    public Sprite GetBalls50(int index)
    {
        return balls50[index - 1];
    }
}
