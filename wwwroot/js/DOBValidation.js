$(document).ready(function () {
    $("#DOB").on("change", function () {
        const input = new Date($(this).val());
        const today = new Date();
        let age = today.getFullYear() - input.getFullYear();
        const m = today.getMonth() - input.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < input.getDate())) {
            age--;
        }

        if (age < 8 || age > 80) {
            $("#dob-error").text("Age must be between 8 and 80.");
        } else {
            $("#dob-error").text("");
        }
    });
});
