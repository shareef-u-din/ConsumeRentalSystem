using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace RentalSystem.Helper
{
    public static class ApiHelper
    {
        /// <summary>
        /// Used to get IEnumerable<T> data from api of the generic class
        /// </summary>
        /// <param name="url">api url</param>
        /// <param name="controllerAndOrAction">The remaining part of api url like action and/or controller</param>
        public static IEnumerable<T> GetDataFromApi<T>(string url, string controllerAndOrAction)
        {
            IEnumerable<T> list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    //client
                    var responseTask = client.GetAsync(controllerAndOrAction);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<T>>();
                        readTask.Wait();

                        list = readTask.Result;
                    }
                    else //web api sent error response
                    {
                        //log response status here..

                        list = Enumerable.Empty<T>();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Exception Inside ApiHelper Class in GetDataFromApi Method for Class:" + typeof(T), e);
            }
            return list;
        }

        /// <summary>
        /// Used to  get object from api of the generic class type <T>
        /// </summary>
        /// <param name="url">api url</param>
        /// <param name="controllerAndOrAction">The remaining part of api url like action and/or controller</param>
        public static T GetFromApi<T>(T Tobject, string url, string controllerAndOrAction)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    //HTTP GET
                    //client
                    var responseTask = client.GetAsync(controllerAndOrAction);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<T>();
                        readTask.Wait();

                        Tobject = (T)readTask.Result;
                    }
                    else //web api sent error response
                    {
                        //log response status here..

                        Tobject = default(T);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Exception Inside ApiHelper Class in GetFromApi Method for Class:" + typeof(T), e);
            }
            return Tobject;
        }

        /// <summary>
        /// Used to post object of generic class<T> to api
        /// </summary>
        /// <param name="Tobject">The generic object</param>
        /// <param name="url">api url</param>
        /// <param name="controllerAndOrAction">The remaining part of api url like action and/or controller</param>
        public static T Add<T>(T Tobject, string url, string controllerAndOrAction)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    //HTTP  Post
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage httpResponseMessage = client.PostAsJsonAsync(url + controllerAndOrAction, Tobject).Result;

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var readTask = httpResponseMessage.Content.ReadAsAsync<T>();
                        readTask.Wait();

                        Tobject = (T)readTask.Result;
                    }
                    else //web api sent error response
                    {
                        //log response status here..

                        Tobject = default(T);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Exception Inside ApiHelper Class in Add Method for Class:" + typeof(T), e);
            }
            return Tobject;

        }

    }
}