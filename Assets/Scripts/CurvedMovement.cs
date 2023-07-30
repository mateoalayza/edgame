using UnityEngine;
using DG.Tweening;

public class CurvedMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float curveHeight = 2.0f;
    public float moveDuration = 3.0f;
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one * 1.5f;
    public float scaleDuration = 2.0f;
    public float delayBetweenLoops = 1.0f;

    private bool movingToStart = false;

    private void Start()
    {
        StartLooping();
    }

    private void StartLooping()
    {
        movingToStart = false;
        MoveObject(endPoint.position, endScale, moveDuration, scaleDuration);
    }

    private void MoveObject(Vector3 targetPosition, Vector3 targetScale, float moveDuration, float scaleDuration)
    {
        transform.DOKill(); // Detener cualquier animación actual en el objeto.

        // Movimiento en forma de curva con movimiento sinusoidal en el eje Y.
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad));
        sequence.Join(transform.DOScale(targetScale, scaleDuration).SetEase(Ease.InOutQuad));

        // Al completar la animación, llamamos a OnMoveComplete para determinar el siguiente movimiento.
        sequence.OnComplete(OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        // Cambiar la dirección del movimiento y realizar el siguiente movimiento.
        if (movingToStart)
        {
            MoveObject(endPoint.position, endScale, moveDuration, scaleDuration);
            movingToStart = false;
        }
        else
        {
            MoveObject(startPoint.position, startScale, moveDuration, scaleDuration);
            movingToStart = true;
        }
    }

    private void Update()
    {
        // Movimiento sinusoidal en el eje Y para dar la sensación de flotar.
        float newY = startPoint.position.y + Mathf.Sin(Time.time) * curveHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
