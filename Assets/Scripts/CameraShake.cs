using DG.Tweening;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
    public void Shake()
    {
        transform
            .DOShakePosition(
                duration: 0.15f, 
                strength: 0.05f,   
                vibrato: 0,       
                randomness: 90,
                fadeOut: true
            )
            .SetEase(Ease.OutQuad);
    }
}
