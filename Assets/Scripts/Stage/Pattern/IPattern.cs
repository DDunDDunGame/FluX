using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPattern
{
    void Initialize(Player player);
    void OnUpdate();
    void Destroy();
}
