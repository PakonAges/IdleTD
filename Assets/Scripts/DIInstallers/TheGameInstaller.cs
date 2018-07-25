using UnityEngine;
using Zenject;

public class TheGameInstaller : MonoInstaller<TheGameInstaller>
{
    public override void InstallBindings()
    {
        InstallDataProviders();
        InstallMapBuilder();
        
        //Init UI Manager

        //Build Navigaton

        //Check offline duration and simulate it
        //Show UI "WelcomeBack"

        //The game process
    }

    private void InstallDataProviders()
    {
        Container.Bind<SaveLoader>().AsSingle().NonLazy();
        Container.Bind<PlayerSaveData>().FromComponentsOn(this.gameObject).AsSingle();
        Container.Bind<IMapDataProvider>().To<MapDataProvider>().AsSingle().WhenInjectedInto<MapGenerator>();
        //Check for Save game
        //if yes -> Build saved map
        //if no -> build new one
        //And map state!

        //GetMapGenerationData from save! And Bind it!
    }

    private void InstallMapBuilder()
    {
        Container.Bind<IMapGenerator>().To<MapGenerator>().AsSingle().NonLazy();
        Container.Bind<MapBuilder>().FromComponentsOn(this.gameObject).AsSingle().NonLazy();
        Container.Bind<NavMeshCreator>().FromComponentsOn(this.gameObject).AsSingle();
        Container.Bind<LocalCreepWayBuilder>().AsSingle().NonLazy();
        Container.Bind<MapManager>().AsSingle().NonLazy();
        Container.Bind<WaypointsSpawner>().AsSingle().NonLazy();
        Container.Bind<GlobalCreepPath>().AsSingle().NonLazy();
        //add some rules where which model to build (How to choose based on player progress!

    }
}