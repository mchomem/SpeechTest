# SpeechTest

SpeechTest was created to utilize speech processing and also to test speech recognition.
Initially, attempts were made to use Microsoft.Speech.Recognition and System.Speech.Recognition, but both demonstrated some level of issues due to testing on Windows 11 (since it uses a different speech recognition component).
Vosk and NAudio are now being used for speech recognition.

## Requirements *

For speech recognition it is necessary to use model files that are found on the website [https://alphacephei.com/vosk/models](https://alphacephei.com/vosk/models).

As of 2025-07-23, the model files used for speech recognition are "vosk-model-small-pt-0.3" or "vosk-model-pt-fb-v0.1.1-20220516_2113", the difference between them is the model size.

However, only the small model is functional.

* OBS: They are listed here as requirements but the necessary files are already available in the project, you just need to download it.