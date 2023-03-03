var _results = []
var _communicationUrl;

communicateSuccessfulCallToAsyncFunction = {
    'success': true,
};

function initializeFullInteroperability(communicationUrl) {
    _communicationUrl = communicationUrl;
}

function setResult(id, result) {
    console.log(_communicationUrl);
    _results[id] = {
        result: result
    }

    window.location = `${_communicationUrl}?${id}`;
}

function setError(id, error) {
    _results[id] = {
        error: error
    }

    window.location = `${_communicationUrl}?${id}`;
}

function getResult(id) {
    try {
        var numId = Number(id);
        var requestedResult = _results[numId];
        delete _results[numId]
        return requestedResult;
    } catch (ex) {
        return {
            'error': JSON.stringify(ex.toString())
        }
    }
}
