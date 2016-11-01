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
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.mr, op = "mr %r{0}, %r{1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.mflr, op = "mflr %r{0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.mtlr, op = "mtlr %r{0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.li, op = "li %r{0}, {1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.addi, op = "addi %r{0}, %r{1}, 0x{2}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.beq, op = "beq 0x{0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bne, op = "bne cr{0}, 0x{1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bl, op = "bl {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.cmpwi, op = "cmpwi %r{0}, {1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.extsb, op = "extsb %r{0}, %r{1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.extsb_, op = "extsb. %r{0}, %r{1}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.lis, op = "lis %r{1} {1}@{2}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.lwz, op = "lwz %r{0}, {1}(%r{2})" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.b, op = "b {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.ba, op = "ba {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bla, op = "bla {0}" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.b, op = "b" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bf, op = "bf" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.bfl, op = "bfl" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.ba, op = "ba" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.stw, op = "stw %r{0} {1}@{2}(r{3})" });
            ids.Add(new PowerPC.FuncID() { id = (UInt32)op_codes.nop, op = "nop" });
            
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
                    byte gpr = 0;
                    switch(id.id)
                    {
                        // TODO, figure out the masks for seperating the variables.

                        // Sort out Branchees out, get raw address.
                        case (UInt32)op_codes.bl:
                        case (UInt32)op_codes.b:
                        case (UInt32)op_codes.ba:
                        case (UInt32)op_codes.bla:
                            Int32 addr = (Int32)(op_code & 0x3FFFFFC);
                            id.op = string.Format(id.op, "0x" + addr.ToString("X8"));
                            break;

                        // Sort out Move From/to link register.
                        case (UInt32)op_codes.mflr:
                        case (UInt32)op_codes.mtlr:
                            gpr = (byte)((op_code &0x1f));
                            id.op = string.Format(id.op, gpr.ToString());
                            break;

                        // Sort out Load Immediate
                        case (UInt32)op_codes.li:
                            gpr = (byte)((op_code & 0x1f));
                            UInt16 val = (UInt16)((op_code & 0xFFFF0));
                            id.op = string.Format(id.op, gpr.ToString(), val.ToString());
                            break;
                        case (UInt32)op_codes.lwz:
                            byte D = (byte)((op_code & 0x1f));
                            byte a = (byte)((op_code & 0xF8000));
                            UInt16 d = (UInt16)((op_code & 0xFFFF));
                            id.op = string.Format(id.op, D.ToString(), d.ToString(), a.ToString());
                            break;
                        case (UInt32)op_codes.mr:
                            d = (byte)((op_code & 0x3FC0000));
                            a = (byte)(((op_code) << 8) & 0x3FC0000);

                            id.op = string.Format(id.op, d.ToString(), a.ToString());
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
