using System.Collections.Generic;
using Code.Services.IInvocation.DTO;
using Code.Services.IInvocation.StaticData;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;

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
            InvocationStaticData randomStaticData = _invocationStaticDataService.GetRandomInvocation();
            if (randomStaticData == null) 
                return null;

            return randomStaticData.InvocationType switch
            {
                InvocationType.Unit => new UnitDTO(randomStaticData.Id, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                InvocationType.Build => new BuildingDTO(randomStaticData.Id, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                InvocationType.Skill => new SkillDTO(randomStaticData.Id, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                _ => new InvocationDTO(randomStaticData.Id, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType)
            };
        }

        public UnitDTO GenerateRandomUnitDTO()
        {
            List<InvocationStaticData> allInvocations = _invocationStaticDataService.GetAllInvocations();
            List<InvocationStaticData> units = allInvocations.FindAll(x => x.InvocationType == InvocationType.Unit);
            if (units.Count == 0) 
                return null;

            InvocationStaticData randomUnit = units[UnityEngine.Random.Range(0, units.Count)];
            return new UnitDTO(randomUnit.Id, randomUnit.Prefab, randomUnit.Rank, randomUnit.CardDefinition, randomUnit.InvocationType);
        }

        public BuildingDTO GenerateRandomBuildingDTO()
        {
            List<InvocationStaticData> allInvocations = _invocationStaticDataService.GetAllInvocations();
            List<InvocationStaticData> buildings = allInvocations.FindAll(x => x.InvocationType == InvocationType.Build);
            if (buildings.Count == 0) 
                return null;

            var randomBuilding = buildings[UnityEngine.Random.Range(0, buildings.Count)];
            return new BuildingDTO(randomBuilding.Id, randomBuilding.Prefab, randomBuilding.Rank, randomBuilding.CardDefinition, randomBuilding.InvocationType);
        }

        public SkillDTO GenerateRandomSkillDTO()
        {
            var allInvocations = _invocationStaticDataService.GetAllInvocations();
            var skills = allInvocations.FindAll(x => x.InvocationType == InvocationType.Skill);
            if (skills.Count == 0) return null;

            var randomSkill = skills[UnityEngine.Random.Range(0, skills.Count)];
            return new SkillDTO(randomSkill.Id, randomSkill.Prefab, randomSkill.Rank, randomSkill.CardDefinition, randomSkill.InvocationType);
        }
    }
}

