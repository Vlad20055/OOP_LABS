

using System.ComponentModel.DataAnnotations;

namespace Lab1.Presentation
{
    internal interface IUiInstance
    {
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public void Display();
    }
}
