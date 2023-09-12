namespace GameLibraryRestApi.Data.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Genres { get; set; }
        public string Developer { get; set; }

        public GameModel()
        {
            Id = -1;
            Name = String.Empty;
            Description = String.Empty;
            Genres = new string[0];
            Developer = String.Empty;
        }
    }
}
