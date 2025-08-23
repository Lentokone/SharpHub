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
        public List<T> GetListOfRepositories<T>(string owner)
        {
            // Tässä haetaan kaikki repositoriot
            // MongoManipulator.SearchAll<Repository>();

            // Tähän vaan joku super solid idea siitä että x henkilön repositoriot palautetaan.
            // Onnea matkaan tulevan henkilö joka koskee tähän.
            


            // Onnea matkaan minä
            var list = new List<T>();
            return list;
        }

        public void deleteRepository(Repository repository)
        {
            repository.IsDeleted = true;
            repository.DeletedAt = DateTime.UtcNow;
        }
    }
}
