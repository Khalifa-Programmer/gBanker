

var app = {
    initFns: function () {
        this.globalControl();
        this.textBoxOnlyAlphabet();
        this.checkDoubleSpace();
        this.checkDoubleComma();
        this.checkSingleWordNotification();
        this.notifySpecialCharInName();
        this.removeSpecialCharInName();
        this.notifyNameStartsWith();
        this.notifyNameStartsWithFemale();
        this.notifyNameStartsWithWrongGender();
        this.checkZipcode();
        this.removeBlankSpace();
        this.notifySpecialCharInAddress();
        this.checkNumericField();
        this.mobileNoStartWithZero();
        this.addSpaceAfterDot();
        this.addSpaceAfterDotCommaHashDashColon();
        this.checkDoubleCommaNotification();
    },

    textBoxOnlyAlphabet: function () {
        if (!$(".textBoxOnlyAlphabet") || $(".textBoxOnlyAlphabet").length <= 0) {
            return;
        }
        $(".textBoxOnlyAlphabet").keypress((e) => {
            let key = e.keyCode;
           // console.log('console.log(e.keyCode)')
          //  console.log(e.keyCode)
            if ((key >= 48 && key <= 57) || (key >= 2534 && key <= 2543)) { // 2534 2543
                e.preventDefault();
            }
        });
    },
    checkDoubleSpace: function () {
        if (!$(".checkDoubleSpace") || $(".checkDoubleSpace").length <= 0) {
            return;
        }
        $(".checkDoubleSpace").on("keyup", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                inputedValue = inputedValue.replace(/ {2,}/g, ' ');
                //console.log(inputedValue);                
                $(this).val(inputedValue.replace(/^\s+/, ""));
            }
        });
    },
    checkDoubleComma: function () {
        if (!$(".checkDoubleComma") || $(".checkDoubleComma").length <= 0) {
            return;
        }
        $(".checkDoubleComma").on("keyup", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                inputedValue = inputedValue.replace(/,{2,}/g, ',');
                //console.log(inputedValue);                
                $(this).val(inputedValue);
            }
        });
    },
    checkDoubleCommaNotification: function () {
        if (!$(".checkDoubleCommaNotification") || $(".checkDoubleCommaNotification").length <= 0) {
            return;
        }
        $(".checkDoubleCommaNotification").on("focusout", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                if (inputedValue.length > 2) {
                    if (inputedValue.match(/, , /g)) {
                        // the user entered a duplicate comma
                        inputedValue = inputedValue.replace(/, , /g, ', ');
                        $(this).val(inputedValue);
                        $.alert.open(GlobalMessageNotifierConstants.Warning, "Warning! 'No duplicate commas allowed!");
                    }

                }
            }
        });
    },

    checkSingleWordNotification: function () {
        if (!$(".checkSingleWordNotification") || $(".checkSingleWordNotification").length <= 0) {
            return;
        }
        $(".checkSingleWordNotification").on("focusout", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                inputedValue = inputedValue.replace(/\s+$/, "").split(' ');

                if (inputedValue.length === 1) {
                    $(this).val($.trim(inputedValue));
                    $.alert.open(GlobalMessageNotifierConstants.Warning, "Warning! Should contain two words.");
                }
            }
        });
    },
    notifySpecialCharInName: function () {
        if (!$(".notifySpecialCharInName") || $(".notifySpecialCharInName").length <= 0) {
            return;
        }
        $(".notifySpecialCharInName").on("blur", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                let spChars = /[!@#$%^&*_+\-=\[\]{};':"\\|,`<>\/?]+/;
                if (spChars.test(inputedValue) == true) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, "Warning! Contains illegal characters.");
                }
            }
        });

    },
    removeSpecialCharInName: function () {
        if (!$(".removeSpecialCharInName") || $(".removeSpecialCharInName").length <= 0) {
            return;
        }
        $(".removeSpecialCharInName").on("keyup", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                let spChars = /[!@#$%^&*_+\-=\[\]{};':"\\|,`<>\/?]+/;
                if (spChars.test(inputedValue) == true) {
                    $(this).val(inputedValue.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-'));
                }
            }
        });

    },
    notifyNameStartsWith: function () {
        if (!$(".notifyNameStartsWith") || $(".notifyNameStartsWith").length <= 0) {
            return;
        }
        $(".notifyNameStartsWith").on("focusout", function () {
            let inputedValue = $(this).val();
            let inputedValues = inputedValue.split(' ');
            if (inputedValues && inputedValues.length > 0) {

                let nameContainsWith = false;
                let containedNameSpipet = '';
                _.forEach(inputedValues, function (name) {
                    let isExists = _.some(NameWithStartsConstants, ['nameWithStart', _.toUpper(name)]);
                    if (isExists) {
                        nameContainsWith = true;
                        containedNameSpipet = name;
                    }
                });

                //if found any then show notification
                if (nameContainsWith) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, `Warning! Name contains the words - "${containedNameSpipet}"`);
                }
            }
        });

    },
    notifyNameStartsWithFemale: function () {
        if (!$(".notifyNameStartsWithFemale") || $(".notifyNameStartsWithFemale").length <= 0) {
            return;
        }
        $(".notifyNameStartsWithFemale").on("focusout", function () {
            let inputedValue = $(this).val();
            let inputedValues = inputedValue.split(' ');
            if (inputedValues && inputedValues.length > 0) {

                let nameContainsWith = false;
                let containedNameSpipet = '';
                _.forEach(inputedValues, function (name) {
                    let isExists = _.some(NameWithStartsConstantsMale, ['nameWithStart', _.toUpper(name)]);
                    if (isExists) {
                        nameContainsWith = true;
                        containedNameSpipet = name;
                    }
                });

                //if found any then show notification
                if (nameContainsWith) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, `Warning! Name contains the words - "${containedNameSpipet}"`);
                }
            }
        });

    },
    notifyNameStartsWithWrongGender: function () {
        if (!$(".notifyNameStartsWithWrongGender") || $(".notifyNameStartsWithWrongGender").length <= 0) {
            return;
        }
        $(".notifyNameStartsWithWrongGender").on("focusout", function () {
            let inputedValue = $(this).val();
            let inputedValues = inputedValue.split(' ');
            if (inputedValues && inputedValues.length > 0) {

                let nameContainsWith = false;
                let containedNameSpipet = '';
                _.forEach(inputedValues, function (name) {
                    let isExists = _.some(NameWithStartsConstantsWrongGender, ['nameWithStart', _.toUpper(name)]);
                    if (isExists) {
                        nameContainsWith = true;
                        containedNameSpipet = name;
                    }
                });

                //if found any then show notification
                if (nameContainsWith) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, `Warning! Name contains the words - "${containedNameSpipet}"`);
                }
            }
        });

    },
    checkZipcode: function () {
        if (!$(".checkZipcode") || $(".checkZipcode").length <= 0) {
            return;
        }
        $(".checkZipcode").on("focusout", function () {
            let inputedValue = $(this).val();
            let inputedValues = inputedValue.split('');
            if (inputedValues && inputedValues.length > 0) {
                let letterContainsWith = false;
                let containedLetterSpipet = '';
                _.forEach(inputedValues, function (letter) {
                    let isExists = _.some(A_Z_Constants, ['letter', _.toUpper(letter)]);
                    if (isExists) {
                        letterContainsWith = true;
                        containedLetterSpipet = letter;
                    }
                });

                //if found any then show notification
                if (letterContainsWith) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, `Warning! Invalid zipcode. [${containedLetterSpipet}]`);
                }
            }
        });

    },
    removeBlankSpace: function () {
        if (!$(".removeBlankSpace") || $(".removeBlankSpace").length <= 0) {
            return;
        }
        $(".removeBlankSpace").on("keyup", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                inputedValue = inputedValue.replace(/ {2,}/g, ' ');
                //console.log(inputedValue);                
                $(this).val(inputedValue.replaceAll(/\s/g, ''));
            }
        });
    },

    notifySpecialCharInAddress: function () {
        if (!$(".notifySpecialCharInAddress") || $(".notifySpecialCharInAddress").length <= 0) {
            return;
        }
        $(".notifySpecialCharInAddress").on("focusout", function () {
            let inputedValue = $(this).val();
            if (inputedValue) {
                let spChars = /[!@#$%^&*_+\-=\[\]{};':"\\|`<>\?]+/;
                if (spChars.test(inputedValue) == true) {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, "Warning! Contains illegal characters.");
                }
            }
        });

    },
    checkNumericField: function () {
        if (!$(".checkNumericField") || $(".checkNumericField").length <= 0) {
            return;
        }

        $(".checkNumericField").keydown(function (event) {
            //console.log(event.keyCode)
            // Allow only backspace and delete
            if (event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 8) {
                // let it happen, don't do anything
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))) {
                    event.preventDefault();
                }
            }
        });

    },
    mobileNoStartWithZero: function () {
        if (!$(".mobileNoStartWithZero") || $(".mobileNoStartWithZero").length <= 0) {
            return;
        }

        $(".mobileNoStartWithZero").on('focusout', function (event) {
            let inputedValue = $(this).val();
            if (inputedValue) {
                const inputedValues = inputedValue.split('');
               // console.log('console.log(inputedValue);')
              //  console.log(inputedValue);

                if (inputedValues[0] !== "0") {
                    $.alert.open(GlobalMessageNotifierConstants.Warning, "Warning! Mobile No must start with 0");
                }
            }
        })
    },

    globalControl: function () {
        if ($(".datepicker").length > 0) {
            $(".datepicker").datepicker({
                dateFormat: "dd-M-yy",
                showAnim: "scale"
            });
        }

        if ($(".datepicker_mc").length > 0) {
            $(".datepicker_mc").datepicker({
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true,
                yearRange: '-10:+0'
            });
        }
    },

    showNotification: (resp) => {
        $("#loading").hide();
        var result = resp.Result;
        var msg = resp.Message;
        var css = "failed";
        if (result == "OK")
            css = "success";
        $("#dvMessage").attr('class', css);
        $("#dvMessage").html(msg);
        $("#dvMessage").show();        
        $("#dvMessageDown").attr('class', css);
        $("#dvMessageDown").html(msg);
        $("#dvMessageDown").show();
        if (result == "OK") {
            $("#dvMessage").toggle('fade', 1500);
            $("#dvMessageDown").toggle('fade', 1500);           
        }       
    }
    ,
    addSpaceAfterDot: function () {
        if (!$(".addSpaceAfterDot") || $(".addSpaceAfterDot").length <= 0) {
            return;
}

        $(".addSpaceAfterDot").on("keyup", function (e) {
            let key = e.keyCode;
            if (key === 46 || key === 190) {
                let inputedValue = $(this).val();
                //inputedValue = inputedValue.replace('.', '');
                inputedValue = inputedValue + ' ';
                //inputedValue = inputedValue + '. ';
                $(this).val(inputedValue);
            }
        });

    },
    addSpaceAfterDotCommaHashDashColon: function () {
        if (!$(".addSpaceAfterDotCommaHashDashColon") || $(".addSpaceAfterDotCommaHashDashColon").length <= 0) {
            return;
        }
        $(".addSpaceAfterDotCommaHashDashColon").on("keyup", function (e) {
            let key = e.keyCode;
            //console.log("KEYYYYEYEYE")
            //console.log(key);
            if (key === 46 || key === 190 || key === 188 || key === 189 || key === 186 || key === 51) {
                let inputedValue = $(this).val();
                //inputedValue = inputedValue.replace('.', '');
                inputedValue = inputedValue + ' ';
                //inputedValue = inputedValue + '. ';
                $(this).val(inputedValue);
            }
        });

    }
}

