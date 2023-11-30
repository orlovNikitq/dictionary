using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DictionaryApp app = new DictionaryApp();
            app.Run();
        }
    }

    class DictionaryApp
    {
        private Dictionary<string, Dictionary<string, List<string>>> dictionaries;
        string path = @"C:\Users\zunderz\Documents\projc#\classWork\classWork";
        public DictionaryApp()
        {
            dictionaries = new Dictionary<string, Dictionary<string, List<string>>>();
        }
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("1. Создать словарь");
                Console.WriteLine("2. Добавить слово в словарь");
                Console.WriteLine("3. Заменить слово в словаре");
                Console.WriteLine("4. Удалить слово из словаря");
                Console.WriteLine("5. Найти перевод слова");
                Console.WriteLine("6. Экспортировать словарь");
                Console.WriteLine("7. Выйти");

                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        createDictionary();
                        break;
                    case "2":
                        addWord();
                        break;
                    case "3":
                        ReplaceWord();
                        break;
                    case "4":
                        deleteWord();
                        break;
                    case "5":
                        SearchTranslation();
                        break;
                    case "6":
                        ExportDictionary();
                        break;
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите существующий пункт меню.");
                        break;
                }
            }
        }
        private void createDictionary()
        {
            Console.Write("Введите название словаря(тип): ");
            string name = Console.ReadLine();

            if (!dictionaries.ContainsKey(name))
            {
                dictionaries[name] = new Dictionary<string, List<string>>();
                File.Create(Path.Combine(path, $"{name}.txt")).Close();
                Console.WriteLine($"Словарь '{name}' успешно создан.");
            }
            else
            {
                Console.WriteLine($"Словарь '{name}' уже существует.");
            }
        }
        private void addWord()
        {
            Console.Write("Введите название словаря: ");
            string dictionaryName = Console.ReadLine();

            if (dictionaries.ContainsKey(dictionaryName))
            {
                Console.Write("Введите слово для добавления: ");
                string word = Console.ReadLine();
                Console.Write("Введите перевод(ы) через запятую: ");
                string translationsInput = Console.ReadLine();
                List<string> translations = new List<string>(translationsInput.Split(','));

                if (!dictionaries[dictionaryName].ContainsKey(word))
                {
                    dictionaries[dictionaryName][word] = translations;
                    AppendToFile(dictionaryName, $"{word}: {string.Join(", ", translations)}");

                    Console.WriteLine($"Слово '{word}' успешно добавлено в словарь '{dictionaryName}'.");
                }
                else Console.WriteLine($"Слово '{word}' уже существует в словаре '{dictionaryName}'.");
            }
            else Console.WriteLine($"Словарь '{dictionaryName}' не существует.");
        }
        private void AppendToFile(string fileName, string content)
        {
            string filePath = Path.Combine(@"C:\Users\zunderz\Documents\projc#\classWork\classWork", $"{fileName}.txt");
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(content);
            }
        }
        public void deleteWord()
        {
            Console.Write("Введите название словаря: ");
            string dictionaryName = Console.ReadLine();

            if (dictionaries.ContainsKey(dictionaryName))
            {
                Console.Write("Введите слово для удаления: ");
                string wordToDelete = Console.ReadLine();

                if (dictionaries[dictionaryName].ContainsKey(wordToDelete))
                {
                    dictionaries[dictionaryName].Remove(wordToDelete);
                    Console.WriteLine($"Слово '{wordToDelete}' успешно удалено из словаря '{dictionaryName}'.");
                    string filePath = Path.Combine(path, $"{dictionaryName}.txt");
                    File.WriteAllText(filePath, "");

                    foreach (var entry in dictionaries[dictionaryName])
                    {
                        AppendToFile(dictionaryName, $"{entry.Key}: {string.Join(", ", entry.Value)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Слово '{wordToDelete}' не найдено в словаре '{dictionaryName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Словарь '{dictionaryName}' не существует.");
            }
        }
        private void SearchTranslation()
        {
            Console.Write("Введите название словаря: ");
            string dictionaryName = Console.ReadLine();

            if (dictionaries.ContainsKey(dictionaryName))
            {
                Console.Write("Введите слово для поиска перевода: ");
                string wordToSearch = Console.ReadLine();

                if (dictionaries[dictionaryName].ContainsKey(wordToSearch))
                {
                    Console.WriteLine($"Перевод(ы) для слова '{wordToSearch}': {string.Join(", ", dictionaries[dictionaryName][wordToSearch])}");
                }
                else
                {
                    Console.WriteLine($"Слово '{wordToSearch}' не найдено в словаре '{dictionaryName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Словарь '{dictionaryName}' не существует.");
            }
        }
        private void ExportDictionary()
        {
            Console.Write("Введите путь к директории для экспорта словаря: ");
            string exportDirectory = Console.ReadLine();

            try
            {
                foreach (var dictionaryEntry in dictionaries)
                {
                    string exportFilePath = Path.Combine(exportDirectory, $"export_{dictionaryEntry.Key}.txt");

                    using (StreamWriter sw = new StreamWriter(exportFilePath))
                    {
                        sw.WriteLine($"Словарь: {dictionaryEntry.Key}");

                        foreach (var wordEntry in dictionaryEntry.Value)
                        {
                            sw.WriteLine($"{wordEntry.Key}: {string.Join(", ", wordEntry.Value)}");
                        }

                        Console.WriteLine($"Словарь '{dictionaryEntry.Key}' успешно экспортирован в файл: {exportFilePath}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при экспорте словаря: {e.Message}");
            }
        }
        private void ReplaceWord()
        {
            Console.Write("Введите название словаря: ");
            string dictionaryName = Console.ReadLine();

            if (dictionaries.ContainsKey(dictionaryName))
            {
                Console.Write("Введите слово для замены: ");
                string wordToReplace = Console.ReadLine();

                if (dictionaries[dictionaryName].ContainsKey(wordToReplace))
                {
                    Console.Write("Введите новый перевод(ы) через запятую: ");
                    string newTranslationsInput = Console.ReadLine();
                    List<string> newTranslations = new List<string>(newTranslationsInput.Split(','));

                    dictionaries[dictionaryName][wordToReplace] = newTranslations;

                    UpdateFileContent(dictionaryName);

                    Console.WriteLine($"Слово '{wordToReplace}' успешно заменено в словаре '{dictionaryName}'.");
                }
                else
                {
                    Console.WriteLine($"Слово '{wordToReplace}' не найдено в словаре '{dictionaryName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Словарь '{dictionaryName}' не существует.");
            }
        }
        private void UpdateFileContent(string dictionaryName)
        {
            string filePath = Path.Combine(path, $"{dictionaryName}.txt");
            File.WriteAllText(filePath, "");

            foreach (var entry in dictionaries[dictionaryName])
            {
                AppendToFile(dictionaryName, $"{entry.Key}: {string.Join(", ", entry.Value)}");
            }
        }


    }
}
