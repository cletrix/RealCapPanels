using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resoucers
{
    #region PanelBallsPayload

    [Serializable]
    public class PanelData 
    {
        public int maxBallsGrid = 0;
        public List<int> Balls;
        public int indexBalls = 0;
        public int winnersCount = 0;
        public void ResetVariables()
        {
            Balls.Clear();
            indexBalls = 0;
        }
    }
    #endregion
}
