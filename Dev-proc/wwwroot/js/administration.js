$(document).ready(function () {
    var activeRequest = null;
    var timer = null;
    $("#newUser").typeahead(null,
        {
            source: function (query) {
                if (query.length < 4) {
                    return null;
                }

                if (timer !== null) {
                    clearTimeout(timer);
                }

                if (activeRequest !== null) {
                    activeRequest.abort();
                    activeRequest = null;
                }

                timer = setTimeout(function () {
                    activeRequest = $.ajax("/administration/find_user/" + query,
                        {
                            type: "GET",
                            success: function (data) {
                                $('#usersFindResult').html(data);
                            }
                        });
                },
                    500);

                return activeRequest;
            }
        });

    $("#userName").typeahead(null,
        {
            source: function (query) {
                if (query.length < 4) {
                    return null;
                }

                if (timer !== null) {
                    clearTimeout(timer);
                }

                if (activeRequest !== null) {
                    activeRequest.abort();
                    activeRequest = null;
                }

                timer = setTimeout(function () {
                    activeRequest = $.ajax("/administration/find_new_user/" + query,
                        {
                            type: "GET",
                            success: function (data) {
                                $('#usersFindResult').html(data);
                            }
                        });
                },
                    500);

                return activeRequest;
            }
        });
});