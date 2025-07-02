

$(document).ready(startSearch).on('page:load', startSearch);

function startSearch() {
    $(".search").on("focus", searcGrid);
    $(".search").on("keyup", searcGrid);
    $(".search").on("blur", scrolOnSearch);
    
}
function searcGrid() {
    //jtable - child - row
    var text = $(this).val().toLowerCase().trim();  
    $("#grid tr").each(function () {
        var attr = 'attr_' + $('#selectType').val();
        var span = $(this).find("span[" + attr+"='y']");       
        var srcAttr = span.attr(attr);
        var spanText = span.text().toLowerCase().trim();
        if (srcAttr == 'y') {
            var css = span.attr('class');
            if (text.length > 0 && spanText.indexOf(text) >= 0) {
                if (css !== "findElement") {
                    span.addClass('findElement');                    
                }
            }
            else {
                span.attr('class', '');               
            }
        }        
    });
}

function scrolOnSearch() {
    //jtable - child - row
    var text = $(this).val().toLowerCase().trim();
    var scrolled = false;
    $("#grid tr").each(function () {
        var attr = 'attr_' + $('#selectType').val();
        var span = $(this).find("span[" + attr + "='y']");
        var srcAttr = span.attr(attr);
        var spanText = span.text().toLowerCase().trim();
        if (srcAttr == 'y') {
            var css = span.attr('class');
            if (text.length > 0 && spanText.indexOf(text) >= 0) {

                if (!scrolled) {
                    window.scrollTo(0, span.offset().top);
                    scrolled = true;
                }
            }
        }
    });
}

