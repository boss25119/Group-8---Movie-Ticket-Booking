namespace FilmTicketReservation.Models
{
    public enum Seat
    {
        A1, A2, A3, A4, A5,B1,B2,B3,B4,B5,C1,C2,C3,C4,C5

    }
    public class OrderDetail
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public decimal MoviePrice { get; set; }
        public Seat? Seat { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<Movie> Movies;

        public Movie Movie { get; set; }
       
    }
}
