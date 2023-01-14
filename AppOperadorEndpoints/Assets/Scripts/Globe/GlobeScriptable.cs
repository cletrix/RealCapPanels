using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobeSettings", menuName = "Settings/GlobeSettings")]
public class GlobeScriptable : ScriptableObject
{
    public List<infoGlobe> infosGlobe;
    public int currentIndex=0;

    [System.Serializable]
    public class infoGlobe
    {
        public string sorteioDescricao;
        public float sorteioValor;
        public int sorteioOrdem = 1;
    }

    public int GetGlobeOrder()
    {
        return infosGlobe[currentIndex].sorteioOrdem;
    }

    public void SetGlobeOrder(int _order)
    {
        infosGlobe[currentIndex].sorteioOrdem=_order;
    }

    public string GetGlobeDescription()
    {
        return infosGlobe[currentIndex].sorteioDescricao;
    }

    public float GetGlobeValue()
    {
        return infosGlobe[currentIndex].sorteioValor;
    }
}
