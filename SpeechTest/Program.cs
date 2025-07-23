using NAudio.Wave;
using System.Runtime.Versioning;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using Vosk;

namespace SampleSynthesis;

[SupportedOSPlatform("windows")]
public class Program
{
    private static string VoiceBR = "Microsoft Maria Desktop";
    private static string VoiceEN = "Microsoft Zira Desktop";

    static void Main(string[] args)
    {
        Console.Title = "Microsoft System.Speech Test";

        StartPresentation();
        ShowMenu();

        // https://www.nuget.org/packages/System.Speech/7.0.0-rc.1.22426.10
        // https://learn.microsoft.com/pt-br/dotnet/api/system.speech.recognition.recognizedaudio?view=netframework-4.8
        // https://learn.microsoft.com/pt-br/dotnet/api/system.speech.synthesis.speechsynthesizer.speak?view=netframework-4.8#system-speech-synthesis-speechsynthesizer-speak(system-string)
    }

    static void StartPresentation()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("BR or EN presentation?[1, 2 ou 3 para pular]: ");
            string? optionPresentation = Console.ReadLine();
            Console.Clear();

            if (optionPresentation == "1")
            {
                string textMessage = "Olá! Bem vindo ao aplicativo de teste de fala. Em seguida, vamos iniciar o sistema com o menu contendo algumas opções de teste, selecione uma opção para testar a aplicação.";
                Console.WriteLine(textMessage);
                Presentation(VoiceBR, textMessage);
                break;
            }
            else if (optionPresentation == "2")
            {
                string textMessage = "Hello! Welcome to the speech testing app. Next, we'll launch the system with a menu containing some testing options. Select an option to test the application.";
                Console.WriteLine(textMessage);
                Presentation(VoiceEN, textMessage);
                break;
            }
            else if (optionPresentation == "3")
                break;
            else
            {
                Console.WriteLine("Invalid option. Press any button and try again.");
                Console.ReadKey();
            }
        }
    }

    static void Presentation(string voiceType, string textMessage)
    {
        SpeechSynthesizer synth = new SpeechSynthesizer();
        synth.SelectVoice(voiceType);
        synth.SetOutputToDefaultAudioDevice();
        synth.Speak(textMessage);
    }

    static void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a below option:\n");
            Console.WriteLine("1 - Brazilian voice salutation");
            Console.WriteLine("2 - American voice salutation");
            Console.WriteLine("3 - Get installed Windows voices");
            Console.WriteLine("4 - Check installed recognize");
            Console.WriteLine("5 - Speach a command");
            Console.WriteLine("6 - Relaunch presentation");
            Console.WriteLine("0 - Exit\n");
            Console.Write("Option: ");

            string? option = Console.ReadLine();

            if (string.IsNullOrEmpty(option))
            {
                Console.WriteLine("Invalid option. Please try again.");
            }

            if (option == "1")
            {
                SalutationVoiceBR();
            }
            else if (option == "2")
            {
                SalutationVoiceEN();
            }
            else if (option == "3")
            {
                CheckInstalledVoices();
            }
            else if (option == "4")
            {
                CheckInstalledRecognizers();
            }
            else if (option == "5")
            {
                ListeningHumanVoiceVosks();
            }
            else if (option == "6")
            {
                StartPresentation();
            }
            else if (option == "0")
            {
                SetBreakMessage("Press any key to exit.");
                break;
            }
        }
    }

    static void SalutationVoiceBR()
    {
        Console.Clear();

        // Initialize a new instance of the SpeechSynthesizer.  
        SpeechSynthesizer synth = new SpeechSynthesizer();

        synth.SelectVoice("Microsoft Maria Desktop");
        
        //synth.SelectVoice("male");
        //synth.SelectVoiceByHints(VoiceGender.Male);

        // Configure the audio output.   
        synth.SetOutputToDefaultAudioDevice();

        string salutation = string.Empty;

        DateTime now = DateTime.Now;

        if (now.Hour >= 6 && now.Hour <= 12)
            salutation = "Bom dia";
        else if (now.Hour >= 13 && now.Hour <= 18)
            salutation = "Boa tarde";
        else
            salutation = "Boa noite";

        string message = $"{salutation}! Agora são {DateTime.Now.ToString("HH:mm")} do dia {DateTime.Now.ToString("dd/MM/yyyy")}";

        Console.WriteLine(message);
        synth.SpeakAsync(message);

        SetBreakMessage("\nPress any key to continue.");
    }

    static void SalutationVoiceEN()
    {
        Console.Clear();

        // Initialize a new instance of the SpeechSynthesizer.  
        SpeechSynthesizer synth = new SpeechSynthesizer();

        synth.SelectVoice("Microsoft Zira Desktop");

        //synth.SelectVoice("male");
        //synth.SelectVoiceByHints(VoiceGender.Male);

        // Configure the audio output.   
        synth.SetOutputToDefaultAudioDevice();

        string salutation = string.Empty;

        DateTime now = DateTime.Now;

        if (now.Hour >= 6 && now.Hour <= 12)
            salutation = "Good morning";
        else if (now.Hour >= 13 && now.Hour <= 18)
            salutation = "Good afternoon";
        else
            salutation = "Good night";

        string message = $"{salutation}! Now is {DateTime.Now.ToString("HH:mm")} from day {DateTime.Now.ToString("yyyy-MM-dd")}";

        Console.WriteLine(message);
        synth.SpeakAsync(message);

        SetBreakMessage("\nPress any key to continue.");
    }

    static void ListeningHumanVoiceVosks()
    {
        Vosk.Vosk.SetLogLevel(0); // Silencia logs
        var model = new Model("model-small-brazilian"); // pasta com o modelo pt-BR

        using var recognizer = new VoskRecognizer(model, 16000.0f);
        using var waveIn = new WaveInEvent();

        waveIn.DeviceNumber = 0;
        waveIn.WaveFormat = new WaveFormat(16000, 1);
        waveIn.DataAvailable += (s, a) =>
        {
            if (recognizer.AcceptWaveform(a.Buffer, a.BytesRecorded))
            {
                Console.WriteLine(recognizer.Result());
            }
            else
            {
                Console.WriteLine(recognizer.PartialResult());
            }
        };

        Console.WriteLine("Fale algo (pt-BR):");
        waveIn.StartRecording();
        Console.ReadLine();
        waveIn.StopRecording();
    }

    [Obsolete("Don't work in Windows 11")]
    static void ListeningHumanVoice()
    {
        // https://learn.microsoft.com/pt-br/dotnet/api/system.speech.recognition.speechrecognitionengine?view=dotnet-plat-ext-8.0

        // No recognize installed.
        // https://stackoverflow.com/questions/9741053/platformnotsupportedexception-using-net-speech-recognition
        // https://stackoverflow.com/questions/27631339/error-no-recognizer-is-installed-with-recognition-speech
        // https://stackoverflow.com/questions/16856798/speechrecognitionengine-installedrecognizers-returns-no-recognizer-installed

        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

        // Crie uma gramática simples
        Choices choices = new Choices("Olá", "Como vai", "Obrigado", "Tchau");
        GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
        Grammar grammar = new Grammar(grammarBuilder);

        // Carregue a gramática no reconhecedor
        recognizer.LoadGrammar(grammar);

        // Associe um manipulador de eventos para lidar com a detecção de voz
        recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

        // Inicie o reconhecedor
        recognizer.SetInputToDefaultAudioDevice();
        recognizer.RecognizeAsync(RecognizeMode.Multiple);

        // Inicie o reconhecedor
        recognizer.SetInputToDefaultAudioDevice();
        recognizer.RecognizeAsync(RecognizeMode.Multiple);

        Console.WriteLine("Aguardando comandos de voz. Pressione Enter para sair.");
        Console.ReadLine();
    }

    [Obsolete("Don't work in Windows 11")]
    private static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        // Este método é chamado quando uma palavra é reconhecida
        Console.WriteLine($"Palavra reconhecida: {e.Result.Text}");
    }

    [Obsolete("Don't work in Windows 11")]
    static void CheckInstalledRecognizers()
    {
        var recognizerInfoBR = SpeechRecognitionEngine.InstalledRecognizers().FirstOrDefault(r => r.Culture.Name == "pt-BR");
        var recognizerInfoUS = SpeechRecognitionEngine.InstalledRecognizers().FirstOrDefault(r => r.Culture.Name == "en-US");

        Console.WriteLine($"Recognizer culture pt-BR: {(recognizerInfoBR != null ? "installed" : "not installed")}");
        Console.WriteLine($"Recognizer culture en-US: {(recognizerInfoUS != null ? "installed" : "not installed")}");

        foreach (RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
            Console.WriteLine($"Recognizer: {ri.Name}, Culture: {ri.Culture}");

        Console.ReadKey();
    }

    static void CheckInstalledVoices()
    {
        Console.Clear();

        SpeechSynthesizer synth = new SpeechSynthesizer();
        var voices = synth.GetInstalledVoices();

        foreach (var voice in voices)
            Console.WriteLine(voice.VoiceInfo.Name);

        SetBreakMessage("\nPress any key to continue.");
    }

    static void SetBreakMessage(string message = "Press any key to continue.")
    {
        Console.WriteLine(message);
        Console.ReadKey();
    }
}
