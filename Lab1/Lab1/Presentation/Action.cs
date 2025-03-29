

namespace Lab1.Presentation
{
    internal class Action : IUiInstance
    {
        public string Name { get; set; } = string.Empty;

        public bool IsRunning { get; set; } = false;

        public delegate void ActionHandler();

        public required ActionHandler Handler { get; set; }

        public void Display()
        {
            Console.WriteLine($"\n=== {Name.ToUpper()} ==="); 

            IsRunning = true;
            Console.CursorVisible = true;
            Handler.Invoke();
            Console.CursorVisible = false;
            Console.ReadLine();
            IsRunning = false;
        }
    }
}
