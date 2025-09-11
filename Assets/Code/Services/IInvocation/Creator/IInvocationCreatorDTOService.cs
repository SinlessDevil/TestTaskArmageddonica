using Code.StaticData.Invocation.DTO;

namespace Code.Services.IInvocation.Creator
{
    public interface IInvocationCreatorDTOService
    {
        InvocationDTO GetInvocationDTO(string invocationId);
        UnitDTO GetUnitDTO(string unitId);
        BuildingDTO GetBuildingDTO(string buildingId);
        SkillDTO GetSkillDTO(string skillId);
    }
}
