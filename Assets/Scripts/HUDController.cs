using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    [SerializeField] Slider _healthSlider;
    [SerializeField] Text _candyText;
    [SerializeField] Image _flashImage;

    PlayerController _playerRef;

    private int _candyAmount;

    // Set variables
    private void Awake()
    {
        _playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
        _maxHealth = _playerRef.MaxHealth;
        _currentHealth = _playerRef.CurrentHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;
    }

    // Candy counter, will be used in HUD / Shop later
    public void AddCandy(int amount)
    {
        _candyAmount += amount;
        _candyText.text = _candyAmount.ToString();
    }

    // Event Subscribing
    private void OnEnable()
    {
        _playerRef.PlayerDamage += OnPlayerDamage;
    }

    private void OnDisable()
    {
        _playerRef.PlayerDamage -= OnPlayerDamage;
    }

    // Event
    void OnPlayerDamage()
    {
        _currentHealth = _playerRef.CurrentHealth;
        _healthSlider.value = _currentHealth;
        StartCoroutine(ImageFlashCoroutine(.5f));
    }

    IEnumerator ImageFlashCoroutine(float fadeDuration)
    {
        Color initialColor = new Color(1f, .7f, .7f, .8f);
        Color targetColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0f;

        _flashImage.color = initialColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _flashImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        _flashImage.color = targetColor;
    }
}

