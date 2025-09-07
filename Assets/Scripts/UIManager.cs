using CodeMonkey.Utils;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private GameObject _gameOver;
    private static GameObject popupPrefab;
    
    void Awake() => popupPrefab = _popupPrefab;

    public void Restart() => SceneManager.LoadScene(1);

    public void Continue() => SceneManager.LoadScene(0);

    public void GameOver() => _gameOver.SetActive(true);
    
    public static void UpdateTextObject(TextMeshProUGUI textObject, object value, bool updateDimensions = false) {
        if (textObject == null) return;
        
        textObject.text = value.ToString();

        if (!updateDimensions) return;
        
        Vector2 preferredDimensions = textObject.GetPreferredValues();
        textObject.rectTransform.sizeDelta = new(preferredDimensions.x, textObject.rectTransform.sizeDelta.y);
    }

    public static void UpdateSlider(Slider slider, float percentage, bool flipColor) {
        slider.value = percentage / 100f;
        
        Image fill = slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        fill.color = flipColor
            ? TriColorLerp(Color.green, Color.yellow, Color.red, slider.value)
            : TriColorLerp(Color.red, Color.yellow, Color.green, slider.value);
    }

    private static Color TriColorLerp(Color a, Color b, Color c, float t) {
        if (t > 0.5f)
            return Color.Lerp(b, c, t * 2 - 1f);
        
        return Color.Lerp(a, b, t * 2);
    }
    
    public static void Popup(string text, Vector3 position, Color? color = null, float size = 1f)
    {
        if (color == null) color = Color.white;
        
        GameObject canvas = Instantiate(popupPrefab, position + Vector3.up, Quaternion.identity);
        RectTransform rectTransform = canvas.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * 0.0025f * size;
        
        TextMeshProUGUI textMesh = canvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = color.Value;

        float startingTime = 0.75f;
        float time = startingTime;
        
        FunctionUpdater.Create(() => {
            rectTransform.position += new Vector3(0, Time.unscaledDeltaTime / startingTime);
            textMesh.color = new(textMesh.color.r, textMesh.color.g, textMesh.color.b, textMesh.color.a - Time.unscaledDeltaTime / startingTime);
            
            time -= Time.unscaledDeltaTime;
            if (time <= 0f)
            {
                Destroy(canvas);
                return true;
            }

            return false;
        }, "WorldTextPopup");
    }

}