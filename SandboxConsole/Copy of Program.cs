using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxConsole
{
    class WebDataDownloader
{

    static void Main()
    {
        WebDataDownloader downloader = new WebDataDownloader();
        string[] addresses = { "http://www.msnbc.com", "http://www.yahoo.com",
                                 "http://www.nytimes.com", "http://www.washingtonpost.com",
                                 "http://www.latimes.com", "http://www.newsday.com" };
        CancellationTokenSource cts = new CancellationTokenSource();

        // Create a UI thread from which to cancel the entire operation
        Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Press c to cancel");
            if (Console.ReadKey().KeyChar == 'c')
                cts.Cancel();
        });

        // Using a neutral search term that is sure to get some hits.
        Task<string[]> webTask = downloader.GetWordCounts(addresses, "the", cts.Token);

        // Do some other work here unless the method has already completed. 
        if (!webTask.IsCompleted)
        {
            // Simulate some work.
            Thread.SpinWait(5000000);
        }

        string[] results = null;
        try
        {
            results = webTask.Result;
        }
        catch (AggregateException e)
        {
            foreach (var ex in e.InnerExceptions)
            {
                OperationCanceledException oce = ex as OperationCanceledException;
                if (oce != null)
                {
                    if (oce.CancellationToken == cts.Token)
                    {
                        Console.WriteLine("Operation canceled by user.");
                    }
                }
                else
                    Console.WriteLine(ex.Message);
            }
        }

        if (results != null)
        {
            foreach (var item in results)
                Console.WriteLine(item);
        }
        Console.ReadKey();
    }

    Task<string[]> GetWordCounts(string[] urls, string name, CancellationToken token)
    {
        TaskCompletionSource<string[]> tcs = new TaskCompletionSource<string[]>();
        WebClient[] webClients = new WebClient[urls.Length];

        // If the user cancels the CancellationToken, then we can use the 
        // WebClient's ability to cancel its own async operations.
        token.Register(() =>
            {
                foreach (var wc in webClients)
                {
                    if (wc != null)
                        wc.CancelAsync();
                }                    
            });

        object m_lock = new object();
        int count = 0;
        List<string> results = new List<string>();
        for (int i = 0; i < urls.Length; i++)
        {
            webClients[i] = new WebClient();

            #region callback
            // Specify the callback for the DownloadStringCompleted 
            // event that will be raised by this WebClient instance.
            webClients[i].DownloadStringCompleted += (obj, args) =>
             {
                 if (args.Cancelled == true)
                 {
                     tcs.TrySetCanceled();
                     return;
                 }
                 else if (args.Error != null)
                 {
                     // Pass through to the underlying Task 
                     // any exceptions thrown by the WebClient 
                     // during the asynchronous operation.
                     tcs.TrySetException(args.Error);
                     return;
                 }
                 else
                 {
                     // Split the string into an array of words, 
                     // then count the number of elements that match 
                     // the search term. 
                     string[] words = null;
                     words = args.Result.Split(' ');
                     string NAME = name.ToUpper();
                     int nameCount = (from word in words.AsParallel()
                                      where word.ToUpper().Contains(NAME)
                                      select word)
                                     .Count();

                     // Associate the results with the url, and add new string to the array that  
                     // the underlying Task object will return in its Result property.
                     results.Add(String.Format("{0} has {1} instances of {2}", args.UserState, nameCount, name));
                 }

                 // If this is the last async operation to complete, 
                 // then set the Result property on the underlying Task. 
                 lock (m_lock)
                 {
                     count++;
                     if (count == urls.Length)
                     {
                         tcs.TrySetResult(results.ToArray());
                     }
                 }
             };
            #endregion

            // Call DownloadStringAsync for each URL.
            Uri address = null;
            try
            {
                address = new Uri(urls[i]);
                // Pass the address, and also use it for the userToken  
                // to identify the page when the delegate is invoked.
                webClients[i].DownloadStringAsync(address, address);
            }

            catch (UriFormatException ex)
            {
                // Abandon the entire operation if one url is malformed. 
                // Other actions are possible here.
                tcs.TrySetException(ex);
                return tcs.Task;
            }
        }

        // Return the underlying Task. The client code 
        // waits on the Result property, and handles exceptions 
        // in the try-catch block there. 
        return tcs.Task;
    }
}
