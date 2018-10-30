module("formatDate", function (hooks) {
	hooks.beforeEach(function () {
		this.date = new Date(2018, 8, 30, 21, 7, 5);
	});

	test("will format date into yyyy-MM-dd by default", function () {
		var result = formatDate(this.date);
		equal(result, "2018-09-30", "error");
	});

	test("will format date into yyyy-MM-dd", function () {
		var result = formatDate(this.date, "yyyy-MM-dd");
		equal(result, "2018-09-30", "error");
	});

	test("will format date into MM/dd/yyyy", function () {
		var result = formatDate(this.date, "MM/dd/yyyy");
		equal(result, "09/30/2018", "error");
	});

	test("will format date into MM/dd/yyyy HH:mm (24 hour)", function () {
		var result = formatDate(this.date, "MM/dd/yyyy HH:mm");
		equal(result, "09/30/2018 21:07", "error");
	});

	test("will format date into MM/dd/yyyy hh:mm", function () {
		var result = formatDate(this.date, "MM/dd/yyyy hh:mm");
		equal(result, "09/30/2018 09:07", "error");
	});

	test("will format date into MM/dd/yyyy hh:mm tt", function () {
		var result = formatDate(this.date, "MM/dd/yyyy hh:mm tt");
		equal(result, "09/30/2018 09:07 PM", "error");
	});

	test("will format date into MM/dd/yy", function () {
		var result = formatDate(this.date, "MM/dd/yy");
		equal(result, "09/30/18", "error");
	});

	test("will format date into MM/dd/yy HH:mm (24 hour)", function () {
		var result = formatDate(this.date, "MM/dd/yy HH:mm");
		equal(result, "09/30/18 21:07", "error");
	});

	test("will format date into MM/dd/yy hh:mm", function () {
		var result = formatDate(this.date, "MM/dd/yy hh:mm");
		equal(result, "09/30/18 09:07", "error");
	});

	test("will format date into MM/dd/yy hh:mm tt", function () {
		var result = formatDate(this.date, "MM/dd/yy hh:mm tt");
		equal(result, "09/30/18 09:07 PM", "error");
	});

	test("will format date into PM MM/dd/yy hh:mm tt", function () {
		var dt = new Date(2018, 8, 7, 12, 45);
		var result = formatDate(dt, "MM/dd/yy hh:mm tt");
		equal(result, "09/07/18 12:45 PM", "error");
	});

	test("will format date into AM MM/dd/yy hh:mm tt", function () {
		var dt = new Date(2018, 8, 7, 0, 45);
		var result = formatDate(dt, "MM/dd/yy hh:mm tt");
		equal(result, "09/07/18 12:45 AM", "error");
	});
});

module("saturdays", function (hooks) {
	hooks.beforeEach(function () {
		var last = new Date();
		if (last.getDay() !== 6) {
			while (last.getDay() !== 6) {
				last.setDate(last.getDate() - 1);
			}
		} else {
			last.setDate(last.getDate() - 7);
		}
		var next = new Date(last);
		next.setDate(next.getDate() + 7);
		this.lastSaturday = last;
		this.nextSaturday = next;
	});

	test("lastSaturday", function () {
		var result = getLastSaturday();
		equal(result.toLocaleDateString(), this.lastSaturday.toLocaleDateString(), "error");
	});

	test("nextSaturday", function () {
		var result = getNextSaturday();
		equal(result.toLocaleDateString(), this.nextSaturday.toLocaleDateString(), "error");
	});
});

module("addDays", function (hooks) {
	hooks.beforeEach(function () {
		var today = new Date();
		var yesterday = new Date(today);
		var tomorrow = new Date(today);
		yesterday.setDate(yesterday.getDate() - 1);
		tomorrow.setDate(tomorrow.getDate() + 1);
		this.today = today;
		this.yesterday = yesterday;
		this.tomorrow = tomorrow;
	});

	test("addDays increment", function () {
		var result = addDays(new Date(), 1);
		equal(result.toLocaleDateString(), this.tomorrow.toLocaleDateString(), "error");
	});

	test("addDays decrement", function () {
		var result = addDays(new Date(), -1);
		equal(result.toLocaleDateString(), this.yesterday.toLocaleDateString(), "error");
	});

	test("addDays 0", function () {
		var result = addDays(new Date(), 0);
		equal(result.toLocaleDateString(), this.today.toLocaleDateString(), "error");
	});
});

