using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores current collision data
/// For objects labeled with the targeted tag.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Container : MonoBehaviour {

    /* --- Components --- */
    protected Collider2D collisionFrame;

    /* --- Variables --- */
    // A container to store all objects currently in contact with.
    [SerializeField] public List<Collider2D> container = new List<Collider2D>();
    [HideInInspector] protected string targetTag;

    /* --- Unity --- */
    void Awake() {
        collisionFrame = GetComponent<Collider2D>();
        collisionFrame.isTrigger = true;
        SetTarget();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Add(collider);
    }

    void OnTriggerExit2D(Collider2D collider) {
        Remove(collider);
    }

    /* --- Methods --- */
    void Add(Collider2D collider) {
        if (!container.Contains(collider) && collider.tag == targetTag) {
            container.Add(collider);
            OnAdd(collider);
        }
    }

    void Remove(Collider2D collider) {
        if (container.Contains(collider)) {
            container.Remove(collider);
            OnRemove(collider);
        }
    }

    /* --- Event Methods --- */
    public virtual void OnAdd(Collider2D collider) { 
        
    }

    public virtual void OnRemove(Collider2D collider) {

    }

    /* --- Callback Methods --- */
    public bool IsEmpty() {
        return (container.Count == 0);
    }

    protected virtual void SetTarget() {
        //
    }

}
