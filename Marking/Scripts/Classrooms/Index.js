$(document).ready(function () {
    $("#AddAssessmentDialog").dialog({ autoOpen: false, modal: true });

    $("tr.ClassroomRow").each(function () {
        var row = $(this);
        var ClassroomID = row.attr("id").substr(9);
        var header = row.find("td span.Header");

        header.click(function () {
            if(row.hasClass("open")) {
                row.siblings("tr.childOf" + ClassroomID).hide();
                CloseClassroom(row, ClassroomID);
            } else {
                row.siblings("tr.ClassroomRow").each(function () {
                    CloseClassroom($(this), $(this).attr("id").substr(9));
                });
                row.siblings("tr.AssessmentRow").hide();
                OpenClassroom(row, ClassroomID);
                row.siblings("tr.childOf" + ClassroomID).show();
            }
        });

        $("#AddAssessment" + ClassroomID).click(function() {
            $("#AddAssessmentDialog").dialog("open");
            $("#AddNewLink").attr("href", $("#AddNewLink").attr("href") + "/" + ClassroomID);
            $("#ReUseLink").attr("href", $("#ReUseLink").attr("href") + "/" + ClassroomID);
        });

    });

    $("#YearDropdown").change(function () {
        window.location.replace($(this).val());
    });

    function OpenClassroom(row, ClassroomID) {
        row.removeClass("closed");
        row.addClass("open");
    }

    function CloseClassroom(row, ClassroomID) {
        row.removeClass("open");
        row.addClass("closed");
    }
});