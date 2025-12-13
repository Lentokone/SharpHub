using LibGit2Sharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;
using Repository = SharpHub.Models.Repository;

namespace SharpHub.Controllers
{
    [Authorize]
    [Route("RepositoryManager")]
    public class RepositoryManagerController : Controller
    {
        private const string REPO_BASE_PATH = "/var/sharphub/repos";
        [HttpGet("")]
        public IActionResult Index()
        {
            var owner = User.Identity?.Name;
            List<Repository>? repos;
            List<Repository> repositories = new();
            // List<Repository> repositories = new List<Repository>();
            
            if (owner != null)
            {
                repos = GetListOfRepositories(owner);
                if (repos is { Count: > 0})
                {
                }
            }
            // Info for future me.
            // After removing the testing jumble.
            // The real repositories list is called repos. The one above, set to fetch from the database.
            if (repositories != null)
            {
                // This is for testing. Remember to remove.
                repositories.Add(new Repository(
                    repositoryName: "SharpGit",
                    owner: "test",
                    description: "A C# Git wrapper",
                    repositoryPath: "/repos/sharpgit"
                ));

                repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "The MVC frontend for SharpGit",
                    repositoryPath: "/repos/sharphub"
                )); repositories.Add(new Repository(
                    repositoryName: "SharpHub",
                    owner: "test",
                    description: "",
                    repositoryPath: "/repos/sharphub"
                ));
            }
            var vm = new RepositoryManagerViewModel
            {
                Username = owner ?? "Unknown",
                RepositoryDetailsViewModel = null,
                RepositoryListViewModel = new RepositoryListViewModel { Repositories = repositories ?? new List<Repository>() }
            };

            return View(vm);
        }

        //[HttpGet("apina")]
        [HttpGet("{username}/{repositoryName}")]
        public IActionResult RepoDetails(string username, string repositoryName)
        {
            // Tähän järkevä "if/else" joka:
            // Tarkistaa onko /username/repository olemassa
            // Jos on:
            // jep.
            // Jos ei:
            // return Error view tai jtn
            var vm = new RepositoryManagerViewModel
            {
                Username = username,
                IsRepoDetails = true,
                RepositoryDetailsViewModel = new RepositoryDetailsViewModel { Username = username, RepositoryName = repositoryName },
                RepositoryListViewModel = null

                /*
                Username = "test",
                IsRepoDetails = true,
                RepositoryDetailsViewModel = new RepositoryDetailsViewModel { Username = "apina", RepositoryName = "apina" },
                RepositoryListViewModel = new RepositoryListViewModel()
                */
            };
            return View("Index", vm);
        }

        /*
        * Redundant
        */
        public IActionResult _CreateRepo()
        {
            return PartialView();
        }
        /*
        * Redundant?
        */
        public IActionResult _DeleteRepo()
        {
            // Tälle annetaan viewmodel jossa on tietty repository
            // On käytössä vaan RepositoryDetails komponentissa.
            return PartialView();
        }
        /*
        * Redundant?
        */

        [HttpPost]
        public IActionResult CreateRepositoryMVC(CreateRepositoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var owner = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(vm.RepositoryName) || string.IsNullOrWhiteSpace(owner))
                return BadRequest("Required values missing.");
            var repository = CreateRepositoryCore(vm.RepositoryName, owner, vm.Description);
            //return Ok(repository);
            return RedirectToAction("RepoDetails", new { owner, vm.RepositoryName });
        }

        public Repository CreateRepositoryCore(string repositoryName, string owner, string description)
        {
            if (string.IsNullOrWhiteSpace(repositoryName) || string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("Invalid repository input.");
            }
            // var repositoryPath = $"{REPO_BASE_PATH}/{owner}/{repositoryName}.git";
            // var repositoryPath = Path.Combine(REPO_BASE_PATH, owner, $"{repositoryName}.git");

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
            // string rootedPath = LibGit2Sharp.Repository.Init(repositoryPath, true);
            
            var newRepo = new Repository(repositoryName, owner, description, repositoryPath);
            MongoManipulator.Save(newRepo);
            return newRepo;
        }


        /*
         * -----------------
         * TAG OF REDUNDANCY
         * -----------------
         */
        // Tämä ei tule toimimaan
        // Varmaan parempi hakea repo nimellä
        public Repository GetRepository(Repository wantedRepo)
        {
            wantedRepo = MongoManipulator.Search(wantedRepo);
            return wantedRepo;
        }


        /*
         * -----------------
         * TAG OF REDUNDANCY
         * -----------------
         */
        // Tulevaisuuden arvoikas Olli.
        // Muista tarkistaa mitä kirjoitat kun olet humalassa.
        // Ettet unohda mitään tyhmää.
        public List<Repository> GetListOfRepositories(string owner)
        {
            if (string.IsNullOrEmpty(owner))
            {
                throw new ArgumentException("Omistaja ei voi olla null tai tyhjä.");
            }

            try
            {
                return MongoManipulator.SearchAllRepositories(owner);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virhe haettaessa repositorioita omistajalla {owner}: {ex.Message}");
                return [];
            }
        }
        /*
         * -----------------
         * TAG OF REDUNDANCY
         * -----------------
         */

        /*
         * ------------
         * TAG OF MAYBE
         * ------------
         */
        public List<string> GetListOfRepositoryNames(string owner)
        {
            var repos = GetListOfRepositories(owner);
            return [.. repos.Select(r => r.RepositoryName)];
        }




        public IActionResult ChangeRepoDescription(Repository wantedRepo, string newDesc)
        {
            if (string.IsNullOrWhiteSpace(newDesc) || wantedRepo == null)
            {
                return BadRequest("Invalid input.");
            }
            wantedRepo.Description = newDesc;
            MongoManipulator.Save(wantedRepo);
            return Ok();
        }

        public void DeleteRepository(Repository repository)
        {
            repository.IsDeleted = true;
            repository.DeletedAt = DateTime.UtcNow;
        }
    }
}
