// Back To Top Begin
$(document).ready(function () {
  $(window).scroll(function () {
    if ($(this).scrollTop() > 40) {
      $("#BackToTop").fadeIn();
    } else {
      $("#BackToTop").fadeOut();
    }
  });

  $("#BackToTop").click(function () {
    $("html, body").animate({ scrollTop: 0 }, 0);
  });
});
// Back To Top End

//Slider begin
$(document).ready(function () {
  $(".carousel").carousel({
    interval: 5000,
  });
});
//Slider end

//Owl Carousel begin
$(document).ready(function () {
  $(".owl-carousel").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
      0: {
        items: 1,
      },
      600: {
        items: 1,
      },
      1000: {
        items: 2,
      },
    },
  });
});
//Owl Carousel end

//DatetimePicker begin
$(document).ready(function () {
  var DateTime = new Date();

    flatpickr("#mainCheckIn", {
        minDate: "today",
        enableTime: false,
        dateFormat: "F, d Y",
        defaultDate: new Date(),
        onChange: function () {
            console.log($("#mainCheckIn").val());
            
        }
    });

    flatpickr("#mainCheckOut", {
        minDate: "today",
        enableTime: false,
        dateFormat: "F, d Y",
        defaultDate: new Date(DateTime.getUTCFullYear(), DateTime.getUTCMonth(), DateTime.getUTCDate()+1),
        onChange: function () {
            console.log($("#mainCheckOut").val());

        }
    });
});
//DatetimePicker end

// Guests toogle begin
$(document).ready(function () {
  $("#guests").click(function () {
    $(".detailWrap_innerForm").toggle();

    // alert($('.detailWrap_AllPassenger').text());
  });
});
// Guests toogle end

// Guests Selects begin
$(document).ready(function () {
    $(".HelloAdult").change(function () {
    var Passengers =
      "Adults: " +
      $(".aAdult").val() +
      " " +
      "Children: " +
      $(".aChildren").val() +
      " " +
      "Infant: " +
      $(".aInfant").val();
    $(".detailWrap_AllPassenger").html(Passengers);
    });

    $(".HelloChildren").change(function () {
        var Passengers =
            "Adults: " +
            $(".aAdult").val() +
            " " +
            "Children: " +
            $(".aChildren").val() +
            " " +
            "Infant: " +
            $(".aInfant").val();
        $(".detailWrap_AllPassenger").html(Passengers);
    });

    $(".HelloInfant").change(function () {
        var Passengers =
            "Adults: " +
            $(".aAdult").val() +
            " " +
            "Children: " +
            $(".aChildren").val() +
            " " +
            "Infant: " +
            $(".aInfant").val();
        $(".detailWrap_AllPassenger").html(Passengers);
    });

 
});
// Guests Selects end

// Fetch query for Wheather begin
$(document).ready(function () {
  fetch(
    "https://api.weatherapi.com/v1/current.json?key=5a58d23c7812464daea113700220603&q=Baku&aqi=no"
  )
    .then((response) => response.json())

    .then((data) => {
      console.log(data.current.temp_c);

      console.log(data.current.cloud);

      $(".header-inner_number").html(data.current.temp_c);
      $(".header-inner_weatherTemperature").html(
        data.current.temp_c + `<sup>o</sup>C`
      );
      $(".header-inner_weather-temperature").html(
        data.current.temp_c + `<sup>o</sup>C`
      );

      if (data.current.is_day == 1 && data.current.cloud >= 50) {
        console.log("gunduz buludlu");
        $(".header-inner_weathers-icon").html(`
        <i class="fa-solid fa-cloud-sun header-inner_weathersIcon"></i>
        `);

        $(".header-inner_weatherIconWrap").html(`
        <i class="fa-solid fa-cloud-sun header-inner_weathersIcon"></i>
        `);
      } else if (data.current.is_day == 1 && data.current.cloud <= 50) {
        console.log("gunduz buludsuz");
        $(".header-inner_weathers-icon").html(`
        <i class="fa-solid fa-sun header-inner_weathersIcon"></i>
        `);

        $(".header-inner_weatherIconWrap").html(`
        <i class="fa-solid fa-sun header-inner_weathersIcon"></i>
        `);
      } else if (data.current.is_day == 0 && data.current.cloud >= 50) {
        console.log("axsam buludlu");
        $(".header-inner_weathers-icon").html(`
        <i class="fa-solid fa-cloud-moon header-inner_weathersIcon"></i>
        `);

        $(".header-inner_weatherIconWrap").html(`
        <i class="fa-solid fa-cloud-moon header-inner_weathersIcon"></i>
        `);
      } else if (data.current.is_day == 0 && data.current.cloud <= 50) {
        console.log("axsam buludsuz");
        $(".header-inner_weathers-icon").html(`
        <i class="fa-solid fa-moon header-inner_weathersIcon"></i>
        `);

        $(".header-inner_weatherIconWrap").html(`
        <i class="fa-solid fa-moon header-inner_weathersIcon"></i>
        `);
      }
    })

    .catch((error) => {
      console.log(error);
    });
});
// Fetch query for Wheather end

//Header one mobile begin
$(document).ready(function () {
  $(".header-inner_hamburger").click(function () {
    if ($(".mobilemenu").height() > 50) {
      $(".mobilemenu").css("height", "0%");
      $(".mobilemenu").css("z-index", "0");
    } else if ($(".mobilemenu").height() < 50) {
      $(".mobilemenu").css("height", "350px");
      $(".mobilemenu").css("z-index", "5");
    }
  });
});
//Header one mobile end

//Loader hide begin
$(document).ready(function () {
  setInterval(loading, 3000);

  function loading() {
    $(".loadingType").hide();
  }
});

//Loader hide end


//Rules begin
$(document).ready(function () {
   
  $('.rules_link').click(function(){
      $('.rules').css('display', 'none')
      $('.rules').css('position', 'unset')
  });
    $('.detailWrap_rulesLink').click(function () {
        $('.rules').css('display', 'inline-block')
        $('.rules').css('position', 'fixed')

    });
});
//Rules end


//Toastr option begin
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-bottom-left",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
}
//Toastr option end

//Toastr js begin
$(document).ready(function () {
    if ($("#success-toaster").length) {
        toastr["success"]($("#success-toaster").val());
    }
});
//Toastr js end 





