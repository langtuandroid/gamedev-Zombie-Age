using MANAGERS;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class SpriteControllerInstaller : MonoInstaller
    {
        [SerializeField] private SpriteController _spriteController;
        public override void InstallBindings()
        {
            Container.Bind<SpriteController>().FromInstance(_spriteController).AsSingle();
        }
    }
}