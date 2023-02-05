using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{ 
    public abstract void Attack();
    public abstract void Jump();
    public abstract void UpdateDirection(float value);
}