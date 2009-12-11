/*
This file is part of PacketDotNet

PacketDotNet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PacketDotNet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with PacketDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 *  Copyright 2009 Chris Morgan <chmorgan@gmail.com>
 */
using System;
using MiscUtil.Conversion;

namespace PacketDotNet
{
    /// <summary>
    /// Represents a Linux cooked capture packet, the kinds of packets
    /// received when capturing on an 'any' device
    /// </summary>
    public class LinuxSLLPacket : LinkLayer
    {
        /// <value>
        /// Information about the packet direction
        /// </value>
        public LinuxSLLType Type
        {
            get
            {
                return (LinuxSLLType)EndianBitConverter.Big.ToInt16(header.Bytes,
                                                      header.Offset + LinuxSLLFields.PacketTypePosition);
            }

            set
            {
                var theValue = (Int16)value;
                EndianBitConverter.Big.CopyBytes(theValue,
                                                 header.Bytes,
                                                 header.Offset + LinuxSLLFields.PacketTypePosition);
            }
        }

        /// <value>
        /// The Linux ARPHRD_ value
        /// </value>
        public int ARPHRDValue
        {
            get
            {
                return EndianBitConverter.Big.ToInt16(header.Bytes,
                                                      header.Offset + LinuxSLLFields.LinuxARPHRDPosition);
            }

            set
            {
                var theValue = (Int16)value;
                EndianBitConverter.Big.CopyBytes(theValue,
                                                 header.Bytes,
                                                 header.Offset + LinuxSLLFields.LinuxARPHRDPosition);
            }
        }

        /// <value>
        /// Number of bytes in the link layer address of the sender of the packet
        /// </value>
        public int LinkLayerAddressLength
        {
            get
            {
                return EndianBitConverter.Big.ToInt16(header.Bytes,
                                                      header.Offset + LinuxSLLFields.LinkLayerAddressLengthPosition);
            }

            set
            {
                // range check
                if((value < 0) || (value > 8))
                {
                    throw new System.InvalidOperationException("value of " + value + " out of range of 0 to 8");
                }

                var theValue = (Int16)value;
                EndianBitConverter.Big.CopyBytes(theValue,
                                                 header.Bytes,
                                                 header.Offset + LinuxSLLFields.LinkLayerAddressLengthPosition);
            }
        }

        /// <value>
        /// Link layer header bytes, maximum of 8 bytes
        /// </value>
        public byte[] LinkLayerHeader
        {
            get
            {
                var headerLength = LinkLayerAddressLength;
                var theHeader = new Byte[headerLength];
                Array.Copy(header.Bytes, header.Offset + LinuxSLLFields.LinkLayerHeaderPosition,
                           theHeader, 0,
                           headerLength);
                return theHeader;
            }

            set
            {
                // update the link layer length
                LinkLayerAddressLength = value.Length;

                // copy in the new link layer header bytes
                Array.Copy(value, 0,
                           header.Bytes, header.Offset + LinuxSLLFields.LinkLayerHeaderPosition,
                           value.Length);
            }
        }

        public EthernetPacketType EthernetProtocolType
        {
            get
            {
                return (EthernetPacketType)EndianBitConverter.Big.ToInt16(header.Bytes,
                                                      header.Offset + LinuxSLLFields.EthernetProtocolTypePosition);
            }

            set
            {
                var theValue = (Int16)value;
                EndianBitConverter.Big.CopyBytes(theValue,
                                                 header.Bytes,
                                                 header.Offset + LinuxSLLFields.EthernetProtocolTypePosition);
            }
        }
    }
}
