namespace library.Models
{
    public class Tag
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }

        public Tag() {
            Codigo = 0;
            Descricao = string.Empty;
        }
    }
}
