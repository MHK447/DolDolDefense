using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerSkillBase
{
    public int SkillIdx = 0;

    public int SkillLevel = 0;

    public float SkillCoolTime = 0f;

    public float Skilldeltatime = 0f;

    protected InGameBaseStage InGameStage = null;

    public PlayerSkillBase()
    {
        OnInitalize();
    }

    public virtual void OnInitalize()
    {
        InGameStage = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage;

        var td = Tables.Instance.GetTable<PlayerSkillInfo>().GetData(SkillIdx);


        if(td != null)
        {
            Skilldeltatime = 0f;
            SkillCoolTime = td.base_atk_speed * 0.01f;
        }

    }

    public virtual void Update()
    {
        if(InGameStage == null) return;

        if(InGameStage.PlayerUnit == null) return;

        if(!GameRoot.Instance.UserData.InGamePlayerData.IsGameStartProperty.Value) return;

        if(SkillCoolTime <= 0f) return;


        Skilldeltatime += Time.deltaTime;

        if(Skilldeltatime >= SkillCoolTime)
        {
            OnSkillUse();
            Skilldeltatime = 0f;
        }
    }

    public virtual void OnSkillUse()
    {

    }
}

