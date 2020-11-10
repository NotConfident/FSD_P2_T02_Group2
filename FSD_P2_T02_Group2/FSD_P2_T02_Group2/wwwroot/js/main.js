
(function ($) {
    "use strict";


    /*==================================================================
    [ Validate after type ]*/
    $('.validate-input .input100').each(function(){
        $(this).on('blur', function(){
            if(validate(this) == false){
                showValidate(this);
            }
            else {
                $(this).parent().addClass('true-validate');
            }
        })    
    })
  
  
    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('submit',function(){
        var check = true;

        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i]);
                check=false;
            }
        }

        return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
           $(this).parent().removeClass('true-validate');
        });
    });

    function validate(input) {
        var today = new Date();
        var time = today.getTime();
        if ($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
            if ($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else if ($(input).attr('name') == "DepartureCountry" || $(input).attr('name') == "ArrivalCountry") {
            if ($(input).val() == '') {
                return false;
            }
            if ($.isNumeric($(input).val())) {
                return false;
            }
        }
        else if ($(input).attr('name') == "DepartureCity") {
            if ($.isNumeric($(input).val())) {
                return false;
            }
            if ($(input).val() == $("#arrivalCity").val()) {
                return false;
            }
        }
        else if ($(input).attr('name') == "ArrivalCity") {
            if ($.isNumeric($(input).val())) {
                return false;
            }
            if ($(input).val() == $("#departureCity").val()) {
                return false;
            }
        }
        else if ($(input).attr('name') == "DepartureDateTime") {
            if ($(input).val().trim() != '' && time >= Date.parse($(input).val())){
                return false;
            }
        }
        else if ($(input).attr('name') == "EconomyClassPrice") {
            if (!$.isNumeric($(input).val())) {
                return false;
            }
            if ($(input).val() > 5000 || $(input).val() < 0) {
                return false;
            }
        }
        else if ($(input).attr('name') == "BusinessClassPrice") {
            if (!$.isNumeric($(input).val())) {
                return false;
            }
            if ($(input).val() > 10000 || $(input).val() < 0) {
                return false;
            }
        }
        else {
            if ($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');

        $(thisAlert).append('<span class="btn-hide-validate">&#xf136;</span>')
        $('.btn-hide-validate').each(function(){
            $(this).on('click',function(){
               hideValidate(this);
            });
        });
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();
        $(thisAlert).removeClass('alert-validate');
        $(thisAlert).find('.btn-hide-validate').remove();
    }
    
    

})(jQuery);