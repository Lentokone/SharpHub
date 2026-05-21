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
            List<Repository> repositories = new();

            if (owner != null)
            {
                repositories = GetListOfRepositories(owner);
            }
            var vm = new RepositoryManagerViewModel
            {
                Username = owner ?? "Unknown",
                RepositoryDetailsViewModel = null,
                RepositoryListViewModel = new RepositoryListViewModel { Repositories = repositories }
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
            var owner = User.Identity?.Name;

            if (repositoryToShow.Owner != owner)
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
                var list = MongoManipulator.SearchAllRepositories(owner);
                var cleanList = new List<Repository>();
                foreach (var repo in list)
                    if (!repo.IsDeleted)
                        cleanList.Add(repo);
                return cleanList;
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
