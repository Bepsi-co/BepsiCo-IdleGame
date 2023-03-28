using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KyleUpgradeManager))]
public class KyleProducer : ProducerBase
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        BaseProduction = 100;
    }
}
