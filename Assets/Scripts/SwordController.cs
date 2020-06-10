using UnityEngine;

public class SwordController : MonoBehaviour
{
    public const uint MAX_COMBO = 2;
    protected Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        string clipName = null;
        if (currentClipInfo.Length > 0)
        {
            clipName = currentClipInfo[0].clip.name;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(clipName);
            switch (clipName)
            {
                case null:
                    _animator.SetTrigger("StartCombo");
                    _animator.SetInteger("CurrentCombo", 1);
                    break;
                case "Combo 1":
                    break;
                case "Post Combo 1":
                    _animator.SetInteger("CurrentCombo", 2);
                    break;
                case "Combo 2":
                    break;
                case "Post Combo 2":
                    _animator.SetInteger("CurrentCombo", 0);
                    break;
                default:
                    throw new System.Exception("Invalid combo");
            }
        }
        else
        {
            switch (clipName)
            {
                case null:
                    break;
                case "Combo 1":
                    break;
                case "Post Combo 1":
                    // Reset combo if nothing is being pressed and no other combo has been queued
                    int combo = _animator.GetInteger("CurrentCombo");
                    if (combo != 2)
                    {
                        _animator.SetInteger("CurrentCombo", 0);
                    }
                    break;
                case "Combo 2":
                    break;
                case "Post Combo 2":
                   _animator.SetInteger("CurrentCombo", 0);
                    break;
                default:
                    throw new System.Exception("Invalid combo");
            }
        }
    }
}
