using _2_Weapon;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class WeaponControllerInstaller : MonoInstaller
    {
        [SerializeField] private WeaponsManager _weapons;
        public override void InstallBindings()
        {
            Container.Bind<WeaponsManager>().FromInstance(_weapons).AsSingle();
        }
    }
}