module("isDate", function (hooks) {
	hooks.beforeEach(function () {

	});

	test("isDate string", function () {
		var result = isDate("9/30/2018");
		notOk(result, "error");
	});

	test("isDate number", function () {
		var result = isDate(2018);
		notOk(result, "error");
	});

	test("isDate bool", function () {
		var result = isDate(false);
		notOk(result, "error");
	});

	test("isDate decimal", function () {
		var result = isDate(20.54);
		notOk(result, "error");
	});

	test("isDate undefined", function () {
		var result = isDate();
		notOk(result, "error");
	});

	test("isDate null", function () {
		var result = isDate(null);
		notOk(result, "error");
	});

	test("isDate valid Date", function () {
		var result = isDate(new Date());
		ok(result, "error");
	});
});

module("isValidFormat - MM/dd/yy hh:mm tt", function (hooks) {
	hooks.beforeEach(function () {
		this.format = "MM/dd/yy hh:mm tt";
	});

	test("check empty to format by default", function () {
		var result = isValidFormat("");
		notOk(result, "error");
	});

	test("check empty to format", function () {
		var result = isValidFormat("", this.format);
		notOk(result, "error");
	});

	test("check partial no time to format", function () {
		var result = isValidFormat("08/30/18", this.format);
		notOk(result, "error");
	});

	test("check partial with time no tt to format", function () {
		var result = isValidFormat("08/30/18 11:05", this.format);
		notOk(result, "error");
	});

	test("check partial with time but HH to format", function () {
		var result = isValidFormat("08/30/18 14:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month to format", function () {
		var result = isValidFormat("8/30/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit date to format", function () {
		var result = isValidFormat("08/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month & date to format", function () {
		var result = isValidFormat("8/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit hour to format", function () {
		var result = isValidFormat("08/30/18 9:05", this.format);
		notOk(result, "error");
	});

	test("check invalid full wrong tt to format", function () {
		var result = isValidFormat("08/30/18 11:05 TM", this.format);
		notOk(result, "error");
	});

	test("check valid full AM to format", function () {
		var result = isValidFormat("08/30/18 10:05 AM", this.format);
		ok(result, "error");
	});

	test("check valid full PM to format", function () {
		var result = isValidFormat("08/30/18 10:05 PM", this.format);
		ok(result, "error");
	});
});

module("isValidFormat - MM/dd/yyyy hh:mm tt", function (hooks) {
	hooks.beforeEach(function () {
		this.format = "MM/dd/yyyy hh:mm tt";
	});

	test("check empty to format", function () {
		var result = isValidFormat("", this.format);
		notOk(result, "error");
	});

	test("check partial year to format", function () {
		var result = isValidFormat("08/30/18", this.format);
		notOk(result, "error");
	});

	test("check partial no time to format", function () {
		var result = isValidFormat("08/30/2018", this.format);
		notOk(result, "error");
	});

	test("check partial with time no tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05", this.format);
		notOk(result, "error");
	});

	test("check partial with time but HH to format", function () {
		var result = isValidFormat("08/30/2018 14:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month to format", function () {
		var result = isValidFormat("8/30/2018 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit date to format", function () {
		var result = isValidFormat("08/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month & date to format", function () {
		var result = isValidFormat("8/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit hour to format", function () {
		var result = isValidFormat("08/30/2018 9:05", this.format);
		notOk(result, "error");
	});

	test("check invalid full wrong tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05 TM", this.format);
		notOk(result, "error");
	});

	test("check valid full AM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 AM", this.format);
		ok(result, "error");
	});

	test("check valid full PM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 PM", this.format);
		ok(result, "error");
	});
});

module("isValidFormat - MM/dd/yyyy", function (hooks) {
	hooks.beforeEach(function () {
		this.format = "MM/dd/yyyy";
	});

	test("check empty to format", function () {
		var result = isValidFormat("", this.format);
		notOk(result, "error");
	});

	test("check partial year to format", function () {
		var result = isValidFormat("08/30/18", this.format);
		notOk(result, "error");
	});

	test("check partial no time to format", function () {
		var result = isValidFormat("08/30/2018", this.format);
		ok(result, "error");
	});

	test("check partial with time no tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05", this.format);
		ok(result, "error");
	});

	test("check partial with time but HH to format", function () {
		var result = isValidFormat("08/30/2018 14:05", this.format);
		ok(result, "error");
	});

	test("check partial with single digit month to format", function () {
		var result = isValidFormat("8/30/2018 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit date to format", function () {
		var result = isValidFormat("08/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month & date to format", function () {
		var result = isValidFormat("8/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit hour to format", function () {
		var result = isValidFormat("08/30/2018 9:05", this.format);
		ok(result, "error");
	});

	test("check invalid full wrong tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05 TM", this.format);
		ok(result, "error");
	});

	test("check valid full AM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 AM", this.format);
		ok(result, "error");
	});

	test("check valid full PM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 PM", this.format);
		ok(result, "error");
	});
});

module("isValidFormat - MM/dd/yy", function (hooks) {
	hooks.beforeEach(function () {
		this.format = "MM/dd/yy";
	});

	test("check empty to format", function () {
		var result = isValidFormat("", this.format);
		notOk(result, "error");
	});

	test("check partial year to format", function () {
		var result = isValidFormat("08/30/18", this.format);
		ok(result, "error");
	});

	test("check partial no time to format", function () {
		var result = isValidFormat("08/30/2018", this.format);
		ok(result, "error");
	});

	test("check partial with time no tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05", this.format);
		ok(result, "error");
	});

	test("check partial with time but HH to format", function () {
		var result = isValidFormat("08/30/2018 14:05", this.format);
		ok(result, "error");
	});

	test("check partial with single digit month to format", function () {
		var result = isValidFormat("8/30/2018 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit date to format", function () {
		var result = isValidFormat("08/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month & date to format", function () {
		var result = isValidFormat("8/5/18 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit hour to format", function () {
		var result = isValidFormat("08/30/2018 9:05", this.format);
		ok(result, "error");
	});

	test("check invalid full wrong tt to format", function () {
		var result = isValidFormat("08/30/2018 11:05 TM", this.format);
		ok(result, "error");
	});

	test("check valid full AM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 AM", this.format);
		ok(result, "error");
	});

	test("check valid full PM to format", function () {
		var result = isValidFormat("08/30/2018 10:05 PM", this.format);
		ok(result, "error");
	});
});

module("isValidFormat - yyyy-MM-dd HH:mm", function (hooks) {
	hooks.beforeEach(function () {
		this.format = "yyyy-MM-dd HH:mm";
	});

	test("check empty to format", function () {
		var result = isValidFormat("", this.format);
		notOk(result, "error");
	});

	test("check partial year to format", function () {
		var result = isValidFormat("18-08-30", this.format);
		notOk(result, "error");
	});

	test("check partial no time to format", function () {
		var result = isValidFormat("2018-08-30", this.format);
		notOk(result, "error");
	});

	test("check partial with time no tt to format", function () {
		var result = isValidFormat("2018-08-30 11:05", this.format);
		ok(result, "error");
	});

	test("check partial with time but HH to format", function () {
		var result = isValidFormat("2018-08-30 14:05", this.format);
		ok(result, "error");
	});

	test("check partial with time but 12 to format", function () {
		var result = isValidFormat("2018-08-30 12:05", this.format);
		ok(result, "error");
	});

	test("check partial with time but 00 to format", function () {
		var result = isValidFormat("2018-08-30 00:00", this.format);
		ok(result, "error");
	});

	test("check partial with time but 24 to format", function () {
		var result = isValidFormat("2018-08-30 24:00", this.format);
		ok(result, "error");
	});

	test("check partial with single digit month to format", function () {
		var result = isValidFormat("2018-8-30 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit date to format", function () {
		var result = isValidFormat("2018-08-5 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit month & date to format", function () {
		var result = isValidFormat("2018-8-3 09:05", this.format);
		notOk(result, "error");
	});

	test("check partial with single digit hour to format", function () {
		var result = isValidFormat("2018-08-30 9:05", this.format);
		notOk(result, "error");
	});

	test("check invalid full wrong tt to format", function () {
		var result = isValidFormat("2018-08-30 11:05 TM", this.format);
		ok(result, "error");
	});

	test("check valid full AM to format", function () {
		var result = isValidFormat("2018-08-30 10:05 AM", this.format);
		ok(result, "error");
	});

	test("check valid full PM to format", function () {
		var result = isValidFormat("2018-08-30 10:05 PM", this.format);
		ok(result, "error");
	});
});
