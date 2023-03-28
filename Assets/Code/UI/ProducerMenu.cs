using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine.UI;
using Assets.Code.Helpers;

[Serializable] public class SerializableDictionary_StringSprite : SerializableDictionary<string, Sprite> { }

[RequireComponent(typeof(Image))]
public class ProducerMenu : MonoBehaviour
{
    // pointers
    private Camera mainCam = null;

    // menu object and component pointers
    GameObject menuRoot = null;
    Image backgroundImage = null;
    List<ProducerMenuCard> producerCards = new List<ProducerMenuCard>();

    // private data
    public List<Type> _producers = new List<Type>();
    List<ProducerBase> _purchasedProducers = new List<ProducerBase>();

    // inspector
    [SerializeField] [Tooltip("Menu Background")]
    Sprite background;
    [SerializeField] [Tooltip("Card Background")]
    Sprite cardBackground;
    [SerializeField] [Tooltip("Card Font")]
    TMP_FontAsset cardFont;
    [SerializeField] [Tooltip("Card Font Size")]
    float cardFontSize = 25;
    [SerializeField] [Tooltip("Card Images")]
    SerializableDictionary_StringSprite cardImages = new SerializableDictionary_StringSprite();
    [SerializeField] [Tooltip("Card Color")]
    Color cardColor;

    // Start is called before the first frame update
    void Start()
    {
        // bind pointers
        mainCam = GameObject.Find("MainCamera")?.GetComponent<Camera>();
        menuRoot = gameObject;
        backgroundImage = gameObject.GetComponent<Image>();

        // find all producers (active and inactive)
        _purchasedProducers.AddRange(FindObjectsOfType<ProducerBase>(true));

        // generate producer cards
        _producers = Reflection.FindAllDerivedFrom<ProducerBase>().ToList();
        CardConfig cardConfig = new CardConfig()
        {
            parent = menuRoot,
            offset = new Vector2(0f, -0.01f),
            background = cardBackground,
            font = cardFont,
            fontSize = cardFontSize,
            color = cardColor
        };
        foreach (Type prodType in _producers)
        {
            // get image if provided
            if (cardImages.ContainsKey((string)prodType.GetField("Name").GetValue(null)))
                cardConfig.image = cardImages[(string)prodType.GetField("Name").GetValue(null)];
            else
                cardConfig.image = null; // TODO: default image?

            if (isProducerPurchased(prodType))
                producerCards.Add(new ProducerMenuCard(_purchasedProducers.Where(x => x.GetType().IsEquivalentTo(prodType)).First(), cardConfig));
            else
                producerCards.Add(new ProducerMenuCard(prodType, cardConfig));

        }
    }

    private bool isProducerPurchased(Type producerType)
    {
        return _purchasedProducers
            .Select<ProducerBase, Type>(x => x.GetType())
            .Any(t => Reflection.IsSameOrSubclass(t, producerType));
    }

    // inner class to help with making UI cards per producer
    internal class ProducerMenuCard
    {
        public Type ProducerType { get => _ProducerType; private set => _ProducerType = value; }
        private Type _ProducerType;

        public ProducerBase Producer { get => _Producer; private set => _Producer = value; }
        private ProducerBase _Producer;

        public bool IsPurchased { get => _IsPurchased; }
        private bool _IsPurchased;

        // game objects
        private GameObject cardRoot;
        private GameObject cardImage;
        private GameObject cardTmpTitle;
        private GameObject cardTmpLevel;
        private ButtonDescriptor cardBtnLevelUp;
        private ButtonDescriptor cardBtnUpgrade;

        #region Constructors
        // generated card for an unpurchased producer
        internal ProducerMenuCard(Type producerType, CardConfig config) 
        {
            // parameter validation
            if(!Reflection.IsSameOrSubclass(producerType, typeof(ProducerBase))) {
                throw new ArgumentException("ProducerMenuCard must take a type that derived from ProducerBase");
            }
            ProducerType = producerType;
            Producer = null;
            ContructorMain(config);
        }

