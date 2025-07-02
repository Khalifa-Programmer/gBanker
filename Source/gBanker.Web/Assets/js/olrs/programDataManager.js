
var programDataManager = {
    init: function () {
        this.initChoosen();
        this.initDate();
    },

    initChoosen: function () {

        if ($(".chosen").length > 0) {
            $(".chosen").val('').trigger("liszt:updated");
            jQuery(".chosen").chosen();
        }
    },
    initDate: function () {
        $("#MNYR").datepicker({
            dateFormat: "mm/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#MNYR").datepicker('setDate', new Date());
    },
    goNextRow: function (e, tabIndex) {
        //up arrow 38
        if (e.keyCode == 38) {
            var $newClass = $(`.pd-tbl-data .input_amount_${parseInt(tabIndex) - 1}`);
            $newClass.focus();
            $newClass.select();
        }
        else if (e.keyCode == 40) {
            var $newClass = $(`.pd-tbl-data .input_amount_${parseInt(tabIndex) + 1}`);
            $newClass.focus();
            $newClass.select();
        }

    }
}

$(function () {
    programDataManager.init(); 
   
    $(".pd-tbl-data").on('click', '.input-amount', function () {
        var $that = $(this);       
        $that.select();
    });
   
    $('.pd-tbl-data').on('keyup', '.input-amount', function () {
        let gTotalAmount = 0;
        $('.pd-tbl-data').find('td.td-input-amount').each(function () {
            var $tr = $(this).parents('tr')
            if ($tr.find('.input-amount').length <= 0) return false;

            let amount = $tr.find('.input-amount').val();
            if (amount && amount.length > 0) {
                gTotalAmount = parseInt(gTotalAmount) + parseInt(amount);
            }
        });

        $('.pd-tbl-data #grandTotal').html(gTotalAmount);
    })
    
});
