using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerUpgradeManager : UpgradeManagerBase
{
    #region UpgradeData
    // Upgrade Data
    // Path 1
    protected double BonusProduction = 0;

    // Path 2
    protected double BonusProductionMultPerProc = 0;
    protected int BonusProductionMultChance = 0;
    protected int BonusProductionMultProcs = 0;
    protected int tickCounter = 0;

    // Path 3
    protected double ProductionBoost = 0;
    protected List<ProducerBase> otherProducers;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }
}
