using UnityEngine;
using BanpoFri;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class LevelupRewardComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ChoiceDesc;


    [SerializeField]
    private TextMeshProUGUI UpgradeNameText;

    [SerializeField]
    private Image BgImg;

    [SerializeField]
    private Image BackImg;

    [SerializeField]
    private Image FrontImg;


    [SerializeField]
    private Button ChoiceBtn;



    [SerializeField]
    private GameObject RecommendLabel;

    [SerializeField]
    private Image[] BlockLevelImages = new Image[3];

    [SerializeField]
    private Image[] BlockRootLevelImages = new Image[3];

    [SerializeField]
    private GameObject BlockLevelGroup;


    private System.Action<InGameUpgrade> OnSelectCallback;

    private InGameUpgrade Upgrade;


    private Sprite BlinkEmptySprite;
    private Sprite BlinkFilledSprite;
    private bool DoBlink;

    void Awake()
    {
        if (ChoiceBtn != null)
            ChoiceBtn.onClick.AddListener(OnClickChoice);
    }



    public void Set(InGameUpgrade upgrade, System.Action<InGameUpgrade> onselectcallback)
    {
        Upgrade = upgrade;

        OnSelectCallback = onselectcallback;

        FrontImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_Upgrade, upgrade.UpgradeChoiceData.front_img);
        BackImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_Upgrade, upgrade.UpgradeChoiceData.back_img);

        bool recommended = Upgrade.IsRecommend;

        ProjectUtility.SetActiveCheck(RecommendLabel, recommended);

        BgImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_LevelUp, $"Common_Frame_Levelup_{upgrade.UpgradeChoiceData.category}");

        UpgradeNameText.text = Tables.Instance.GetTable<Localize>().GetString(upgrade.UpgradeChoiceData.choice_name);
       
    }




    public void OnClickChoice()
    {
        OnSelectCallback?.Invoke(Upgrade);
    }


}
