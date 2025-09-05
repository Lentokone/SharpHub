namespace SharpHub.Models
{
    public class REPOMANAGERtestTEST
    {
        public IEnumerable<Repository> Repositories { get; set; } = new List<Repository>();

        public Repository NewRepository { get; set; } = new Repository("Testt", "ttest", "", "jep");
    }
}
