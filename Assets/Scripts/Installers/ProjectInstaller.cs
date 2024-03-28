using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UIController _uiController;
        [SerializeField] private DataController _dataController;
        [SerializeField] private MusicController _musicManager;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private UpgradeController _upgradeController;
        public override void InstallBindings()
        {
            Container.Bind<UIController>().FromInstance(_uiController).AsSingle();
            Container.Bind<DataController>().FromInstance(_dataController).AsSingle();
            Container.Bind<MusicController>().FromInstance(_musicManager).AsSingle();
            Container.Bind<SoundController>().FromInstance(_soundController).AsSingle();
            Container.Bind<WeaponController>().FromInstance(_weaponController).AsSingle();
            Container.Bind<UpgradeController>().FromInstance(_upgradeController).AsSingle();
        }
    }
}