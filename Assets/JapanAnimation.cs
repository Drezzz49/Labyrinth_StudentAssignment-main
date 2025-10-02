using UnityEngine;

public class JapanAnimation : MonoBehaviour
{

    Animator charactorAnimator;

    void Start()
    {
        charactorAnimator = transform.Find("Kuratchi_l_rigged_ver.1.0").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
