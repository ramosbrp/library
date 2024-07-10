namespace library.Models
{
    public class LivroImpresso : Livro
    {
        public float Peso { get; set; }
        public int? TipoEncadernacaoID { get; set; }
        public TipoEncadernacao TipoEncadernacao { get; set; }

        public LivroImpresso() {
            Peso = 0;
            TipoEncadernacao = new TipoEncadernacao();

        }
    }
}
