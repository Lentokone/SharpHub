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

            //NOTE Remember to nuke the second List when done.

            if (owner != null)
            {
                repos = GetListOfRepositories(owner);
                if (repos is { Count: > 0})
                {
                    foreach (Repository r in repos)
                    {
                        if (!r.IsDeleted)
                            repositories.Add(r);
                    }
                }
            }
            var vm = new RepositoryManagerViewModel
            {
                Username = owner ?? "Unknown",
                RepositoryDetailsViewModel = null,
                RepositoryListViewModel = new RepositoryListViewModel { Repositories = repositories ?? new List<Repository>() }
            };

            return View(vm);
        }

        [HttpGet("{username}/{repositoryName}")]
        public IActionResult RepoDetails(string username, string repositoryName)
        {
            var repositoryToShow = MongoManipulator.SearchRepositoryByName(repositoryName, username);
            if (repositoryToShow is null)
            {
                TempData["error"] = "No such repository found";
                return RedirectToAction("Index", "Home");
            }
            var vm = new RepositoryManagerViewModel
            {
                Username = username,
                IsRepoDetails = true,
                RepositoryDetailsViewModel = new RepositoryDetailsViewModel { Username = username, ObjectId = repositoryToShow._id.ToString(), CurrentRepository = repositoryToShow },
                RepositoryListViewModel = null
            };
            return View("Index", vm);
        }

        public Repository CreateRepositoryCore(string repositoryName, string owner, string description)
        {
            if (string.IsNullOrWhiteSpace(repositoryName) || string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("Invalid repository input.");
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
            // 26/02/2026. On varmaan tärkeä. Oletan että tuo tekee sen Bare repository, palvelimelle
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
        // Tulevaisuuden arvoikas Olli.
        // Muista tarkistaa mitä kirjoitat kun olet humalassa.
        // Ettet unohda mitään tyhmää.
        //
        // 26/02/2026
        // Ei ole reedundantti
        // Tärkeä kai, emt, mutta on käytössä
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

        // ?
        /* ? REDUNDANT?*/
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
    }
}
