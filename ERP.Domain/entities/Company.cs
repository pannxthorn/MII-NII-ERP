using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.entities
{
    [Table("Company")]
    public class Company
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(@"CompanyId", Order = 1, TypeName = BaseConst.INT)]
        [Required]
        [Key]
        public int CompanyId { get; set; } // CompanyId (Primary key)

        [Column(@"CompanyCode", Order = 2, TypeName = BaseConst.VARCHAR_50)]
        [Required]
        [MaxLength(50)]
        public required string CompanyCode { get; set; } // CompanyCode (length: 50)

        [Column(@"CompanyName", Order = 3, TypeName = BaseConst.VARCHAR_300)]
        [Required]
        [MaxLength(300)]
        public required string CompanyName { get; set; } // CompanyName (length: 300)

        [Column(@"TaxNo", Order = 4, TypeName = BaseConst.VARCHAR_13)]
        [MaxLength(13)]
        public string? TaxNo { get; set; } // TaxNo (length: 13)

        [Column(@"Phone", Order = 5, TypeName = BaseConst.VARCHAR_100)]
        [MaxLength(100)]
        public string? Phone { get; set; } // Phone (length: 100)

        [Column(@"Fax", Order = 6, TypeName = BaseConst.VARCHAR_100)]
        [MaxLength(100)]
        public string? Fax { get; set; } // Fax (length: 100)

        [Column(@"Email", Order = 7, TypeName = BaseConst.VARCHAR_100)]
        [MaxLength(100)]
        public string? Email { get; set; } // Email (length: 100)

        [Column(@"Line", Order = 8, TypeName = BaseConst.VARCHAR_100)]
        [MaxLength(100)]
        public string? Line { get; set; } // Line (length: 100)

        [Column(@"Facebook", Order = 9, TypeName = BaseConst.VARCHAR_100)]
        [MaxLength(100)]
        public string? Facebook { get; set; } // Facebook (length: 100)

        [Column(@"Website", Order = 10, TypeName = BaseConst.VARCHAR_300)]
        [MaxLength(300)]
        public string? Website { get; set; } // Website (length: 300)

        [Column(@"Logo", Order = 11, TypeName = BaseConst.VARCHAR_300)]
        [MaxLength(300)]
        public string? Logo { get; set; } // Logo (length: 300)

        [Column(@"Comment", Order = 12, TypeName = BaseConst.VARCHAR_4000)]
        [MaxLength(4000)]
        public string? Comment { get; set; } // Comment (length: 4000)

        [Column(@"Created_By_Id", Order = 13, TypeName = BaseConst.INT)]
        [Required]
        public int Created_By_Id { get; set; } // Created_By_Id

        [Column(@"Creation_Date", Order = 14, TypeName = BaseConst.DATETIME)]
        [Required]
        public System.DateTime Creation_Date { get; set; } // Creation_Date

        [Column(@"Last_Update_By_Id", Order = 15, TypeName = BaseConst.INT)]
        [Required]
        public int Last_Update_By_Id { get; set; } // Last_Update_By_Id

        [Column(@"Last_Update_By_Date", Order = 16, TypeName = BaseConst.DATETIME)]
        [Required]
        public System.DateTime Last_Update_By_Date { get; set; } // Last_Update_By_Date

        [Column(@"IsActive", Order = 17, TypeName = BaseConst.BIT)]
        [Required]
        public bool IsActive { get; set; } // IsActive

        [Column(@"IsDelete", Order = 18, TypeName = BaseConst.BIT)]
        [Required]
        public bool IsDelete { get; set; } // IsDelete

        public virtual ICollection<Branch> Branches {  get; set; }

        public Company()
        {
            Branches = new List<Branch>();
        }
    }
}
