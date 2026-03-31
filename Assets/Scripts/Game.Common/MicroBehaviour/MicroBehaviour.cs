using System;

namespace Game.Common;

[Serializable]
public abstract class MicroBehaviour
{
    public virtual void Awake() { }

    public virtual void Start() { }

    public virtual void OnDestroy() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void OnEnable() { }

    public virtual void OnDisable() { }
}