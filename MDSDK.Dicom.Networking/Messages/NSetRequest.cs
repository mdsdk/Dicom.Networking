// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

namespace MDSDK.Dicom.Networking.Messages
{
    [Command(CommandType.N_SET_RQ, true)]
    public sealed class NSetRequest : Request
    {
        public string RequestedSOPClassUID { get; set; }

        public string RequestedSOPInstanceUID { get; set; }
    }

    public class NSetRequestMessage<TModificationList> : DicomMessage<NSetRequest, TModificationList>
        where TModificationList : new()
    {
    }
}