using UnityEngine;
using Zenject;

public class DeBugMapGeneratorInstaller : MonoInstaller
{
    public GameObject MapGeneratorGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        CreateGenerator();
        InstallDebugModules();
        Container.BindInstance(MapGenerationData).AsSingle();
    }

    private void CreateGenerator()
    {
        Instantiate(MapGeneratorGO);
    }

    private void InstallDebugModules()
    {
        Container.Bind<IMapGenerator>().To<DebugMapGenerator>().AsSingle().NonLazy();
        Container.Bind<DebugSectionBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
    }
}