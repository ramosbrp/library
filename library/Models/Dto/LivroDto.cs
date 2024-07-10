namespace library.Models.Dto
{
    public class LivroDto
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }

        public string Autor { get; set; }
        public DateTime Lancamento { get; set; }
        public List<Tag> Tags { get; set; }

        public LivroDto() {
            Codigo = 0;
            Titulo = string.Empty;
            Autor = string.Empty;
            Lancamento = DateTime.MinValue;
            Tags = new List<Tag>();
        }
    }
}
