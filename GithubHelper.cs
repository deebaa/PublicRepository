using Octokit;
using Quartz;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PublicRepository
{
    public class GithubHelper 
    {
        ApplicationConfig config;
        DBHelper dbHelper;

        public GithubHelper()
        {
            config = new ApplicationConfig();
            dbHelper = new DBHelper();
        }

        public string GetURI(string username)
        {
            string apiUrl = config.apiURL.Replace("{username}", username);
            return apiUrl;
        }

        public async Task<List<string>> GetPublicRepositories(User user, string apiUrl)
        {
            List<string> repositories = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("C# App");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var repos = await response.Content.ReadFromJsonAsync<List<Repository>>();
                    repositories = repos.ConvertAll(repo => repo.Name);
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve repositories for {user.user}. Status code: {response.StatusCode}");
                }
            }

            return repositories;
        }

        public void PrintUserRepos(Dictionary<string, List<string>> dictUserRepos)
        {
            foreach (var userRepos in dictUserRepos)
            {
                if (userRepos.Value != null && userRepos.Value.Count > 0)
                {
                    Console.WriteLine($"\n*****Public repositories of user {userRepos.Key}*****");
                    userRepos.Value.ForEach(repository => Console.WriteLine(repository));                   
                }
                else
                {
                    Console.WriteLine($"\nNo repositories found for user {userRepos.Key}");
                }                
            }

        }



    }
}
