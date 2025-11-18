# Cogwork Waltz

A Hollow Knight: Silksong mod which replaces the music in the Cogwork Dancers boss fight with [Cogwork Waltz by Joe Zeng](https://youtu.be/ZbU_AZupH_w), a rearrangement that puts the song in 3/8 time. Made with the author's permission.

## Installation

Install via r2modman or Thunderstore Mod Manager. Cogwork Waltz' dependencies should be installed automatically.

For manual installation, extract the `.zip` file and place the resulting folder in the `BepInEx/plugins` directory for your Silksong install. You'll also have to manually install [WavLib](https://thunderstore.io/c/hollow-knight-silksong/p/SFGrenade/WavLib/).

## Notes on the timing

The rearrangement was made to be loopable and to (mostly) fit the tempo of the original music - which is very cool. However, there are some shenanigans that I wanted to write down for posterity.

Phase 4, musically, is easy: Joe Zeng noted that it's the only phase in the rearrangement that's not synced with the original. The original game has boss movements in that pase at a speed of 1.25 seconds per measure, and the rearrangement, 1.5 seconds per measure; so I just sped that phase up a little bit to match.

Doing that sent me down a rabbit hole of gameplay and musical timing. Gameplay wise, it turns out all the phases are weird - and I'll give special thanks to a musically-inclined friend of mine for describing the nature of the bizareness to me. Please note that I am not a musician and don't fully grasp everything written past this point.

The boss AI aims and moves on a specific pace. In each phase, movements are timed to be 1 second apart, 0.75 seconds, 0.75 seconds, and 1.25 seconds, respectively. This does NOT match with the tempo of the music.

Phase 2 is where this is the messiest and most apparent. It has the music at 75 bpm and the boss movement at 80 bpm. I did not alter the boss AI in any way and the rearrangement follows the bpm of the original music, so since the original desyncs over time in phase 2, so too will this song.

Curiously in the original track the difference in phase 3's music tempo and boss tempo is offset from the beat in, as my friend put it, a rhythmically meaningful way. It's 120 bpm in the music and 80 bpm in boss movements; which does desync the boss from the beat, but at a pace which creates a "natural triplet feel".

I might change the boss timing in a future update out of spite, but for now, that's the state of things.
