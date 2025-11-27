using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain.Entities
{
    public class Salesman
    {
        public Guid SalesmanID { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Phone { get; set; }
        public int Salary { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Address { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Region { get; set; }
    }
}
