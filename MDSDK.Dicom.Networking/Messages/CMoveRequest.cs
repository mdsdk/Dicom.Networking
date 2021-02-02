// This is a generated file. Do not modify.

using MDSDK.Dicom.Serialization;

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.C_MOVE_RQ, true)]
    public class CMoveRequest : IRequest
    {
        public string AffectedSOPClassUID { get; set; }

        public CommandType CommandField { get; set; }

        public ushort MessageID { get; set; }

        public RequestPriority Priority { get; set; }

        public ushort CommandDataSetType { get; set; }

        public string MoveDestination { get; set; }
    }
}
