using _4_Gameplay;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class SupportManagerInstaller : MonoInstaller
    {
        [SerializeField] private SupportManager _supportManager;
        public override void InstallBindings()
        {
            Container.Bind<SupportManager>().FromInstance(_supportManager).AsSingle();
        }
    }
}