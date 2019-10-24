using Unity;
using Unity.Lifetime;
using WebRTC.Service;

namespace WebRTC.DI {
    public static class DependencyResolver {
        public static void Resolve(UnityContainer container) {
            container.RegisterType(typeof(Swap), new HierarchicalLifetimeManager());
        }
    }
}