namespace library.Models
{
    public class LivroDigital : Livro
    {
        public string Formato { get; set; }

        public LivroDigital() { 
            Formato = string.Empty;
        }
    }
}
