using Code.StaticData.Invocation.DTO;

namespace Code.Services.IInvocation.Randomizer
{
    public interface IRandomizerService
    {
        InvocationDTO GenerateRandomInvocationDTO();
        UnitDTO GenerateRandomUnitDTO();
        BuildingDTO GenerateRandomBuildingDTO();
        SkillDTO GenerateRandomSkillDTO();
    }
}

