using Code.Logic.Grid;
using Code.Logic.Invocation.Builds;
using Code.Logic.Invocation.Skills;
using Code.Logic.Invocation.Units;
using Code.Services.IInvocation.DTO;
using Code.Services.IInvocation.StaticData;
using Code.StaticData.Invocation.Data;
using UnityEngine;

namespace Code.Services.IInvocation.Factories
{
    public class InvocationFactory : IInvocationFactory
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;

        public InvocationFactory(IInvocationStaticDataService invocationStaticDataService)
        {
            _invocationStaticDataService = invocationStaticDataService;
        }

        public GameObject CreateInvocation(InvocationDTO invocationDTO, Cell targetCell)
        {
            InvocationStaticData staticData = _invocationStaticDataService.GetInvocationData(invocationDTO.Id);
            if (staticData?.Prefab == null) 
                return CreateFallbackObject(invocationDTO.CardDefinition.ToString(), targetCell);
            
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            return Object.Instantiate(staticData.Prefab, spawnPosition, Quaternion.identity);
        }

        public Unit CreateUnit(UnitDTO unitDTO, Cell targetCell) => 
            CreateInvocation(unitDTO, targetCell).GetComponent<Unit>();

        public Build CreateBuilding(BuildingDTO buildingDTO, Cell targetCell) => 
            CreateInvocation(buildingDTO, targetCell).GetComponent<Build>();

        public Skill CreateSkill(SkillDTO skillDTO, Cell targetCell) => 
            CreateInvocation(skillDTO, targetCell).GetComponent<Skill>();

        private GameObject CreateFallbackObject(string name, Cell targetCell)
        {
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            GameObject fallbackObject = new GameObject($"Invocation_{name}");
            fallbackObject.transform.position = spawnPosition;
            return fallbackObject;
        }

        private Vector3 GetSpawnPosition(Cell targetCell)
        {
            return targetCell.transform.position + Vector3.up * 0.5f;
        }
    }
}