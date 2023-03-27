using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobeSettings", menuName = "Settings/GlobeSettings")]
public class GlobeScriptable : ScriptableObject
{
    public List<infoGlobe> infosGlobe;
    public int currentIndex = 0;

    [System.Serializable]
    public class infoGlobe
    {
        public string sorteioDescricao;
        public float sorteioValor;
        public int sorteioOrdem = 1;
    }

    public void SetGlobeOrder(int _order)
    {
        if (currentIndex < infosGlobe.Count)
            infosGlobe[currentIndex].sorteioOrdem = _order;
    }
    public int GetGlobeOrder()
    {
        if (infosGlobe.Count > 0)
        {
            if (currentIndex < infosGlobe.Count)
                return infosGlobe[currentIndex].sorteioOrdem;
            else
                return infosGlobe[currentIndex - 1].sorteioOrdem;
        }
        else
            return 999;
    }

    public void SetGlobeDesc(string _desc)
    {
        if (currentIndex < infosGlobe.Count)
            infosGlobe[currentIndex].sorteioDescricao = _desc;
    }
    public string GetGlobeDescription()
    {
        if (infosGlobe.Count > 0)
        {
            if (currentIndex < infosGlobe.Count)
            {
                return infosGlobe[currentIndex].sorteioDescricao;
            }
            else
                return infosGlobe[currentIndex - 1].sorteioDescricao;
        }
        else
            return "Null";
    }

    public void SetGlobeValue(float _value)
    {
        if (currentIndex < infosGlobe.Count)
            infosGlobe[currentIndex].sorteioValor = _value;
    }
    public float GetGlobeValue()
    {
        if (infosGlobe.Count > 0)
        {
            if (currentIndex < infosGlobe.Count)
                return infosGlobe[currentIndex].sorteioValor;
            else
                return infosGlobe[currentIndex - 1].sorteioValor;
        }
        else
            return 0f;
    }
}
