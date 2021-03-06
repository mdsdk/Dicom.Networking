// This is a generated file. Do not modify.

#pragma warning disable 1591

using MDSDK.Dicom.Serialization;

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.N_GET_RSP)]
    public class NGetResponse : IResponse, IMayHaveDataSet
    {
        public string AffectedSOPClassUID { get; set; }

        public CommandType CommandField { get; set; }

        public ushort MessageIDBeingRespondedTo { get; set; }

        public ushort CommandDataSetType { get; set; }

        public ushort Status { get; set; }

        public string AffectedSOPInstanceUID { get; set; }
    }
}

#pragma warning restore 1591
