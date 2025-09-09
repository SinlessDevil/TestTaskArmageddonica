using System.Collections.Generic;
using Code.StaticData.Invocation.Data;

namespace Code.Services.IInvocation.StaticData
{
    public interface IInvocationStaticDataService
    {
        void LoadData();
        InvocationStaticData GetInvocationData(string id);
        List<InvocationStaticData> GetAllInvocations();
        InvocationStaticData GetRandomInvocation();
        List<UnitStaticData> GetUnitCollection();
        List<BuildStaticData> GetBuildCollection();
        List<SkillStaticData> GetSkillCollection();
    }
}