using System.Collections.Generic;
using Code.Services.IInvocation.StaticData;
using Code.Services.UniqueId;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.IInvocation.Randomizer
{
    public class InvocationDataRandomizerService : IRandomizerService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IUniqueIdService _uniqueIdService;

        public InvocationDataRandomizerService(
            IInvocationStaticDataService invocationStaticDataService,
            IUniqueIdService uniqueIdService)
        {
            _invocationStaticDataService = invocationStaticDataService;
            _uniqueIdService = uniqueIdService;
        }

        public InvocationDTO GenerateRandomInvocationDTO()
        {
            InvocationStaticData randomStaticData = _invocationStaticDataService.GetRandomInvocation();
            if (randomStaticData == null) 
                return null;

            string uniqueId = _uniqueIdService.GenerateUniqueId(randomStaticData.InvocationType.ToString());

            return randomStaticData.InvocationType switch
            {
                InvocationType.Unit => new UnitDTO(randomStaticData.Id, uniqueId, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                InvocationType.Build => new BuildingDTO(randomStaticData.Id, uniqueId, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                InvocationType.Skill => new SkillDTO(randomStaticData.Id, uniqueId, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType),
                _ => new InvocationDTO(randomStaticData.Id, uniqueId, randomStaticData.Prefab, randomStaticData.Rank, randomStaticData.CardDefinition, randomStaticData.InvocationType)
            };
        }

        public UnitDTO GenerateRandomUnitDTO()
        {
            List<UnitStaticData> units = _invocationStaticDataService.GetUnitCollection();
            if (units.Count == 0) 
                return null;

            UnitStaticData randomUnit = units[UnityEngine.Random.Range(0, units.Count)];
            string uniqueId = _uniqueIdService.GenerateUniqueId("Unit");
            return new UnitDTO(randomUnit.Id, uniqueId, randomUnit.Prefab, randomUnit.Rank, randomUnit.CardDefinition, randomUnit.InvocationType);
        }

        public BuildingDTO GenerateRandomBuildingDTO()
        {
            List<BuildStaticData> buildings = _invocationStaticDataService.GetBuildCollection();
            if (buildings.Count == 0) 
                return null;

            BuildStaticData randomBuilding = buildings[UnityEngine.Random.Range(0, buildings.Count)];
            string uniqueId = _uniqueIdService.GenerateUniqueId("Building");
            return new BuildingDTO(randomBuilding.Id, uniqueId, randomBuilding.Prefab, randomBuilding.Rank, randomBuilding.CardDefinition, randomBuilding.InvocationType);
        }

        public SkillDTO GenerateRandomSkillDTO()
        {
            List<SkillStaticData> skills = _invocationStaticDataService.GetSkillCollection();
            if (skills.Count == 0) 
                return null;

            SkillStaticData randomSkill = skills[UnityEngine.Random.Range(0, skills.Count)];
            string uniqueId = _uniqueIdService.GenerateUniqueId("Skill");
            return new SkillDTO(randomSkill.Id, uniqueId, randomSkill.Prefab, randomSkill.Rank, randomSkill.CardDefinition, randomSkill.InvocationType);
        }
    }
}

