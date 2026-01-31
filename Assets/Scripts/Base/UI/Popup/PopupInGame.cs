using UnityEngine;
using BanpoFri;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

using DG.Tweening;
using Unity.VisualScripting;
[UIPath("UI/Popup/PopupInGame", true)]
public class PopupInGame : CommonUIBase
{


    [SerializeField]
    private Button PauseBtn;

    [SerializeField]
    private TextMeshProUGUI WaveText;

    [SerializeField]
    private TextMeshProUGUI TimeText;

    private CompositeDisposable disposables = new CompositeDisposable();

    [SerializeField]
    private InGameSlotGroup InGameSlotGroup;

    [SerializeField]
    private Slider WaveSlider;


    [SerializeField]
    private TextMeshProUGUI HpText;

    [SerializeField]
    private Slider HpSlider;


    protected override void Awake()
    {
        base.Awake();

        //PauseBtn.onClick.AddListener(OnClickPause);
    }


    public void Init()
    {

        GameRoot.Instance.UserData.Ingamesilvercoin.Value += 2000;

        disposables.Clear();

        GameRoot.Instance.UserData.InGamePlayerData.WaveTimePorperty.Subscribe(x =>
        {
            var value = 20 - x;
            TimeText.text = $"00:{value}";
            WaveSlider.value = (float)x / 20;
        }).AddTo(disposables);

        GameRoot.Instance.UserData.Waveidx.Subscribe(x =>
        {
            WaveText.text = $"Wave {x}";
        }).AddTo(disposables);


        GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.CurHpProperty.Subscribe(x=> {SetHpText(x);}).AddTo(disposables);


        InGameSlotGroup.Init();
    }

    public void SetHpText(int hp)
    {
        HpText.text = $"{hp}/{GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.StartHpProperty.Value}";
        HpSlider.value = (float)hp / (float)GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.StartHpProperty.Value;
    }

    public void OnClickPause()
    {
        GameRoot.Instance.UISystem.OpenUI<PopupStageGiveup>();
    }

    public void OnClickReRoll()
    {

    }



    void OnDisable()
    {
        disposables.Clear();
    }

    void OnDestroy()
    {
        disposables.Clear();
    }

}
