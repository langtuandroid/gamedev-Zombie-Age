using _5_Upgrade;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers.SceneInstallers
{
    public class UpgradeManagerInstaller : MonoInstaller
    {
        [SerializeField] private UpgradeManager _upgradeManager;
        public override void InstallBindings()
        {
            Container.Bind<UpgradeManager>().FromInstance(_upgradeManager).AsSingle();
        }
    }
}