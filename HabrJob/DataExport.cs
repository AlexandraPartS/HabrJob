using System.Xml.Serialization;

namespace habrJob
{
    /// <summary>
    /// Class that provides a meta list for exporting data
    /// </summary>
    internal class DataExport
    {
        static string exportpath = "HabrJobs";
        public enum MetaType
        {
            XML,
            MQ,
            Console,
            etc
        }

        static public void Export(MetaType meta, List<HabrJob> jobs) 
        {
            switch (meta)
            {
                case MetaType.XML:
                    WriteToXMLFile(jobs);
                    break;
                case MetaType.MQ:
                    WriteToBD(jobs);
                    break;
                case MetaType.Console:
                    Console.Write(jobs);
                    break;
                default:
                    throw new Exception();
            }
        }

        static void WriteToXMLFile(List<HabrJob> jobs)
        {
            try
            {
                using (FileStream fs = new FileStream(exportpath+".xml", FileMode.Create))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HabrJob>));
                    xmlSerializer.Serialize(fs, jobs);
                }
            }
            catch(Exception e)
            {
                DataReceiver.ondevice?.Invoke($"Exception: {e.Message}");
            }
        }
        static void WriteToBD(List<HabrJob> jobs) { }
    }
}
