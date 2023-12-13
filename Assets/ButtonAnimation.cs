using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Serialized Fields

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float scale = 1.2f;
    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private Ease ease = Ease.InOutSine;

    #endregion

    #region Private Fields

    private Vector3 _originalScale;
    private Quaternion _originalRotation;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _originalScale = transform.localScale;
        _originalRotation = transform.rotation;
    }

    #endregion

    #region Pointer Events

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ReferenceEquals(gameObject, null))
        {
            transform.DOScale(_originalScale * scale, duration)
                .SetEase(ease);

            transform.DORotateQuaternion(_originalRotation * Quaternion.Euler(rotationAngle, 0, 0), duration)
                .SetEase(ease);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ReferenceEquals(gameObject, null))
        {
            transform.DOScale(_originalScale, duration)
                .SetEase(ease);

            transform.DORotateQuaternion(_originalRotation, duration)
                .SetEase(ease);
        }
    }

    #endregion

    #region Scene Handling

    private void OnDisable()
    {
        DOTween.Kill(gameObject);
    }

    #endregion
}