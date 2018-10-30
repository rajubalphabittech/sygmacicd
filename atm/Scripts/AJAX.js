var gPendingRequests = 0;
var gPendingRequestsMsg = 'There are still pending updates.  Exiting this page may cause them to not complete.';

function AddAJAXRequest() {
    gPendingRequests++;
}

function RemoveAJAXRequest() {
    gPendingRequests--;
}

function ArePendingAJAXRequests() {
    return (gPendingRequests > 0);
}

function SetPendingAJAXRequestMessage(message) {
    gPendingRequestsMsg = message;
}

onbeforeunload = function() {
    if (ArePendingAJAXRequests())
        return gPendingRequestsMsg;
}
