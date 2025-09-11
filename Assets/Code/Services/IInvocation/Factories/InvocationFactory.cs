using System;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Logic.Invocation.Builds;
using Code.Logic.Invocation.Skills;
using Code.Logic.Invocation.Units;
using Code.Services.IInvocation.StaticData;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services.IInvocation.Factories
{
    public class InvocationFactory : IInvocationFactory
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;

        public InvocationFactory(IInvocationStaticDataService invocationStaticDataService)
        {
            _invocationStaticDataService = invocationStaticDataService;
        }

        public Invocation CreateInvocationByType(InvocationDTO dto, Cell targetCell, Quaternion rotation)
        {
            Invocation invocation;
            
            switch (dto.InvocationType)
            {
                case InvocationType.Unit:
                    invocation = CreateUnit((UnitDTO)dto, targetCell);
                    break;
                case InvocationType.Build:
                    invocation = CreateBuilding((BuildingDTO)dto, targetCell);
                    break;
                case InvocationType.Skill:
                    invocation = CreateSkill((SkillDTO)dto, targetCell);
                    break;
                case InvocationType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            invocation.Initialize(dto.UniqueId);

            return invocation;
        }
        
        public Invocation CreateInvocationBase(InvocationDTO invocationDTO, Cell targetCell)
        {
            InvocationStaticData staticData = _invocationStaticDataService.GetInvocationData(invocationDTO.Id);
            if (staticData?.Prefab == null) 
                return CreateFallbackInvocation(invocationDTO.CardDefinition.ToString(), targetCell);
            
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            return Object.Instantiate(staticData.Prefab, spawnPosition, Quaternion.identity).GetComponent<Invocation>();
        }

        public Unit CreateUnit(UnitDTO unitDTO, Cell targetCell) => 
            CreateInvocationBase(unitDTO, targetCell).GetComponent<Unit>();

        public Build CreateBuilding(BuildingDTO buildingDTO, Cell targetCell) => 
            CreateInvocationBase(buildingDTO, targetCell).GetComponent<Build>();

        public Skill CreateSkill(SkillDTO skillDTO, Cell targetCell) => 
            CreateInvocationBase(skillDTO, targetCell).GetComponent<Skill>();

        private Invocation CreateFallbackInvocation(string name, Cell targetCell)
        {
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            GameObject fallbackObject = new GameObject($"Invocation_{name}");
            fallbackObject.transform.position = spawnPosition;
            Invocation invocation = fallbackObject.AddComponent<Invocation>();
            return invocation;
        }

        private Vector3 GetSpawnPosition(Cell targetCell) => targetCell.transform.position;
    }
}