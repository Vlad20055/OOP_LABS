

using Lab1.Domain.Users;

namespace Lab1.Presentation
{
    internal class Menu : IUiInstance
    {
        public bool IsRunning {get; set;} = false;
        public string Name { get; set; } = string.Empty;
        public List<(string Label, IUiInstance)> Options = new List<(string, IUiInstance)>();
        int CurrOption = 0;


        public void Display()
        {
            IsRunning = true;
            Console.CursorVisible = false;

            while (IsRunning)
            {
                ShowOptions();
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        CurrOption = Math.Max(0, CurrOption - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        CurrOption = Math.Min(Options.Count - 1, CurrOption + 1);
                        break;
                    case ConsoleKey.Enter:
                        ExecuteSelectedOption();
                        break;
                    case ConsoleKey.Home:
                        IsRunning = false;
                        break;
                }
            }

            Console.CursorVisible = true;
        }

        private void ExecuteSelectedOption()
        {
            if (Options.Count == 0) return;

            var (_, instance) = Options[CurrOption];

            instance.Display();
        }

        private void ShowOptions()
        {
            Console.Clear();
            Console.WriteLine($"\n=== {Name.ToUpper()} ===");

            for (var i = 0; i < Options.Count; i++)
            {
                var prefix = i == CurrOption ? " ► " : "   ";
                Console.ForegroundColor = i == CurrOption ? ConsoleColor.Cyan : ConsoleColor.Gray;
                Console.WriteLine($"{prefix}{Options[i].Label}");
            }

            Console.ResetColor();
            Console.WriteLine("\n[UP/DOWN] Navigate | [ENTER] Select | [HOME] Exit");
        }
    }
}