        // generates card for a purchased producer
        internal ProducerMenuCard(ProducerBase producer, CardConfig config)
        {
            // parameters
            ProducerType = producer.GetType();
            Producer = producer;
            ContructorMain(config);
        }

        // Common contructor method called by both above
        private void ContructorMain(CardConfig config)
        {
            // make game object root
            generateCardRoot(config.parent, config.offset, config.color, config.background);

            // make card elements
            generateCardImage(cardRoot, config.image);
            generateCardTmpTitle(cardRoot, (string)ProducerType.GetField("Name").GetValue(null), 
                config.font, config.fontSize);
            generateCardTmpLevel(cardRoot, config.font, config.fontSize);
            generateCardBtnLevel(cardRoot, config.font, config.fontSize);
            generateCardBtnUpgrade(cardRoot, config.font, config.fontSize);

            // set purchase state
            setIsPurchased(Producer != null);
        }
        #endregion

        // methods to generate the individual UI objects on the card
        // offset is a multiplier of width and height of parent
        #region UIgeneration
        private void generateCardRoot(GameObject parent, Vector2 offset, Color color, Sprite background = null)
        {
            // create
            cardRoot = new GameObject("Card", typeof(RectTransform));

            // transform
            UI.Arrange((RectTransform)cardRoot.transform, (RectTransform)parent.transform, 
                AnchorPresets.TopCenter, PivotPresets.TopCenter, 
                offset, 
                new Vector2(0.9f, 0.2f));

            // background
            var cardRoot_ImageComp = cardRoot.AddComponent<Image>();
            if (background != null)
                cardRoot_ImageComp.sprite = background;
            else
                cardRoot_ImageComp.color = color;
        }

        private void generateCardImage(GameObject parent, Sprite image = null)
        {
            // create
            cardImage = new GameObject("Image", typeof(RectTransform));

            // transform
            UI.Arrange((RectTransform)cardImage.transform, (RectTransform)parent.transform, 
                AnchorPresets.TopLeft, PivotPresets.TopLeft, 
                new Vector2(0.05f, -0.05f),
                new Vector2(0.2f, 0.9f));

            // background
            var cardImage_ImageComp = cardImage.AddComponent<Image>();
            if (image != null)
                cardImage_ImageComp.sprite = image;
        }

        private void generateCardTmpTitle(GameObject parent, string title, TMP_FontAsset font, float fontSize)
        {
            // create
            cardTmpTitle = new GameObject("TmpTitle", typeof(RectTransform));

            // transform
            UI.Arrange((RectTransform)cardTmpTitle.transform, (RectTransform)parent.transform, 
                AnchorPresets.TopLeft, PivotPresets.TopLeft, 
                new Vector2(0.3f, -0.05f),
                new Vector2(0.3f, 0.3f));

            // title text
            var cardTmpTitle_TmpComp = cardTmpTitle.AddComponent<TextMeshProUGUI>();
            cardTmpTitle_TmpComp.SetText(title);
            cardTmpTitle_TmpComp.font = font;
            cardTmpTitle_TmpComp.fontSize = fontSize;
        }

        private void generateCardTmpLevel(GameObject parent, TMP_FontAsset font, float fontSize)
        {
            // create
            cardTmpLevel = new GameObject("TmpLevel", typeof(RectTransform));

            // transform
            UI.Arrange((RectTransform)cardTmpLevel.transform, (RectTransform)parent.transform,
                AnchorPresets.TopLeft, PivotPresets.TopLeft,
                new Vector2(0.65f, -0.05f),
                new Vector2(0.3f, 0.3f));

            // level text
            var cardTmpLevel_TmpComp = cardTmpLevel.AddComponent<TextMeshProUGUI>();
            cardTmpLevel_TmpComp.SetText("| LVL XXX");
            cardTmpLevel_TmpComp.font = font;
            cardTmpLevel_TmpComp.fontSize = fontSize;
        }

