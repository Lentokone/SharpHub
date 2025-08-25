using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;

namespace SharpHub.Controllers
{
    public class RepositoryManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Nonni
        // Tänne siis tulee kaikki repositoryihin liittyvät toiminnot.

        // Eli luodaan uusi repo, haetaan repo, poistetaan repo, jne.
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
                return MongoManipulator.SearchAll(owner);
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
