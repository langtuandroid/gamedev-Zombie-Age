using _3_LevelSelection;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class LevelSelectionInstaller : MonoInstaller
    {
        [SerializeField] private LevelSelectionController _levelSelectionController;
        public override void InstallBindings()
        {
            Container.Bind<LevelSelectionController>().FromInstance(_levelSelectionController).AsSingle();
        }
    }
}   