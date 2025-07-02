


var organiztionManager = {
    init: function () {
        this.initChoosen();
    },
    initChoosen: function () {
        if ($(".chosen").length > 0) {       
            jQuery(".chosen").chosen();
        }
    },
}

$(function () {
    organiztionManager.init();
});
