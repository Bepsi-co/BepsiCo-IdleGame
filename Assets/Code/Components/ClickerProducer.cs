using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickerUpgradeManager))]
public class ClickerProducer : ProducerBase
{
    public override void Start(){
        base.Start();
        BaseProduction = 100;
    }
    
    //When clicked, generate income
    void OnMouseDown(){
        Coreptr.Bank += BaseProduction;
    }

    //Need this blank to avoid running the FixedUpdate base in ProducerBase
    public override void FixedUpdate(){
        
    }
}
