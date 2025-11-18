using BepInEx;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using WavLib;

namespace CogworkWaltz;

[BepInAutoPlugin(id: "io.github.kaycodes13.cogworkwaltz")]
public partial class CogworkWaltzPlugin : BaseUnityPlugin {

	private static List<AudioClip> Phases = [];

	private void Awake()
	{
		Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");

		Assembly asm = Assembly.GetExecutingAssembly();
		string[] resources = asm.GetManifestResourceNames();

		Phases.Add(LoadEmbeddedWav(resources.First(n => n.EndsWith("sting.wav")), asm, "sting"));

		for (int i = 1; i <= 4; i++) {
			Phases.Add(LoadEmbeddedWav(resources.First(n => n.EndsWith($"phase_{i}.wav")), asm, $"phase_{i}"));
		}

		SceneManager.sceneLoaded += ReplaceMusic;
	}

	private void OnDestroy() {
		SceneManager.sceneLoaded -= ReplaceMusic;
		Phases.Clear();
	}

	private void ReplaceMusic(Scene scene, LoadSceneMode _) {
		if (scene.name != "Cog_Dancers_boss")
			return;

		PlayMakerFSM
			fsm = GameObject.Find("Dancer Control")
				.GetComponents<PlayMakerFSM>()
				.First(fsm => fsm.FsmName == "Music Control");

		foreach(FsmState state in fsm.FsmStates) {
			if (state.Name == "Stop") {
				state.Actions.OfType<AudioPlaySimple>().First()
					.oneShotClip = Phases[0];
				continue;
			}
			for (int i = 1; i <= 4; i++) {
				if (state.Name == $"P{i} Music") {
					state.Actions.OfType<SetAudioClip>().First()
						.audioClip = Phases[i];
					break;
				}
			}
		}
	}

	private static AudioClip LoadEmbeddedWav(string path, Assembly asm, string name) {
		WavData wavData = new();
		using (Stream stream = asm.GetManifestResourceStream(path)) {
			wavData.Parse(stream);
		}
		float[] wavSoundData = wavData.GetSamples();

		AudioClip audioClip = AudioClip.Create(
			name,
			wavSoundData.Length / wavData.FormatChunk.NumChannels,
			wavData.FormatChunk.NumChannels,
			(int)wavData.FormatChunk.SampleRate,
			false
		);
		audioClip.SetData(wavSoundData, 0);

		return audioClip;
	}

}
