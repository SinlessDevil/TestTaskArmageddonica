using System;
using System.Collections.Generic;
using Code.UI.Game.Finish.InvocationIcon;

namespace Code.UI.Game.Finish.Win
{
    public interface IWinWindowPM
    {
        event Action ClosedWindowEvent;
        float PlayerScore { get; }
        List<InvocationIconComposite> GetInvocationCompositeCollection();
        void OnCloseWindow();
        void Cleanup();
    }
}
