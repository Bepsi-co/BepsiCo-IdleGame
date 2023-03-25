using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;


public class UpgradeMenu : MonoBehaviour
{
    // pointers
    private Camera mainCam = null;

    // mouse click debounce
    private bool debounceIsMouseDown = false;
    private int debounceCounter = 0;
    private const int debounceFrames = 3;

    // menu object and component pointers
    GameObject UpgradeMenuRoot;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        UpgradeMenuRoot = new GameObject("UpgradeMenuRoot");
        UpgradeMenuRoot.AddComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if(debounceIsMouseDown)
        {
            debounceCounter++;
            if(debounceCounter >= debounceFrames)
            {
                debounceCounter = 0;
                debounceIsMouseDown = false;
            }
        } 
        else
        {
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                debounceIsMouseDown = true;

                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    UpgradeManagerBase umb = hit.collider.GetComponentInParent<UpgradeManagerBase>();
                    if (umb != null)
                    {

                    }
                }
            }
        }
    }

    private GameObject createUpgradeCard(Transform parent, int i)
    {
        GameObject root = new GameObject("UpgradeCard" + i);
        root.transform.parent = parent;
        root.AddComponent<TextMesh>();

        return root;
    }
}
