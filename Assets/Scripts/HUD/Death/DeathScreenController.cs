using UnityEngine;

public class DeathScreenController : MonoBehaviour
{
    public Animator deathAnimator;

    public void ShowDeathScreen()
    {
        deathAnimator.SetTrigger("FadeIn");
    }
}
