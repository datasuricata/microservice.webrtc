using System;

namespace WebRTC.Service.Validators {
    public class Validator : Exception {
        public Validator(string message) : base(message) {
        }

        public static void When(bool hasError, string message) {
            if (hasError)
                throw new Validator(message);
        }
    }

    public class MessageValidator {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}