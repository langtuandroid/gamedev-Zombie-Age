using MODULES.Soldiers;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class SoldierInstaller : MonoInstaller
    {
        [SerializeField] private Soldier _soldier;
        public override void InstallBindings()
        {
            Container.Bind<Soldier>().FromInstance(_soldier).AsSingle();
        }
    }
}