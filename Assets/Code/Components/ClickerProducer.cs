using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickerUpgradeManager))]
public class ClickerProducer : ProducerBase
{
    //When clicked, generate income
    void OnMouseDown(){
        BaseProduction = 100;
        Coreptr.Bank += BaseProduction;
    }

    //Need this blank to avoid running the FixedUpdate base in ProducerBase
    public override void FixedUpdate(){
        
    }
}
