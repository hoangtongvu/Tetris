using SaintsField;
using SaintsField.Playa;
using System;
using UnityEditor;
using UnityEngine;

namespace Game.Common.SerializableWrapper;

[Serializable]
public sealed class SerializableStructWrapper<TValue>
    where TValue : struct, ISerializableStruct
{
    [SerializeField, ReadOnly]
    private string _key;

    [ShowInInspector]
    public TValue Value
    {
        get
        {
            if (string.IsNullOrEmpty(this._key))
                return default;

            TValue value = default;
            value.AssignFromSerializableKey(_key);
            return value;
        }
        set
        {
            _key = value.ToSerializableKey();
#if UNITY_EDITOR
            EditorUtility.SetDirty(Selection.activeObject);
#endif
        }
    }
}