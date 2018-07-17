using UnityEngine;
using Zenject;

public class MapGeneratorInstaller : MonoInstaller
{
    public GameObject MapGeneratorGO;

    public override void InstallBindings()
    {
        Container.Bind<MapGenerator>().AsSingle().NonLazy();
        Container.Bind<MapBuilder>().FromComponentOn(MapGeneratorGO).AsSingle();
        Container.Bind<SectionObjectsSpawner>().FromComponentOn(MapGeneratorGO).AsSingle();
    }
}