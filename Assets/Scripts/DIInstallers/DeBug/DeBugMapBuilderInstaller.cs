using UnityEngine;
using Zenject;

/// <summary>
/// Used on the deBug Map Building Scene.
/// Scene is supposed to build Global Map, based on gereated Data
/// Only Visual side of the Map, without further logic
/// </summary>
public class DeBugMapBuilderInstaller : MonoInstaller<DeBugMapBuilderInstaller>
{
    public GameObject MapBuilderGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        Container.BindInstance(MapGenerationData).AsSingle();
        Container.Bind<IMapGenerator>().To<MapGenerator>().AsSingle().NonLazy();
        Container.Bind<IMapDataProvider>().To<DeBugMapDataProvider>().AsSingle().WhenInjectedInto<MapGenerator>();
        Container.Bind<DebugMapBuilder>().FromComponentOn(MapBuilderGO).AsSingle();
    }
}