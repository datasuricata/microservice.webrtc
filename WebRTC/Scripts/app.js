// Global variables
var apiKey = "YOUR_API_KEY";
var sessionId = "YOUR_SESSION_ID";
var token = "YOUR_TOKEN";

// Setting up authentication from server (ASP View or other engine)
function setAttributes(key, id, hash) {
    apiKey = key;
    sessionId = id;
    token = hash;
}

// Connecting to the session and creating a publisher
// Handling all of our errors here by alerting them
function handleError(error) {
    if (error) {
        alert(error.message);
        console.log(error.message);
    }
}

function initializeSession() {
    var session = OT.initSession(apiKey, sessionId);

    // Subscribe to a newly created stream
    session.on('streamCreated', function (event) {
        session.subscribe(event.stream, 'subscriber', {
            insertMode: 'append',
            width: '100%',
            height: '100%'
        }, handleError);
    });

    // Create a publisher
    var publisher = OT.initPublisher('publisher', {
        insertMode: 'append',
        width: '100%',
        height: '100%'
    }, handleError);

    // Connect to the session
    session.connect(token, function (error) {
        // If the connection is successful, publish to the session
        if (error) {
            handleError(error);
        } else {
            session.publish(publisher, handleError);
        }
    });
}