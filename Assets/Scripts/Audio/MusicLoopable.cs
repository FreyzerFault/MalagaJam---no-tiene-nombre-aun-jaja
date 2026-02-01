using System.Collections;
using UnityEngine;

namespace Audio
{
    public class MusicLoopHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource introSource;
        [SerializeField] private AudioSource loopableSource;

        private void Start() => StartCoroutine(SwitchClipCoroutine());

        private IEnumerator SwitchClipCoroutine()
        {
            yield return new WaitUntil(() => !introSource.isPlaying);
            loopableSource.Play();
        }
    }
}