        private void generateCardBtnLevel(GameObject parent, TMP_FontAsset font, float fontSize)
        {
            // create
            cardBtnLevelUp = UI.GenerateButton((RectTransform)parent.transform, "BtnLevelUp", 
                AnchorPresets.TopLeft, PivotPresets.TopLeft, new Vector2(0.3f, -0.4f), new Vector2(0.3f, 0.55f));
            cardBtnLevelUp.textComp.font = font;
            cardBtnLevelUp.textComp.fontSize = fontSize;
        }

        private void generateCardBtnUpgrade(GameObject parent, TMP_FontAsset font, float fontSize)
        {
            // create
            cardBtnUpgrade = UI.GenerateButton((RectTransform)parent.transform, "BtnUpgrade",
                AnchorPresets.TopLeft, PivotPresets.TopLeft, new Vector2(0.65f, -0.4f), new Vector2(0.3f, 0.55f));
            cardBtnUpgrade.textComp.font = font;
            cardBtnUpgrade.textComp.fontSize = fontSize;
        }
        #endregion

        // methods that handle card state being different for purchased upgrades
        #region IsPurchasedBindings
        private void setIsPurchased(bool isPurchased)
        {
            setIsPurchased_CardTmpLevel(isPurchased);
            setIsPurchased_CardBtnLevelUp(isPurchased);
            setIsPurchased_CardBtnUpgrade(isPurchased);
            _IsPurchased = isPurchased;
        }
        private void setIsPurchased_CardTmpLevel(bool isPurchased)
        {
            if (isPurchased)
            {
                var cardTmpLevel_TmpObserve = cardTmpLevel.AddComponent<TMP_Observe>();
                cardTmpLevel_TmpObserve.URI = new string[] { Producer.gameObject.name + "." + ProducerType.ToString() + ".Level" };
                cardTmpLevel_TmpObserve.format = "| LVL {0:000}";
            }
        }
        private void setIsPurchased_CardBtnLevelUp(bool isPurchased)
        {
            if (isPurchased)
            {
                cardBtnLevelUp.btnComp.onClick.AddListener(btnLevelUp_OnClick);
                var cardBtnLevelUp_TmpObserve = cardBtnLevelUp.textComp.gameObject.AddComponent<TMP_Observe>();
                cardBtnLevelUp_TmpObserve.URI = new string[] { Producer.gameObject.name + "." + ProducerType.ToString() + ".LevelCost" };
                cardBtnLevelUp_TmpObserve.format = "LVL UP\r\n{0:C2}";
            }
            else
            {
                cardBtnLevelUp.textComp.SetText("Purchase" + Environment.NewLine + ((double)_ProducerType.GetField("PurchasePrice").GetValue(null)).ToString("#.##"));
                cardBtnLevelUp.btnComp.onClick.AddListener(btnPurchase_OnClick);
            }
        }
        private void setIsPurchased_CardBtnUpgrade(bool isPurchased)
        {
            if (isPurchased)
            {
                cardBtnUpgrade.gameObject.SetActive(true);
                cardBtnUpgrade.textComp.SetText("Upgrades");
                cardBtnUpgrade.btnComp.onClick.AddListener(btnUpgrade_OnClick);
            }
            else
            {
                cardBtnUpgrade.gameObject.SetActive(false);
            }
        }
        #endregion

        // bindings to the buttons on the card
        #region ButtonBindings
        private void btnLevelUp_OnClick()
        {
            if(Producer != null)
            {
                Producer.LevelUp();
            }
        }

        private void btnPurchase_OnClick()
        {
            GameObject producer_go = new GameObject((string)ProducerType.GetField("Name").GetValue(null));
            Producer = producer_go.AddComponent(ProducerType) as ProducerBase; // due to required component this should also add the upgrade manager
            producer_go.transform.parent = GameObject.Find("Producers").transform;

            // enable various components
            setIsPurchased(true);
        }

        private void btnUpgrade_OnClick()
        {
            // TODO: implement upgrade menu
        }
        #endregion
    }

    internal struct CardConfig
    {
        public GameObject parent;
        public Vector3 offset;
        public Sprite background;
        public TMP_FontAsset font;
        public float fontSize;
        public Sprite image;
        public Color color;
    }
}
