using SandboxConsole.Trees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using SandboxConsole.Lock;
namespace SandboxConsole
{
    class WebDataDownloader
    {
        static LockObject _sync = new LockObject();
        static void Main()
        {
            var t1 = new Thread(() => { lock (_sync) Thread.Sleep(10000); });
            t1.IsBackground = true;
            t1.Start();

            _sync.Data = 6;

            var t2 = new Thread(() => { 
                lock (_sync) 
                Thread.Sleep(3000);
                _sync.Data = 9; });
            t2.Start();
            Console.ReadLine();
        }



        private static void DeadLock()
        {
            
        }
        private static void ThreadVsTask()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Tree tr = Tree.CreateSomeTree(9, 1);
            var sw = Stopwatch.StartNew();
            WalkTree1(tr);
            Console.WriteLine("Elapsed = " + sw.ElapsedMilliseconds / 1000.0);
            //  Console.ReadLine();

            sw.Restart();

            WalkTree2(tr);
            Console.WriteLine("Elapsed = " + sw.ElapsedMilliseconds / 1000.0);
            Console.ReadLine();
        }

        static void WalkTree1(Tree tr)
        {

            if (tr == null) return;

            Thread left = new Thread(() => WalkTree1(tr.Left));
            Thread right = new Thread(() => WalkTree1(tr.Right));
            left.Start();
            right.Start();

            left.Join();
            right.Join();
            ProcessItem(tr.Data);
        }

        static void WalkTree2(Tree tr)
        {

            if (tr == null) return;

            Task left = new Task(() => WalkTree2(tr.Left));
            Task right = new Task(() => WalkTree2(tr.Right));
            left.Start();
            right.Start();

            left.Wait(); right.Wait();
            //Task.WaitAll(new Task[] { left, right });
            

            ProcessItem(tr.Data);

        }

        private static int ProcessItem (int treeData)
        {
            Thread.SpinWait(4000000);
            return treeData;
        }
        private static void DowloadTest()
        {
            WebDataDownloader downloader = new WebDataDownloader();
            string[] addresses = { "http://www.msnbc.com", "http://www.yahoo.com",
                                 "http://www.nytimes.com", "http://www.washingtonpost.com",
                                 "http://www.latimes.com", "http://www.newsday.com" };
            CancellationTokenSource cts = new CancellationTokenSource();
            string keyword = "the";
            // Create a UI thread from which to cancel the entire operation
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Press c to cancel");
                while (true)
                {
                    if (Console.ReadLine().ToString().Equals("c", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cts.Cancel();
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        keyword = Console.ReadLine();
                    }
                }
            });

            // Using a neutral search term that is sure to get some hits.
            Task<string[]> webTask = downloader.GetWordCounts(addresses, keyword, cts.Token);

            // Do some other work here unless the method has already completed. 
            //if (!webTask.IsCompleted)
            //{
            //    // Simulate some work.
            //    Thread.SpinWait(5000000);
            //}

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

        //async string[] Download ()
        //{
        //    await GetWordCounts
        //}
        async Task<string[]> GetWordCounts(string[] urls, string name, CancellationToken token)
        {
            TaskCompletionSource<string[]> tcs = new TaskCompletionSource<string[]>();
            var taskList = new List<Task>(10);
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
           // int count = 0;
            ConcurrentQueue<string> results = new ConcurrentQueue<string>();
            for (int i = 0; i < urls.Length; i++)
             //    Parallel.ForEach<string>(urls, url =>
                {
                    webClients[i] = new WebClient();
                    string url = urls[i];
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
                             string[] words = args.Result.Split(' ');
                             string NAME = name.ToUpper();
                             int nameCount = (from word in words.AsParallel()
                                              where word.ToUpper().Contains(NAME)
                                              select word)
                                             .Count();

                             // Associate the results with the url, and add new string to the array that  
                             // the underlying Task object will return in its Result property.
                             results.Enqueue(String.Format("{0} has {1} instances of {2}", url, nameCount, name));//args.UserState
                         }

                         // If this is the last async operation to complete, 
                         // then set the Result property on the underlying Task. 
                         //lock (m_lock)
                         //{
                         //    count++;
                         //    if (count == urls.Length)
                         //    {
                         //        tcs.TrySetResult(results.ToArray());
                         //    }
                         //}
                         //tcs.TrySetResult(results.ToArray());
                     };
                    #endregion

                    // Call DownloadStringAsync for each URL.
                    Uri address = null;
                    try
                    {
                        address = new Uri(urls[i]);
                        // Pass the address, and also use it for the userToken  
                        // to identify the page when the delegate is invoked.
                        taskList.Add(webClients[i].DownloadStringTaskAsync(address));//.DownloadStringAsync(address, address);
                        //    await Task.Factory.ContinueWhenAll(taskList.ToArray(),
                        //                      s => { tcs.TrySetResult(results.ToArray()); });//.ConfigureAwait(false);
                        //tcs.TrySetResult(results.ToArray());
                    }
                    catch (UriFormatException ex)
                    {
                        // Abandon the entire operation if one url is malformed. 
                        // Other actions are possible here.
                        tcs.TrySetException(ex);
                        //  return tcs.Task;
                    }
                } //for loop
             //   );
            await Task.WhenAll(taskList.ToArray()).ConfigureAwait(false);

            tcs.TrySetResult(results.ToArray());
            // Return the underlying Task. The client code 
            // waits on the Result property, and handles exceptions 
            // in the try-catch block there. 
            return  tcs.Task.Result;
        }
    }
}
