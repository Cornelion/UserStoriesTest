using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Payment
    {
        public string PayId { get; set; }
        public string CustomerName { get; set; }
        public string Orderid { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string DecodedStatus
        {
            get
            {
                StatusCode sc = (StatusCode)Int32.Parse(Status);
                return sc.ToString();
            }
        }
        public enum StatusCode
        {
            Invalid = 0,
            Cancelled = 1,
            Refused = 2,
            Authorised = 5,
            Authorisation_Waiting = 51,
            Authorisation_not_known = 52,
            Author_deletion_refused = 63,
            Payment_deleted = 7,
            Payment_deletion_pending = 71,
            Refund = 8,
            Refund_Pending = 81,
            Payment_requested = 9,
            Payment_processing = 91,
            Payment_Uncertain = 92,
            Payment_refused = 93
        }
    }
}
