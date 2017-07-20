﻿/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Archives
{
    internal sealed class SkyrimArchiveReader : ArchiveReader
    {
        ArchiveFlags flags;

        SortedDictionary<string, SortedDictionary<string, FileInfo>> sorted = new SortedDictionary<string, SortedDictionary<string, FileInfo>>();

        public SkyrimArchiveReader(string path)
            : base(path)
        {           
        }

        protected override void DoOpen()
        {
            using (CustomBinaryReader reader = new CustomBinaryReader(new FileStream(ArchivePath, FileMode.Open, FileAccess.Read)))
            {
                uint signature = reader.ReadUInt32();
                if (signature != 0x415342)
                {
                    throw new InvalidDataException("File is not BSA");
                }

                uint version = reader.ReadUInt32();
                uint folderOffset = reader.ReadUInt32();
                flags = (ArchiveFlags)reader.ReadUInt32();
                uint folderCount = reader.ReadUInt32();
                uint fileCount = reader.ReadUInt32();
                uint totalFolderNameLength = reader.ReadUInt32();
                uint totalFileNameLength = reader.ReadUInt32();
                uint fileExtensions = reader.ReadUInt32();

                FolderInfo[] folders = new FolderInfo[(int)folderCount];

                // Read folders
                reader.BaseStream.Position = folderOffset;
                for (int i = 0; i < folderCount; i++)
                {
                    ulong hash = reader.ReadUInt64();
                    uint count = reader.ReadUInt32();
                    uint offset = reader.ReadUInt32();
                    offset -= totalFileNameLength + 36 + 24 * folderCount;
                    folders[i] = new FolderInfo()
                    {
                        FileCount = count,
                        ContentOffset = offset
                    };
                }

                // Read folder content (name and files)
                foreach (var folder in folders)
                {
                    reader.BaseStream.Seek(folder.ContentOffset, SeekOrigin.Begin);
                    byte folderNameLength = reader.ReadByte();
                    folder.Path = Encoding.UTF8.GetString(reader.ReadBytes(folderNameLength - 1));
                    byte zero = reader.ReadByte();

                    folder.Files = new FileInfo[folder.FileCount];
                    for (int i = 0; i < folder.FileCount; i++)
                    {
                        reader.BaseStream.Seek(folder.ContentOffset + folderNameLength + 1 + i * 16, SeekOrigin.Begin);
                        ulong hash = reader.ReadUInt64();
                        uint size = reader.ReadUInt32();

                        bool compressed = flags.HasFlag(ArchiveFlags.DefaultCompressed);
                        if ((size & 0xf0000000) != 0)
                        {
                            size &= 0xfffffff;
                            compressed = !compressed;
                        }

                        uint offset = reader.ReadUInt32();
                        folder.Files[i] = new FileInfo()
                        {
                            Size = size,
                            DataOffset = offset,
                            IsCompressed = compressed
                        };
                    }
                }

                long total = fileCount;
                long loaded = 0;
                string filename = Path.GetFileName(ArchivePath);

                // Read file names
                foreach (var folder in folders)
                {
                    foreach (var file in folder.Files)
                    {
                        file.Filename = reader.ReadStringZeroTerminated();
                        loaded++;
                    }
                }

                // Convert to nested sorted dictionary for fast search
                for (int i = 0; i < folderCount; i++)
                {
                    var files = new SortedDictionary<string, FileInfo>();
                    for (int j = 0; j < folders[i].FileCount; j++)
                    {
                        files.Add(folders[i].Files[j].Filename, folders[i].Files[j]);
                    }
                    sorted.Add(folders[i].Path, files);
                }

                return;
            }
        }

        protected override bool DoFileExists(string path)
        {
            return FindFileInfo(path) != null;
        }

        protected override Stream DoGetFileStream(string path)
        {
            var file = FindFileInfo(path);
            if (file == null)
            {
                throw new InvalidOperationException("File not found in archive: " + path);
            }

            Stream stream = new FileStream(ArchivePath, FileMode.Open, FileAccess.Read);
            stream.Position = file.DataOffset;
            long length = file.Size;

            if (flags.HasFlag(ArchiveFlags.FileNameBeforeData))
            {
                // Consume (skip) filename before data
                var singleByteBuffer = new byte[1];
                stream.Read(singleByteBuffer, 0, 1);
                var stringLength = singleByteBuffer[0];
                stream.Seek(stringLength, SeekOrigin.Current);
                // Adjust length according to consumed data
                length -= stringLength + 1;
            }

            if (file.IsCompressed)
            {
                // Read original size
                var originalSizeBuffer = new byte[4];
                stream.Read(originalSizeBuffer, 0, 4);
                uint originalSize = BitConverter.ToUInt32(originalSizeBuffer, 0);
                var compressedByteData = new byte[length];
				stream.Read(compressedByteData, 0,(int) length);
				var uncompressedByteData = new byte[originalSize];
                LZ4.Decode(compressedByteData, 0, (int)length, uncompressedByteData, 0, (int)originalSize, true);
				return new MemoryStream(uncompressedByteData);
                // Deflate stream
                //return new CustomDeflateStream(stream, originalSize);
            }
            else
            { 
                return new ArchiveSubstream(stream, length);
            }
        }

        private FileInfo FindFileInfo(string path)
        {
            string dir = Path.GetDirectoryName(path).ToLower();
            string filename = Path.GetFileName(path).ToLower();

            if (sorted.ContainsKey(dir) && sorted[dir].ContainsKey(filename))
            {
                return sorted[dir][filename];
            }
            else
            {
                return null;
            }
        }

        class FolderInfo
        {
            public uint FileCount { get; set; }
            public uint ContentOffset { get; set; }
            public string Path { get; set; }
            public FileInfo[] Files { get; set; }

            public override string ToString()
            {
                return Path;
            }
        }

        class FileInfo
        {
            public uint Size { get; set; }
            public uint DataOffset { get; set; }
            public bool IsCompressed { get; set; }
            public string Filename { get; set; }

            public override string ToString()
            {
                return Filename;
            }
        }

        [Flags]
        enum ArchiveFlags : uint
        {
            HasNamesForDirectories = 0x0001,
            HasNamesForFiles = 0x0002,
            DefaultCompressed = 0x0004,
            Unknown3 = 0x0008,
            Unknown4 = 0x0010,
            Unknown5 = 0x0020,
            Unknown6 = 0x0040,
            Unknown7 = 0x0080,
            FileNameBeforeData = 0x0100,
            Unknown9 = 0x0200,
            Unknown10 = 0x0400,
            Unknown11 = 0x0800,
            Unknown12 = 0x100,
            Unknown13 = 0x200,
            Unknown14 = 0x400,
            Unknown15 = 0x800
        }
    }
}
