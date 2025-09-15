using Code.StaticData.Invocation.DTO;

namespace Code.Services.Invocations.RandomizerDTO
{
    public interface IInvocationDTORandomizerService
    {
        InvocationDTO GenerateRandomInvocationDTO();
        UnitDTO GenerateRandomUnitDTO();
        BuildDTO GenerateRandomBuildingDTO();
        SkillDTO GenerateRandomSkillDTO();
    }
}

