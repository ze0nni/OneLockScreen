using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LockScreenComponent : MonoBehaviour
{
    public Sprite CommonDot;
    public Sprite ActiveDot;
    public Sprite RejectedDot;

    public float DotsDistance = 5f;

    readonly private int size = 3;

    void Start() {
        var canvasSize = GetComponent<RectTransform>().rect.size;

        var dotsOrigin = new Vector2(
            (canvasSize.x - (size - 1) *DotsDistance) / 2f,
            (canvasSize.y - (size - 1) * DotsDistance) / 2f
        );

        for (var x = 0; x < size; x ++) {
            for (var y = 0; y < size; y++) {
                //TODO: Prefab
                var dotGo = new GameObject("Dot", typeof(Image), typeof(LocketScreenDotComponent));
                dotGo.transform.SetParent(this.transform, false);

                var dot = dotGo.GetComponent<LocketScreenDotComponent>();

                var dotImage = dotGo.GetComponent<Image>();
                dotImage.sprite = CommonDot;

                var transform = dotGo.GetComponent<RectTransform>();
                transform.position = new Vector2(x, y) * DotsDistance + dotsOrigin; ;
            }
        }
    }
}
