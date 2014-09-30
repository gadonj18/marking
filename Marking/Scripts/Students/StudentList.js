$(document).ready(function () {
    $("td.CriteriaData input[type='text'], td.CriteriaData textarea, td.CriteriaData select, td.CriteriaData input[type='radio']").blur(function () {
        CheckChanged($(this));
    });

    $("td.CriteriaData select, td.CriteriaData input[type='radio']").change(function () {
        CheckChanged($(this));
    });

    function CheckChanged(Field) {
        var Value = $(Field).val().trim();
        var OldValue = $(Field).siblings("input.OldValue").first().val().trim();
        if (Value !== OldValue) {
            var CriterionID = $(Field).siblings("input.CriterionID").first().val().trim();
            var StudentID = $(Field).siblings("input.StudentID").first().val().trim();
            UpdateValue(CriterionID, StudentID, Value);
            $(Field).siblings("input.OldValue").first().val(Value);
        }
    }

    function UpdateValue(CriterionID, StudentID, Value, Key) {
        $.ajax({
            type: "POST",
            url: "Students/UpdateMark",
            data: "{ CriterionID: " + CriterionID + ", StudentID: " + Student_id + ", Value: '" + Value + "', Key: " + (Key === undefined ? "NULL" : "'" + Key + "'") + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                alert('test');
            }
        });
    }
});