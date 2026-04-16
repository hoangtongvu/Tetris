using ZBase.Foundation.PubSub;
using UnityEngine;
using Unity.Entities;

namespace Game.Common.UnityComponentRegistering;

public struct UnityComponentRegisterMessage<TID, TTarget>: IMessage
    where TID : unmanaged
    where TTarget : Component
{
    public TID Id;
    public UnityObjectRef<TTarget> Target;
}

public struct UnityComponentRegisterMessage<TTarget>: IMessage
    where TTarget : Component
{
    public UnityObjectRef<TTarget> Target;
}