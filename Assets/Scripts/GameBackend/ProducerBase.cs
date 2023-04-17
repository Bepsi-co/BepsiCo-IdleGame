
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeManagerBase))]
//[RequireComponent(typeof(ProducerVisual))]
public class ProducerBase : MonoBehaviour
{
    #region Public Properties
    public double Production { get => _Production; set => _Production = value; }
    protected double _Production;

    public double ProductionLastTick { get => _ProductionLastTick; set => _ProductionLastTick = value; }
    protected double _ProductionLastTick;

    public long Level { get => _Level; protected set => _Level = value; }
    protected long _Level;

    public double LevelPrice { get => _LevelPrice; protected set => _LevelPrice = value; }
    protected double _LevelPrice;
    #endregion

    // pointers
    protected Core Coreptr = null;
    [NonSerialized] public UpgradeManagerBase UMptr = null;

    // inspector
    [SerializeField]
    public ProducerConfigSO config;


    // Start is called before the first frame update
    public virtual void Awake()
    {
        // pointers
        Coreptr = GameObject.Find("Core").GetComponent<Core>();
        UMptr = GetComponentInParent<UpgradeManagerBase>();

        // data init
        ProductionLastTick = 0;
        Level = 0;

        // config
        Production = config.BaseProduction;
        LevelPrice = config.BaseLevelPrice;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        //Divide per tick value by 50 to convert to a per second value.
        Coreptr.Bank += Tick() / 50.0f;
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
        ProductionThisTick += Production;

        // return
        ProductionLastTick = ProductionThisTick;
        return ProductionThisTick;
    }

    // returns true on success
    public virtual bool LevelUp()
    {
        if(Coreptr != null && Coreptr.Bank > LevelPrice)
        {
            Level++;
            config.BaseLevelPrice *= 2;
            return true;
        }
        else
        {
            return false;
        }
    }
}
