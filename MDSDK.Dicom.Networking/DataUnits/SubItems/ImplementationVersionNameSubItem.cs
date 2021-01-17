﻿// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

using MDSDK.BinaryIO;

namespace MDSDK.Dicom.Networking.DataUnits.SubItems
{
    class ImplementationVersionNameSubItem : SubItem
    {
        public ImplementationVersionNameSubItem()
            : base(DataUnitType.ImplementationVersionNameSubItem)
        {
        }

        public byte[] ImplementationVersionName { get; set; }

        public override void ReadContentFrom(BinaryStreamReader input)
        {
            ImplementationVersionName = input.ReadRemainingBytes();
        }

        public override void WriteContentTo(BinaryStreamWriter output)
        {
            output.WriteBytes(ImplementationVersionName);
        }
    }
}
