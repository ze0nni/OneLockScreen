using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LockScreenComponent : MonoBehaviour
{
    public GameObject DotPrefab;
    public float DotsDistance = 120f;

    readonly private int size = 3;

    void Start() {
        var dotsOrigin = getDotsOrigin();

        for (var x = 0; x < size; x ++) {
            for (var y = 0; y < size; y++) {
                var dotGo = Instantiate(DotPrefab);
                dotGo.transform.SetParent(this.transform, false);

                var dot = dotGo.GetComponent<LocketScreenDotComponent>();

                var transform = dotGo.GetComponent<RectTransform>();
                transform.position = new Vector2(x, y) * DotsDistance + dotsOrigin; ;
            }
        }
    }

    private Vector2 getDotsOrigin() {
        var canvasSize = GetComponent<RectTransform>().rect.size;

        return new Vector2(
            (canvasSize.x - (size - 1) * DotsDistance) / 2f,
            (canvasSize.y - (size - 1) * DotsDistance) / 2f
        );
    }
}
