using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace RealEstateScrapeConsole
{
    public class PropertyModels
    {
        [Key]
        public int Id { get; set; }

        public string County { get; set; }    

        public string DateListed { get; set; }

        public string Url { get; set; }

        public string Address { get; set; }

        public int Price { get; set; }

        public int SquareFeet  { get; set; }

        public int LotSize { get; set; }

        public string Description { get; set; }

    }
}
