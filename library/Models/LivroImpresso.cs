namespace library.Models
{
    public class LivroImpresso : Livro
    {
        public float Peso { get; set; }
        public int? TipoEncadernacaoID { get; set; }
        public TipoEncadernacao TipoEncadernacao { get; set; }
    }
}
