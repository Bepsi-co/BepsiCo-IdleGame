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

    protected override double Tick(){
        double ProductionThisTick = 0;

        // Production from Upgrades
        if (UMptr != null)
            {
                
                ProductionThisTick += UMptr.Tick();
            }
        //See if the user is clicking the click object (zone)
        if (Input.GetMouseButtonDown(0))
         {
             RaycastHit raycastHit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out raycastHit, 100f))
             {
                 if (raycastHit.transform != null)
                 {
                    // Base Production
                    ProductionThisTick += BaseProduction;
                    
                 }
             }
         }
         return ProductionThisTick;
    }
}
