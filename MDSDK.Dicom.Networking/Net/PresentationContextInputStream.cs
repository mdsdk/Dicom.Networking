﻿// Copyright (c) Robin Boerdijk - All rights reserved - See LICENSE file for license terms

using MDSDK.BinaryIO;
using System;
using System.IO;

namespace MDSDK.Dicom.Networking.Net
{
    internal sealed class PresentationContextInputStream : InputStreamBase
    {
        private readonly DicomConnection _connection;

        private readonly FragmentType _fragmentType;

        private FragmentHeader _fragmentHeader;

        private long _fragmentEndPosition;

        public byte PresentationContextID { get; private set; }

        public PresentationContextInputStream(DicomConnection connection, FragmentType fragmentType)
        {
            _connection = connection;

            _fragmentType = fragmentType;

            StartReadFragment(isFirstFragment: true);

            PresentationContextID = _fragmentHeader.PresentationContextID;
        }

        private void StartReadFragment(bool isFirstFragment)
        {
            if (_fragmentHeader != null)
            {
                throw new Exception("Logic error");
            }

            var dataReader = new BinaryDataReader(_connection.Input, ByteOrder.BigEndian);
            var fragmentHeader = FragmentHeader.ReadFrom(dataReader);

            if (_connection.TraceWriter != null)
            {
                NetUtils.TraceOutput(_connection.TraceWriter, $"PC {fragmentHeader.PresentationContextID} received ", fragmentHeader);
                _connection.TraceWriter.Flush();
            }

            if (!isFirstFragment && (fragmentHeader.PresentationContextID != PresentationContextID))
            {
                throw new IOException($"Expected presentation context ID {PresentationContextID} but got {_fragmentHeader.PresentationContextID}");
            }

            if (fragmentHeader.FragmentType != _fragmentType)
            {
                throw new IOException($"Expected {_fragmentType} but got {fragmentHeader.FragmentType}");
            }

            _fragmentHeader = fragmentHeader;
            _fragmentEndPosition = _connection.Input.Position + (fragmentHeader.Length - 2);
        }

        private void EndReadFragment(out bool wasLastFragment)
        {
            if (_fragmentHeader == null)
            {
                throw new Exception("Logic error");
            }
            
            if (_connection.Input.Position != _fragmentEndPosition)
            {
                throw new Exception("Logic error");
            }

            wasLastFragment = _fragmentHeader.IsLastFragment;
            
            _fragmentHeader = null;
            _fragmentEndPosition = 0;
        }

        private bool _atEnd;

        public override int Read(Span<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer));
            }

            if (_atEnd)
            {
                return 0;
            }

            while (_connection.Input.Position == _fragmentEndPosition)
            {
                EndReadFragment(out bool wasLastFragment);
                if (wasLastFragment)
                {
                    _atEnd = true;
                    return 0;
                }
                if (_connection.Input.Position == _connection.EndOfDataTransferPDUPosition)
                {
                    _connection.ReadNextDataTransferPDU();
                }
                StartReadFragment(isFirstFragment: false);
            }

            var maxBytesToRead = Math.Min(buffer.Length, _fragmentEndPosition - _connection.Input.Position);

            return _connection.Input.ReadSome(buffer.Slice(0, (int)maxBytesToRead));
        }

        internal void SkipToEnd()
        {
            Span<byte> buffer = stackalloc byte[4096];
            while (Read(buffer) > 0) { continue; }
        }
    }
}
