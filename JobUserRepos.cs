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
    public class GetUserPublicReposfromGithub : IJob
    {
        public Dictionary<string, List<string>> dictUserRepos { get; set; }
        public GithubHelper githelper { get; set; }
        public List<User> lstUsers { get; set; }
        public GetUserPublicReposfromGithub()
        {
            githelper = new GithubHelper();
            dictUserRepos = new Dictionary<string, List<string>>();
            lstUsers = new List<User>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("\nGetUserPublicRepos Job started--------------\n");

            JobDataMap jobdatamap = context.JobDetail.JobDataMap;
            lstUsers = (List<User>)jobdatamap["lstusers"];                                   
            
            foreach (User user in lstUsers)
            {                
                List<string> repositories = await githelper.GetPublicRepositories(user, githelper.GetURI(user.user));
                dictUserRepos.Add(user.user, repositories);
            }
            githelper.PrintUserRepos(dictUserRepos);

            await Console.Out.WriteLineAsync("\nGetUserPublicRepos Job ended--------------\n");
        }

        



    }
}
