using NuGet.Common;

namespace Task_3.Models
{
    public class VM_Vehicles
    {

        public  IEnumerable<Vehicle> Vehicles { get; set; }

        public FileUpload FileUpload { get; set; }
    }
}
