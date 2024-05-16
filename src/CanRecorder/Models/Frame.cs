using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanRecorder.Models
{
    public record Frame(ulong timestamp, uint id, List<byte> data);
}
