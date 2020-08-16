using System;

namespace Models
{
    public class Deployment
    {
        public string Id { get; set; }
        public string ReleaseId { get; set; }
        public string EnvironmentId { get; set; }
        public string DeployedAt { get; set; }
    }

    public class Common
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

	public class Release
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public string Created { get; set; }
    }
}

