using UnityEngine;
using Zenject;

public class TheGameInstaller : MonoInstaller<TheGameInstaller>
{
    public override void InstallBindings()
    {
        InstallSaveLoader();
        InstallMapBuilder();
        
        //Init UI Manager

        //Build Navigaton

        //Check offline duration and simulate it
        //Show UI "WelcomeBack"

        //The game process
    }

    private void InstallSaveLoader()
    {
        Container.Bind<SaveLoader>().AsSingle().NonLazy();
        Container.Bind<PlayerSaveData>().FromComponentsOn(this.gameObject).AsSingle();
        //Check for Save game
        //if yes -> Build saved map
        //if no -> build new one
        //And map state!

        //GetMapGenerationData from save! And Bind it!
    }

    private void InstallMapBuilder()
    {
        Container.Bind<IMapGenerator>().To<MapGenerator>().AsSingle().NonLazy();
        //Bind Map Buiilder. But make a refactor!
        //I need all model prefabs in some collection
        //Then Builder -> non monobehaviour
        //Then some rules where which model to build (How to choose based on player progress!

    }
}