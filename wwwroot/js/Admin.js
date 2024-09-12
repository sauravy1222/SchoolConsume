$(document).ready(function () {
    // First AJAX call to populate teacher dropdown
    $.ajax({
        url: "https://localhost:44386/api/Admin/GetTeacher",
        type: "GET",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {
            var htmlContent = "";
            $.each(result, function (index, item) {
                htmlContent += "<option value='" + item.TeacherId + "'>" + item.FirstName + "</option>";
            });
            $('#TeacherId').html(htmlContent);
        },
        error: function () {
            console.log("Failed to load teachers");
        }
    });
    $.ajax({
        url: "/Teacher/GetClass",
        type: "Get",
        dataType: "json",
        success: function (result) {
            var htmlContent = "";
            var i = 1;
            $.each(result, function (index, item) {
                console.log("Processing item:", item);

                htmlContent += "<option value='" + item.classId + "'>" + item.className + "</option>";
                i++;
            });
            console.log(htmlContent);
            $('#AddClassInFees').html(htmlContent);
            $('#FetchAllClasses').html(htmlContent);

        },
        error: function (xhr, status, error) {


            console.error("AJAX Error:", status, error);
            console.error("Response Text:", xhr.responseText);
        }
    })

    // Second AJAX call to get the number of students
    $.ajax({
        url: "https://localhost:44386/api/Admin/GetNoOfStudents",
        type: "GET",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {
            $('#noofstudents').html(result);
        },
        error: function () {
            console.log("Failed to load the number of students");
        }
    });

    // Third AJAX call to get the number of teachers
    $.ajax({
        url: "https://localhost:44386/api/Admin/NoOfTeachers",
        type: "GET",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {
            $('#NoOfTeachers').html(result);
        },
        error: function () {
            console.log("Failed to load the number of teachers");
        }
    });

    // Function to handle attendance submission
    function submitAttendance() {
        var selectedStudents = [];
        var checkboxes = document.querySelectorAll('input[name="selectedStudents"]:checked');

        checkboxes.forEach(function (checkbox) {
            var studentId = checkbox.value;
            var classId = checkbox.getAttribute('data-classid');
            selectedStudents.push({
                StudentId: studentId,
                ClassId: classId,
                Status: true // Marking as present
            });
        });

        if (selectedStudents.length > 0) {
            fetch('/api/StudentAttendance', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(selectedStudents)
            })
                .then(response => {
                    if (response.ok) {
                        alert('Students added to present list.');
                    } else {
                        alert('Failed to add students to present list.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        } else {
            alert('Please select at least one student.');
        }
    }

    // Attach submitAttendance function to button click
    $('#submitAttendanceButton').on('click', function () {
        submitAttendance();
    });
});
