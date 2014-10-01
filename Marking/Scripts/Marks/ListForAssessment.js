$(document).ready(function () {
    $("tr.StudentRow").click(function () {
        StudentID = $(this).find("input.StudentID").first().val().trim();
        $("tr.CriteriaRow").hide();
        $(this).siblings("tr.Student_" + StudentID).show();
    });

    $("td.CriteriaData input[type='text'], td.CriteriaData textarea, td.CriteriaData select, td.CriteriaData input[type='radio']").blur(function () {
        CheckChanged($(this), $(this).val().trim());
    });

    $("td.CriteriaData select, td.CriteriaData input[type='radio']").change(function () {
        CheckChanged($(this), $(this).val().trim());
    });

    $("td.CriteriaData input[type='checkbox']").change(function () {
        var Value = $.map($("td.CriteriaData input[type='checkbox'][name='" + $(this).attr("name") + "']:checked"), function (elem) { return elem.value || ""; }).join("|");
        CheckChanged($(this), Value);
    });

    function CheckChanged(Field, Value) {
        var OldValue = $(Field).siblings("input.OldValue").first().val().trim();
        if (Value !== OldValue) {
            var CriterionID = $(Field).siblings("input.CriterionID").first().val().trim();
            var StudentID = $(Field).siblings("input.StudentID").first().val().trim();
            UpdateValue(CriterionID, StudentID, Value);
            $(Field).siblings("input.OldValue").first().val(Value);
        }
    }

    function UpdateValue(CriterionID, StudentID, Value, Key) {
        if(Key === undefined) Key = "";
        var data = { "CriterionID": CriterionID, "StudentID": StudentID, "Value": Value, "Key": Key };
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Marks/UpdateMark",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log(msg);
            }
        });
    }
});