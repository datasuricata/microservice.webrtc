using Newtonsoft.Json;
using OpenTokSDK;
using System;
using System.Collections.Generic;
using WebRTC.Service.Validators;

namespace WebRTC.Models {
    public class Project {

        #region - attributes -

        public string Id { get; set; }
        public int Key { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<Stream> Sessions { get; set; } = new List<Stream>();

        [JsonIgnore]
        public OpenTok Server { get; set; }

        #endregion

        #region - ctor -

        public Project(int key, string secret, string name) {
            Id = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            CreatedAt = DateTimeOffset.UtcNow;
            Secret = secret;
            Name = name;
            Key = key;

            Validate();

            Server = new OpenTok(Key, Secret);
        }

        #endregion

        #region - methods -

        public void ChangeProjectConfig(int key, string secret, string name) {
            Key = key <= 0
                ? Key : key;

            Secret = string.IsNullOrEmpty(secret)
                ? Secret : secret;

            Name = string.IsNullOrEmpty(name)
                ? Name : name;
        }

        //public void StopAllBroadcasts() {
        //    Sessions.ForEach(s => {
        //        s.StopBroadcast();
        //    });
        //}

        #endregion

        private void Validate() {
            Validator.When(Key <= 0,
                "Informe ao menos uma chave de API");

            Validator.When(string.IsNullOrEmpty(Secret),
                "Token secreto é obrigatório");

            Validator.When(string.IsNullOrEmpty(Name),
                "Informe um nome ao projeto");
        }
    }
}