using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[Serializable]
public class TMP_Observe : MonoBehaviour
{
    protected TextMeshProUGUI TMPptr = null;
    protected object[] datas;

    // inspector fields
    [Tooltip("Gameobject.Component.Field")]
    [SerializeField]
    string[] URI;
    [Tooltip("Gold: {0:C2}")]
    [SerializeField]
    string format;

    // Start is called before the first frame update
    void Start()
    {
        TMPptr = GetComponent<TextMeshProUGUI>();
        datas = new object[URI.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < URI.Length; i++)
        {
            datas[i] = GetData(URI[i]);
        }
        Debug.Log("format = " + format);
        Debug.Log("datas = " + datas.Select(x => x.ToString()).ToArray());
        string text = string.Format(format, datas.Select(x => x.ToString()).ToArray());
        Debug.Log("text = " + text);
        TMPptr.SetText(text);
    }

    private object GetData(string uri)
    {
        string[] subs = uri.Split('.');
        Component comp = GameObject.Find(subs[0]).GetComponent(subs[1]);
        if (subs[2].Contains('['))
        {
            object[] idxs = subs[2]
                .Substring(subs[2].IndexOf('['), subs[2].LastIndexOf(']') - subs[2].IndexOf('['))
                .Split(',')
                .Select(x => (object)int.Parse(x)).ToArray();
            return comp.GetType().GetProperty(subs[2]).GetValue(comp, idxs);
        }
        else
        {
            return comp.GetType().GetProperty(subs[2]).GetValue(comp);
        }
    }
}
