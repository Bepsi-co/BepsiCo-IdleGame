using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KyleUpgradeManager))]
public class KyleProducer : ProducerBase
{
    // configuration
    public new const string Name = "Kyle";
    public new const double PurchasePrice = 100;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        BaseProduction = 100;
        LevelCost = 100;
    }
}
