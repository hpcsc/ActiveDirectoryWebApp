using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChangeNotification;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace ADChangeNotification
{
    class Program
    {
        static void Main(string[] args)
        {            
            TrackUsingChangeNotification();           
        }

        private static void TrackUsingDirSync()
        {
            using (var connection = CreateConnection("192.168.3.252"))
            {
                var request = new SearchRequest("ou=Samples,dc=david,dc=2359media,dc=com",
                    "(|(objectClass=*)(isDeleted=TRUE))", SearchScope.OneLevel, null);

                //var dirSyncRC = new DirSyncRequestControl(cookie, DirectorySynchronizationOptions.IncrementalValues, Int32.MaxValue);
                //request.Controls.Add(dirSyncRC);

                var searchResponse = (SearchResponse)connection.SendRequest(request);

                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    foreach (System.Collections.DictionaryEntry attr in entry.Attributes)
                    {
                        Console.WriteLine("{0}: {1}", attr.Key, ((DirectoryAttribute)attr.Value)[0]);
                        //foreach (var t in attr)
                        //{
                        //    //Console.WriteLine("{0}: {1}", t);
                        //    //Attribute Sub Value below
                        //    Console.WriteLine(attr.Path);
                        //}
                    }
                }
            }
        }

        private static void TrackUsingChangeNotification()
        {
            using (var connect = CreateConnection("192.168.3.252"))
            {
                using (var notifier = new ChangeNotifier(connect))
                {
                    //register some objects for notifications (limit 5)                    
                    notifier.Register("ou=Samples,dc=david,dc=2359media,dc=com", SearchScope.OneLevel);

                    notifier.ObjectChanged += notifier_ObjectChanged;

                    Console.WriteLine("Waiting for changes...");
                    Console.WriteLine();
                    Console.ReadLine();
                }
            }
        }

        private static LdapConnection CreateConnection(string host)
        {
            return new LdapConnection(new LdapDirectoryIdentifier(host), new NetworkCredential("hpcsc", "david.123"), AuthType.Ntlm);
        }

        static void notifier_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("=============== Change notification ==============");

            Console.WriteLine(e.Result.DistinguishedName);
            foreach (string attrib in e.Result.Attributes.AttributeNames)
            {
                foreach (var item in e.Result.Attributes[attrib].GetValues(typeof(string)))
                {
                    Console.WriteLine("\t{0}: {1}", attrib, item);
                }
            }
            Console.WriteLine();            
            Console.WriteLine("=============== End notification =================");
            Console.WriteLine("==================================================");
            Console.WriteLine();
        }
    }
}
