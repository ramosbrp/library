namespace library.Models
{
    public class TipoEncadernacao
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Formato { get; set; }

        public TipoEncadernacao() {
            Codigo = 0;
            Nome = string.Empty;
            Descricao = string.Empty;
            Formato = string.Empty;
        }
    }
}
