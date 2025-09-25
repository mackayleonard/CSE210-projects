using System;
using System.Collections.Generic;
using System.IO;

namespace JournalProgram
{
    // Represents a single journal entry
    public class Entry
    {
        public string Date { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }

        public Entry(string date, string prompt, string response)
        {
            Date = date;
            Prompt = prompt;
            Response = response;
        }

        public void DisplayEntry()
        {
            Console.WriteLine($"Date: {Date}");
            Console.WriteLine($"Prompt: {Prompt}");
            Console.WriteLine($"Response: {Response}");
            Console.WriteLine("-------------------------------------");
        }

        // Format entry for saving
        public string ToFileString()
        {
            return $"{Date}|{Prompt}|{Response}";
        }

        // Create entry from saved line
        public static Entry FromFileString(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 3)
            {
                return new Entry(parts[0], parts[1], parts[2]);
            }
            return null;
        }
    }

    // Holds a collection of journal entries
    public class Journal
    {
        private List<Entry> _entries = new List<Entry>();

        public void AddEntry(Entry entry)
        {
            _entries.Add(entry);
        }

        public void DisplayAll()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("No journal entries yet.\n");
                return;
            }

            foreach (Entry e in _entries)
            {
                e.DisplayEntry();
            }
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Entry e in _entries)
                {
                    writer.WriteLine(e.ToFileString());
                }
            }
            Console.WriteLine($"Journal saved to {filename}\n");
        }

        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.\n");
                return;
            }

            _entries.Clear();
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                Entry entry = Entry.FromFileString(line);
                if (entry != null)
                {
                    _entries.Add(entry);
                }
            }
            Console.WriteLine($"Journal loaded from {filename}\n");
        }
    }

    // Provides random prompts
    public class PromptGenerator
    {
        private List<string> _prompts = new List<string>()
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What is something new I learned today?",
            "What made me smile today?"
        };

        private Random _random = new Random();

        public string GetRandomPrompt()
        {
            int index = _random.Next(_prompts.Count);
            return _prompts[index];
        }
    }

    // Main program with menu
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            PromptGenerator promptGenerator = new PromptGenerator();

            bool running = true;
            while (running)
            {
                Console.WriteLine("Journal Menu:");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display the journal");
                Console.WriteLine("3. Save the journal to a file");
                Console.WriteLine("4. Load the journal from a file");
                Console.WriteLine("5. Quit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        string prompt = promptGenerator.GetRandomPrompt();
                        Console.WriteLine($"\nPrompt: {prompt}");
                        Console.Write("Your response: ");
                        string response = Console.ReadLine();
                        string date = DateTime.Now.ToShortDateString();
                        Entry entry = new Entry(date, prompt, response);
                        journal.AddEntry(entry);
                        Console.WriteLine("Entry added!\n");
                        break;

                    case "2":
                        Console.WriteLine("\nYour Journal Entries:\n");
                        journal.DisplayAll();
                        break;

                    case "3":
                        Console.Write("\nEnter filename to save to: ");
                        string saveFile = Console.ReadLine();
                        journal.SaveToFile(saveFile);
                        break;

                    case "4":
                        Console.Write("\nEnter filename to load from: ");
                        string loadFile = Console.ReadLine();
                        journal.LoadFromFile(loadFile);
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice, try again.\n");
                        break;
                }
            }
        }
    }
}
