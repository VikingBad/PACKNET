namespace PACKNET.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime Ultima_Actualizacion { get; set; }

        public ICollection<Paquete> ListaPaquetes { get; set; }
    }
}
    