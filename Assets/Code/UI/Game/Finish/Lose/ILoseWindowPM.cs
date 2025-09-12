using System;
using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;
using Code.UI.Game.Finish.InvocationIcon;

namespace Code.UI.Game.Finish.Lose
{
    public interface ILoseWindowPM
    {
        event Action ClosedWindowEvent;
        float PlayerScore { get; }
        List<InvocationDTO> UsedUnits { get; }
        List<InvocationIconComposite> GetInvocationCompositeCollection();
        void OnCloseWindow();
        void Cleanup();
    }
}
