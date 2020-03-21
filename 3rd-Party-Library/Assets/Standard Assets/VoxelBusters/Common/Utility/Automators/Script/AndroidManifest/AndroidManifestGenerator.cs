using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using System.Xml;
using UnityEditor;

namespace VoxelBusters.Utility
{
    public partial class AndroidManifestGenerator
    {
        #region Methods

        public void SaveManifest(string _packageName, string _path, string _minSDKVersion, string _targetSDKVersion)
        {
            SaveManifest(_packageName, _path, "1", "1.0", _minSDKVersion, _targetSDKVersion);
        }

        public void SaveManifest(string _packageName, string _path, string _versionCode, string _versionName, string _minSDKVersion, string _targetSDKVersion)
        {
            // Settings
            XmlWriterSettings _settings = new XmlWriterSettings();
            _settings.Encoding = new System.Text.UTF8Encoding(true);
            _settings.ConformanceLevel = ConformanceLevel.Document;
            _settings.Indent = true;

            // Generate and write manifest
            using (XmlWriter _xmlWriter = XmlWriter.Create(_path, _settings))
            {
                _xmlWriter.WriteStartDocument();
                {
                    //********************
                    // Manifest
                    //********************
                    _xmlWriter.WriteComment("AUTO GENERATED MANIFEST FILE FROM AndroidManifestGenerator. DONT MODIFY HERE.");

                    _xmlWriter.WriteStartElement("manifest");
                    WriteAttributeString(_xmlWriter, "xmlns", "android", null, "http://schemas.android.com/apk/res/android");
                    WriteAttributeString(_xmlWriter, null, "package", null, _packageName);
                    WriteAttributeString(_xmlWriter, "android", "versionCode", null, _versionCode);
                    WriteAttributeString(_xmlWriter, "android", "versionName", null, _versionName);

                    {
                        //Specify min and target versions

                        _xmlWriter.WriteStartElement("uses-sdk");
                        {
                            WriteAttributeString(_xmlWriter, "android", "minSdkVersion", null, _minSDKVersion);
                            WriteAttributeString(_xmlWriter, "android", "targetSdkVersion", null, _targetSDKVersion);
                        }
                        _xmlWriter.WriteEndElement();

                        //********************
                        // Application
                        //********************
                        _xmlWriter.WriteStartElement("application");
                        {
                            WriteApplicationProperties(_xmlWriter);
                        }
                        _xmlWriter.WriteEndElement();

                        //********************
                        // Permission
                        //********************
                        _xmlWriter.WriteComment("Permissions");
                        WritePermissions(_xmlWriter);
                    }
                    _xmlWriter.WriteEndElement();
                }
                _xmlWriter.WriteEndDocument();
            }
        }

        protected virtual void WriteApplicationProperties(XmlWriter _xmlWriter)
        { }

        protected virtual void WritePermissions(XmlWriter _xmlWriter)
        { }

        protected void WriteActivity(XmlWriter _xmlWriter, string _name, string _theme = null, string _orientation = null, string _configChanges = null, string _exported = null, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("activity");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);

                if (_theme != null)
                    WriteAttributeString(_xmlWriter, "android", "theme", null, _theme);

                if (_orientation != null)
                    WriteAttributeString(_xmlWriter, "android", "screenOrientation", null, _orientation);

                if (_configChanges != null)
                    WriteAttributeString(_xmlWriter, "android", "configChanges", null, _configChanges);

                if (_exported != null)
                    WriteAttributeString(_xmlWriter, "android", "exported", null, _exported);

            }
            _xmlWriter.WriteEndElement();
        }

        protected void WriteAction(XmlWriter _xmlWriter, string _name, string _permission = null, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("action");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);

                if (_permission != null)
                    WriteAttributeString(_xmlWriter, "android", "permission", null, _permission);
            }
            _xmlWriter.WriteEndElement();
        }

        protected void WriteCategory(XmlWriter _xmlWriter, string _name, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("category");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);
            }
            _xmlWriter.WriteEndElement();
        }

        protected void WriteService(XmlWriter _xmlWriter, string _name, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("service");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);
            }
            _xmlWriter.WriteEndElement();
        }

        protected void WriteService(XmlWriter _xmlWriter, string _name, string _permission = null, string _intentFilterAction = null, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("service");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);
                if (_permission != null)
                {
                    WriteAttributeString(_xmlWriter, "android", "permission", null, _permission);
                }

                if (_intentFilterAction != null)
                {
                    _xmlWriter.WriteStartElement("intent-filter");
                    {
                        WriteAction(_xmlWriter: _xmlWriter,
                                    _name: _intentFilterAction
                                    );
                    }
                    _xmlWriter.WriteEndElement();
                }
            }
            _xmlWriter.WriteEndElement();
        }

        protected void WritePermission(XmlWriter _xmlWriter, string _name, string _protectionLevel, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("permission");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);
                WriteAttributeString(_xmlWriter, "android", "protectionLevel", null, _protectionLevel);
            }
            _xmlWriter.WriteEndElement();
        }

        protected void WriteUsesPermission(XmlWriter _xmlWriter, string _name, Feature[] _features = null, string _comment = null)
        {
            if (_comment != null)
                _xmlWriter.WriteComment(_comment);

            _xmlWriter.WriteStartElement("uses-permission");
            {
                WriteAttributeString(_xmlWriter, "android", "name", null, _name);
            }
            _xmlWriter.WriteEndElement();

            if (_features != null)
            {
                int _count = _features.Length;

                for (int _iter = 0; _iter < _count; _iter++)
                {
                    Feature _curFeature = _features[_iter];

                    _xmlWriter.WriteStartElement("uses-feature");
                    {
                        WriteAttributeString(_xmlWriter, "android", "name", null, _curFeature.Name);
                        WriteAttributeString(_xmlWriter, "android", "required", null, _curFeature.Required ? "true" : "false");
                    }
                    _xmlWriter.WriteEndElement();
                }
            }
        }

        protected void WriteAttributeString(XmlWriter _xmlWriter, string _prefix, string _localName, string _nameSpace, string _value)
        {
#if NET_4_6 || NET_STANDARD_2_0
            if (string.IsNullOrEmpty(_prefix) && string.IsNullOrEmpty(_nameSpace))
                _xmlWriter.WriteAttributeString(_localName, _value);
            else
                _xmlWriter.WriteAttributeString(_prefix, _localName, _nameSpace, _value);
#else
            if (!string.IsNullOrEmpty (_nameSpace))
			{
				_xmlWriter.WriteAttributeString (_prefix + ":" + _localName, _nameSpace, _value);
			}
			else if (!string.IsNullOrEmpty (_prefix))
			{
				_xmlWriter.WriteAttributeString (_prefix + ":" + _localName, _value);
			}
			else
			{
				_xmlWriter.WriteAttributeString (_localName, _value);
			}
#endif
		}

		private ApiCompatibilityLevel GetAPICompatibilityLevel ()
		{
#if !UNITY_5_6_OR_NEWER
			return UnityEditor.PlayerSettings.apiCompatibilityLevel;
#else
			return UnityEditor.PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
		}

#endregion
	}
}
#endif
