using VH.Tools;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().FromNew().AsSingle();
    }
}