namespace library.Models.Dto
{
    public class LivroDetalhesDto : LivroDto
    {
        public string Autor { get; set; }
        public string Tipo { get; set; } // "Digital" ou "Impresso"
        public string FormatoOrPeso { get; set; } // Poderia ser formato de livro digital ou peso de livro impresso

        public LivroDetalhesDto()
        {
            Autor = string.Empty;
            Tipo = string.Empty;
            FormatoOrPeso = string.Empty;

        }
    }
}
