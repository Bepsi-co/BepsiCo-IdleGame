using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeManagerBase))]
//[RequireComponent(typeof(ProducerVisual))]
public abstract class ProducerBase : MonoBehaviour
{
    #region Public Properties
    public double BaseProduction { get => _BaseProduction; set => _BaseProduction = value; }
    protected double _BaseProduction;

    public double ProductionLastTick { get => _ProductionLastTick; set => _ProductionLastTick = value; }
    protected double _ProductionLastTick;

    public long Level { get => _Level; protected set => _Level = value; }
    protected long _Level;
    public long LevelCost { get => _LevelCost; protected set => _LevelCost = value; }
    protected long _LevelCost;
    #endregion

    // pointers
    protected Core Coreptr = null;
    public UpgradeManagerBase UMptr = null;
    //protected ProducerVisual PVptr = null;


    // configuration
    #region constants
    public const string Name = "";
    public const double PurchasePrice = 100;
    #endregion


    // Start is called before the first frame update
    public virtual void Start()
    {
        Coreptr = GameObject.Find("Core").GetComponent<Core>();
        UMptr = GetComponentInParent<UpgradeManagerBase>();
        //PVptr = GetComponentInParent<ProducerVisual>();
        ProductionLastTick = 0;
        Level = 0;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        //Divide per tick value by 50 to convert to a per second value.
        Coreptr.Bank += Tick() / 50f;
    }
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

    // returns true on success
    public virtual bool LevelUp()
    {
        if(Coreptr != null && Coreptr.Bank > LevelCost)
        {
            Level++;
            LevelCost *= 2;
            return true;
        }
        else
        {
            return false;
        }
    }
}
