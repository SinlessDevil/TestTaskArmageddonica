using System;

namespace Code.Infrastructure
{
    public interface ILoadingCurtain
    {
        public event Action StartedShowLoadingEvent;
        public event Action FinishedShowLoadingEvent;
        
        public bool IsActive { get; }
        public void Show();
        public void Hide();
    }
}