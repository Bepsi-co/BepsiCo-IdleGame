using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine.UI;
using Assets.Code.Helpers;

[RequireComponent(typeof(Image))]
public class ProducerMenu : MonoBehaviour
{
    // pointers
    private Camera mainCam = null;

    // mouse click debounce
    private bool debounceIsMouseDown = false;
    private int debounceCounter = 0;
    private const int debounceFrames = 3;

    // menu object and component pointers
    GameObject menuRoot;
    Image backgroundImage;
    List<ProducerMenuCard> producerCards;

    // inspector fields
    public List<Type> _producers = new List<Type>() { typeof(KyleProducer) }; // TODO: make this work

    // private data
    List<ProducerBase> _purchasedProducers;

    // Start is called before the first frame update
    void Start()
    {
        // bind pointers
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        menuRoot = gameObject;
        backgroundImage = gameObject.GetComponent<Image>();

        // find all producers (active and inactive)
        _purchasedProducers.AddRange(FindObjectsOfType<ProducerBase>(true));

        // generate producer cards
        producerCards = new List<ProducerMenuCard>();
        foreach (Type prod_type in _producers)
        {

        }

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

    // inner class to help with making UI cards per producer
    class ProducerMenuCard
    {
        public Type ProducerType { get => _producerType; private set => _producerType = value; }
        private Type _producerType;

        // game objects
        GameObject cardRoot;
        GameObject cardImage;
        GameObject cardTmpTitle;
        GameObject cardTmpLevel;
        GameObject cardBtnLevelUp;
        GameObject cardBtnUpgrade;


        ProducerMenuCard(Type producerType, GameObject parent, Vector3 offset)
        {
            // parameter validation
            if(Reflection.IsSameOrSubclass(producerType, typeof(ProducerBase)))
                ProducerType = producerType;
            else
                throw new ArgumentException("Parmater is not derived from ProducerBase", "producerType");

            // make game object root
            cardRoot = new GameObject(parent.name + "." + ProducerType.ToString() + "Card", typeof(RectTransform));
            ((RectTransform)cardRoot.transform).parent = parent.transform;
            ((RectTransform)cardRoot.transform).position = offset;

            /* ============ make card elements ============= */
            // Image
            cardImage = new GameObject(cardRoot.name + ".Image");
            cardImage.transform.parent = cardRoot.transform;
            UI.SetAnchor((RectTransform)cardImage.transform, AnchorPresets.MiddleLeft);
            UI.SetPivot((RectTransform)cardImage.transform, PivotPresets.MiddleLeft);
            cardImage.AddComponent<Image>();

            // Title
            cardTmpTitle = new GameObject(cardRoot.name + ".TmpTitle", typeof(RectTransform));
            cardTmpTitle.transform.parent = cardRoot.transform;
            UI.SetAnchor((RectTransform)cardTmpTitle.transform, AnchorPresets.MiddleLeft, (int)cardTmpTitle.GetComponent<Image>().rectTransform.rect.width);
            UI.SetPivot((RectTransform)cardTmpTitle.transform, PivotPresets.MiddleLeft);
            var cardTmpTitle_TmpComp = cardTmpTitle.AddComponent<TextMeshProUGUI>();
            cardTmpTitle_TmpComp.SetText((string)producerType.GetProperty("Name").GetConstantValue());

            // Level
            cardTmpLevel = new GameObject(cardRoot.name + ".TmpLevel", typeof(RectTransform));
            cardTmpLevel.transform.parent = cardRoot.transform;
            UI.SetAnchor((RectTransform)cardTmpLevel.transform, AnchorPresets.TopRight);
            UI.SetPivot((RectTransform)cardTmpLevel.transform, PivotPresets.TopRight);
            var cardTmpLevel_TmpComp = cardTmpLevel.AddComponent<TextMeshProUGUI>();
            cardTmpLevel_TmpComp.SetText("| LVL XXX");
            var cardTmpLevel_TmpObserve = cardTmpLevel.AddComponent<TMP_Observe>();
            cardTmpLevel_TmpObserve.enabled = false;

            // Button Level Up
            cardBtnLevelUp = new GameObject(cardRoot.name + ".BtnLevelUp", typeof(RectTransform));
            cardBtnLevelUp.transform.parent = cardRoot.transform;
            UI.SetAnchor((RectTransform)cardBtnLevelUp.transform, AnchorPresets.BottomLeft, (int)cardTmpLevel.GetComponent<Image>().rectTransform.rect.width);
            UI.SetPivot((RectTransform)cardBtnLevelUp.transform, PivotPresets.BottomLeft);
            var cardBtnLevelUp_BtnComp = cardBtnLevelUp.AddComponent<Button>();
            var cardBtnLevelUp_TmpComp = cardBtnLevelUp.AddComponent<TextMeshProUGUI>();
            cardBtnLevelUp_TmpComp.SetText("Purchase" + Environment.NewLine + ((double)producerType.GetProperty("PurchasePrice").GetConstantValue()).ToString("#.##"));
            var cardBtnLevelUp_TmpObserve= cardBtnLevelUp.AddComponent<TMP_Observe>();
            cardBtnLevelUp_TmpComp.enabled = false;

            // Button Upgrade
            cardBtnUpgrade = new GameObject(cardRoot.name + ".BtnUpgrade", typeof(RectTransform));
            cardBtnUpgrade.transform.parent = cardRoot.transform;
            UI.SetAnchor((RectTransform)cardBtnUpgrade.transform, AnchorPresets.BottomRight);
            UI.SetPivot((RectTransform)cardBtnUpgrade.transform, PivotPresets.BottomRight);
            var cardBtnUpgrade_BtnComp = cardBtnUpgrade.AddComponent<Button>();
            var cardBtnUpgrade_TmpComp = cardBtnUpgrade.AddComponent<TextMeshProUGUI>();
            cardBtnUpgrade_TmpComp.SetText("Upgrades");

        }
    }
}
