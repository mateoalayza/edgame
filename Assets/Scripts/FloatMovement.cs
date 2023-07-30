using UnityEngine;
using DG.Tweening;

public class FloatMovement : MonoBehaviour
{
    public float floatDuration = 1.0f; // Duración de un ciclo completo de flotación.
    public float floatHeight = 1.0f; // Altura máxima de flotación.

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        StartFloating();
    }

    private void StartFloating()
    {
        // Calculamos la posición final de la flotación.
        Vector3 targetPosition = initialPosition + Vector3.up * floatHeight;

        // Creamos un bucle infinito para la animación de flotación con Dotween.
        transform.DOMove(targetPosition, floatDuration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo); // Hacemos que el movimiento vuelva a la posición inicial (Yoyo) de forma infinita.
    }
}

