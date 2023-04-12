using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleUpgradeManager : UpgradeManagerBase
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
    public override void Start()
    {
        base.Start();

        // set upgrade array
        #region Upgrades
        /* Upgrades */
        // hopefully replace this with some kind of loader
        #region Path1
        // Path 1
        Upgrade upgradeP1U1 = new Upgrade()
        {
            Cost = 10,
            Description = "Increase BaseIncome by 10",
            OnPurchase = (p, um) => { BonusProduction += 10;  p.BaseProduction += 10; },
            OnTick = null
        };
        Upgrade upgradeP1U2 = upgradeP1U1;
        Upgrade upgradeP1U3 = upgradeP1U1;
        Upgrade upgradeP1U4 = upgradeP1U1; 
        Upgrade upgradeP1U5 = upgradeP1U1;
        #endregion
        #region Path2
        // Path 2
        Upgrade upgradeP2U1 = new Upgrade()
        {
            Cost = 100,
            Description = "1% chance per second to change career granting 1% increased income",
            OnPurchase = (p, um) => { BonusProductionMultChance = 1; BonusProductionMultPerProc = 0.01; },
            OnTick = (p, um) =>
            {
                if(tickCounter >= 59)
                {
                    tickCounter = 0;
                    int rng = Random.Range(0, 100);
                    if(rng < BonusProductionMultChance)
                    {
                        BonusProductionMultProcs++;
                    }
                }
                else
                {
                    tickCounter++;
                }
                return (p.BaseProduction * BonusProductionMultProcs * BonusProductionMultPerProc);
            }
        };
        Upgrade upgradeP2U2 = new Upgrade()
        {
            Cost = 500,
            Description = "+1% chance per second",
            OnPurchase = (p, um) => { BonusProductionMultChance += 1; },
            OnTick = null
        };
        Upgrade upgradeP2U3 = new Upgrade()
        {
            Cost = 5000,
            Description = "+2% chance per second",
            OnPurchase = (p, um) => { BonusProductionMultChance += 2; },
            OnTick = null
        };
        Upgrade upgradeP2U4 = new Upgrade()
        {
            Cost = 15000,
            Description = "+1% incease income per proc",
            OnPurchase = (p, um) => { BonusProductionMultPerProc += 1; },
            OnTick = null
        };
        Upgrade upgradeP2U5 = new Upgrade()
        {
            Cost = 500000,
            Description = "+1% chance per second; +1% incease income per proc",
            OnPurchase = (p, um) => { BonusProductionMultChance += 1; BonusProductionMultPerProc += 1; },
            OnTick = null
        };
        #endregion
        #region Path3
        // Path 3
        Upgrade upgradeP3U1 = new Upgrade()
        {
            Cost = 100,
            Description = "Kyle opens a Brewery. No longer produces income, boosts other members income by 5%",
            OnPurchase = (p, um) => { p.BaseProduction = BonusProduction; ProductionBoost = 0.05; },
            OnTick = (p, um) =>
            {
                // TODO. adds a callback to ui for purchasing new producers
                double acc = 0;
                foreach(ProducerBase prod in otherProducers)
                {
                    acc += prod.BaseProduction * ProductionBoost;
                }
                return acc;
            }
        };
        Upgrade upgradeP3U2 = new Upgrade()
        {
            Cost = 1000,
            Description = "Income boost increased by 5%",
            OnPurchase = (p, um) => { ProductionBoost += 0.05; },
            OnTick = null
        };
        Upgrade upgradeP3U3 = upgradeP3U2;
        Upgrade upgradeP3U4 = upgradeP3U2;
        Upgrade upgradeP3U5 = upgradeP3U2;
        #endregion
        #endregion
        Upgrades = new Upgrade[,] {
            { upgradeP1U1, upgradeP1U2, upgradeP1U3, upgradeP1U4, upgradeP1U5 },
            { upgradeP2U1, upgradeP2U2, upgradeP2U3, upgradeP2U4, upgradeP2U5 },
            { upgradeP3U1, upgradeP3U2, upgradeP3U3, upgradeP3U4, upgradeP3U5 }
        };

        // default reset cost
        ResetCost = 1000;

    }
}
