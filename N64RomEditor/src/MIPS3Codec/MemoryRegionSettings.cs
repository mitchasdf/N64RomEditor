using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class MemoryRegionSettings
    {
        public static int ProgramOffset { get; set; } = 0;
        public static List<CustomMemoryRegion> CustomRegions = new List<CustomMemoryRegion>();
    }
    public class CustomMemoryRegion
    {
        public string Name { get; set; } = "Unnamed region";
        public int VirtualAddressStart { get; set; }
        public int RegionLength { get; set; }
        public int RamMinusRomOffset { get; set; }
        public CustomMemoryRegion(int virtualAddressStart, int regionLength, int ramMinusRomOffset)
        {
            VirtualAddressStart = virtualAddressStart;
            RegionLength = regionLength;
            RamMinusRomOffset = ramMinusRomOffset;
            MemoryRegionSettings.CustomRegions.Add(this);
        }
        public CustomMemoryRegion(int virtualAddressStart, int regionLength, int ramMinusRomOffset, string name)
        {
            Name = name;
            VirtualAddressStart = virtualAddressStart;
            RegionLength = regionLength;
            RamMinusRomOffset = ramMinusRomOffset;
            MemoryRegionSettings.CustomRegions.Add(this);
        }
    }
}
