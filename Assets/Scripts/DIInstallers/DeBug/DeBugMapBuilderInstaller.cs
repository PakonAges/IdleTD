using UnityEngine;
using Zenject;

public class DeBugMapBuilderInstaller : MonoInstaller<DeBugMapBuilderInstaller>
{
    public GameObject MapBuilderGO;

    public override void InstallBindings()
    {
        Container.Bind<DebugMapBuilder>().FromComponentOn(MapBuilderGO).AsSingle();
    }
}