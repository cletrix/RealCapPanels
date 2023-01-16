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
        if (currentIndex < infosGlobe.Count)
            return infosGlobe[currentIndex].sorteioOrdem;
        else
            return infosGlobe[currentIndex - 1].sorteioOrdem;
    }


    public string GetGlobeDescription()
    {
        if (currentIndex < infosGlobe.Count)
        {
            return infosGlobe[currentIndex].sorteioDescricao;
        }
        else
            return infosGlobe[currentIndex - 1].sorteioDescricao;
    }

    public float GetGlobeValue()
    {
        if (currentIndex < infosGlobe.Count)
            return infosGlobe[currentIndex].sorteioValor;
        else
            return infosGlobe[currentIndex - 1].sorteioValor;
    }
}
