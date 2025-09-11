using System.Collections.Generic;
using Code.Services.IInvocation.Creator;
using Code.Services.IInvocation.StaticData;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.IInvocation.Randomizer
{
    public class InvocationDataRandomizerService : IRandomizerService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IInvocationCreatorDTOService _invocationCreatorDtoService;

        public InvocationDataRandomizerService(
            IInvocationStaticDataService invocationStaticDataService,
            IInvocationCreatorDTOService invocationCreatorDtoService)
        {
            _invocationStaticDataService = invocationStaticDataService;
            _invocationCreatorDtoService = invocationCreatorDtoService;
        }

        public InvocationDTO GenerateRandomInvocationDTO()
        {
            InvocationStaticData randomStaticData = _invocationStaticDataService.GetRandomInvocation();
            if (randomStaticData == null) 
                return null;

            return _invocationCreatorDtoService.GetInvocationDTO(randomStaticData.Id);
        }

        public UnitDTO GenerateRandomUnitDTO()
        {
            List<UnitStaticData> units = _invocationStaticDataService.GetUnitCollection();
            if (units.Count == 0) 
                return null;

            UnitStaticData randomUnit = units[UnityEngine.Random.Range(0, units.Count)];
            return _invocationCreatorDtoService.GetUnitDTO(randomUnit.Id);
        }

        public BuildingDTO GenerateRandomBuildingDTO()
        {
            List<BuildStaticData> buildings = _invocationStaticDataService.GetBuildCollection();
            if (buildings.Count == 0) 
                return null;

            BuildStaticData randomBuilding = buildings[UnityEngine.Random.Range(0, buildings.Count)];
            return _invocationCreatorDtoService.GetBuildingDTO(randomBuilding.Id);
        }

        public SkillDTO GenerateRandomSkillDTO()
        {
            List<SkillStaticData> skills = _invocationStaticDataService.GetSkillCollection();
            if (skills.Count == 0) 
                return null;

            SkillStaticData randomSkill = skills[UnityEngine.Random.Range(0, skills.Count)];
            return _invocationCreatorDtoService.GetSkillDTO(randomSkill.Id);
        }
    }
}

