// This is a generated file. Do not modify.

#pragma warning disable 1591

using MDSDK.Dicom.Serialization;

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.N_DELETE_RQ)]
    public class NDeleteRequest : IRequest, IHasNoDataSet
    {
        public string RequestedSOPClassUID { get; set; }

        public CommandType CommandField { get; set; }

        public ushort MessageID { get; set; }

        public ushort CommandDataSetType { get; set; }

        public string RequestedSOPInstanceUID { get; set; }
    }
}

#pragma warning restore 1591
