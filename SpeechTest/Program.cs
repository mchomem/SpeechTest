using NAudio.Wave;
using SpeechTest.Models;
using System.Runtime.Versioning;
using System.Speech.Synthesis;
using Vosk;

namespace SampleSynthesis;

[SupportedOSPlatform("windows")]
public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Microsoft System.Speech Test";
        StartPresentation();
        ShowMenu();
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
                string textMessage = "Olá! Bem vindo ao aplicativo de teste de fala. Em seguida, vamos iniciar o sistema com o menu contendo algumas opções de teste. Selecione uma opção para testar a aplicação.";
                Console.WriteLine(textMessage);
                Speak(TypeVoice.VoiceMaria, textMessage);
                break;
            }
            else if (optionPresentation == "2")
            {
                string textMessage = "Hello! Welcome to the speech testing app. Next, we'll launch the system with a menu containing some testing options. Select an option to test the application.";
                Console.WriteLine(textMessage);
                Speak(TypeVoice.VoiceZira, textMessage);
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

    static void Speak(string voiceType, string textMessage)
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
            Console.WriteLine("4 - Speach a command");
            Console.WriteLine("5 - Relaunch presentation");
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
                ListeningHumanVoiceVosks();
            }
            else if (option == "5")
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
        Speak(TypeVoice.VoiceMaria, message);
        SetBreakMessage("\nPress any key to continue.");
    }

    static void SalutationVoiceEN()
    {
        Console.Clear();

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
        Speak(TypeVoice.VoiceZira, message);
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
