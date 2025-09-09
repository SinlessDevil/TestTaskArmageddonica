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
            var units = _invocationStaticDataService.GetUnitCollection();
            if (units.Count == 0) 
                return null;

            var randomUnit = units[UnityEngine.Random.Range(0, units.Count)];
            return new UnitDTO(randomUnit.Id, randomUnit.Prefab, randomUnit.Rank, randomUnit.CardDefinition, randomUnit.InvocationType);
        }

        public BuildingDTO GenerateRandomBuildingDTO()
        {
            var buildings = _invocationStaticDataService.GetBuildCollection();
            if (buildings.Count == 0) 
                return null;

            var randomBuilding = buildings[UnityEngine.Random.Range(0, buildings.Count)];
            return new BuildingDTO(randomBuilding.Id, randomBuilding.Prefab, randomBuilding.Rank, randomBuilding.CardDefinition, randomBuilding.InvocationType);
        }

        public SkillDTO GenerateRandomSkillDTO()
        {
            var skills = _invocationStaticDataService.GetSkillCollection();
            if (skills.Count == 0) return null;

            var randomSkill = skills[UnityEngine.Random.Range(0, skills.Count)];
            return new SkillDTO(randomSkill.Id, randomSkill.Prefab, randomSkill.Rank, randomSkill.CardDefinition, randomSkill.InvocationType);
        }
    }
}

