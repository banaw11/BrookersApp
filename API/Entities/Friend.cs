namespace API.Entities
{
    public class Friend
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int FriendId { get; set; }
        public AppUser FriendUser { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
