using System.Collections.Generic;
using Code.Services.Invocations.CreatorDTO;
using Code.Services.Invocations.StaticData;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.Invocations.RandomizerDTO
{
    public class InvocationDTORandomizerService : IInvocationDTORandomizerService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IInvocationCreatorDTOService _invocationCreatorDtoService;

        public InvocationDTORandomizerService(
            IInvocationStaticDataService invocationStaticDataService,
            IInvocationCreatorDTOService invocationCreatorDtoService)
        {
            _invocationStaticDataService = invocationStaticDataService;
            _invocationCreatorDtoService = invocationCreatorDtoService;
        }

        public InvocationDTO GenerateRandomInvocationDTO()
        {
            InvocationStaticData randomStaticData = _invocationStaticDataService.GetRandomInvocation();
            return randomStaticData == null ? null : _invocationCreatorDtoService.GetInvocationDTO(randomStaticData.Id);
        }

        public UnitDTO GenerateRandomUnitDTO()
        {
            List<UnitStaticData> units = _invocationStaticDataService.GetUnitCollection();
            if (units.Count == 0) 
                return null;

            UnitStaticData randomUnit = units[UnityEngine.Random.Range(0, units.Count)];
            return _invocationCreatorDtoService.GetUnitDTO(randomUnit.Id);
        }

        public BuildDTO GenerateRandomBuildingDTO()
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

