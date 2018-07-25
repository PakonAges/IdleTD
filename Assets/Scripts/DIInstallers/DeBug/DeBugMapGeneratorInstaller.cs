using UnityEngine;
using Zenject;


/// <summary>
/// Used in deBug scene to generate local Map config and visualize it on the scene
/// </summary>
public class DeBugMapGeneratorInstaller : MonoInstaller
{
    public GameObject MapGeneratorGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        Container.BindInstance(MapGenerationData).AsSingle();
        CreateGenerator();
        InstallDebugModules();
    }

    private void CreateGenerator()
    {
        Container.BindInstance(MapGeneratorGO).AsSingle();
    }

    private void InstallDebugModules()
    {
        Container.Bind<IMapGenerator>().To<DebugMapGenerator>().AsSingle().NonLazy();
        Container.Bind<IMapDataProvider>().To<DeBugMapDataProvider>().AsSingle().WhenInjectedInto<DebugMapGenerator>();
        Container.Bind<DebugSectionBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
    }
}