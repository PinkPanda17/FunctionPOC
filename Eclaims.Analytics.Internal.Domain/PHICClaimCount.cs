using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclaims.Analytics.Internal.Domain
{
    public class PHICClaimCount
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int Denied { get; set; }
        public int Submitted { get; set; }
        public int Returned { get; set; }
        public int WithVoucher { get; set; }
        public int Vouchering { get; set; }
        public int WithCheque { get; set; }
        public int InProcess { get; set; }
        public int TotalClaimCount { get; set; }
        public string ClientName { get; set; }
        public string Region { get; set; }
        public string PMCC { get; set; }
    }
}
