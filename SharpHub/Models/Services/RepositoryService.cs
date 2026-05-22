using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpHub.Models.Services
{
    public static partial class RepositoryService
    {
        private const string REPO_BASE_PATH = "/var/sharphub/repos";

        public static Repository CreateRepositoryCore(string repositoryName, string owner, string description)
        {
            if (string.IsNullOrWhiteSpace(repositoryName) || string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("Invalid given input.");
            }
            repositoryName = repositoryName.Trim();
            if (!ValidRepoNameRegex().IsMatch(repositoryName) || !ValidRepoNameRegex().IsMatch(owner))
            {
                throw new ArgumentException("Repository and owner strings did not pass Regex");
            }

            if (MongoManipulator.RepositoryExists(owner, repositoryName))
            {
                throw new InvalidOperationException("Repository name already exists for this owner.");
            }

            var ownerPath = Path.Combine(REPO_BASE_PATH, owner);
            var repositoryPath = Path.Combine(ownerPath, $"{repositoryName}.git");

            if (!Directory.Exists(ownerPath))
            {
                Directory.CreateDirectory(ownerPath);
            }

            if (Directory.Exists(repositoryPath))
            {
                throw new InvalidOperationException("Repository path already exists.");
            }
            // Tarkeä?
            string rootedPath = LibGit2Sharp.Repository.Init(repositoryPath, true);

            var newRepo = new Repository(repositoryName, owner, description, repositoryPath);
            MongoManipulator.Save(newRepo);
            return newRepo;
        }


        [GeneratedRegex("^[a-zA-Z0-9_-]{1,100}$")]
        private static partial Regex ValidRepoNameRegex();
    }
}
