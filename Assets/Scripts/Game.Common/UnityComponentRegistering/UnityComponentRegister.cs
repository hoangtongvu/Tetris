using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Common.UnityComponentRegistering;

public abstract class UnityComponentRegister<TTarget> : SaiMonoBehaviour
    where TTarget : Component
{
    [SerializeField] private TTarget Target;

    private void Awake()
    {
        if (!this.Target) this.Target = GetComponent<TTarget>();

        UnityComponentRegisterMessenger.MessagePublisher.Publish(new UnityComponentRegisterMessage<TTarget>
        {
            Target = this.Target,
        });
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.Target = GetComponent<TTarget>();
    }
}