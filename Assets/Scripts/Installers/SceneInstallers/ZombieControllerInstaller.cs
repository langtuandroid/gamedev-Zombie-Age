using MANAGERS;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class ZombieControllerInstaller : MonoInstaller
    {
        [SerializeField] private ZombieController _zombieController;
        public override void InstallBindings()
        {
            Container.Bind<ZombieController>().FromInstance(_zombieController).AsSingle();
        }
    }
}