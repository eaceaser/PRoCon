// Copyright 2010 Geoffrey 'Phogue' Green
// 
// http://www.phogue.net
//  
// This file is part of PRoCon Frostbite.
// 
// PRoCon Frostbite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// PRoCon Frostbite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace PRoCon.Core.Remote {
    public class Packet : ICloneable {

        public static readonly int INT_PACKET_HEADER_SIZE = 12;
        
        #region Setters and Getters

        public bool OriginatedFromServer { get; private set; }

        public bool IsResponse { get; private set; }

        public UInt32 SequenceNumber { get; private set; }

        public List<string> Words { get; private set; }

        public UInt32 PacketSize { get; private set; }

        //public DateTime CreationTime { get; private set; }

        #endregion

        // Used if we have recieved a packet and need it decoded..
        public Packet(byte[] a_bRawPacket) {
            this.NullPacket();

            //this.CreationTime = DateTime.Now;

            this.DecodePacket(a_bRawPacket);
        }

        // Used if we'll be using EncodePacket to send to the server.
        public Packet(bool blOriginatedFromServer, bool blIsResponse, UInt32 ui32SequenceNumber, List<string> a_lstWords) {
            this.OriginatedFromServer = blOriginatedFromServer;
            this.IsResponse = blIsResponse;
            this.SequenceNumber = ui32SequenceNumber;
            this.Words = a_lstWords;

            //this.CreationTime = DateTime.Now;
        }
        
        public Packet(bool isOriginatedFromServer, bool isResponse, UInt32 sequenceNumber, string commandString)
            : this(isOriginatedFromServer, isResponse, sequenceNumber, Packet.Wordify(commandString)) {

        }

        private void NullPacket() {
            this.OriginatedFromServer = false;
            this.IsResponse = false;
            this.SequenceNumber = 0;
            this.Words = new List<string>();
        }


        // Veeerrryy basic replacment for CommandLineToArgvW, since
        // I wasn't using anything that advanced in it anyway.
        public static List<string> Wordify(string strCommand) {
            List<string> lstReturn = new List<string>();
            //lstReturn.RemoveAll(String.IsNullOrEmpty);

            string strFullWord = String.Empty;
            int iQuoteStack = 0;
            bool blEscaped = false;

            //for (int i = 0; i < strCommand.Length; i++) {
            foreach (char cInput in strCommand) {

                if (cInput == ' ') {
                    if (iQuoteStack == 0) {
                        lstReturn.Add(strFullWord);
                        strFullWord = String.Empty;
                    }
                    else {
                        strFullWord += ' ';
                    }
                }
                else if (cInput == 'n' && blEscaped == true) {
                    strFullWord += '\n';
                    blEscaped = false;
                }
                else if (cInput == 'r' && blEscaped == true) {
                    strFullWord += '\r';
                    blEscaped = false;
                }
                else if (cInput == 't' && blEscaped == true) {
                    strFullWord += '\t';
                    blEscaped = false;
                }
                else if (cInput == '"') {
                    if (blEscaped == false) {
                        if (iQuoteStack == 0) {
                            iQuoteStack++;
                        }
                        else {
                            iQuoteStack--;
                        }
                    }
                    else {
                        strFullWord += '"';
                    }
                }
                else if (cInput == '\\') {
                    if (blEscaped == true) {
                        strFullWord += '\\';
                        blEscaped = false;
                    }
                    else {
                        blEscaped = true;
                    }
                }
                else {
                    strFullWord += cInput;
                    blEscaped = false;
                }
            }

            lstReturn.Add(strFullWord);

            return lstReturn;
        }

        public static string bltos(bool blBoolean) {
            return (blBoolean == true ? "true" : "false");
        }

        public static UInt32 DecodePacketSize(byte[] a_bRawPacket) {
            UInt32 ui32ReturnPacketSize = 0;

            if (a_bRawPacket.Length >= Packet.INT_PACKET_HEADER_SIZE) {
                ui32ReturnPacketSize = BitConverter.ToUInt32(a_bRawPacket, 4);
            }

            return ui32ReturnPacketSize;
        }

        public byte[] EncodePacket() {

            // Construct the header uint32
            UInt32 ui32Header = this.SequenceNumber & 0x3fffffff;

            if (this.OriginatedFromServer == true) {
                ui32Header += 0x80000000;
            }

            if (this.IsResponse == true) {
                ui32Header += 0x40000000;
            }

            // Construct the remaining packet headers
            UInt32 ui32PacketSize = Convert.ToUInt32(Packet.INT_PACKET_HEADER_SIZE);
            UInt32 ui32Words = Convert.ToUInt32(this.Words.Count);

            // Encode each word (WordLength, Word Bytes, Null Byte)
            byte[] a_bEncodedWords = new byte[] { };
            foreach (string word in this.Words) {

                string strWord = word;

                // Truncate words over 64 kbs (though the string is Unicode it gets converted below so this does make sense)
                if (strWord.Length > UInt16.MaxValue - 1) {
                    strWord = strWord.Substring(0, UInt16.MaxValue - 1);
                }

                byte[] a_bAppendEncodedWords = new byte[a_bEncodedWords.Length + strWord.Length + 5];

                a_bEncodedWords.CopyTo(a_bAppendEncodedWords, 0);

                BitConverter.GetBytes(strWord.Length).CopyTo(a_bAppendEncodedWords, a_bEncodedWords.Length);
                Encoding.GetEncoding(1252).GetBytes(strWord + Convert.ToChar(0x00)).CopyTo(a_bAppendEncodedWords, a_bEncodedWords.Length + 4);

                a_bEncodedWords = a_bAppendEncodedWords;
            }

            // Get the full size of the packet.
            ui32PacketSize += Convert.ToUInt32(a_bEncodedWords.Length);

            // Now compile the whole packet.
            byte[] a_bEncodedPacket = new byte[ui32PacketSize];
            this.PacketSize = ui32PacketSize;
            BitConverter.GetBytes(ui32Header).CopyTo(a_bEncodedPacket, 0);
            BitConverter.GetBytes(ui32PacketSize).CopyTo(a_bEncodedPacket, 4);
            BitConverter.GetBytes(ui32Words).CopyTo(a_bEncodedPacket, 8);
            a_bEncodedWords.CopyTo(a_bEncodedPacket, Packet.INT_PACKET_HEADER_SIZE);

            return a_bEncodedPacket;
        }

        public void DecodePacket(byte[] a_bRawPacket) {

            this.NullPacket();

            UInt32 ui32Header = BitConverter.ToUInt32(a_bRawPacket, 0);
            this.PacketSize = BitConverter.ToUInt32(a_bRawPacket, 4);
            //UInt32 ui32PacketSize = BitConverter.ToUInt32(a_bRawPacket, 4); // Unused here.
            UInt32 ui32Words = BitConverter.ToUInt32(a_bRawPacket, 8);

            this.OriginatedFromServer = Convert.ToBoolean(ui32Header & 0x80000000);
            this.IsResponse = Convert.ToBoolean(ui32Header & 0x40000000);
            this.SequenceNumber = ui32Header & 0x3fffffff;

            int iWordOffset = 0;

            for (UInt32 ui32WordCount = 0; ui32WordCount < ui32Words; ui32WordCount++) {
                UInt32 ui32WordLength = BitConverter.ToUInt32(a_bRawPacket, Packet.INT_PACKET_HEADER_SIZE + iWordOffset);

                this.Words.Add(Encoding.GetEncoding(1252).GetString(a_bRawPacket, Packet.INT_PACKET_HEADER_SIZE + iWordOffset + 4, (int)ui32WordLength));

                iWordOffset += Convert.ToInt32(ui32WordLength) + 5; // WordLength + WordSize + NullByte
            }
        }

        public static string Compress(string text) {

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();

            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true)) {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(string compressedText) {

            byte[] gzBuffer = Convert.FromBase64String(compressedText);

            using (MemoryStream ms = new MemoryStream()) {

                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress)) {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public string ToDebugString() {

            string strReturn = String.Empty;

            for (int i = 0; i < this.Words.Count; i++) {
                if (i > 0) {
                    strReturn += " ";
                }

                strReturn += String.Format("[{0}-{1}]", i, this.Words[i]);
            }

            return strReturn;
        }

        public override string ToString() {

            string strReturn = String.Empty;

            for (int i = 0; i < this.Words.Count; i++) {
                if (i > 0) {
                    strReturn += " ";
                }

                strReturn += this.Words[i];
            }

            return strReturn;
        }

        public object Clone() {
            return new Packet(this.OriginatedFromServer, this.IsResponse, this.SequenceNumber, new List<string>(this.Words));
        }
    }
}
