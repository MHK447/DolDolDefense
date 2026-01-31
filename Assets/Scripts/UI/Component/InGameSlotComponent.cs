using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class InGameSlotComponent : MonoBehaviour
{
    public enum InGameSlotStatus
    {
        Active=  1,
        Rolling = 2,
        NoneSelect = 3,
        Normal = 4,
    }

    [SerializeField]
    private Transform ActiveRoot;

    [SerializeField]
    private Transform RandomRollingRoot;

    [SerializeField]
    private Transform DisabledRoot;

    [SerializeField]
    private Transform NormalRoot;

    [SerializeField]
    private Image SelectChoiceImg;

    [SerializeField]
    private Image BgImg;

    public void SetChoice(int slotidx , int grade , InGameSlotStatus status)
    {
        var td = Tables.Instance.GetTable<InGameSlotInfo>().GetData(slotidx);

        if(td != null)
        {
            SelectChoiceImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_Common, td.icon);
            BgImg.color = Config.Instance.GetImageColor($"slot_grade_{grade}");


            ProjectUtility.SetActiveCheck(ActiveRoot.gameObject, status == InGameSlotStatus.Active);
            ProjectUtility.SetActiveCheck(RandomRollingRoot.gameObject, status == InGameSlotStatus.Rolling);
            ProjectUtility.SetActiveCheck(DisabledRoot.gameObject, status == InGameSlotStatus.NoneSelect);
            ProjectUtility.SetActiveCheck(NormalRoot.gameObject, status == InGameSlotStatus.Normal);
            ProjectUtility.SetActiveCheck(SelectChoiceImg.gameObject, status != InGameSlotStatus.Rolling);
        }
    }

}

