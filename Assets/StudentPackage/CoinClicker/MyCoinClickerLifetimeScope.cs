using UnityEngine;
using VContainer;
using VContainer.Unity;
using DIStudy.CoinClicker.Student;

public class MyCoinClickerLifetimeScope : LifetimeScope
{
    [SerializeField]
    private MyGameConfig m_Config = new MyGameConfig();

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(m_Config);

        builder.Register<IScoreService, MyScoreService>(Lifetime.Singleton);
        builder.Register<ISaveService, MyPlayerPrefsSaveService>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<MyAudioManager>().As<IAudioService>();
        builder.RegisterComponentInHierarchy<MyCoinSpawner>();
        builder.RegisterComponentInHierarchy<MyGameHudController>();

        builder.RegisterEntryPoint<MyGameDirector>();
    }
}
