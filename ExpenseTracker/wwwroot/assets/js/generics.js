
function CopytoClipboard(target) {
    var text = $(target).text().trim();
    $.gritter.add({
        title: text,
        text: 'Copied to Clipboard',
        sticky: false,
        time: ''
    });

    var input = document.createElement('input');
    input.setAttribute('value', text);
    document.body.appendChild(input);
    input.select();
    var result = document.execCommand('copy');
    document.body.removeChild(input)
    return result;
}
function UnderDevelopment() {
    $.gritter.add({
        title: 'Under Development',
        text: 'This feature is under development and will be integrated soon.',
        sticky: false,
        time: ''
    });
}
function commify(n) {
    var parts = n.toString().split(".");
    const numberPart = parts[0];
    const decimalPart = parts[1];
    const thousands = /\B(?=(\d{3})+(?!\d))/g;
    return numberPart.replace(thousands, ",") + (decimalPart ? "." + decimalPart : "");
}
// Generic code for any dropdown to fill called by ajax
function FillDropdown(selector, vData) {
    if (vData.length > 0) {
        var vItems = [];
        for (var i in vData) {
            if (vData[i].Selected)
                vItems.push('<option selectedselected=selected value="' + vData[i].Value + '">' + vData[i].Text + '</option>');
            else
                vItems.push('<option value="' + vData[i].Value + '">' + vData[i].Text + '</option>');
        }
        $(selector).empty();
        $(selector).append(vItems.join(''));
        return true;
    }
    else {
        $(selector).empty();
        return false;
    }
}
function FillDropdownTitle(selector, vData) {
    if (vData.length > 0) {
        var vItems = [];
        for (var i in vData) {
            if (vData[i].Selected)
                vItems.push('<option selectedselected=selected value="' + vData[i].Value + '" title="' + vData[i].Title + '">' + vData[i].Text + '</option>');
            else
                vItems.push('<option value="' + vData[i].Value + '" title="' + vData[i].Title + '">' + vData[i].Text + '</option>');
        }
        $(selector).empty();
        $(selector).append(vItems.join(''));
        return true;
    }
    else {
        $(selector).empty();
        return false;
    }
}


function NullSafeFloat(thisval) {
    var retvalue = 0.0;
    if ($.isNumeric(thisval)) {
        retvalue = parseFloat(thisval);
    }
    return retvalue;
}
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};

function LoadPartialView(sDivID, sURL) {
    BlockUI();
    $("#" + sDivID).load(sURL);
    //$('#SubHeader').text( $('#PartialTitle').html());
    UnblockUI();
}
function UnblockUI() {
    $.unblockUI();
}
function BlockUI() {
    $.blockUI({ message: '<div class="loader"><div class="line-scale-pulse-out"><div class="bg-warning"></div><div class="bg-warning"></div><div class="bg-warning"></div><div class="bg-warning"></div><div class="bg-warning"></div></div></div>' });
}

function checkValidPhone(phoneNumber) {
    var phoneno = /^\d{10}$/;
    if (phoneNumber.match(phoneno)) {
        return true;
    }
    else {
        return false;
    }
}

function checkValidEmail(email) {
    // Validate email format
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);
};
function AllowNumberOnly(event) {


    if (event.shiftKey == true) {
        event.preventDefault();
    }

    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 110 ||
        event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
        event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

    } else {
        event.preventDefault();
    }

    //if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
    //    event.preventDefault();
}

function RemoveDocument(enDocumentID) {
    $.ajax(
        {
            type: "POST", //HTTP POST Method
            url: "/Document/RemoveDocument?sen=" + enDocumentID, // Controller/View
            success: function (successdata) {
                location.reload();
            }
        });//ajax
}