$(function () {
    app.initFns()
})

const GlobalMessageNotifierConstants = {
    Success: 'OK',
    Warning: 'Warning',
    Failed: 'Failed',
    Error: "Error"
}

const NameWithStartsConstants = [
    { nameWithStart: 'MST' },
    { nameWithStart: 'MS' },
    { nameWithStart: 'MISS' },
    { nameWithStart: 'LAT' },
    { nameWithStart: 'LET' },
    { nameWithStart: 'MRS' },

    { nameWithStart: 'MRS.' },
    { nameWithStart: 'MST.' },
    { nameWithStart: 'MS.' },
    { nameWithStart: 'MISS.' },
    { nameWithStart: 'LAT.' },
    { nameWithStart: 'LET.' }
]

const NameWithStartsConstantsMale = [
    { nameWithStart: 'MR' },
    { nameWithStart: 'MD' },
    { nameWithStart: 'LAT' },
    { nameWithStart: 'LET' },

    { nameWithStart: 'MR.' },
    { nameWithStart: 'MD.' },
    { nameWithStart: 'LAT.' },
    { nameWithStart: 'LET.' }
]

const NameWithStartsConstantsWrongGender = [
    { nameWithStart: 'Mst' },
    { nameWithStart: 'Ms' },
    { nameWithStart: 'Miss' }
]

