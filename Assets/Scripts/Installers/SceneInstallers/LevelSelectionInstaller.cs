using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class LevelSelectionInstaller : MonoInstaller
    {
        [SerializeField] private LevelSelectionInstaller _levelSelectionInstaller;
        public override void InstallBindings()
        {
            Container.Bind<LevelSelectionInstaller>().FromInstance(_levelSelectionInstaller).AsSingle();
        }
    }
}