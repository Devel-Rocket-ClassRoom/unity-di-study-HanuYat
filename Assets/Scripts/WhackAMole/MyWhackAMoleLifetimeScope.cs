using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MyWhackAMoleLifetimeScope : LifetimeScope
{
    [SerializeField]
    private MyGameConfig m_Config = new MyGameConfig();

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(m_Config);

        builder.Register<IScoreService, MyScoreService>(Lifetime.Singleton);
        builder.Register<ISaveService, MyPlayerPrefsSaveService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MyGameDirector>().As<IRoundService>();

        builder.RegisterComponentInHierarchy<MyAudioManager>().As<IAudioService>();
        builder.RegisterComponentInHierarchy<MyMoleSpawner>();
        builder.RegisterComponentInHierarchy<MyGameHudController>();
    }
}
