using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace DevOpsDeploy
{
    public class ReleaseRetention
    {
        private static readonly string _environmentsfilename   = @"\Environments.json";
        private static readonly string _projectsfilename       = @"\Projects.json";
        private static readonly string _intputDirectoryPath = @"C:\Repos\DevOpsDeploy\input";
        private static readonly string _deploymentsfilename = @"\Deployments.json";
        private static readonly string _releasesfilename = @"\Releases.json";

        public static void GetRetained(int releaseRetention)
        {

            var deploymentsList = GetDeploymentItems();
            //Load Projects
            var releasesList = GetReleaseItems();

            // Order and Join Deployments to Projects
            // Then Group By Projects to Environments
            var releaseData = deploymentsList.Join(releasesList,
                deployments => deployments.ReleaseId,
                releases => releases.Id,
                (deployments, releases) => new
                {
                    Project = releases.ProjectId,
                    Environment = deployments.EnvironmentId,
                    Release = releases.Id,
                    Deployment = deployments.Id,
                    DeployTime = deployments.DeployedAt

                }).OrderByDescending(p => p.Project)
                .ThenByDescending(e => e.Environment)
                .ThenByDescending(t => t.DeployTime)
                .GroupBy(x => (x.Project, x.Environment))
                .SelectMany(x => x.Take(releaseRetention));

            // Output Results
            Console.WriteLine("#-------------------------#");
            Console.WriteLine("    Retained Releases      ");
            Console.WriteLine("#-------------------------#");
            foreach (var release in releaseData)
            {
                Console.WriteLine(release);
            }
        }

        private static JArray GetJsonArrayFromFile(string jsonFile)
        {
            JArray json = null;
            // Ensure file exists
            if (File.Exists(_intputDirectoryPath + jsonFile))
            {
                json = JArray.Parse(File.ReadAllText(_intputDirectoryPath + jsonFile));
            }
            else
            {
                Console.WriteLine("{0}{1} - File Not found", _intputDirectoryPath, jsonFile );
            }
            return json;
        }

        private static IList<Deployment> GetDeploymentItems()
        {
            // Return List of Deployment Array Data
            JArray json = GetJsonArrayFromFile(_deploymentsfilename);
            IList<Deployment> deploymentList = json.ToObject<IList<Deployment>>();  
            return deploymentList;
        }

        private static IList<Release> GetReleaseItems()
        {
            // Return List of Release Array Data
            JArray json = GetJsonArrayFromFile(_releasesfilename);
            IList<Release> releaseList = json.ToObject<IList<Release>>();
            return releaseList;
        }

    }
}

