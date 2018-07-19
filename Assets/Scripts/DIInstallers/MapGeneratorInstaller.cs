using System;
using UnityEngine;
using Zenject;

public class MapGeneratorInstaller : MonoInstaller
{
    public GameObject MapGeneratorGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        InstallDebugModules();
        //Container.Bind<MapBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
        //Container.Bind<SectionObjectsSpawner>().FromComponentOn(MapGeneratorGO).AsSingle();
        Container.BindInstance(MapGenerationData).AsSingle();
    }

    private void InstallDebugModules()
    {
        Container.Bind<DebugMapGenerator>().AsSingle().NonLazy();
        Container.Bind<DebugSectionBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
    }
}