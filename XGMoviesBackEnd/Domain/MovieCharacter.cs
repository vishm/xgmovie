namespace XGMoviesBackEnd.Domain
{
    public class MovieCharacter
    {
        public int id { get; set; }
        public string Name { get; set; }
        public Actor PlayedBy { get; set; }
    }
}