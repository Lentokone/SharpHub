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
        public Repository CreateRepository(string repositoryName, string owner, string description)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner))
            {
                throw new ArgumentException("Repository name, owner, and path cannot be null or empty.");
            }

            // This is for when we will have a viewmodel for creating repos.
            var RepoInput = new CreateRepoInput(repositoryName, owner, description);
            // Nonni. Pikkusen infoa.
            // RepositoryPath on polku palvelimella, jossa repo sijaitsee.
            // Ihan hyvä syöttö koopilot
            // Jatketaan
            // Eli repo path on polkua esim /var/repos/username/reponame.git
            // Eli ei anneta käyttäjän itse määritellä sitä.
            // Vaan tehdään se annetulla reponame ja ownerilla.
            // Esim. /var/repos/owner/repositoryName.git

            var repositoryPath = $"{REPO_BASE_PATH}/{RepoInput.Owner}/{RepoInput.RepositoryName}.git";

            var newRepo = new Repository(RepoInput.RepositoryName, RepoInput.Owner, RepoInput.Description, repositoryPath);
            MongoManipulator.Save(newRepo);
            return newRepo;
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
