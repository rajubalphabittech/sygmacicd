module("comment callbacks", function (hooks) {
	hooks.beforeEach(function () {
		var jQueryMethods = {
			val: function () { },
			html: function () { },
			dialog: function () { },
			css: function () { },
			addClass: function () { },
			prev: function () { },
			text: function () { },
			valValue: '',
			htmlValue: '',
			dialogValue: ''
		};
		this.htmlStub = sinon.stub(jQueryMethods, 'html')
			.callsFake(function fakeFn() {
				return jQueryMethods.htmlValue;
			})
			.withArgs('data').callsFake(function fakeFn() {
				this.htmlValue = 'data';
				return;
			})
			.withArgs('error').callsFake(function fakeFn() {
				this.htmlValue = 'error';
				return;
			})
			.withArgs(sinon.match.any).callsFake(function fakeFn(arg) {
				this.htmlValue = arg;
				return;
			});
		this.dialogStub = sinon.stub(jQueryMethods, 'dialog')
			.withArgs(sinon.match.any).callsFake(function fakeFn(arg) {
				return jQueryMethods;
			});
		this.valStub = sinon.stub(jQueryMethods, 'val')
			.callsFake(function fakeFn() {
				return jQueryMethods.valValue;
			})
			.withArgs("").callsFake(function fakeFn() {
				this.valValue = "";
				return;
			});
		this.prevStub = sinon.stub(jQueryMethods, 'prev').returns(jQueryMethods);
		this.textStub = sinon.stub(jQueryMethods, 'text')
			.callsFake(function fakeFn() {
				return jQueryMethods;
			})
			.withArgs("").callsFake(function fakeFn() {
				return jQueryMethods;
			})
			.withArgs(sinon.match.any).callsFake(function fakeFn(arg) {
				return jQueryMethods;
			});
		this.classStub = sinon.stub(jQueryMethods, 'addClass').returns(jQueryMethods);
		this.cssStub = sinon.stub(jQueryMethods, 'css').returns(jQueryMethods);
		sinon.stub(window, '$').returns(jQueryMethods);

		this.jQueryMethods = jQueryMethods;
	});

	hooks.afterEach(function () {
		this.jQueryMethods.html.restore();
		this.jQueryMethods.dialog.restore();
		this.jQueryMethods.val.restore();
		this.jQueryMethods.prev.restore();
		this.jQueryMethods.text.restore();
		this.jQueryMethods.addClass.restore();
		this.jQueryMethods.css.restore();
		window.$.restore();
	});

	// --- load comment

	test("loadCommentFailCallBack", function () {
		// arrange

		// act
		loadCommentFailCallBack('error');

		// asserts
		equal($("#dialog-window").html(), 'error');
		sinon.assert.calledWith(this.dialogStub, 'open');
	});

	test("loadCommentPreCall not opening dialog", function () {
		// arrange

		// act
		loadCommentPreCall(10, false);

		// asserts
		equal($("#dialog-window").html(), "<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
		sinon.assert.neverCalledWith(this.dialogStub, 'open');
	});

	test("loadCommentPreCall opening dialog", function () {
		// arrange

		// act
		loadCommentPreCall(10, true);

		// asserts
		equal($("#dialog-window").html(), "<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
		sinon.assert.calledWith(this.dialogStub, 'open');
	});

	test("loadCommentDoneCallBack with data", function () {
		// arrange

		// act
		loadCommentDoneCallBack('data');

		// asserts
		equal($("#dialog-window").html(), 'data');
	});

	// --- create comment

	test("createCommentDoneCallBack opening dialog with customer communication", function () {
		// arrange
		var commentParameters = [];
		commentParameters.stopNumber = 10;
		commentParameters.comment = "my comment";
		commentParameters.category = "Customer Communication";

		// act
		createCommentDoneCallBack('data', commentParameters);

		// asserts
		equal($("#dialog-window").html(), 'data');
		equal($("#comment-box").val(), "");
		sinon.assert.called(this.dialogStub);
		sinon.assert.called(this.prevStub);
		sinon.assert.calledWith(this.textStub, commentParameters.comment);
	});

	test("createCommentDoneCallBack NOT opening dialog without customer communication", function () {
		// arrange
		var commentParameters = [];
		commentParameters.stopNumber = 10;
		commentParameters.comment = "my comment";
		commentParameters.category = "Whatever";

		// act
		createCommentDoneCallBack('data', commentParameters);

		// asserts
		equal($("#dialog-window").html(), 'data');
		equal($("#comment-box").val(), "");
		sinon.assert.called(this.dialogStub);
		sinon.assert.neverCalledWith(this.prevStub);
		sinon.assert.neverCalledWith(this.textStub);
	});

	// --- load last customer communication

	test("loadLastCustomerCommunicationDoneCallBack null data", function () {
		// arrange
		var data = null;
		var commentParameters = [];
		commentParameters.stopNumber = 10;

		// act
		loadLastCustomerCommunicationDoneCallBack(data, commentParameters);

		// asserts
		sinon.assert.called(this.prevStub);
		sinon.assert.calledWith(this.textStub, '');
		ok(true);
	});

	test("loadLastCustomerCommunicationDoneCallBack non-null data", function () {
		// arrange
		var data = [];
		data.LongComment = 'data';
		var commentParameters = [];
		commentParameters.stopNumber = 10;

		// act
		loadLastCustomerCommunicationDoneCallBack(data, commentParameters);

		// asserts
		sinon.assert.called(this.prevStub);
		sinon.assert.calledWith(this.textStub, data.LongComment);
		ok(true);
	});

	// --- update comment

	test("updateCommentDoneCallBack with customer communication", function () {
		// arrange
		var commentParameters = [];
		commentParameters.stopNumber = 10;
		commentParameters.comment = "my comment";
		commentParameters.category = "Customer Communication";

		// act
		updateCommentDoneCallBack('data', commentParameters);

		// asserts
		equal($("#dialog-window").html(), 'data');
		equal($("#comment-box").val(), "");
		sinon.assert.called(this.dialogStub);
		sinon.assert.called(this.prevStub);
		sinon.assert.calledWith(this.textStub, commentParameters.comment);
	});

	test("updateCommentDoneCallBack without customer communication", function () {
		// arrange
		var commentParameters = [];
		commentParameters.stopNumber = 10;
		commentParameters.comment = "my comment";
		commentParameters.category = "WHATEVER";
		var cbStub = sinon.stub(window, 'loadLastCustomerCommunication').withArgs(sinon.match.any).returns(null);
		loadLastCustomerCommunication = window.loadLastCustomerCommunication; // HACK

		// act
		updateCommentDoneCallBack('data', commentParameters);

		// asserts
		equal($("#dialog-window").html(), 'data');
		equal($("#comment-box").val(), "");
		sinon.assert.called(cbStub);
		sinon.assert.called(this.dialogStub);
		sinon.assert.notCalled(this.prevStub);
		sinon.assert.notCalled(this.textStub);
		loadLastCustomerCommunication.restore();  // this actually doesn't restore the myFunc function, only window.myFunc
		loadLastCustomerCommunication = window.loadLastCustomerCommunication;
	});
});
