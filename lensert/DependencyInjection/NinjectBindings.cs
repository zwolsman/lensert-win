using Lensert.Core;
using Lensert.Helpers;
using Ninject.Modules;

namespace Lensert.DependencyInjection
{
    internal sealed class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IHotkeyHandler>().To<LensertHotkeyHandler>().InSingletonScope();
            Bind<IImageUploader>().To<LensertClient>().InSingletonScope();
        }
    }
}
