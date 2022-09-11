using System.ComponentModel.DataAnnotations;

namespace Task_3.Models
{
    public class Vehicle
    {
        [Key]
        public string Reg { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Colour { get; set; }

        public string Fuel { get; set; }

        public string Message { get; set; }

        public bool Error { get; set; }
    }
}
