﻿/*************************************************************************************\
File Name  :  LZ4Transform.cs
Project    :  MSSQL Compressed Backup

Copyright 2009 Clay Lenhart <clay@lenharts.net>

This file is part of MSSQL Compressed Backup.

MSSQL Compressed Backup is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MSSQL Compressed Backup is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MSSQL Compressed Backup.  If not, see <http://www.gnu.org/licenses/>.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\*************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using QuickLZ;

namespace MSBackupPipe.StdPlugins.Transform
{
    public class QLZTransform : IBackupTransformer
    {

        private static readonly Dictionary<string, ParameterInfo> MBackupParamSchema;
        private static readonly Dictionary<string, ParameterInfo> MRestoreParamSchema;
        static QLZTransform()
        {
            MBackupParamSchema = new Dictionary<string, ParameterInfo>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"level", new ParameterInfo(false, false)}
            };

            MRestoreParamSchema = new Dictionary<string, ParameterInfo>(StringComparer.InvariantCultureIgnoreCase);
        }

        #region IBackupTransformer Members

        public Stream GetBackupWriter(Dictionary<string, List<string>> config, Stream writeToStream)
        {
            ParameterInfo.ValidateParams(MBackupParamSchema, config);

            Console.WriteLine("Compressor: QLZ");

            return new QuickLZStream(writeToStream, CompressionMode.Compress,1);
        }

        public string Name
        {
            get { return "QLZ"; }
        }

        public Stream GetRestoreReader(Dictionary<string, List<string>> config, Stream readFromStream)
        {
            ParameterInfo.ValidateParams(MRestoreParamSchema, config);

            Console.WriteLine("Compressor: QLZ");

            return new QuickLZStream(readFromStream, CompressionMode.Decompress, 1);
        }

        public string CommandLineHelp
        {
            get
            {
                return @"QLZ Usage: QLZ will compress (or uncompress) the data.";
            }
        }

        #endregion
    }
}
