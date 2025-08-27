using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;

namespace SharpHub.Controllers
{
    public class RepositoryManager : Controller
    {
        private const string REPO_BASE_PATH = "/var/sharphub/repos";
        public IActionResult Index()
        {
            return View();
        }

        // Nonni
        // Tänne siis tulee kaikki repositoryihin liittyvät toiminnot.

        // Eli luodaan uusi repo, haetaan repo, poistetaan repo, jne.

        // Ei valmista vielä.
        [HttpPost]
        public IActionResult CreateRepository(CreateRepositoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var owner = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(vm.RepositoryName) || string.IsNullOrWhiteSpace(owner))
                return BadRequest("Required values missing.");

            var RepoInput = new CreateRepoInput(vm.RepositoryName, owner, vm.Description);

            var repositoryPath = $"{REPO_BASE_PATH}/{RepoInput.Owner}/{RepoInput.RepositoryName}.git";

            var newRepo = new Repository(RepoInput.RepositoryName, RepoInput.Owner, RepoInput.Description, repositoryPath);
            MongoManipulator.Save(newRepo);
            return Ok(newRepo);
        }

        public Repository GetRepository(Repository wantedRepo)
        {
            wantedRepo = MongoManipulator.Search(wantedRepo);
            return wantedRepo;
        }

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

        public void DeleteRepository(Repository repository)
        {
            repository.IsDeleted = true;
            repository.DeletedAt = DateTime.UtcNow;
        }
    }
}
