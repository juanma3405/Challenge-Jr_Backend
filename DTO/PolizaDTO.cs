namespace AdminPolizasAPI.DTO
{
    public class PolizaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<PolizasCoberturasDTO> PolizasCoberturas { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
