namespace Contract.Model
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool isActor { get; set; }
        public bool isDirector { get; set; }
        public bool isScenarist { get; set; }
        public bool isPhotographer { get; set; }
        public bool isComposer { get; set; }
        public bool isDescription { get; set; }
    }
}
