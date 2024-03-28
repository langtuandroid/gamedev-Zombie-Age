using MANAGERS;
using UnityEngine;
using Zenject;

namespace Installers.SceneInstallers
{
    public class ObjectPoolInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPoolController _objectPoolController;
        public override void InstallBindings()
        {
            Container.Bind<ObjectPoolController>().FromInstance(_objectPoolController).AsSingle();
        }
    }
}