using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System.Collections;
using DG.Tweening;

public enum SlotStatusInfo
{
    Money = 1,
    Ability = 2,
    Stat = 3,
}

public class InGameSlotGroup : MonoBehaviour
{
    [SerializeField]
    private Button GachaBtn;


    [SerializeField]
    private TextMeshProUGUI GachaCoinText;

    [SerializeField]
    private TextMeshProUGUI PriceText;

    [SerializeField]
    private List<InGameSlotComponent> SlotComponentList = new List<InGameSlotComponent>();
    
    [SerializeField]
    private DOTweenAnimation TweenAnim;

    private CompositeDisposable disposables = new CompositeDisposable();

    private int BasePrice = 20;

    private int GachaCount = 0;

    private bool IsGachaOn = false;

    private Coroutine GachaCorutine = null;

    void Awake()
    {
        GachaBtn.onClick.AddListener(OnClickGacha);
    }




    public void Init()
    {
        IsGachaOn = false;

        foreach (var slot in SlotComponentList)
        {
            slot.SetChoice((int)SlotStatusInfo.Money, 1, InGameSlotComponent.InGameSlotStatus.NoneSelect);
        }

        BasePrice = 20;
        GachaCount = 0;

        disposables.Clear();
        GameRoot.Instance.UserData.Ingamesilvercoin.Subscribe(x =>
        {
            SetStatusGachaCoin();
        }).AddTo(disposables);


    }

    public void OnClickGacha()
    {
        var price = GetPrice();


        if (GameRoot.Instance.UserData.Ingamesilvercoin.Value >= price)
        {
            ProjectUtility.Vibrate();
            GameRoot.Instance.UserData.Ingamesilvercoin.Value -= price;
            TweenAnim.DORestart();
            GachaCount++;
            GetPrice();
            SetStatusGachaCoin();
            GachaCorutine = StartCoroutine(StartGachaOn());
        }
    }


    public IEnumerator StartGachaOn()
    {
        IsGachaOn = true;
        GachaBtn.interactable = false;

        var getrandvalue = Random.Range((int)SlotStatusInfo.Money, (int)SlotStatusInfo.Stat + 1);
        var getrandgradevalue = ProjectUtility.GetGachaSlotGrade();

        foreach (var slot in SlotComponentList)
        {
            slot.SetChoice(getrandvalue, getrandgradevalue, InGameSlotComponent.InGameSlotStatus.Rolling);
        }

        yield return new WaitForSeconds(0.5f);

        // 등급에 따른 Active 슬롯 개수 결정
        int activeCount = 0;
        switch (getrandgradevalue)
        {
            case 1: activeCount = 3; break; // 1등급: 3개
            case 2: activeCount = 4; break; // 2등급: 4개
            case 3: activeCount = 5; break; // 3등급: 5개
            case 4: activeCount = 6; break; // 4등급: 6개
            default: activeCount = SlotComponentList.Count; break; // 그 외: 전부
        }

        // 랜덤으로 activeCount 개수만큼 인덱스 선택
        var totalSlots = SlotComponentList.Count;
        var randomIndices = Enumerable.Range(0, totalSlots).OrderBy(x => Random.value).Take(activeCount).ToHashSet();

        // 선택된 인덱스는 Active, 나머지는 NoneSelect
        for (int i = 0; i < totalSlots; i++)
        {
            var status = randomIndices.Contains(i)
                ? InGameSlotComponent.InGameSlotStatus.Active
                : InGameSlotComponent.InGameSlotStatus.NoneSelect;

            SlotComponentList[i].SetChoice(getrandvalue, getrandgradevalue, status);
        }

        yield return new WaitForSeconds(0.5f);

        IsGachaOn = false;
        SetStatusGachaCoin();
    }

    public int GetPrice()
    {
        return BasePrice + (GachaCount * 3);
    }


    public void SetStatusGachaCoin()
    {
        var price = GetPrice();
        GachaCoinText.text = GameRoot.Instance.UserData.Ingamesilvercoin.Value.ToString();
        PriceText.text = price.ToString();

        GachaBtn.interactable = GameRoot.Instance.UserData.Ingamesilvercoin.Value >= price && !IsGachaOn;

        PriceText.color = GameRoot.Instance.UserData.Ingamesilvercoin.Value >= price ? Color.white : Color.red;

    }


    void OnDestroy()
    {
        disposables.Clear();

        if (GachaCorutine != null)
        {
            GameRoot.Instance.StopCoroutine(GachaCorutine);
            GachaCorutine = null;
        }
    }

    void OnDisable()
    {
        disposables.Clear();

        if (GachaCorutine != null)
        {
            GameRoot.Instance.StopCoroutine(GachaCorutine);
            GachaCorutine = null;
        }
    }


}

