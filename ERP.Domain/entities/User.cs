using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.entities
{
    [Table("User")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(@"UserId", Order = 1, TypeName = BaseConst.INT)]
        [Required]
        [Key]
        public int UserId { get; set; } // UserId (Primary key)

        [Column(@"CompanyId", Order = 2, TypeName = BaseConst.INT)]
        public int? CompanyId { get; set; } // CompanyId

        [Column(@"BranchId", Order = 3, TypeName = BaseConst.INT)]
        public int? BranchId { get; set; } // BranchId

        [Column(@"UserName", Order = 4, TypeName = BaseConst.VARCHAR_100)]
        [Required]
        [MaxLength(100)]
        public required string UserName { get; set; } // UserName (length: 100)

        [Column(@"Password", Order = 5, TypeName = BaseConst.VARCHAR_500)]
        [Required]
        [MaxLength(500)]
        public required string Password { get; set; } // Password (length: 500)

        [Column(@"Comment", Order = 6, TypeName = BaseConst.VARCHAR_4000)]
        [MaxLength(4000)]
        public string? Comment { get; set; } // Comment (length: 4000)

        [Column(@"Created_By_Id", Order = 7, TypeName = BaseConst.INT)]
        [Required]
        public int Created_By_Id { get; set; } // Created_By_Id

        [Column(@"Creation_Date", Order = 8, TypeName = BaseConst.DATETIME)]
        [Required]
        public System.DateTime Creation_Date { get; set; } // Creation_Date

        [Column(@"Last_Update_By_Id", Order = 9, TypeName = BaseConst.INT)]
        [Required]
        public int Last_Update_By_Id { get; set; } // Last_Update_By_Id

        [Column(@"Last_Update_By_Date", Order = 10, TypeName = BaseConst.DATETIME)]
        [Required]
        public System.DateTime Last_Update_By_Date { get; set; } // Last_Update_By_Date

        [Column(@"IsActive", Order = 11, TypeName = BaseConst.BIT)]
        [Required]
        public bool IsActive { get; set; } // IsActive

        [Column(@"IsDelete", Order = 12, TypeName = BaseConst.BIT)]
        [Required]
        public bool IsDelete { get; set; } // IsDelete

        public virtual Company? Company { get; set; }
    }
}
