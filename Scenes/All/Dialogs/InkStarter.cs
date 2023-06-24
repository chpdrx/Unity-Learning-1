using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using StarterAssets;
using Cinemachine;


public class InkStarter : MonoBehaviour
{
	// файл с текстом inky
	[SerializeField] private TextAsset inkJSONAsset = null;
	public Story story;
	public static event Action<Story> OnCreateStory;
	// canvas, на котором скрипт
	[SerializeField] private Canvas canvas = null;
	[SerializeField] private GameObject _canvas;

	// префабы UI текста и кнопки
	[SerializeField] private Text textPrefab = null;
	[SerializeField] private Button buttonPrefab = null;
	// список аудиофайлов, которые нужно проигрывать
	[SerializeField] private List<AudioClip> _audioClips;
	private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
	// на канвасе должен висеть AudioSource
    private AudioSource _audioSource;

    void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		InitializeAudioClips();
	}

    private void OnEnable()
    {
		Cursor.lockState = CursorLockMode.Confined;
		// Очищает UI от дочерних объектов
		RemoveChildren();
		StartStory();
	}

    // Корректирует названия аудиофайлов и формирует словарь
    private void InitializeAudioClips()
    {
        foreach (var clip in _audioClips)
        {
			_clips.Add(clip.name.ToLower().Replace(" ", ""), clip);
        }
    }

    // Уничтожает все дочерние объекты на canvas
    void RemoveChildren()
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
		}
	}

	// Запускает историю из файла с текстом inky
	void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);
		// переводит камеру на говорящего нпс
		StartCoroutine(RefreshView());
	}

	// Основной метод, формирует и выводит на экран текст и кнопки с вариантами выбора
	IEnumerator RefreshView()
	{
		RemoveChildren();

		// Выводит весь текст поочерёдно, пока он есть
		while (story.canContinue)
		{
			// пока проигрывается голос не продолжать код.
			while (_audioSource.isPlaying)
				yield return null;
			// Continue получает новую строку текста
			string text = story.Continue();
			// Убирает пробелы в тексте
			text = text.Trim();
			// Отображает текст на экране
			CreateContentView(text);

			// Проигрываение голоса
			TalkClip();
			// Переключение камер по тегам в тексте
			//TalkCamera();
		}

		// Отображает все варианты выбора, если они есть
		if (story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button choice_button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				choice_button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
			}
		}
		// Если текст кончился, то по кнопке вызывает функцию EndStory
		else
		{
			Button choice = CreateChoiceView("End of story");
			choice.onClick.AddListener(delegate {
				EndStory();
			});
		}
	}

    // Формирует кнопку с выбором
    void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		StartCoroutine(RefreshView());
	}

	// Вставляет текст из строки в текстовый префаб и рисует его на canvas
	void CreateContentView(string text)
	{
		Text storyText = Instantiate(textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent(canvas.transform, false);
	}

	// Формирует кнопку с выбором и текстом выбора
	Button CreateChoiceView(string text)
	{
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(canvas.transform, false);

		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text>();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Проигрывает аудиофайл с голосом
	private void PlayClip(string clipName)
	{
		if (_clips.TryGetValue(clipName.ToLower(), out var clip))
		{
			_audioSource.PlayOneShot(clip);
		}
	}

	// Проигрывает аудиофайл с голосом по тегу из Inky
	void TalkClip()
	{
		foreach (var tag in story.currentTags)
		{
			if (tag.StartsWith("Talk."))
			{
				var clipName = tag.Substring("Talk.".Length, tag.Length - "Talk.".Length);
				PlayClip(clipName);
			}
		}
	}

	// Возвращает приоритет камеры, выключает курсор и выключает canvas с диалогом нпс.
	void EndStory()
    {
		RemoveChildren();
		_canvas.SetActive(false);
	}
}
