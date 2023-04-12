using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMP_Observe : MonoBehaviour
{
    const string formatterPattern = @"({[^{}]+})";

    protected TextMeshProUGUI TMPptr = null;
    protected object[] datas;
    protected PropertyInfo[] props;
    protected int[][] isArray; // null if no index, otherwise int array of indicies
    protected Component[] comps;
    protected string[] formatters; // the formatters for each element (the stuff between {})

    // inspector fields
    [SerializeField] [Tooltip("Gameobject.Component.Field")]
    public string[] URIs;
    [SerializeField] [Tooltip("Score: {0:C2}")]
    public string format;

    // Start is called before the first frame update
    void Start()
    {
        // construct arrays
        TMPptr = GetComponent<TextMeshProUGUI>();
        datas = new object[URIs.Length];
        props = new PropertyInfo[URIs.Length];
        isArray = new int[URIs.Length][];
        comps = new Component[URIs.Length];
        formatters = new string[URIs.Length];

        for (int i = 0; i < URIs.Length; i++)
        {
            // try to find prop. fills in props[i], isArray[i], comps[i]
            findProp(i);
        }

        // gather formatters
        formatters = 
            Regex.Matches(format, formatterPattern)
            .OfType<Match>()
            .Select(m => m.Groups[1].Value)
            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i < props.Length; i++)
        {
            if (comps[i] is not null)
            {
                try
                {
                    if (isArray[i] is not null)
                        datas[i] = props[i].GetValue(comps[i], isArray[i].Cast<object>().ToArray());
                    else
                        datas[i] = props[i].GetValue(comps[i], null);
                }
                catch (Exception)
                { // catch in the event that getting value by reflection fails (most likely component got disposed of)
                    comps[i] = null; // mark null, future updates will try to find it
                }
            } 
            else
            {
                findProp(i);
            }
        }

        if (URIs.Length > 0)
        {
            // format each piece of data individually then run full string formatter
            // this has to be done or it will ignore type specific formatters (ie currency for doubles)
            TMPptr.SetText(string.Format(format, formatters.Zip(datas, (f, d) => string.Format(f, d)).ToArray()));
        }
    }

    // uses reflection to find the property described by the uri
    // uri has form GameObject.Component.Field[index1][index2][...]
    private void findProp(int uriIdx)
    {
        string[] subs = URIs[uriIdx].Split(new char[] { '.', '[', ']' });
        comps[uriIdx] = GameObject.Find(subs[0])?.GetComponent(subs[1]);
        if (comps[uriIdx] is not null) // check if we found the gameobject/component
        {
            props[uriIdx] = comps[uriIdx].GetType().GetProperty(subs[2]);
            if (subs.Length > 3) // check if field has indexers
                isArray[uriIdx] = subs.Skip(3).Select(x => int.Parse(x)).ToArray();
            else
                isArray[uriIdx] = null;
        }
    }
}
