using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingSurface : MonoBehaviour
{
    [Header("Velocidad a la que se mueve la superficie circular")] [Range(-180f, 180f)] public float rotateSpeed = 10f;
    [Header("Cuanto afecta al movimiento del jugador esta superficie? (puede desincronizarse asi que usar con cuidado o simplemente usar el valor predeterminado)")] public float playerMoveFactor = 1.2f;
    
    public Transform targetT;
    void FixedUpdate()
    {
        targetT.eulerAngles -= new Vector3(0f,0f,rotateSpeed*Time.deltaTime);
    }
}
