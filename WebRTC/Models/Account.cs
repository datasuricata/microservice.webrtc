using System;
using System.Collections.Generic;
using WebRTC.Helpers;
using WebRTC.Service.Validators;

namespace WebRTC.Models {
    public class Account {

        #region - attributes -

        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Project { get; set; }
        public UserRole Role { get; set; }

        #endregion

        #region - ctor -
        public Account(string email, string password, UserRole role) {
            Id = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            CreatedAt = DateTimeOffset.UtcNow;
            Email = email;
            Password = password.Encrypt();
            Role = role;

            Validate();
        }
        #endregion

        public void LinkAccount(string projectId) {
            Project = projectId;
        }

        private void Validate() {
            Validator.When(string.IsNullOrEmpty(Email),
                "E-mail é obrigatório");

            Validator.When(string.IsNullOrEmpty(Password),
                "Senha é obrigatória");
        }
    }
}