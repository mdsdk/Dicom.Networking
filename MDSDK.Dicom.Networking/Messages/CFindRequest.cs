// This is a generated file. Do not modify.

using MDSDK.Dicom.Serialization;

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.C_FIND_RQ, true)]
    public class CFindRequest : IRequest
    {
        public string AffectedSOPClassUID { get; set; }

        public CommandType CommandField { get; set; }

        public ushort MessageID { get; set; }

        public RequestPriority Priority { get; set; }

        public ushort CommandDataSetType { get; set; }
    }
}
