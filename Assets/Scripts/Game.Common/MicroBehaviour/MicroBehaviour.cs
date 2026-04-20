using Reflex.Core;
using Reflex.Extensions;
using SaintsField;
using System;
using UnityEngine;

namespace Game.Common;

[Serializable]
public abstract class MicroBehaviour
{
    [SerializeField, ReadOnly] protected MicroBehavioursExecutor executor;
    protected Container sceneContainer;

    public void Init(MicroBehavioursExecutor executor)
    {
        this.executor = executor;
    }

    public void AssignSceneContainer()
    {
        this.sceneContainer = this.executor.gameObject.scene.GetSceneContainer();
    }

    public virtual void InjectDependencies() { }

    protected void InjectSingle<T>(out T t)
    {
        t = this.sceneContainer.Single<T>();
    }

    public virtual void Awake() { }

    public virtual void LoadComponents() { }

    protected void LoadComponentInChildren<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = this.executor.GetComponentInChildren<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", this.executor.gameObject);
    }

    protected void LoadComponentInCtrl<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = this.executor.GetComponent<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", this.executor.gameObject);
    }

    protected void LoadCtrl<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = this.executor.GetComponentInParent<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", this.executor.gameObject);
    }

    protected void LoadTransformInChildrenByName(out Transform t, string name)
    {
        t = this.executor.transform.Find(name);
        if (t != null) return;

        Debug.LogError($"Can't find child Transform with name {name}", this.executor.gameObject);
    }

    protected void FindFirstObjectByType<T>(out T t)
        where T : UnityEngine.Object
    {
        t = UnityEngine.Object.FindFirstObjectByType<T>();
    }

    protected void FindAnyObjectByType<T>(out T t)
        where T : UnityEngine.Object
    {
        t = UnityEngine.Object.FindAnyObjectByType<T>();
    }

    protected void FindObjectsByType<T>(out T[] tArray, FindObjectsSortMode findObjectsSortMode = FindObjectsSortMode.None)
        where T : UnityEngine.Object
    {
        tArray = UnityEngine.Object.FindObjectsByType<T>(findObjectsSortMode);
    }

    public virtual void Start() { }

    public virtual void OnDestroy() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void OnEnable() { }

    public virtual void OnDisable() { }
}