using UnityEngine;
using Zenject;

public class DeBugMapBuilderInstaller : MonoInstaller<DeBugMapBuilderInstaller>
{
    public GameObject MapBuilderGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        Container.BindInstance(MapGenerationData).AsSingle();
        Container.Bind<MapGenerator>().AsSingle().NonLazy();
        Container.Bind<DebugMapBuilder>().FromComponentOn(MapBuilderGO).AsSingle();
    }
}