using Code.StaticData.Invocation.DTO;

namespace Code.Services.Invocations.CreatorDTO
{
    public interface IInvocationCreatorDTOService
    {
        InvocationDTO GetInvocationDTO(string invocationId);
        UnitDTO GetUnitDTO(string unitId);
        BuildDTO GetBuildingDTO(string buildingId);
        SkillDTO GetSkillDTO(string skillId);
    }
}
