using OpenTokSDK;
using System;
using System.Linq;
using WebRTC.Service.Validators;

namespace WebRTC.Models {
    public class Stream {

        #region - attributes -

        public string Id { get; set; }
        public string SessionReferenceId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public Project Project { get; set; }

        #endregion

        #region - ctor -

        public Stream(string name, Project project) {
            Id = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            CreatedAt = DateTimeOffset.UtcNow;
            Project = project;
            Name = name;

            Validate();
        }
        public Stream() {
        }

        #endregion

        #region - methods -

        public void StartSession(string ip) {
            var session = Project.Server.CreateSession();
            //var session = Project.Server.CreateSession(ip, OpenTokSDK.MediaMode.ROUTED, OpenTokSDK.ArchiveMode.MANUAL);
            SessionReferenceId = session.Id;
            IpAddress = ip;
        }
        public void StopSession() {
            SessionReferenceId = null;
            IpAddress = null;
        }
        public string GenerateToken(Role role) {

            Validator.When(string.IsNullOrEmpty(SessionReferenceId),
                "Não existe sessão ativa, por favor consulte o administrador.");
            
            return Project.Server.GenerateToken(SessionReferenceId, role);
        }

        #endregion

        private void Validate() {
            Validator.When(Project == null,
                "Vincule com algum projeto");

            Validator.When(string.IsNullOrEmpty(Name),
                "Informe um nome para sessão");
        }
    }
}