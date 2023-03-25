using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickerUpgradeManager))]
public class ClickerProducer : ProducerBase
{
    bool clicked = false;
    public override void Start(){
        base.Start();
        BaseProduction = 100;
    }
    
    //When clicked
    void OnMouseDown(){
        clicked = true;
    }

    protected override double Tick(){
        double ProductionThisTick = 0;

        // Production from Upgrades
        if (UMptr != null)
        {
            ProductionThisTick += UMptr.Tick();
        }
        //If the object was clicked this tick, then generate the click income
        if (clicked){
            ProductionThisTick += BaseProduction;
            clicked = false;
        }

         return ProductionThisTick*50;
    }
}
