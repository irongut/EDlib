using EDlib.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    public interface IInaraService
    {
        Task<(string data, DateTime updated)> GetData(InaraHeader header, IList<InaraEvent> input, DownloadOptions options);
    }
}