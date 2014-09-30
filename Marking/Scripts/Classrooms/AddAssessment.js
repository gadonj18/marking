$(document).ready(function () {
    $("#YearFilter").change(function () {
        RefreshList();
    });

    $("#GradeFilter").change(function () {
        RefreshList();
    });

    $("#ClassroomFilter").change(function () {
        RefreshList();
    });

    $("#ResetFilter").click(function () {
        var year = $("#YearFilter").val("");
        var grade = $("#GradeFilter").val("");
        var classroom = $("#ClassroomFilter").val("");
        RefreshList();
    });

    function RefreshList() {
        var year = $("#YearFilter").val();
        var grade = $("#GradeFilter").val();
        var classroom = $("#ClassroomFilter").val();

        $("#AssessmentTable tbody tr").remove();

        for(var item in assessments) {
            if ((year === "" && grade === "" && classroom === "") ||  (
                (year === "" || parseInt(year, 10) === assessments[item].Item3) && 
                (grade === "" || parseInt(grade, 10) === assessments[item].Item4) && 
                (classroom === "" || parseInt(classroom, 10) === assessments[item].Item2)
            )) {

                $("#AssessmentTable tbody").append($("<tr/>")
                    .attr("class", "AssessmentRow")
                    .attr("id", "Assessment" + assessments[item].Item1)
                    .click(function () {
                        $("#AssessmentID").val($(this).attr("id").substr(10));
                        $("#AssessmentForm").submit();
                    })
                    .append($("<td/>")
                        .attr("colspan", "4")
                        .text(assessments[item].Item5 + ": " + assessments[item].Item6)
                    )
                );
            }
        }
    }

    RefreshList();
});