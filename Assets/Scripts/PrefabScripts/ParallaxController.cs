using UnityEngine;
using UnityEngine.UI;

public class ParallaxController : MonoBehaviour
{
    [SerializeField]  private RawImage _texture;
    [SerializeField]  private float _x, _y;

    void Update()
    {
        _texture.uvRect = new Rect(_texture.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _texture.uvRect.size);
    }
}