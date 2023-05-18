using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
    private void SelfDestruction()
    {
        Destroy(gameObject);
    }
}
