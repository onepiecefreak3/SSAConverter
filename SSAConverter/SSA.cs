using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SSAConverter.IO;
using System.Xml.Serialization;
using System.IO;

namespace SSAConverter
{
    public class SSA
    {
        [XmlElement("Header")]
        public Header header;
        [XmlElement("Parts")]
        public List<Part> parts = new List<Part>();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Header
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x4)]
            public string magic;
            public short unk1;
            public short unk2;
            public int zero1;
            public int unk3;
            public int zero2;
            public int partCount;
            public int unk4;
            public int unk5;
        }

        public class Part
        {
            public int partVal;
            public string name;
            [XmlElement("Area")]
            public List<Area> area = new List<Area>();
        }

        public class Area
        {
            public byte[] areaVal;

            public int orgx;
            public int orgy;
            public int tbdt;
            public int myid;
            public int paid;
            public int chid;
            public int sucd;
            public int pcid;

            public byte[] prio;
            public byte[] posx;
            public byte[] posy;
            public byte[] angl;
            public byte[] scax;
            public byte[] scay;
            public byte[] tran;
            public byte[] hide;
            public byte[] flph;
            public byte[] flpv;
            public byte[] udat;

            public int pcol;
            public int palt;
            public int vert;
            public byte[] imgx;
            public byte[] imgy;
            public int imgw;
            public int imgh;

            public int orfx;
            public int orfy;
        }

        public static SSA FromByteArray(byte[] input)
        {
            var ssa = new SSA();

            using (var br = new BinaryReaderX(new MemoryStream(input)))
            {
                ssa.header = br.ReadStruct<Header>();

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var magic = br.ReadString(4);
                    var size = br.ReadInt32();
                    switch (magic)
                    {
                        case "PART":
                            ssa.parts.Add(new Part());
                            ssa.parts.Last().partVal = br.ReadInt32();
                            break;
                        case "NAME":
                            ssa.parts.Last().name = br.ReadString(size);
                            break;
                        case "AREA":
                            ssa.parts.Last().area.Add(new Area());
                            ssa.parts.Last().area.Last().areaVal = br.ReadBytes(size);
                            break;
                        case "ORGX":
                            ssa.parts.Last().area.Last().orgx = br.ReadInt32();
                            break;
                        case "ORGY":
                            ssa.parts.Last().area.Last().orgy = br.ReadInt32();
                            break;
                        case "TBDT":
                            ssa.parts.Last().area.Last().tbdt = br.ReadInt32();
                            break;
                        case "MYID":
                            ssa.parts.Last().area.Last().myid = br.ReadInt32();
                            break;
                        case "PAID":
                            ssa.parts.Last().area.Last().paid = br.ReadInt32();
                            break;
                        case "CHID":
                            ssa.parts.Last().area.Last().chid = br.ReadInt32();
                            break;
                        case "PCID":
                            ssa.parts.Last().area.Last().pcid = br.ReadInt32();
                            break;
                        case "SUCD":
                            ssa.parts.Last().area.Last().sucd = br.ReadInt32();
                            break;
                        case "PRIO":
                            ssa.parts.Last().area.Last().prio = br.ReadBytes(size + 8);
                            break;
                        case "POSX":
                            ssa.parts.Last().area.Last().posx = br.ReadBytes(size + 8);
                            break;
                        case "POSY":
                            ssa.parts.Last().area.Last().posy = br.ReadBytes(size + 8);
                            break;
                        case "ANGL":
                            ssa.parts.Last().area.Last().angl = br.ReadBytes(size + 8);
                            break;
                        case "SCAX":
                            ssa.parts.Last().area.Last().scax = br.ReadBytes(size + 8);
                            break;
                        case "SCAY":
                            ssa.parts.Last().area.Last().scay = br.ReadBytes(size + 8);
                            break;
                        case "TRAN":
                            ssa.parts.Last().area.Last().tran = br.ReadBytes(size + 8);
                            break;
                        case "HIDE":
                            ssa.parts.Last().area.Last().hide = br.ReadBytes(size + 8);
                            break;
                        case "FLPH":
                            ssa.parts.Last().area.Last().flph = br.ReadBytes(size + 8);
                            break;
                        case "FLPV":
                            ssa.parts.Last().area.Last().flpv = br.ReadBytes(size + 8);
                            break;
                        case "UDAT":
                            ssa.parts.Last().area.Last().udat = br.ReadBytes(size + 8);
                            break;
                        case "PCOL":
                            ssa.parts.Last().area.Last().pcid = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "PALT":
                            ssa.parts.Last().area.Last().palt = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "VERT":
                            ssa.parts.Last().area.Last().vert = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "IMGX":
                            ssa.parts.Last().area.Last().imgx = br.ReadBytes(size + 8);
                            break;
                        case "IMGY":
                            ssa.parts.Last().area.Last().imgy = br.ReadBytes(size + 8);
                            break;
                        case "IMGW":
                            ssa.parts.Last().area.Last().imgw = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "IMGH":
                            ssa.parts.Last().area.Last().imgh = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "ORFX":
                            ssa.parts.Last().area.Last().orfx = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        case "ORFY":
                            ssa.parts.Last().area.Last().orfy = br.ReadInt32();
                            br.ReadBytes(8);
                            break;
                        default:
                            throw new NotImplementedException($"Unknown parameter {magic}. Error occured at Position 0x{br.BaseStream.Position:x8}");
                    }
                }
            }

            return ssa;
        }

        public static byte[] ToByteArray(SSA ssa)
        {
            return null;
        }
    }
}
