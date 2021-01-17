// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.N_ACTION_RQ, true)]
    public sealed class NActionRequest : Request
    {
        public string RequestedSOPClassUID { get; set; }

        public string RequestedSOPInstanceUID { get; set; }

        public ushort ActionTypeID { get; set; }
    }

    public class NActionRequestMessage<TActionInformation> : DicomMessage<NActionRequest, TActionInformation>
        where TActionInformation : new()
    {
    }
}