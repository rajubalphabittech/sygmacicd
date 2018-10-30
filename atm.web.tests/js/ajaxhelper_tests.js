module("getData", function (hooks) {
	hooks.beforeEach(function () {
		this.server = sinon.createFakeServer();
	});

	hooks.afterEach(function () {
		this.server.restore();
	});

	test("getData 200 should call done callback", function () {
		// arrange
		var url = "/routenotification/list?centerNumber=22&filterStartDate=&filterEndDate=";
		this.server.respondWith("GET", url, [200, { "Content-Type": "application/json" }, '[{ "id": 12, "comment": "Hey there" }]']);
		var preCall = sinon.spy(); var doneCallback = sinon.spy(); var failCallBack = sinon.spy(); var alwaysCallBack = sinon.spy();

		// act
		getData(url, preCall, doneCallback, failCallBack, alwaysCallBack);
		this.server.respond();

		// asserts
		sinon.assert.calledOnce(preCall);
		sinon.assert.calledWith(doneCallback, [{ id: 12, comment: "Hey there" }]);
		sinon.assert.calledOnce(alwaysCallBack);
		sinon.assert.notCalled(failCallBack);
		sinon.assert.callOrder(preCall, doneCallback, alwaysCallBack);
		ok(this.server.requests.length === 1, "One request was executed");
		ok(this.server.responses.length === 1, "One response received");
	});

	test("getData 500 should call done failCallBack", function () {
		// arrange
		var url = "/routenotification/list?centerNumber=22&filterStartDate=&filterEndDate=";
		this.server.respondWith("GET", url, [500, { "Content-Type": "application/json" }, '[{ "error": "error message" }]']);
		var preCall = sinon.spy(); var doneCallback = sinon.spy(); var failCallBack = sinon.spy(); var alwaysCallBack = sinon.spy();

		// act
		getData(url, preCall, doneCallback, failCallBack, alwaysCallBack);
		this.server.respond();

		// asserts
		sinon.assert.calledOnce(preCall);
		sinon.assert.calledOnce(failCallBack);
		sinon.assert.calledOnce(alwaysCallBack);
		sinon.assert.notCalled(doneCallback);
		sinon.assert.callOrder(preCall, failCallBack, alwaysCallBack);
		ok(this.server.requests.length === 1, "One request was executed");
		ok(this.server.responses.length === 1, "One response received");
	});

	test("getData 404 should call done failCallBack", function () {
		// arrange
		var url = "/routenotification/list?centerNumber=22&filterStartDate=&filterEndDate=";
		this.server.respondWith("GET", url, [404, { "Content-Type": "application/json" }, '[{ "error": "error message" }]']);
		var preCall = sinon.spy(); var doneCallback = sinon.spy(); var failCallBack = sinon.spy(); var alwaysCallBack = sinon.spy();

		// act
		getData(url, preCall, doneCallback, failCallBack, alwaysCallBack);
		this.server.respond();

		// asserts
		sinon.assert.calledOnce(preCall);
		sinon.assert.calledOnce(failCallBack);
		sinon.assert.calledOnce(alwaysCallBack);
		sinon.assert.notCalled(doneCallback);
		sinon.assert.callOrder(preCall, failCallBack, alwaysCallBack);
		ok(this.server.requests.length === 1, "One request was executed");
		ok(this.server.responses.length === 1, "One response received");
	});
});