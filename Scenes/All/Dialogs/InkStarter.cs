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
	// ���� � ������� inky
	[SerializeField] private TextAsset inkJSONAsset = null;
	public Story story;
	public static event Action<Story> OnCreateStory;
	// canvas, �� ������� ������
	[SerializeField] private Canvas canvas = null;
	[SerializeField] private GameObject _canvas;

	// ������� UI ������ � ������
	[SerializeField] private Text textPrefab = null;
	[SerializeField] private Button buttonPrefab = null;
	// ������ �����������, ������� ����� �����������
	[SerializeField] private List<AudioClip> _audioClips;
	private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
	// �� ������� ������ ������ AudioSource
    private AudioSource _audioSource;

    void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		InitializeAudioClips();
	}

    private void OnEnable()
    {
		Cursor.lockState = CursorLockMode.Confined;
		// ������� UI �� �������� ��������
		RemoveChildren();
		StartStory();
	}

    // ������������ �������� ����������� � ��������� �������
    private void InitializeAudioClips()
    {
        foreach (var clip in _audioClips)
        {
			_clips.Add(clip.name.ToLower().Replace(" ", ""), clip);
        }
    }

    // ���������� ��� �������� ������� �� canvas
    void RemoveChildren()
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
		}
	}

	// ��������� ������� �� ����� � ������� inky
	void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);
		// ��������� ������ �� ���������� ���
		StartCoroutine(RefreshView());
	}

	// �������� �����, ��������� � ������� �� ����� ����� � ������ � ���������� ������
	IEnumerator RefreshView()
	{
		RemoveChildren();

		// ������� ���� ����� ���������, ���� �� ����
		while (story.canContinue)
		{
			// ���� ������������� ����� �� ���������� ���.
			while (_audioSource.isPlaying)
				yield return null;
			// Continue �������� ����� ������ ������
			string text = story.Continue();
			// ������� ������� � ������
			text = text.Trim();
			// ���������� ����� �� ������
			CreateContentView(text);

			// ������������� ������
			TalkClip();
			// ������������ ����� �� ����� � ������
			//TalkCamera();
		}

		// ���������� ��� �������� ������, ���� ��� ����
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
		// ���� ����� ��������, �� �� ������ �������� ������� EndStory
		else
		{
			Button choice = CreateChoiceView("End of story");
			choice.onClick.AddListener(delegate {
				EndStory();
			});
		}
	}

    // ��������� ������ � �������
    void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		StartCoroutine(RefreshView());
	}

	// ��������� ����� �� ������ � ��������� ������ � ������ ��� �� canvas
	void CreateContentView(string text)
	{
		Text storyText = Instantiate(textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent(canvas.transform, false);
	}

	// ��������� ������ � ������� � ������� ������
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

	// ����������� ��������� � �������
	private void PlayClip(string clipName)
	{
		if (_clips.TryGetValue(clipName.ToLower(), out var clip))
		{
			_audioSource.PlayOneShot(clip);
		}
	}

	// ����������� ��������� � ������� �� ���� �� Inky
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

	// ���������� ��������� ������, ��������� ������ � ��������� canvas � �������� ���.
	void EndStory()
    {
		RemoveChildren();
		_canvas.SetActive(false);
	}
}
