using System.Linq;
using UnityEngine;

namespace Cogwork_Waltz.Components;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayMakerFSM))]
internal class ResyncCogworkDancers : MonoBehaviour {

	private AudioSource music;
	private PlayMakerFSM dancerControl, beatControl;
	private int mult;
	private float prevMusicTime;
	private bool justLooped;

	private void Awake() {
		music = GetComponent<AudioSource>();
		dancerControl = GetComponents<PlayMakerFSM>().First(x => x.FsmName == "Control");
		beatControl = GetComponents<PlayMakerFSM>().First(x => x.FsmName == "Beat Control");
		mult = 1;
		prevMusicTime = -1;
		justLooped = false;
	}

	private void Update() {
		// Let the vanilla metronome FSM take over in between phases
		if (!music.clip || !music.loop || (!music.isPlaying && !HeroController.instance.IsPaused())) {
			if (!beatControl.enabled) {
				mult = 1;
				prevMusicTime = -1;
				justLooped = false;
				beatControl.enabled = true;
				beatControl.SendEvent("STOP");
				beatControl.SendEvent("BEGIN");
			}
			return;
		}

		// Otherwise, sync the dancers with the music instead of using an unreliable metronome

		beatControl.enabled = false;

		float beat = CurrentBeatTiming;

		if (prevMusicTime > music.time) {
			justLooped = true;
		}
		prevMusicTime = music.time;

		if (mult * beat <= music.time) {
			mult++;
			SendBeatEvent();
		}
		else if (justLooped && ((mult * beat) - music.clip.length) <= music.time) {
			justLooped = false;
			mult = 1;
			SendBeatEvent();
		}
	}

	private float CurrentBeatTiming => beatControl.FsmVariables.FloatVariables[0].Value;

	private void SendBeatEvent() {
		dancerControl.SendEvent("BEAT");
		EventRegister.SendEvent("BEAT", gameObject);
	}
}
