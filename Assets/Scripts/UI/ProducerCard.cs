using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProducerCard : MonoBehaviour
{
    [SerializeField] [Tooltip("reference to producer prefab")]
    public GameObject ProducerPrefab;
    private GameObject gobj_Producer;
    private ProducerBase Producer;

    [SerializeField] [Tooltip("reference to upgrade button")]
    public GameObject gobj_UpgradeButton;
    private Button UpgradeButton;
    private TextMeshProUGUI UpgradeButtonText;

    [SerializeField] [Tooltip("reference to level up button")]
    public GameObject gobj_LevelUpButton;
    private Button LevelUpButton;
    private TextMeshProUGUI LevelUpButtonText;

    [SerializeField] [Tooltip("reference to level text")]
    public GameObject gobj_LevelText;
    private TextMeshProUGUI LevelText;

    public bool IsPurchased { get => isPurchased; private set => setIsPurchased(value); }
    private bool isPurchased;
    
    // Start is called before the first frame update
    void Start()
    {
        Producer = ProducerPrefab.GetComponentInChildren<ProducerBase>();
        UpgradeButtonText = gobj_UpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        UpgradeButton = gobj_UpgradeButton.GetComponent<Button>();
        LevelUpButtonText = gobj_LevelUpButton.GetComponentInChildren<TextMeshProUGUI>();
        LevelUpButton = gobj_LevelUpButton.GetComponent<Button>();
        LevelText = gobj_LevelText.GetComponent<TextMeshProUGUI>();
        setIsPurchased(false);
    }

    private void setIsPurchased(bool _isPurchased)
    {
        isPurchased = _isPurchased;
        if (IsPurchased)
        {
            // button binding updates
            UpgradeButton.onClick.RemoveAllListeners();
            LevelUpButton.onClick.RemoveAllListeners();
            UpgradeButton.onClick.AddListener(btnUpgrade_OnClick);
            LevelUpButton.onClick.AddListener(btnLevelUp_OnClick);

            // text updates
            UpgradeButtonText.SetText("Upgrade");
            UpgradeButtonText.alignment = TextAlignmentOptions.Center;
            LevelUpButtonText.SetText(string.Format("Level Up\r\n{0:C0}", Producer.LevelPrice));
            LevelUpButtonText.alignment = TextAlignmentOptions.Center;
            gobj_LevelUpButton.SetActive(true);
            LevelText.SetText(string.Format("Level: {0:D}", Producer.Level));
            gobj_LevelText.SetActive(true);
        }
        else
        {
            // button binding updates
            UpgradeButton.onClick.RemoveAllListeners();
            LevelUpButton.onClick.RemoveAllListeners();
            UpgradeButton.onClick.AddListener(btnPurchase_OnClick);

            // text updates
            UpgradeButtonText.SetText(string.Format("Purchase\r\n{0:C0}", Producer.config.BasePurchasePrice));
            UpgradeButtonText.alignment = TextAlignmentOptions.Center;
            LevelUpButtonText.SetText("");
            LevelUpButtonText.alignment = TextAlignmentOptions.Center;
            gobj_LevelUpButton.SetActive(false);
            LevelText.SetText(string.Format(""));
            gobj_LevelText.SetActive(false);
        }
    }

    private void btnLevelUp_OnClick()
    {
        Producer.LevelUp();
        LevelText.SetText(string.Format("Level: {0:D}", Producer.Level));
    }

    private void btnPurchase_OnClick()
    {
        // instantiate prefab
        gobj_Producer = Instantiate(ProducerPrefab);
        Producer = gobj_Producer.GetComponentInChildren<ProducerBase>();

        setIsPurchased(true);
    }

    private void btnUpgrade_OnClick()
    {
        // TODO: implement upgrade menu
    }
}
