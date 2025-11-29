using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject target;

    public GameObject effectsManager;
    public GameObject hitEffect;
    public float effectDuration = 0.1f;

    public AudioClip hitSound;

    protected Effect effectScript;


    void Awake()
    {
        effectScript = effectsManager.GetComponent<Effect>();
    }

    public virtual void Process(RaycastHit hit, float dmg)
    {

    }
}
