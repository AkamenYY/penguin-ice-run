using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// ЗВУК ВСЕЙ ИГРЫ. Один объект на всю игру (DontDestroyOnLoad): музыка не перезапускается при смене сцены.
// Музыка: два AudioSource → плавный переход (crossfade) между треком меню и треком уровня.
// Звуки: AudioManager.Play(clip) — со случайной высотой тона, чтобы повторы не звучали «роботом».
// Всё идёт через AudioMixer: Master → Music (→ Tracks) и SFX. Громкость — в децибелах, хранится в save.json.
public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    public AudioMixer mixer;            // GameMixer: группы Master, Music, SFX; снапшоты Normal и Paused
    public AudioSource musicA, musicB;  // два источника музыки (выход — группа Tracks внутри Music)
    public AudioSource[] sfx;           // несколько источников для звуков (выход — группа SFX)
    public AudioClip sceneMusic;        // какую музыку хочет ЭТА сцена (меню или уровень)
    public AudioClip clickClip;         // звук нажатия кнопки
    public float fadeTime;              // длина crossfade, сек
    public float minPitch, maxPitch;    // разброс высоты тона для повторяющихся звуков

    AudioSource current;   // источник, который сейчас играет музыку
    int next;              // какой sfx-источник взять следующим

    void Awake()
    {
        // в новой сцене уже есть AudioManager с прошлой сцены → передаём ему свою музыку и исчезаем
        if (I != null && I != this)
        {
            I.PlayMusic(sceneMusic);
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
        current = musicA;
    }

    void Start()
    {
        ApplyVolumes();          // громкость из save.json (SetFloat в Start — в Awake микшер может не принять)
        PlayMusic(sceneMusic);
    }

    // ---------- музыка ----------

    public void PlayMusic(AudioClip clip)
    {
        if (current.clip == clip && current.isPlaying) return;   // тот же трек уже играет — не перезапускаем

        AudioSource from = current;
        current = (current == musicA) ? musicB : musicA;
        current.clip = clip;
        current.volume = 0f;
        current.Play();

        StopAllCoroutines();
        StartCoroutine(Crossfade(from, current));
    }

    // старый трек тихо уходит, новый тихо приходит. unscaledDeltaTime — работает и на паузе (timeScale = 0)
    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            to.volume = t / fadeTime;
            from.volume = 1f - t / fadeTime;
            yield return null;
        }
        to.volume = 1f;
        from.volume = 0f;
        from.Stop();
    }

    // ---------- звуки ----------

    // vary = true — случайная высота тона (шаги, прыжки, удары); false — ровно (победа, проигрыш, кнопки)
    public static void Play(AudioClip clip, bool vary = true)
    {
        if (I == null || clip == null) return;
        AudioSource s = I.sfx[I.next];
        I.next = (I.next + 1) % I.sfx.Length;   // по кругу — звуки не обрывают друг друга
        s.pitch = vary ? Random.Range(I.minPitch, I.maxPitch) : 1f;
        s.PlayOneShot(clip);
    }

    public static void Click()
    {
        if (I != null) Play(I.clickClip, false);
    }

    // ---------- громкость и пауза ----------

    // слайдер 0..1 → децибелы: Log10(v) × 20. 1 → 0 dB, 0.5 → −6 dB, 0.0001 → −80 dB (тишина)
    public static float ToDecibels(float v)
    {
        return Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f;
    }

    public void ApplyVolumes()
    {
        mixer.SetFloat("MusicVol", ToDecibels(SaveSystem.Data.musicVolume));
        mixer.SetFloat("SFXVol", ToDecibels(SaveSystem.Data.sfxVolume));
    }

    // пауза: снапшот Paused — музыка тише и глуше (lowpass); обратно — снапшот Normal
    public void SetPaused(bool paused)
    {
        mixer.FindSnapshot(paused ? "Paused" : "Normal").TransitionTo(0.3f);
    }
}
