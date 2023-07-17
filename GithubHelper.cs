using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PublicRepository
{
    public class GetUserPublicReposfromGithub : IJob
    {
        ApplicationConfig config = new ApplicationConfig();
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("GetUserPublicRepos Job started--------------");

            List<string> repositories = await GetPublicRepositories(config);

            if (repositories != null && repositories.Count > 0)
            {
                Console.WriteLine($"Public repositories of user {config.username}:");
                foreach (string repository in repositories)
                {
                    Console.WriteLine(repository);
                }
            }
            else
            {
                Console.WriteLine($"No repositories found for user {config.username}");
            }
        }

        static async Task<List<string>> GetPublicRepositories(ApplicationConfig config)
        {
            List<string> repositories = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("C# App");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.token);

                string apiUrl = config.apiURL.Replace("{username}", config.username);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var repos = await response.Content.ReadFromJsonAsync<List<Repository>>();
                    repositories = repos.ConvertAll(repo => repo.Name);
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve repositories. Status code: {response.StatusCode}");
                }
            }

            return repositories;
        }
    }
}
