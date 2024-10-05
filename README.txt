PNGTuber Template
=================

This should help to make your own PNGTuber program.

What happend here?

1. have an Audiobus that uses `Record` and `SpectrumAnalyzer` record to get microphone access and spectrumanalyzer to get the high tones that we use for "blinking"
2. change project settings to get transparent backgrounds for easier use in OBS. (`window/size/transparent` and `window/per_pixel_transparency/allowed`)
3. also setup the project that it can record the microphone (`driver/enable_input`)
4. take the volume of the input and look for the higher pitch sound. When the threshold is reached change to open mouth. You will need to adjust to your input settings.
   Also the high pitched sound is the way for determining blinking. You could make it also that its randomly blinking from time to time.

At this point you can decide what you want to do with it. Implement buttons to have different face expressions. Use a fancy text analyzing library to determine your emotions.
Or simply have fun with experimenting.

When you looking to integrate your PNGTuber Software into Twitch checkout https://github.com/kanimaru/twitcher an "easy" way to integrate twitch into your Godot application.

Provided by kani_dev | Just join and ask me if you have some problems or need help with Godot https://www.twitch.tv/kani_dev
