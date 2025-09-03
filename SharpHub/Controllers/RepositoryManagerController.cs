using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;

namespace SharpHub.Controllers
{
    public class RepositoryManagerController : Controller
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
            return Ok(repository);
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
            string rootedPath = LibGit2Sharp.Repository.Init(repositoryPath, true);
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
