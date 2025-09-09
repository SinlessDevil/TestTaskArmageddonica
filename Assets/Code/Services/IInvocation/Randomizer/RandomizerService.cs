using Code.Services.IInvocation.DTO;
using Code.Services.IInvocation.StaticData;

namespace Code.Services.IInvocation.Randomizer
{
    public class RandomizerService : IRandomizerService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;

        public RandomizerService(IInvocationStaticDataService invocationStaticDataService)
        {
            _invocationStaticDataService = invocationStaticDataService;
        }

        public InvocationDTO GenerateRandomInvocationDTO()
        {
            var randomStaticData = _invocationStaticDataService.GetRandomInvocation();
            if (randomStaticData == null) return null;

            return randomStaticData.Type switch
            {
                InvocationType.Unit => new UnitDTO(randomStaticData),
                InvocationType.Build => new BuildingDTO(randomStaticData),
                InvocationType.Skill => new SkillDTO(randomStaticData),
                _ => new InvocationDTO(randomStaticData)
            };
        }

        public UnitDTO GenerateRandomUnitDTO()
        {
            var allInvocations = _invocationStaticDataService.GetAllInvocations();
            var units = allInvocations.FindAll(x => x.Type == InvocationType.Unit);
            if (units.Count == 0) return null;

            var randomUnit = units[Random.Range(0, units.Count)];
            return new UnitDTO(randomUnit);
        }

        public BuildingDTO GenerateRandomBuildingDTO()
        {
            var allInvocations = _invocationStaticDataService.GetAllInvocations();
            var buildings = allInvocations.FindAll(x => x.Type == InvocationType.Build);
            if (buildings.Count == 0) return null;

            var randomBuilding = buildings[Random.Range(0, buildings.Count)];
            return new BuildingDTO(randomBuilding);
        }

        public SkillDTO GenerateRandomSkillDTO()
        {
            var allInvocations = _invocationStaticDataService.GetAllInvocations();
            var skills = allInvocations.FindAll(x => x.Type == InvocationType.Skill);
            if (skills.Count == 0) return null;

            var randomSkill = skills[Random.Range(0, skills.Count)];
            return new SkillDTO(randomSkill);
        }
    }
}

