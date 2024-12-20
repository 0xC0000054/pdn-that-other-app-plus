////////////////////////////////////////////////////////////////////////
//
// This file is part of pdn-that-other-app-plus, a Effect plugin for
// Paint.NET that exports the current layer to other image editors.
//
// Copyright (c) 2020, 2024 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace ThatOtherAppPlus
{
    [DataContract(Name = nameof(ThatOtherAppPlusSettings), Namespace = "")]
    class ThatOtherAppPlusSettings
    {
        private readonly string path;
        private bool changed;
        private bool createUserFilesDir;
        [DataMember(Name = "Applications")]
        private HashSet<ApplicationInfo> applications;

        public ThatOtherAppPlusSettings(string path)
        {
            if (path == null)
            {
                ExceptionUtil.ThrowArgumentNullException(nameof(path));
            }

            this.path = path;
            this.changed = false;
            this.createUserFilesDir = false;
            this.applications = new HashSet<ApplicationInfo>();
        }

        public HashSet<ApplicationInfo> Applications
        {
            get => this.applications;
            set
            {
                if (value is null)
                {
                    ExceptionUtil.ThrowArgumentNullException(nameof(value));
                }

                this.applications = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Saves any changes to this instance.
        /// </summary>
        public void Flush()
        {
            if (this.changed)
            {
                Save();
                this.changed = false;
            }
        }

        /// <summary>
        /// Loads the saved settings for this instance.
        /// </summary>
        public void LoadSavedSettings()
        {
            try
            {
                using (FileStream stream = new FileStream(this.path, FileMode.Open, FileAccess.Read))
                {
                    XmlReaderSettings readerSettings = new XmlReaderSettings
                    {
                        CloseInput = false,
                        IgnoreComments = true,
                        XmlResolver = null
                    };

                    using (XmlReader xmlReader = XmlReader.Create(stream, readerSettings))
                    {

                        DataContractSerializer serializer = new DataContractSerializer(typeof(ThatOtherAppPlusSettings));
                        ThatOtherAppPlusSettings settings = (ThatOtherAppPlusSettings)serializer.ReadObject(xmlReader);

                        this.applications = settings.applications;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                this.createUserFilesDir = true;
            }
            catch (FileNotFoundException)
            {
                // Use the default settings if the file is not present.
            }
        }

        private void Save()
        {
            if (this.createUserFilesDir)
            {
                DirectoryInfo info = new DirectoryInfo(Path.GetDirectoryName(this.path));

                if (!info.Exists)
                {
                    info.Create();
                }
            }

            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                Indent = true
            };

            using (XmlWriter writer = XmlWriter.Create(this.path, writerSettings))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(ThatOtherAppPlusSettings));
                serializer.WriteObject(writer, this);
            }
        }
    }
}
