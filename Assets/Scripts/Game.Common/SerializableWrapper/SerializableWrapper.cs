using System;
using UnityEngine;

namespace Game.Common.SerializableWrapper;

public abstract class SerializableWrapper<TValue> : ISerializationCallbackReceiver
    where TValue : ISerializableStruct
{
    [SerializeField] private string _key;
    [NonSerialized] public TValue Value;

    public void OnBeforeSerialize() => _key = this.Value.ToSerializableKey();

    public void OnAfterDeserialize() => this.Value.AssignFromSerializableKey(_key);
}