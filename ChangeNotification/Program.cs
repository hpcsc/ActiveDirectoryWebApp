using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Options = System.DirectoryServices.Protocols.DirectorySynchronizationOptions;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace ChangeNotification
{
    class Program
    {
        static void Main(string[] args)
        {            
            TrackUsingChangeNotification();           
            //TrackUsingDirSync();
        }

        private static void TrackUsingDirSync()
        {
            using (var connection = CreateConnection("192.168.3.253"))
            {
                while (true)
                {
                    var request = new SearchRequest("ou=Samples,dc=david,dc=2359media,dc=com",
                    "(|(objectClass=*)(isDeleted=TRUE))", SearchScope.OneLevel, null);

                    byte[] cookie = null;
                    var dirSyncRC = new DirSyncRequestControl(cookie, Options.IncrementalValues, Int32.MaxValue);
                    request.Controls.Add(dirSyncRC);

                    var searchResponse = (SearchResponse)connection.SendRequest(request);

                    if (searchResponse.Entries.Count > 0)
                    {
                        Console.WriteLine("==================================================");
                        Console.WriteLine("=============== Change notification ==============");

                        foreach (SearchResultEntry entry in searchResponse.Entries)
                        {
                            foreach (System.Collections.DictionaryEntry attr in entry.Attributes)
                            {
                                Console.WriteLine("{0}: {1}", attr.Key, ((DirectoryAttribute)attr.Value)[0]);
                            }
                        }

                        Console.WriteLine();
                        Console.WriteLine("=============== End notification =================");
                        Console.WriteLine("==================================================");
                        Console.WriteLine();
                    }
                    

                    Thread.Sleep(1000);
                }                
            }
        }

        private static void TrackUsingChangeNotification()
        {
            using (var connect = CreateConnection("192.168.3.253"))
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
            return new LdapConnection(new LdapDirectoryIdentifier(host), new NetworkCredential("hpcsc", "password.123"), AuthType.Ntlm);
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
