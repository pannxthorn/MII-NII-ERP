using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Shared._base.BaseMessage
{
    public class BaseMessage
    {
        public const string dataSuccess = "สำเร็จ";
        public const string existsMsg = "{0} มีอยู่ในระบบแล้ว";
        public const string existsUserMsg = "ชื่อผู้ใช้งาน {0} มีอยู่ในระบบแล้ว";

        public const string requireBranchMsg = "กรุณาระบุสาขาอย่างน้อย 1 สาขา";
    }
}
