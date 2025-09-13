using System;
using System.Collections.Generic;
using System.Linq;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.Context;
using Code.Services.IInvocation.Factories;
using Code.Services.LevelConductor;
using Code.Services.LocalProgress;
using Code.Services.StaticData;
using Code.StaticData;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data.Skill;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.Skills
{
    public class SkillExecutorService : ISkillExecutorService
    {
        private const int MaxCountUnitInCell = 9;
        
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IGameContext _gameContext;
        private readonly IInvocationFactory _invocationFactory;
        private readonly ILevelConductor _levelConductor;
        private readonly IStaticDataService _staticDataService;

        public SkillExecutorService(
            ILevelLocalProgressService levelLocalProgressService, 
            IGameContext gameContext,
            IInvocationFactory invocationFactory,
            ILevelConductor levelConductor,
            IStaticDataService staticDataService)
        {
            _levelLocalProgressService = levelLocalProgressService;
            _gameContext = gameContext;
            _invocationFactory = invocationFactory;
            _levelConductor = levelConductor;
            _staticDataService = staticDataService;
        }

        public void ExecuteBuildsSkill()
        {
            PlayerGrid playerGrid = _gameContext.PlayerGrid;
            List<BuildDTO> playerBuilds = GetPlayerBuilds();
            List<SkillData> skillsToExecute = playerBuilds.Select(build => build.Skill).ToList();

            Cell[,] cells = playerGrid.Cells;
            
            for (int x = 0; x < cells.GetLength(0); x++)
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Cell cell = cells[x, y];
                    
                if (cell.InvocationController.Invocations.Count <= 0 || 
                    cell.InvocationController.TargetInvocationType != InvocationType.Unit) 
                    continue;
                    
                foreach (SkillData skill in skillsToExecute) 
                    ApplySkillToCell(skill, cell);
            }
        }
        
        public bool SkillExecute(SkillDTO skillDTO, Cell targetCell)
        {
            if (targetCell == null || targetCell.InvocationController == null || 
                targetCell.InvocationController.Invocations.Count == 0)
                return false;
            
            if (targetCell.InvocationController.TargetInvocationType == InvocationType.Build)
                return false;
            
            if (targetCell.InvocationController.TargetInvocationType != InvocationType.Unit)
                return false;
            
            ApplySkillToCell(skillDTO.Skill, targetCell);
            return true;
        }
        
        private List<BuildDTO> GetPlayerBuilds()
        {
            return _levelLocalProgressService.GetPlayerInvocations()
                .Values
                .OfType<BuildDTO>()
                .ToList();
        }
        
        private void ApplySkillToCell(SkillData skill, Cell targetCell)
        {
            Debug.Log("ApplySkillToCell");
            
            if (skill == null || targetCell == null) 
                return;
            
            string uniqueId = targetCell.InvocationController.UniqueId;
            if (string.IsNullOrEmpty(uniqueId)) 
                return;
            
            InvocationDTO invocationDTO = _levelLocalProgressService.GetPlayerInvocations()
                .Values.FirstOrDefault(dto => dto.UniqueId == uniqueId);
            
            if (invocationDTO is UnitDTO unitDTO) 
                ApplySkillToUnit(skill, unitDTO, targetCell);
        }
        
        private void ApplySkillToUnit(SkillData skill, UnitDTO unitDTO, Cell targetCell)
        {
            switch (skill.SkillType)
            {
                case SkillType.Attack:
                    unitDTO.Damage += (int)skill.Value;
                    break;
                case SkillType.Health:
                    unitDTO.Health += (int)skill.Value;
                    break;
                case SkillType.Speed:
                    unitDTO.Speed += (int)skill.Value;
                    break;
                case SkillType.Capacity:
                    SpawnUnitFromCapacity(unitDTO, targetCell);
                    break;
                case SkillType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SpawnUnitFromCapacity(UnitDTO unitDTO, Cell targetCell)
        {
            UnitDTO newUnit = new UnitDTO(unitDTO.Id, unitDTO.UniqueId, unitDTO.Prefab, unitDTO.Rank, 
                unitDTO.CardDefinition, unitDTO.InvocationType, 1, unitDTO.Health, unitDTO.Damage, 
                unitDTO.Speed);
            
            Invocation invocation = _invocationFactory.CreateInvocationByType(newUnit, targetCell, HeadRotation.PlayerRotation);
            targetCell.InvocationController.AddInvocation(invocation, newUnit.InvocationType, newUnit.Id, newUnit.UniqueId);
            _levelConductor.AddInvocationForPlayer(newUnit);
        }
        
        private HeadRotation HeadRotation => _staticDataService.Balance.HeadRotation;
    }
}
