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
        public string op;
    }

    public class Functions
    {
        public static List<FuncID> get_id_list()
        {
            List<FuncID> ids = new List<FuncID>();

            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.mflr, op = "mflr r{0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.addi, op = "addi r{0}, r{1}, 0x{2}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.beq, op = "beq {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bne, op = "bne cr{0}, {1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bl, op = "bl {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.cmpwi, op = "cmpwi r{0}, {1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.li, op = "li r{0}, {1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.lis, op = "lis r{1} {1}@{2}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.lwz, op = "lwz r{0}, {1}@{2}(r{3})" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.nop, op = "nop" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.stw, op = "stw r{0} {1}@{2}(r{3})" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.unknown, op = "Unknown OP" });

            return ids;
        }
        public static FuncID find_func(UInt32 op_code)
        {
            List<FuncID> ids = get_id_list();

            foreach(FuncID id in ids)
            {
                if((UInt32)(op_code & id.id) == id.id)
                {
                    switch(id.id)
                    {
                        case (UInt32)op_codes.mflr:

                            break;
                    }

                    return id;
                }
            }
            return new FuncID();
        }

        public static op_codes op_code_id(UInt32 opcode)
        {
            List<FuncID> funcs = get_id_list();
            foreach(FuncID func in funcs)
            {
                if((opcode & func.id) == func.id)
                {
                    return (op_codes)func.id;
                }
            }
            return op_codes.unknown;
        }
    }
}
