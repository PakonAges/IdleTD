using System;
using UnityEngine;
using Zenject;

public class MapGeneratorInstaller : MonoInstaller
{
    public GameObject MapGeneratorGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        Container.Bind<MapGenerator>().AsSingle().NonLazy();
        Container.Bind<MapBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
        Container.Bind<SectionObjectsSpawner>().FromComponentOn(MapGeneratorGO).AsSingle();
        Container.BindInstance(MapGenerationData).AsSingle();

        InstallDebugModules();
    }

    private void InstallDebugModules()
    {
        Container.Bind<DebugSectionBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
    }
}