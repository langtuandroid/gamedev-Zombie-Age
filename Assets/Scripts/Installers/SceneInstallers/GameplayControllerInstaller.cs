using _4_Gameplay;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class GameplayControllerInstaller : MonoInstaller
    {
        [SerializeField] private GameplayController _gameplayController;
        public override void InstallBindings()
        {
            Container.Bind<GameplayController>().FromInstance(_gameplayController).AsSingle();
        }
    }
}