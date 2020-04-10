using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(LocketScreeRopeComponent))]
public class LockScreenComponent : MonoBehaviour
{
    public GameObject DotPrefab;
    public float DotsDistance = 120f;

    readonly private int size = 3;

    private LocketScreeRopeComponent rope;

    void Start() {
        this.rope = GetComponent<LocketScreeRopeComponent>();

        var dotsOrigin = getDotsOrigin();

        var dotIndex = 0;
        for (var x = 0; x < size; x ++) {
            for (var y = 0; y < size; y++) {
                var dotGo = Instantiate(DotPrefab);
                dotGo.transform.SetParent(this.transform, false);

                var dot = dotGo.GetComponent<LocketScreenDotComponent>();
                dot.DotIndex = dotIndex;
                dot.OnPointerPressed += this.rope.DotEventHandler;
                dot.OnPointerEntered += this.rope.DotEventHandler;

                var transform = dotGo.GetComponent<RectTransform>();
                transform.position = new Vector2(x, y) * DotsDistance + dotsOrigin;

                dotIndex++;
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
