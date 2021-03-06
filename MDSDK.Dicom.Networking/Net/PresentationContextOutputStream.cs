﻿// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

using MDSDK.BinaryIO;
using MDSDK.Dicom.Networking.DataUnits.PDUs;
using System;

namespace MDSDK.Dicom.Networking.Net
{
    internal sealed class PresentationContextOutputStream : OutputStreamBase
    {
        private readonly DicomConnection _connection;

        private readonly DataTransferPDUHeader _dataTransferPDUHeader = new DataTransferPDUHeader();

        private readonly FragmentHeader _fragmentHeader = new FragmentHeader();

        private readonly byte[] _dataBuffer;

        public PresentationContextOutputStream(DicomConnection connection, byte presentationContextID, FragmentType fragmentType)
        {
            _connection = connection;

            _fragmentHeader.PresentationContextID = presentationContextID;
            _fragmentHeader.FragmentType = fragmentType;

            var maxDataTransferPDULength = 64 * 1024 - DataTransferPDUHeader.Size;

            if (connection.MaxDataTransferPDULengthRequestedByPeer > 0)
            {
                NetUtils.ThrowIf(connection.MaxDataTransferPDULengthRequestedByPeer % 2 != 0);
                NetUtils.ThrowIf(connection.MaxDataTransferPDULengthRequestedByPeer < FragmentHeader.Size + 2);

                if (connection.MaxDataTransferPDULengthRequestedByPeer < maxDataTransferPDULength)
                {
                    maxDataTransferPDULength = (int)connection.MaxDataTransferPDULengthRequestedByPeer;
                }
            }

            _dataBuffer = new byte[DataTransferPDUHeader.Size + maxDataTransferPDULength];
        }

        private int _bufferedDataLength;

        public override void Write(ReadOnlySpan<byte> data)
        {
            while (true)
            {
                var writeSpan = _dataBuffer.AsSpan(_bufferedDataLength);
                if (data.Length <= writeSpan.Length)
                {
                    data.CopyTo(writeSpan);
                    _bufferedDataLength += data.Length;
                    break;
                }
                data[..writeSpan.Length].CopyTo(writeSpan);
                data = data[writeSpan.Length..];
                SendDataTransferPDU(isLastFragment: false);
            }
        }

        private void SendDataTransferPDU(bool isLastFragment)
        {
            _dataTransferPDUHeader.Length = (uint)(FragmentHeader.Size + _bufferedDataLength);
            _fragmentHeader.Length = (uint)(2 + _bufferedDataLength);
            _fragmentHeader.IsLastFragment = isLastFragment;
            
            if (_connection.TraceWriter != null)
            {
                NetUtils.TraceOutput(_connection.TraceWriter, $"PC {_fragmentHeader.PresentationContextID} sending ", _dataTransferPDUHeader);
                NetUtils.TraceOutput(_connection.TraceWriter, $"PC {_fragmentHeader.PresentationContextID} sending ", _fragmentHeader);
                _connection.TraceWriter.WriteLine($"PC {_fragmentHeader.PresentationContextID} sending {_bufferedDataLength} bytes of data");
                _connection.TraceWriter.Flush();
            }

            var dataWriter = new BinaryDataWriter(_connection.Output, ByteOrder.BigEndian);
            _dataTransferPDUHeader.WriteTo(dataWriter);
            _fragmentHeader.WriteTo(dataWriter);
            _connection.Output.WriteBytes(_dataBuffer.AsSpan(0, _bufferedDataLength));
            _connection.Output.Flush(isLastFragment ? FlushMode.Deep : FlushMode.Shallow);

            _bufferedDataLength = 0;
        }

        public override void Flush()
        {
            if (_bufferedDataLength > 0)
            {
                SendDataTransferPDU(isLastFragment: true);
            }
        }
    }
}
