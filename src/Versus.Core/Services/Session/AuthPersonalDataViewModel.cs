
using Microsoft.AspNetCore.Components.Forms;

namespace Versus.Core.Services.Session
{
    public class AuthPersonalDataViewModel
    {
        public string Nickname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public IBrowserFile Photo { get; set; }
        public string City { get; set; } = string.Empty;

    }
}
