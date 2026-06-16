using VContainer;
using VContainer.Unity;

public class AppRootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GlobalCounterService>(Lifetime.Singleton);
    }
}
