namespace library.Models.Dto
{
    public class LivroDto
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime Lancamento { get; set; }
        public List<Tag> Tags { get; set; }

        // Campos adicionais para suportar livros digitais e impressos
        public bool IsDigital { get; set; }
        public string Formato { get; set; }  // Apenas para livros digitais
        public bool IsImpressed { get; set; }
        public float Peso { get; set; }     // Apenas para livros impressos
        public int? TipoEncadernacaoID { get; set; } // Chave estrangeira para TipoEncadernacao

        public LivroDto()
        {
            Codigo = 0;
            Titulo = string.Empty;
            Autor = string.Empty;
            Lancamento = DateTime.MinValue;
            Tags = new List<Tag>();
            IsDigital = false;
            Formato = string.Empty;
            IsImpressed = false;
            Peso = 0;
            TipoEncadernacaoID = null;
        }
    }
}
