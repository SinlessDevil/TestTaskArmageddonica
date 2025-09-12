namespace Code.UI.Game.Finish.InvocationIcon
{
    public class InvocationIconComposite
    {
        public InvocationIconView View;
        public IInvocationIconPM PM;
        
        public InvocationIconComposite(InvocationIconView view, IInvocationIconPM pm)
        {
            View = view;
            PM = pm;
        }
    }
}