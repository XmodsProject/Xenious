using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenonPowerPC.PowerPC
{
    public class FuncID
    {
        public UInt32 id;
        public string op_code;
    }

    public class Functions
    {
        public static List<FuncID> get_id_list()
        {
            List<FuncID> ids = new List<FuncID>();

            ids.Add(new PowerPC.FuncID() { id = 0x38000000, op_code = "addi" });
            ids.Add(new PowerPC.FuncID() { id = 0x38000000, op_code = "li" });
            ids.Add(new PowerPC.FuncID() { id = 0x3c000000, op_code = "addis" });
            ids.Add(new PowerPC.FuncID() { id =  0x3c000000, op_code = "lis" });
            ids.Add(new PowerPC.FuncID() { id =  0x30000000, op_code = "addic" });
            ids.Add(new PowerPC.FuncID() { id =  0x34000000, op_code = "addic." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000214, op_code = "add" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000215, op_code = "add." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000614, op_code = "addo" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000615, op_code = "addo." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000014, op_code = "addc" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000015, op_code = "addc." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000414, op_code = "addco" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000415, op_code = "addco." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000114, op_code = "adde" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000115, op_code = "adde." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000514, op_code = "addeo" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000515, op_code = "addeo." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c0001d4, op_code = "addme" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c0001d5, op_code = "addme." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c0005d4, op_code = "addmeo" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c0005d5, op_code = "addmeo." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000194, op_code = "addze" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000195, op_code = "addze." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000594, op_code = "addzeo" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000595, op_code = "addzeo." });
            ids.Add(new PowerPC.FuncID() { id =  0x70000000, op_code = "andi." });
            ids.Add(new PowerPC.FuncID() { id =  0x74000000, op_code = "andis." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000038, op_code = "and" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000039, op_code = "and." });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000078, op_code = "andc" });
            ids.Add(new PowerPC.FuncID() { id =  0x7c000079, op_code = "andc." });
            ids.Add(new PowerPC.FuncID() { id =  0x48000000, op_code = "b" });
            ids.Add(new PowerPC.FuncID() { id =  0x48000002, op_code = "ba" });
            ids.Add(new PowerPC.FuncID() { id =  0x48000001, op_code = "bl" });
            ids.Add(new PowerPC.FuncID() { id =  0x48000003, op_code = "bla" });
            ids.Add(new PowerPC.FuncID() { id = 0x48000001, op_code = "jbsr" });
            ids.Add(new PowerPC.FuncID() { id =  0x48000000, op_code = "jmp" });
            ids.Add(new PowerPC.FuncID() { id =  0x40000000, op_code = "bc" });
            ids.Add(new PowerPC.FuncID() { id =  0x40000002, op_code = "bca" });
            ids.Add(new PowerPC.FuncID() { id =  0x40000001, op_code = "bcl" });
            ids.Add(new PowerPC.FuncID() { id =  0x40000003, op_code = "bcla" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000420, op_code = "bcctr" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000420, op_code = "bcctr" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000421, op_code = "bcctrl" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000421, op_code = "bcctrl" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000020, op_code = "bclr" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000020, op_code = "bclr" });
            ids.Add(new PowerPC.FuncID() { id =   0x4c000021, op_code = "bclrl" });
            ids.Add(new PowerPC.FuncID() { id =  0x4c000021, op_code = "bclrl" });
            ids.Add(new PowerPC.FuncID() { id = 0x42800000, op_code = "b" });
            ids.Add(new PowerPC.FuncID() { id =  0x42800001, op_code = "bl" });
            ids.Add(new PowerPC.FuncID() { id = 0x4e800020, op_code = "blr" });
            ids.Add(new PowerPC.FuncID() { id = 0x7c0802a6, op_code = "mflr" });

            return ids;
        }
        public static FuncID find_func(UInt32 op_code)
        {
            List<FuncID> ids = get_id_list();

            foreach(FuncID id in ids)
            {
                if((UInt32)(op_code & id.id) == id.id)
                {
                    return id;
                }
            }
            return new FuncID();
        }
    }
}
