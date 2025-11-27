using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kawerk.Domain.Entities
{
    public class Branches
    {
        [Key]
        public Guid BranchID { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Location { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Description { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Warranty { get; set; }
    }
}
