using System;
using System.IO;
using System.Xml.Serialization;

namespace VehicleUnbuncher
{
    public static class Options
    {
        public const string OptionsFilePath = "VehicleUnbuncherOptions.xml";

        public static Settings CurrentSettings = new Settings();

        public static void Save()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(OptionsFilePath))
                {
                    xmlSerializer.Serialize(streamWriter, CurrentSettings);
                }
            }
            catch (IOException ex)
            {
#if DEBUG
                Helper.PrintError("Filesystem or IO Error");
#endif
            }
            catch (Exception ex2)
            {
#if DEBUG
                Helper.PrintError(ex2.Message);
#endif
            }
        }

        public static void Load()
        {
            XmlSerializer XmlSerializerInstance = new XmlSerializer(typeof(Settings));
            try
            {
                using (StreamReader streamReader = new StreamReader(OptionsFilePath))
                {
                    Settings LoadedSettings = (Settings)XmlSerializerInstance.Deserialize(streamReader);
                    LoadedSettings.Validate();
                    CurrentSettings = LoadedSettings;
                }
            }
            catch (FileNotFoundException ex)
            {
#if DEBUG
                Helper.PrintError("File not found. This is expected if no config file");
#endif
            }
            catch (IOException ex2)
            {
#if DEBUG
                Helper.PrintError("Filesystem or IO Error");
#endif
            }
            catch (Exception ex3)
            {
#if DEBUG
                Helper.PrintError(ex3.Message);
#endif
            }
        }
    }
}
