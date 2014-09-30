$(document).ready(function () {
    $("select.FieldType").change(function () {
        FieldTypeChanged($(this));
    });

    $("input.AddOptionButton").click(function () {
        AddOptionRow($(this));
    });

    $("#AddFieldButton").click(function () {
        AddCriterionRow();
    });

    $("#CriteriaTable > tbody > tr").children("td.ReOrderCol").children("input.OrderUpButton").click(function () {
        MoveRow($(this).parent().parent(), "up");
    });

    $("#CriteriaTable > tbody > tr").children("td.ReOrderCol").children("input.OrderDownButton").click(function () {
        MoveRow($(this).parent().parent(), "down");
    });

    $("#CriteriaTable > tbody > tr").filter(":first").children("td.ReOrderCol").children("input.OrderUpButton").attr("disabled", true);
    $("#CriteriaTable > tbody > tr").filter(":last").children("td.ReOrderCol").children("input.OrderDownButton").attr("disabled", true);

    $("#AddNoteButton").click(function () {
        AddNote();
    });

    $("#AddAttachment").click(function () {
        AddAttachment();
    });

    $("a.RemoveAttachment").click(function () {
        $(this).siblings("input.AttachmentRemoved").val("1");
        $(this).parent().hide();
    });

    $("a.RemoveNote").click(function () {
        $(this).siblings("input.NoteRemoved").val("1");
        $(this).parent().hide();
    });

    $("a.RemoveNewNote").click(function () {
        $(this).parent().remove();
    });
});

function MoveRow(row1, dir) {
    var numRows = parseInt($("#CriteriaTable > tbody").children("tr").length, 10);

    var row1OrderField = row1.children("td.ReOrderCol").children("input.FieldOrder");
    var row1Order = parseInt(row1OrderField.val(), 10);
    var newOrder1 = row1Order + (dir === "up" ? -1 : 1);
    row1OrderField.val(newOrder1);

    var row2 = (dir == "up" ? row1.prev() : row1.next());
    var row2OrderField = row2.children("td.ReOrderCol").children("input.FieldOrder");
    var row2Order = parseInt(row2OrderField.val(), 10);
    var newOrder2 = row2Order + (dir === "up" ? 1 : -1);
    row2OrderField.val(newOrder2);

    if (dir === "up") row1.insertBefore(row2);
    else row1.insertAfter(row2);

    var upButton1 = row1.children("td.ReOrderCol").children("input.OrderUpButton");
    var downButton1 = row1.children("td.ReOrderCol").children("input.OrderDownButton");
    if (row1Order === 1) upButton1.removeAttr("disabled");
    else if (row1Order === numRows) downButton1.removeAttr("disabled");
    if (newOrder1 === 1) upButton1.attr("disabled", true);
    else if (newOrder1 === numRows) downButton1.attr("disabled", true);

    var upButton2 = row2.children("td.ReOrderCol").children("input.OrderUpButton");
    var downButton2 = row2.children("td.ReOrderCol").children("input.OrderDownButton");
    if (row2Order === 1) upButton2.removeAttr("disabled");
    else if (row2Order === numRows ) downButton2.removeAttr("disabled");
    if (newOrder2 === 1) upButton2.attr("disabled", true);
    else if (newOrder2 === numRows) downButton2.attr("disabled", true);
}

function AddNote() {
    $.ajax({
        type: "POST",
        url: "/Assessments/NewNote",
        cache: false,
        datatype: "html",
        success: function (data) {
            $("#Notes").append(data);
            $("#Notes").children("div.NoteRow").filter(":last").children("a.RemoveNewNote").click(function () {
                $(this).parent().remove();
            });
        }
    });
}

function AddAttachment() {
    $.ajax({
        type: "POST",
        url: "/Assessments/NewAttachment",
        cache: false,
        datatype: "html",
        success: function (data) {
            $("#Attachments").append(data);
            $("#Attachments").children("div.AttachmentRow").filter(":last").children("a.RemoveNewAttachment").click(function () {
                $(this).parent().remove();
            });
        }
    });
}

function AddCriterionRow() {
    $.ajax({
        type: "POST",
        url: "/Assessments/NewCriterionRow",
        cache: false,
        datatype: "html",
        success: function (data) {
            $("#CriteriaTable > tbody").append(data);
            var newRow = $("#CriteriaTable > tbody > tr").filter(":last");
            var index = newRow.find("input[name='Criterion.index']").val();
            newRow.children("td.FieldTypeCol").children("select").change(function () {
                FieldTypeChanged($(this));
            });
            newRow.children("td.AddOptionRow").children("input.AddOptionButton").click(function () {
                AddOptionRow($(this));
            });
        }
    });
}

function AddOptionRow(button) {
    var cell = $(button).parent().siblings("td.FieldOptionsCol");
    var numOptions = parseInt($(cell).children("div").length, 10);
    $.ajax({
        type: "POST",
        url: "/Assessments/NewOptionRow",
        cache: false,
        datatype: "html",
        success: function (data) {
            $(cell).append(data);
            $(cell).children("div:last").children("input.OptionOrder").val(numOptions + 1);
        }
    });
}

function FieldTypeChanged(list) {
    var FieldType = $(list).children("option:selected").text();
    var OldFieldType = $(list).siblings("input.OldFieldType").val();
    var cell = $(list).parent().siblings("td.FieldOptionsCol");
    var addButton = $(list).parent().siblings("td.AddOptionRow").children("input.AddOptionButton");

    switch (FieldType) {
        case "":
        case "Textbox":
        case "TextboxMulti":
            $(cell).html("");
            $(addButton).hide();
            break;
        case "Radio":
        case "Dropdown":
        case "Checkbox":
            if (OldFieldType !== "Radio" && OldFieldType !== "Dropdown" && OldFieldType !== "Checkbox") {
                AddOptionRow(addButton);
                $(addButton).show();
            }
            break;
    }

    $(list).siblings("input.OldFieldType").val(FieldType);
}