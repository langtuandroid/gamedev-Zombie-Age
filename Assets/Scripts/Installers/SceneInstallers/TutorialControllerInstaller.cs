using MANAGERS;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class TutorialControllerInstaller : MonoInstaller
    {
        [SerializeField] private TutorialController _tutorialController;
        public override void InstallBindings()
        {
            Container.Bind<TutorialController>().FromInstance(_tutorialController).AsSingle();
        }
    }
}