const NameWithStartsConstantsWrongGender2 = [
    { nameWithStart: 'Mr' },
    { nameWithStart: 'Md' }
]


const ValidMobileOperatorBD = [
    { nameWithStart: '013' },
    { nameWithStart: '014' },
    { nameWithStart: '015' },
    { nameWithStart: '016' },

    { nameWithStart: '017' },
    { nameWithStart: '018' },
    { nameWithStart: '019' },

]

const ValidMobileDigitBD = [
    { nameWithStart: '00000000' },
    { nameWithStart: '10000000' },
    { nameWithStart: '01000000' },
    { nameWithStart: '00100000' },
    { nameWithStart: '00010000' },
    { nameWithStart: '00001000' },
    { nameWithStart: '00000100' },
    { nameWithStart: '00000010' },
    { nameWithStart: '00000001' },

]

const A_Z_Constants = [
    { letter: 'A' },
    { letter: 'B' },
    { letter: 'C' },
    { letter: 'D' },
    { letter: 'E' },
    { letter: 'F' },
    { letter: 'G' },
    { letter: 'H' },
    { letter: 'I' },
    { letter: 'J' },
    { letter: 'K' },
    { letter: 'L' },
    { letter: 'M' },
    { letter: 'N' },
    { letter: 'O' },
    { letter: 'P' },
    { letter: 'Q' },
    { letter: 'R' },
    { letter: 'S' },
    { letter: 'T' },
    { letter: 'U' },
    { letter: 'V' },
    { letter: 'W' },
    { letter: 'X' },
    { letter: 'Y' },
    { letter: 'Z' }
]

const GenderConstants = {
    Male: 'Male',
    Female: 'Female',
    Transgender: 'T'
}

const MaritalStatusConstants = {
    Married: 'Married',
    Unmarried: 'Unmarried',
    Single: 'Single'
}




var SyncedStatusConstants = {
    NOT_SYNCED: 'NOT SYNCED',
    SYNCED: 'SYNCED'
}
var MFFlagConstants = {
    Male: 'M',
    Female: 'F',
    Neutral: 'N'
}

var BulkSMSAuthConstants ={
    BulkSMSAuthClientKey : "KEY::nObMMomumwvXvlTy6KEfEbjKMbdsO"
}

var DueTypeConstants = {
    Current_Due: 'Current_Due',
    Duration_Over_Due:'Duration_Over_Due'
}

var MFIConstants = {
    Society_For_Social_Service_SSS: 126
}