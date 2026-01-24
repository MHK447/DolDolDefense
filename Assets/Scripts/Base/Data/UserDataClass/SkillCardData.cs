using System;
using System.Collections.Generic;
using UniRx;
using Google.FlatBuffers;

public partial class UserDataSystem
{
    public List<SkillCardData> Skillcarddatas { get; private set; } = new List<SkillCardData>();



    private void SaveData_SkillCardData(FlatBufferBuilder builder)
    {
        // 선언된 변수들은 모두 저장되어야함

        // Skillcarddatas Array 저장
        Offset<BanpoFri.Data.SkillCardData>[] skillcarddatas_Array = null;
        VectorOffset skillcarddatas_Vector = default;

        if(Skillcarddatas.Count > 0){
            skillcarddatas_Array = new Offset<BanpoFri.Data.SkillCardData>[Skillcarddatas.Count];
            int index = 0;
            foreach(var pair in Skillcarddatas){
                var item = pair;
                skillcarddatas_Array[index++] = BanpoFri.Data.SkillCardData.CreateSkillCardData(
                    builder,
                    item.Skillidx,
                    item.Skillevel,
                    item.Skillcount
                );
            }
            skillcarddatas_Vector = BanpoFri.Data.UserData.CreateSkillcarddatasVector(builder, skillcarddatas_Array);
        }



        Action cbAddDatas = () => {
            BanpoFri.Data.UserData.AddSkillcarddatas(builder, skillcarddatas_Vector);
        };

        cb_SaveAddDatas += cbAddDatas;

    }
    private void LoadData_SkillCardData()
    {
        // 로드 함수 내용

        // Skillcarddatas 로드
        Skillcarddatas.Clear();
        int Skillcarddatas_length = flatBufferUserData.SkillcarddatasLength;
        for (int i = 0; i < Skillcarddatas_length; i++)
        {
            var Skillcarddatas_item = flatBufferUserData.Skillcarddatas(i);
            if (Skillcarddatas_item.HasValue)
            {
                var skillcarddata = new SkillCardData
                {
                    Skillidx = Skillcarddatas_item.Value.Skillidx,
                    Skillevel = Skillcarddatas_item.Value.Skillevel,
                    Skillcount = Skillcarddatas_item.Value.Skillcount
                };
                Skillcarddatas.Add(skillcarddata);
            }
        }
    }

}

public class SkillCardData
{
    public int Skillidx { get; set; } = 0;
    public int Skillevel { get; set; } = 0;
    public int Skillcount { get; set; } = 0;

}
