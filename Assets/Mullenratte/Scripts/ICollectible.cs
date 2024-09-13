using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible : IInteractable
{
    public abstract void CollectBehaviour();
}
