console.log("js ");


console.log("thisn");
  $("#input_apiKey").unbind();
  $("#input_apiKey").change(function () {
    var key = $("#input_apiKey")[0].value;
    if (key && key.trim() != "") {

      key = "Bearer " + key;
      console.log("set:" + key);
      window.authorizations.add("key", new ApiKeyAuthorization("Authorization", key, "header"));
    }
  });
  $("#loginButton").unbind();
  $("#loginButton").click(function (parameters) {
    $.ajax({
      type: "POST",
      url: "../../token",
      data: $("#loginForm").serialize(),
      success: function(returnval) {
        key = "Bearer " + returnval.access_token;
        $("#input_apiKey").val(returnval.access_token);
        window.authorizations.add("key", new ApiKeyAuthorization("Authorization", key, "header"));
        $('#LoginFormSection').hide('slow');
      },
      error: function (returnval) {
        $("#loginButton").wiggle();
      },
      dataType: 'json'
    });

  });

