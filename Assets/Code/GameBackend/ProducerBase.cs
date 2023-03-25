using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeManagerBase))]
//[RequireComponent(typeof(ProducerVisual))]
public abstract class ProducerBase : MonoBehaviour
{
    public double BaseProduction { get => _BaseProduction; set => _BaseProduction = value; }
    protected double _BaseProduction;

    protected double ProductionLastTick { get => _ProductionLastTick; set => _ProductionLastTick = value; }
    protected double _ProductionLastTick;

    protected Core Coreptr = null; 
    protected UpgradeManagerBase UMptr = null;
    //protected ProducerVisual PVptr = null;


    // configuration
    public const string Name = "";
    public const double PurchasePrice = 100;

    protected virtual double Tick()
    {
        double ProductionThisTick = 0;

        // Production from Upgrades
        if (UMptr != null)
        {
            ProductionThisTick += UMptr.Tick();
        }

        // Base Production
        ProductionThisTick += BaseProduction;

        // return
        ProductionLastTick = ProductionThisTick;
        return ProductionThisTick;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Coreptr = GameObject.Find("Core").GetComponent<Core>();
        UMptr = GetComponentInParent<UpgradeManagerBase>();
        //PVptr = GetComponentInParent<ProducerVisual>();
        ProductionLastTick = 0;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        //Divide per tick value by 50 to convert to a per second value.
        Coreptr.Bank += Tick()/50;
    }
}
