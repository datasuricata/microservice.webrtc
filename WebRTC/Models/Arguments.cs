namespace WebRTC.Models {

    public class ProjectRequest {
        public int Key { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
    }

    public class StreamRequest {
        public string Name { get; set; }
        public string ProjectId { get; set; }
    }

    public class AccountRequest {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LinkRequest { 
        public string Email { get; set; }
        public string ProjectId { get; set; }
    }

    public class AccountResponse {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Project { get; set; }
        public UserRole Role { get; set; }
    }

    public class ServerResponse {
        public int ApiKey { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }
        public string Info { get; set; }
    }
}