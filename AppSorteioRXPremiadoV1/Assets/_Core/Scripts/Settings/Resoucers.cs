using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Resoucers 
{
    #region GlobeData

    [Serializable]
    public class GlobeData 
    {
        public string editionName;
        public string editionNumber;
        public int order;
        public string date;
        public string description;
        public float value;
        [Space]
        public int Winners;
        public float prizeValue;
        public int ballRaffledCount;
        public int possiblesWinnersCount;
        [Space]
        public List<string> numberBalls;
        public int indexBalls = 0;
        public void ResetRaffle()
        {
            Winners = 0;
            prizeValue = 0;
            ballRaffledCount = 0;
            possiblesWinnersCount = 0;
            numberBalls.Clear();
            indexBalls = 0;
        }
    }
    #endregion

    #region SpinData
    
    [Serializable]
    public class SpinData
    {
        public int currentSpinID;
        public string currentResult;
        public string prizeDescription;
        public float prizeValue;
        public string editionID;
        public List<string> allSpinsResult;


        public void Reset()
        {
            currentSpinID = 1;
            currentResult = string.Empty;
            allSpinsResult.Clear();
        }
    }
    #endregion
}
