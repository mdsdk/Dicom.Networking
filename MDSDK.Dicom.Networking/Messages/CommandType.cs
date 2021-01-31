﻿// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

namespace MDSDK.Dicom.Networking.Messages
{
    public enum CommandType : ushort
    {
        C_STORE_RQ = 0x0001,
        C_STORE_RSP = 0x8001,
        C_GET_RQ = 0x0010,
        C_GET_RSP = 0x8010,
        C_FIND_RQ = 0x0020,
        C_FIND_RSP = 0x8020,
        C_MOVE_RQ = 0x0021,
        C_MOVE_RSP = 0x8021,
        C_ECHO_RQ = 0x0030,
        C_ECHO_RSP = 0x8030,
        N_EVENT_REPORT_RQ = 0x0100,
        N_EVENT_REPORT_RSP = 0x8100,
        N_GET_RQ = 0x0110,
        N_GET_RSP = 0x8110,
        N_SET_RQ = 0x0120,
        N_SET_RSP = 0x8120,
        N_ACTION_RQ = 0x0130,
        N_ACTION_RSP = 0x8130,
        N_CREATE_RQ = 0x0140,
        N_CREATE_RSP = 0x8140,
        N_DELETE_RQ = 0x0150,
        N_DELETE_RSP = 0x8150,
        C_CANCEL_RQ = 0x0FFF,
    }
}
