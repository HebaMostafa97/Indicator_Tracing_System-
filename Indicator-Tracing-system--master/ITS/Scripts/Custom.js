function toggleSidebar() {
  $('#sidebar').toggleClass($(".ActiveContentLanguage").text());
  $('.Content').toggleClass($(".ActiveContentLanguage").text());
}
//date with datepicker //
$('.datepicker').datepicker({
  format: "MM/dd/yyyy"
});

// DeleteObject //
function DeleteObject(obj) {
  var ID = $(obj).attr("data-id");
  var ObjectName = $(obj).attr("data-Controller");
  var Action1 = $(obj).attr("data-Action1");
  var Action2 = $(obj).attr("data-Action2");
  var Name = $(obj).attr("data-Name");
  var MSG1 = $(obj).attr("data-Msg1");
  var MSG2 = $(obj).attr("data-Msg2");
  var okButt = $('.OkButton').text();
  var CancelButt = $('.cancelButton').text();
  $.ajax({
    url: "/" + ObjectName + "/" + Action1 + "/" + ID,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    type: "POST",
    success: function (response) {
      if (response) {
        swal({
          text: MSG1 + " " + Name + "?",
          icon: "warning",
          buttons: [CancelButt, okButt],
          dangerMode: true,
        })
          .then((willDelete) => {
            if (willDelete) {
              $.ajax({
                url: "/" + ObjectName + "/" + Action2 + "/" + ID,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (response) {
                  if (response == "OK") {
                    swal({
                      text: MSG2,
                      icon: "success",
                    });
                    setTimeout(function () {
                      location.reload();
                    }, 3000);
                  }
                  else {
                    swal({
                      text: response,
                      icon: "warning",
                    });
                  }
                }
              });

            }
          });
      }
      else {
        window.location.href = "/Home/AccessDenied";
      }

    }

  });
}
// ActiveObject //
function ActiveObject(obj) {
  var ID = $(obj).attr("data-id");
  var ObjectName = $(obj).attr("data-Controller");
  $.ajax({
    url: "/" + ObjectName + "/" + "Active" + "/" + ID,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    type: "POST",
    success: function (response) {
      location.reload();
    }

  });
}
// Download //
function Downloaded(obj) {
    var ID = $(obj).attr("data-id");
    var model = $(obj).attr("model");
    var m1 = $(obj).attr("msg1");
    var m2 = $(obj).attr("msg2");

    $.ajax({
        url: "/" + model + "/" + "Download" + "/" + ID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        success: function (response) {

            if (response) {
                swal({
                    text: m1,
                    icon: "success",
                    
                });
                setTimeout(function () {
                    location.reload();
                }, 5000);
            }
            else {
                swal({
                    text: m2,
                    icon: "error",

                });
                setTimeout(function () {
                    location.reload();
                }, 5000);
            }
        }

    });
}



function screen_resize() {
    var w = parseInt(window.innerWidth)
    var h = parseInt(window.innerHeight)
    if (w < 1060) {
        $('#sidebarCollapse').click();
    }
}

$(window).resize(function () {
    screen_resize();
});
$(document).ready(function () {
  $("table").addClass("table-hover table-bordered");
  if ($(".grid-pager")[0]) {
    $(".paging").addClass("pager")
    $(".desc").addClass("descripe")
  } else {
    $(".paging").removeClass("pager")
    $(".desc").removeClass("descripe")
  }
  $('#sidebarCollapse').on('click', function () {
      $('#sidebar').toggleClass('active');
  });
  screen_resize();
});