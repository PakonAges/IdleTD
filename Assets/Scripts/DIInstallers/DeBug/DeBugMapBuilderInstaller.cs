using UnityEngine;
using Zenject;

public class DeBugMapBuilderInstaller : MonoInstaller<DeBugMapBuilderInstaller>
{
    public GameObject MapBuilderGO;
    public MapGenerationData MapGenerationData;

    public override void InstallBindings()
    {
        Container.BindInstance(MapGenerationData).AsSingle();
        Container.Bind<DebugSectionBuilder>().FromComponentOn(this.gameObject).AsSingle();
        Container.Bind<IMapGenerator>().To<DebugMapGenerator>().AsSingle().NonLazy();
        Container.Bind<DebugMapBuilder>().FromComponentOn(MapBuilderGO).AsSingle();
    }
}