using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XGMoviesBackEnd.ExternalServices
{
    /// <summary>
    /// Concrete implemenation of the service that will be responsible for obtaining
    /// MovieId given year and title.
    /// </summary>
    public class TheMovieDbOrgService : IMovieIDResolutionService
    {
        String _apiKey;

        public TheMovieDbOrgService()
        {
            var key = File.ReadLines("d:\\TheMovieDbOrgKey.txt").First();
            _apiKey = key;
        }

        public async Task<int> GetMovieIdAsync(string title, ushort year)
        {
            int retValue = 0;
            using (var client = CreateTheMovieDbOrgClient())
            {
                var encodedTitle = HttpUtility.UrlEncode(title);
                var requestUrl = String.Format($"https://api.themoviedb.org/3/search/movie?query={encodedTitle}&year={year}&api_key={_apiKey}");

                // ConfigureAwait() need for libraries used in an ASP.Net context to ensure proper
                // functioning of SycnContext
                // http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async
                var response = await client.GetAsync(requestUrl).ConfigureAwait(false); 
                var data = await response.Content.ReadAsAsync<TheMovieDbOrgSearchMovieResponse>();
                var firstRecord = data.results.FirstOrDefault();
                if ( firstRecord == null)
                {
                    throw new ArgumentException("Unable to find movie id");
                }

                retValue = firstRecord.Id;
            }

            return retValue;
        }


        /// <summary>
        /// Construct relevant HttpClient with headers for interacting with TheMovieDbOrg
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateTheMovieDbOrgClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));            
            
            return client;
        }
    }
}
