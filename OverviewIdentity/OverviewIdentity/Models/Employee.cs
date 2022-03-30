using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OverviewIdentity.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Nome Completo")]
        public string? FullName { get; set; }

        [Column(TypeName = "varchar(10)")]
        [DisplayName("Nome da Empresa")]
        public string? NameCompany { get; set; }

        [Column(TypeName = "varchar(100)")]
        [DisplayName("Cargo")]
        public string? Position { get; set; }

        [Column(TypeName = "varchar(100)")]
        [DisplayName("Localização da Empresa")]
        public string? OfficeLocation { get; set; }
    }
}
