using UnityEngine;
using Zenject;

public class TheGameInstaller : MonoInstaller<TheGameInstaller>
{
    public GameObject TowerPrefab;
    public GameObject BulletPrefab;
    public GameObject CreepPrefab;
    public CreepWavesCollection CreepWavesCollection;
    public PlayerData PlayerData;
    public TowerData DefaultTowerData;

    public override void InstallBindings()
    {
        SignalsInstaller.Install(Container);

        InstallDataProviders();
        InstallMapBuilder();
        InstallCreeps();
        InstallTowers();

    }

    private void InstallDataProviders()
    {
        Container.BindInstance(PlayerData).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerAccountant>().AsSingle();
        Container.Bind<SaveLoader>().AsSingle().NonLazy();
        Container.Bind<PlayerSaveData>().FromComponentsOn(this.gameObject).AsSingle();
        Container.Bind<IMapDataProvider>().To<MapDataProvider>().AsSingle().WhenInjectedInto<MapGenerator>();
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
        Container.BindInstance(CreepWavesCollection).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WaveSpawner>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CreepsManager>().AsSingle().NonLazy();
        //Container.BindMemoryPool<Creep, Creep.Pool>().WithInitialSize(20).FromComponentInNewPrefab(CreepPrefab).UnderTransformGroup("Creeps");
        Container.BindFactory<CreepData, Creep, Creep.Factory>().FromMonoPoolableMemoryPool(x => x.WithInitialSize(16).FromComponentInNewPrefab(CreepPrefab).UnderTransformGroup("Creeps"));
    }

    private void InstallTowers()
    {
        Container.BindInstance(DefaultTowerData).AsSingle().WhenInjectedInto<TowerBuilder>();
        Container.Bind<TowerBuilder>().AsSingle().NonLazy();
        Container.BindFactory<Vector3, TowerData, Tower, Tower.Factory>().FromComponentInNewPrefab(TowerPrefab);
        Container.BindFactory<BulletData, Vector3, Transform, Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x.WithInitialSize(8).FromComponentInNewPrefab(BulletPrefab).UnderTransformGroup("Bullets"));
        //Container.BindFactory<BulletData, Vector3, Transform, Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool<BulletData, Vector3, Transform, BulletPool>(x => x.WithInitialSize(8).FromComponentInNewPrefab(BulletPrefab).UnderTransformGroup("Bullets"));
    }

    //Try with IL2CPP -> Hack
    //public class BulletPool : MonoPoolableMemoryPool<BulletData, Vector3, Transform, IMemoryPool, Bullet>
    //{

    //}
}