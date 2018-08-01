using UnityEngine;
using Zenject;

public class TheGameInstaller : MonoInstaller<TheGameInstaller>
{
    public CreepsCollection CreepsCollection;
    public CreepWavesCollection CreepWavesCollection;

    public override void InstallBindings()
    {
        InstallDataProviders();
        InstallMapBuilder();
        InstallCreeps();
        //Init UI Manager

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
        //And load map state!
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

    private void InstallCreeps()
    {
        Container.BindInstance(CreepsCollection).AsSingle().NonLazy();
        Container.BindInstance(CreepWavesCollection).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WaveSpawner>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CreepsManager>().AsSingle().NonLazy();
        Container.BindMemoryPool<Creep, Creep.Pool>().WithInitialSize(20).FromComponentInNewPrefab(CreepsCollection.CreepsList[0].Prefab).UnderTransformGroup("Creeps");
    }
}