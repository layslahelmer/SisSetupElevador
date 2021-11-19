namespace Elevador
{
    public class Acao
    {
        // Identificação da ação do elevador andar
        public int Id { get; set; }

        // Identiificação dos andares
        public Andar Andar { get; set; }
        public int Ordem { get; set; }
    }
}
