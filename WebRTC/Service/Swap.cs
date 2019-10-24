using System;
using System.Collections.Generic;
using System.Linq;
using WebRTC.Helpers;
using WebRTC.Models;
using WebRTC.Service.Validators;

namespace WebRTC.Service {
    public sealed class Swap {

        private static object _lock = new object();
        private static readonly List<Project> _storage = new List<Project>();
        private static readonly List<Account> _session = new List<Account>();

        #region - project -

        public void AddProject(int key, string secret, string name) {
            var project = new Project(key, secret, name);

            lock (_lock) {
                _storage.Add(project);
            }
        }
        public void RemoveProject(string id) {
            var project = GetProjectById(id);

            lock (_lock) {
                //project.StopAllBroadcasts();
                _storage.Remove(project);
            }
        }
        public IEnumerable<Project> ListPorjects() => _storage;
        public Project GetProjectById(string id) => _storage.FirstOrDefault(x => x.Id == id);
        public Project GetProjectBySessionId(string id) => _storage.FirstOrDefault(x => x.Sessions.Any(a => a.Id == id));

        #endregion

        #region - session -

        public void AddStream(string id, string name) {
            var project = GetProjectById(id);
            var session = new Stream(name, project);

            lock (_lock) {
                project.Sessions.Add(session);
            }
        }
        public void RemoveStream(string id) {
            var session = GetStreamById(id);

            lock (_lock) {
                GetProjectBySessionId(id).Sessions.Remove(session);
            }
        }
        public void StartSession(string id, string ip) {
            var session = GetStreamById(id);

            lock (_lock) {
                if (string.IsNullOrEmpty(session.SessionReferenceId)) {
                    session.StartSession(ip);
                }
            }
        }
        public void StopSession(string id) {
            var session = GetStreamById(id);

            lock (_lock) {
                session.StopSession();
            }
        }
        public IEnumerable<Stream> ListStreams() => _storage.SelectMany(x => x.Sessions);
        public IEnumerable<Stream> ListStreamsActives() => _storage.SelectMany(x => x.Sessions).Where(s => !string.IsNullOrEmpty(s.SessionReferenceId));
        public IEnumerable<Stream> ListStreamsByProjectId(string id) => _storage.Where(x => x.Id == id).SelectMany(x => x.Sessions);
        public Stream GetStreamById(string id) => _storage.SelectMany(x => x.Sessions).FirstOrDefault(x => x.Id == id);
        public string Publish(string id) {
            var session = GetStreamById(id);

            lock (_lock) {
                return session.GenerateToken(OpenTokSDK.Role.PUBLISHER);
            }
        }
        public string Subscribe(string id) {
            var session = GetStreamById(id);

            lock (_lock) {
                return session.GenerateToken(OpenTokSDK.Role.SUBSCRIBER);
            }
        }

        #endregion

        #region - account -

        public void AddAccount(string email, string password, string hash, string bind) {
            if (hash.Encrypt().Equals(bind, StringComparison.InvariantCultureIgnoreCase)) {
                var account = new Account(email, password, UserRole.Client);

                Validator.When(AccountExist(email),
                    "Já existe uma conta com este e-mail cadastrado, contate o suporte");

                lock (_lock) {
                    _session.Add(account);
                }
            }
        }
        public void RemoveAccount(string id) {
            var account = GetAccountById(id);

            lock (_lock) {
                _session.Remove(account);
            }
        }
        public void LinkAccount(string projectId, string email, string hash, string bind) {
            if (hash.Encrypt().Equals(bind, StringComparison.InvariantCultureIgnoreCase)) {
                var account = GetAccountByEmail(email);

                lock (_lock) {
                    account.LinkAccount(projectId);
                }
            }
        }
        public void Seeder(string hash, string bind) {
            if (hash.Encrypt().Equals(bind, StringComparison.InvariantCultureIgnoreCase)) {

                var accounts = new List<Account> {
                    new Account("admin@datasuricata.br", "1q2w3e4r5t!!", UserRole.Admin),
                    new Account("client@datasuricata.br", "xEY8m5dpyA8cYL", UserRole.Client)
                };

                accounts.ForEach(x => x.Password.Encrypt());

                lock (_lock) {
                    _session.AddRange(accounts);
                }
            }
        }
        public IEnumerable<Account> ListAccounts() => _session;
        public Account Authorize(string email, string password) {
            var account = GetAccountByEmail(email);

            Validator.When(account == null, 
                "Usuário não encontrado");

            Validator.When(!account.Password.Equals(password, StringComparison.InvariantCultureIgnoreCase), 
                "Senha não confere");

            return account;
        }
        public Account GetAccountById(string id) => _session.FirstOrDefault(x => x.Id == id);
        public Account GetAccountByEmail(string email) => _session.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        public bool AccountExist(string email) => _session.Exists(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

        #endregion
    }
}