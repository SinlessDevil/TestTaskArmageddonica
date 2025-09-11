using System.Collections.Generic;
using Code.Logic.Invocation;
using Code.StaticData.Invocation;

namespace Code.Logic.Grid
{
    public class CellInvocationController
    {
        public List<Invocation.Invocation> Invocations { get; private set; } = new();
        public InvocationType TargetInvocationType { get; private set; } = InvocationType.Unknown;
        public string UniqueId { get; private set; } = string.Empty;
        
        public void AddInvocation(Invocation.Invocation invocation, InvocationType targetInvocationType, string uniqueId)
        {
            if (HasFreeCell())
            {
                UniqueId = uniqueId;
                TargetInvocationType = targetInvocationType;   
            }
            
            Invocations.Add(invocation);
        }

        public void ClearInvocations()
        {
            Invocations.Clear();
            UniqueId = string.Empty;
            TargetInvocationType = InvocationType.Unknown;
        }
        
        public bool HasFreeCell() => Invocations.Count == 0;

        public bool HasAddedAdditionalInvocation(string uniqueId)
        {
            if (TargetInvocationType == InvocationType.Unit)
            {
                return UniqueId == uniqueId;
            }

            return false;
        }
    }
}
