using Contract.Enum;

namespace Contract.Model
{
    public class MovieType
    {
        // KLASA GATUNKOW FILMOW
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public MOVIE_TYPE Type { get; set; }
        public string Description { get; set; }
    }
}
