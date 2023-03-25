using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeManagerBase))]
//[RequireComponent(typeof(ProducerVisual))]
public abstract class ProducerBase : MonoBehaviour
{
    public double BaseProduction { get => _BaseProduction; set => _BaseProduction = value; }
    protected double _BaseProduction;

    protected double ProductionLastTick { get => _ProductionLastTick; private set => _ProductionLastTick = value; }
    protected double _ProductionLastTick;

    protected Core Coreptr = null; 
    protected UpgradeManagerBase UMptr = null;
    //protected ProducerVisual PVptr = null;

    protected virtual double Tick()
    {
        double ProductionThisTick = 0;

        // Production from Upgrades
        if (UMptr != null)
        {
            
            ProductionThisTick += UMptr.Tick();
        }

        // Base Production
        ProductionThisTick = BaseProduction;

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
        Coreptr.Bank += Tick();
    }
}
